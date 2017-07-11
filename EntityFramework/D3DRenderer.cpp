#include "D3DRenderer.h"
#include "Entity.h"
#include <vector>
#include <array>
#include "ModelComponent.h"
#include "D3DTexture.h"


namespace KLM_FRAMEWORK
{


	int DXRenderer::s_ScreenWidth{ 800 };
	int DXRenderer::s_ScreenHeight{ 600 };
	int   DXRenderer::s_Samples{ 4 };
	bool  DXRenderer::s_WireframeGreen{ false };

	Mat4 DXRenderer::s_IdentityMatrix{ Mat4(1.0f) };

	HWND m_hWnd;
	ID3D11Device*				DXRenderer::s_pDevice{ nullptr };
	ID3D11DeviceContext*		DXRenderer::s_pDeviceContext{ nullptr };
	IDXGISwapChain*				DXRenderer::s_pSwapChain{ nullptr };
	ID3D11RenderTargetView*		DXRenderer::s_pRenderTargetView{ nullptr };
	ID3D11Buffer*				DXRenderer::s_pUniformBuffer{ nullptr };
	ID3D11DepthStencilState*	DXRenderer::s_pDepthStencilState{ nullptr };
	ID3D11DepthStencilView*		DXRenderer::s_pDepthStencilView{ nullptr };
	ID3D11Texture2D*			DXRenderer::s_pDepthStencilBuffer{ nullptr };
	ID3D11RasterizerState*		DXRenderer::s_pRasteriserState{ nullptr };      // TO DO
	ID3D11BlendState*			DXRenderer::s_pBlendState{ nullptr };
	ID3D11BlendState*			DXRenderer::s_pNoBlendState{ nullptr }; // TO DO
	ID3D11SamplerState*			DXRenderer::s_pSamplerState{ nullptr };

	D3D11_RASTERIZER_DESC		DXRenderer::s_RasterizerDesc{ D3D11_FILL_SOLID, D3D11_CULL_NONE, 0,0 };
	D3D_DRIVER_TYPE				DXRenderer::s_DriverType{ D3D_DRIVER_TYPE_HARDWARE };
	D3D_FEATURE_LEVEL			DXRenderer::s_FeatureLevel{ D3D_FEATURE_LEVEL_11_0 };
	D3D11_VIEWPORT				DXRenderer::s_Viewport{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

	ID3D11ShaderResourceView*	DXRenderer::s_Srv{ nullptr };
	ID3D11Buffer*				DXRenderer::s_LightBuffer{ nullptr };


	Camera* DXRenderer::s_CurrentCamera{ nullptr };


	bool DXRenderer::Initialize(const int width, const int height, const HWND handle)
	{
		s_ScreenWidth = width;
		s_ScreenHeight = height;
		const UINT createDeviceFlags = 0;

		const std::array<D3D_DRIVER_TYPE, 3> driverTypes =
		{
			D3D_DRIVER_TYPE_HARDWARE,
			D3D_DRIVER_TYPE_WARP,
			D3D_DRIVER_TYPE_REFERENCE
		};

		const UINT drvTypeCount = driverTypes.size();

		const std::array<D3D_FEATURE_LEVEL, 4> featureLevels =
		{
			D3D_FEATURE_LEVEL_11_0,
			D3D_FEATURE_LEVEL_10_1,
			D3D_FEATURE_LEVEL_10_0,
			D3D_FEATURE_LEVEL_9_3
		};

		const UINT featureLevelsCount = featureLevels.size();

		DXGI_SWAP_CHAIN_DESC swDesc;

		// clear out the struct for use
		ZeroMemory(&swDesc, sizeof(DXGI_SWAP_CHAIN_DESC));

		//fill the swap desc structure
		swDesc.BufferCount = 1;                                   // double buffering (one back buffer)

		swDesc.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;   // 32bit color
		swDesc.BufferDesc.Width = width;
		swDesc.BufferDesc.Height = height;
		swDesc.BufferDesc.RefreshRate.Numerator = 60;
		swDesc.BufferDesc.RefreshRate.Denominator = 1;           // 60:1 fps
		swDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;    // how swap chain is to be used
		swDesc.OutputWindow = handle;                            // the window to be used
		swDesc.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;            // discard buffer that has been shown
		swDesc.SampleDesc.Count = 4;                             // how many multisamples
		swDesc.Windowed = TRUE;                                  // windowed/full-screen mode
		swDesc.Flags = DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;   // allow full-screen switching

		HRESULT result;

		for (int i = 0; i < drvTypeCount; ++i)
		{
			result = D3D11CreateDeviceAndSwapChain(nullptr, driverTypes[i], nullptr, createDeviceFlags,
				&featureLevels[0], featureLevelsCount, D3D11_SDK_VERSION, &swDesc, &s_pSwapChain, &s_pDevice,
				&s_FeatureLevel, &s_pDeviceContext);
			if (SUCCEEDED(result))
			{
				s_DriverType = driverTypes[i];
				break;
			}
		}

		if (FAILED(result))
		{
			std::cout << "Could not create device and swap chain" << std::endl;
			::MessageBox(nullptr, "Failed to create device and swapchain\nApp must now exit", "Device fail", MB_OK | MB_ICONEXCLAMATION);
			return false;
		}

		//CREATE RENDER TARGET VIEW
		ID3D11Texture2D* pBackBufferTexture = nullptr;
		s_pSwapChain->GetBuffer(NULL, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&pBackBufferTexture));
		result = s_pDevice->CreateRenderTargetView(pBackBufferTexture, nullptr, &s_pRenderTargetView);

		pBackBufferTexture->Release();

		if (FAILED(result)) {
			std::cout << "Could not create render target view" << std::endl;
			::MessageBox(nullptr, "Failed to create Render Target View\nApp must now exit", "Render Target view Fail", MB_ICONEXCLAMATION | MB_OK);
			return false;
		}

		// Create depth/stencil buffer
		CreateDepthBuffer();
		// set depth/stencil
		s_pDeviceContext->OMSetRenderTargets(1, &s_pRenderTargetView, s_pDepthStencilView);
		// Create blend states
		CreateBlendStates();

		SetUpRasterizerState();

		//VIEWPORT SETUP
		s_Viewport.Width = width;
		s_Viewport.Height = height;
		s_Viewport.TopLeftX = 0;
		s_Viewport.TopLeftY = 0;
		s_Viewport.MinDepth = 0.0f;
		s_Viewport.MaxDepth = 1.0f;

		s_pDeviceContext->RSSetViewports(1, &s_Viewport);

		//Create uniform buffer
		UniformBufferStructure uniforms;
		uniforms.ModelViewProjectionMatrix = s_IdentityMatrix;
		uniforms.Ambient = Vec4{ 1.0f, 1.0f, 1.0f, 1.0f };
		uniforms.Diffuse = Vec4{ 1.0f, 1.0f, 1.0f, 1.0f };
		uniforms.Specular = Vec4{ 1.0f, 1.0f, 1.0f, 1.0f };
		CreateUniformBuffer(sizeof(UniformBufferStructure), &uniforms, true, true, &s_pUniformBuffer);

		//ResizeWindow(width, height);

		CreateLightBuffer();

		LightBase::InitializeLightSystem();

		return true;
	}

	void DXRenderer::Render(Entity* const entity)
	{
		

		Component* c = entity->GetComponentFirst(ComponentType::MODEL_COMPONENT);
		if (!c) return;

		ModelComponent* mc = static_cast<ModelComponent*>(c);
		Material* material = const_cast<Material*>(mc->GetMaterial());

		if (!material) return;
		//PRINTL("Render ENTITY: " + entity->GetName() + " is at position: " + ToString(entity->GetTransform()->GetWorldPosition()));

		UniformBufferStructure ubs;
		ubs.Ambient = (*material->GetAmbientColPtr());
		ubs.Diffuse = (*material->GetDiffuseColPtr());
		ubs.Specular = (*material->GetSpecularColPtr());


		const Texture* diffuseMapGen = material->GetDiffuseMap();
		const Texture* specularMapGen = material->GetSpecularMap();
		const Texture* normalMapGen = material->GetNormalMap();


		if (diffuseMapGen)
		{
			const void* diffuse = material->GetDiffuseMap()->GetApiSpecificTexture();
			D3DTexture* diffuseMap = const_cast<D3DTexture*>(static_cast<const D3DTexture*>(diffuse));		
			ID3D11SamplerState* samplerState = diffuseMap->GetSamplerState();
			ID3D11ShaderResourceView* ou = const_cast<ID3D11ShaderResourceView*>(diffuseMap->GetData());
			s_pDeviceContext->PSSetShaderResources(0, 1, &ou);
			s_pDeviceContext->PSSetSamplers(0, 1, &samplerState);	
		}
		if (specularMapGen)
		{
			const void* specular = material->GetSpecularMap()->GetApiSpecificTexture();
			D3DTexture* specularMap = const_cast<D3DTexture*>(static_cast<const D3DTexture*>(specular));
			ID3D11SamplerState* samplerState = specularMap->GetSamplerState();
			ID3D11ShaderResourceView* ou = const_cast<ID3D11ShaderResourceView*>(specularMap->GetData());
			s_pDeviceContext->PSSetShaderResources(1, 1, &ou);
			s_pDeviceContext->PSSetSamplers(0, 1, &samplerState);
		}
		if (normalMapGen)
		{
			const void* normal = material->GetNormalMap()->GetApiSpecificTexture();
			D3DTexture* normalMap = const_cast<D3DTexture*>(static_cast<const D3DTexture*>(normal));
			ID3D11SamplerState* samplerState = normalMap->GetSamplerState();
			ID3D11ShaderResourceView* ou = const_cast<ID3D11ShaderResourceView*>(normalMap->GetData());
			s_pDeviceContext->PSSetShaderResources(2, 1, &ou);
			s_pDeviceContext->PSSetSamplers(2, 1, &samplerState);
		}



		if (!s_CurrentCamera) return;
		if (!s_CurrentCamera->isActive()) return;


		Mat4 worldView;
		if (s_CurrentCamera->GetParent() == nullptr)
		{
			worldView = s_CurrentCamera->GetTransformMatrix() * entity->GetTransform()->GetWorldMat();
		}
		else
		{
			Entity* parent = s_CurrentCamera->GetParent();
			worldView =
				s_CurrentCamera->SetTransformMatrix(parent->GetTransform()->GetWorldMat())
				* entity->GetTransform()->GetWorldMat();

			ubs.CameraPosition = (parent->GetTransform()->GetWorldMat()*glm::vec4(0, 0, 0, 1));
		}
		Mat4 MVP = /*glm::perspectiveFovLH(glm::radians(60.0f),
			(float)s_ScreenWidth,
			(float)s_ScreenHeight,
			0.001f,
			1000.0f)**/ 
			s_CurrentCamera->GetProjectionMatrix(s_ScreenWidth,s_ScreenHeight)* worldView;


		ubs.ModelViewProjectionMatrix = MVP;
		ubs.InvTranspWorldMatrix = glm::transpose(glm::inverse(entity->GetTransform()->GetWorldMat()));
		ubs.WorldMatrix = entity->GetTransform()->GetWorldMat();

		UpdateUniformBuffer(s_pUniformBuffer, sizeof(UniformBufferStructure), &ubs, 0);


		// s_pDeviceContext->RSSetViewports(1, &s_Viewport);

		material->SetCurrentShaders();
		mc->GetMesh()->GetVBO()->Draw(PrimitiveType::TRIANGLES);


	}


	void DXRenderer::Update(const float deltaTime, const float totalTime)
	{
		if (LightBase::IsRequestingUpdate())
		{
			UpdateDynamicBuffer(s_LightBuffer, sizeof(ShaderLightInfoStruct) 
				* LightBase::MAX_LIGHTS, &const_cast<ShaderLightInfoStruct*>(LightBase::GetLightInfo())[0]);
			LightBase::UpdateFinished();
		}
	}




	void DXRenderer::ClearScreen(const Colour & colour)
	{
		s_pDeviceContext->ClearRenderTargetView(s_pRenderTargetView, D3DXCOLOR(colour.r, colour.g, colour.b, colour.a));
		s_pDeviceContext->ClearDepthStencilView(s_pDepthStencilView, D3D11_CLEAR_DEPTH | D3D11_CLEAR_STENCIL, 1.0f, 0);
	}

	void DXRenderer::SetCullMode(const CullMode mode)
	{

	}

	void DXRenderer::SetFillMode(const FillMode mode)
	{

	}

	void DXRenderer::CreateUniformBuffer(const UINT size, void* const data, const bool dynamic, const bool CPUupdates, ID3D11Buffer** const output)
	{
		D3D11_BUFFER_DESC cbDesc;
		cbDesc.ByteWidth = size;
		cbDesc.MiscFlags = 0;
		cbDesc.StructureByteStride = 0;
		cbDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;

		if (dynamic && CPUupdates)
		{
			cbDesc.Usage = D3D11_USAGE_DYNAMIC;
			cbDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
		}
		else if (dynamic && !CPUupdates)
		{
			cbDesc.Usage = D3D11_USAGE_DEFAULT;
			cbDesc.CPUAccessFlags = 0;
		}
		else
		{
			cbDesc.Usage = D3D11_USAGE_IMMUTABLE;
			cbDesc.CPUAccessFlags = 0;
		}

		// Fill in the subresource data.
		D3D11_SUBRESOURCE_DATA InitData;

		InitData.pSysMem = data;
		InitData.SysMemPitch = 0;
		InitData.SysMemSlicePitch = 0;

		// Create the buffer.
		s_pDevice->CreateBuffer(&cbDesc, &InitData, output);
	}

	void DXRenderer::UpdateUniformBuffer(ID3D11Buffer * const buffer, const UINT dataSize, void * const data, const UINT slot)
	{
		D3D11_MAPPED_SUBRESOURCE ms;
		s_pDeviceContext->Map(buffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &ms);
		memcpy(ms.pData, data, dataSize);
		s_pDeviceContext->Unmap(buffer, NULL);
		s_pDeviceContext->VSSetConstantBuffers(slot, 1, &buffer);
		s_pDeviceContext->PSSetConstantBuffers(slot, 1, &buffer);
	}

	void DXRenderer::UpdateUniformBuffer(const UniformBufferStructure & buffer) const
	{
		D3D11_MAPPED_SUBRESOURCE ms;
		s_pDeviceContext->Map(s_pUniformBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &ms);
		memcpy(ms.pData, &buffer, sizeof(UniformBufferStructure));
		s_pDeviceContext->Unmap(s_pUniformBuffer, NULL);
		s_pDeviceContext->VSSetConstantBuffers(0, 1, &s_pUniformBuffer);
		s_pDeviceContext->PSSetConstantBuffers(0, 1, &s_pUniformBuffer);
	}

	void DXRenderer::UpdateDynamicBuffer(ID3D11Buffer * const buffer, const UINT dataSize, void * const data)
	{
		D3D11_MAPPED_SUBRESOURCE ms;
		s_pDeviceContext->Map(buffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &ms);
		memcpy(ms.pData, data, dataSize);
		s_pDeviceContext->Unmap(buffer, NULL);
	}

	HRESULT DXRenderer::CreateStructuredBuffer(const UINT iNumElements, const bool isCpuWritable, const bool isGpuWritable, ID3D11Buffer ** ppBuffer, ID3D11ShaderResourceView ** ppSRV, ID3D11UnorderedAccessView ** ppUAV, void * pInitialData, UINT data_size)
	{
		HRESULT hr = static_cast<HRESULT>(0);


		D3D11_BUFFER_DESC bufferDesc;
		//ZeroMemory(&bufferDesc, sizeof(bufferDesc));
		bufferDesc.ByteWidth = iNumElements * data_size;

		if ((!isCpuWritable) && (!isGpuWritable))
		{
			bufferDesc.CPUAccessFlags = 0;
			bufferDesc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
			bufferDesc.Usage = D3D11_USAGE_IMMUTABLE;
		}
		else if (isCpuWritable && (!isGpuWritable))
		{
			bufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
			bufferDesc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
			bufferDesc.Usage = D3D11_USAGE_DYNAMIC;
		}
		else if ((!isCpuWritable) && isGpuWritable)
		{
			bufferDesc.CPUAccessFlags = 0;
			bufferDesc.BindFlags = (D3D11_BIND_SHADER_RESOURCE |
				D3D11_BIND_UNORDERED_ACCESS);
			bufferDesc.Usage = D3D11_USAGE_DEFAULT;
		}
		else
		{
			assert((!(isCpuWritable && isGpuWritable)));
		}

		bufferDesc.MiscFlags = D3D11_RESOURCE_MISC_BUFFER_STRUCTURED;
		bufferDesc.StructureByteStride = data_size;

		D3D11_SUBRESOURCE_DATA InitData;

		InitData.pSysMem = pInitialData;
		InitData.SysMemPitch = 0;
		InitData.SysMemSlicePitch = 0;
		HRESULT hrt = s_pDevice->CreateBuffer(&bufferDesc, &InitData, ppBuffer);

		if (FAILED(hrt))
		{
			std::cout << "FAILED TO CREATE  STRUCTURED BUFFER " << std::endl;
			std::cout << hr << std::endl;

		}

		D3D11_SHADER_RESOURCE_VIEW_DESC srvDesc;
		ZeroMemory(&srvDesc, sizeof(srvDesc));
		srvDesc.Format = DXGI_FORMAT_UNKNOWN;
		srvDesc.ViewDimension = D3D11_SRV_DIMENSION_BUFFER;
		srvDesc.Buffer.ElementWidth = iNumElements;
		hrt = s_pDevice->CreateShaderResourceView(*ppBuffer, &srvDesc, ppSRV);
		if (FAILED(hrt))
		{
			std::cout << "FAILED TO CREATE RESOURCE VIEW FOR STRUCTURED BUFFER " << std::endl;

		}
		else
		{
			std::cout << "RESOURCE VIEW FOR STRUCTURED BUFFER CREATED" << std::endl;
		}

		if (isGpuWritable)
		{
			assert(ppUAV != NULL);

			D3D11_UNORDERED_ACCESS_VIEW_DESC uavDesc;
			ZeroMemory((&uavDesc), sizeof(uavDesc));
			uavDesc.Format = DXGI_FORMAT_UNKNOWN;
			uavDesc.ViewDimension = D3D11_UAV_DIMENSION_BUFFER;
			uavDesc.Buffer.NumElements = iNumElements;
			s_pDevice->CreateUnorderedAccessView(*ppBuffer, &uavDesc, ppUAV);
		}

		return hr;
	}

	void DXRenderer::CreateLightBuffer()
	{
		CreateStructuredBuffer(LightBase::MAX_LIGHTS, true, false,
			&s_LightBuffer,
			&s_Srv,
			nullptr,
			&const_cast<ShaderLightInfoStruct*>(LightBase::GetLightInfo())[0],
			sizeof(ShaderLightInfoStruct));

		s_pDeviceContext->PSSetShaderResources(4, 1, &s_Srv);
	}



	bool DXRenderer::CreateDepthBuffer()
	{
		D3D11_TEXTURE2D_DESC depthStencilDesc;
		ZeroMemory(&depthStencilDesc, sizeof(depthStencilDesc));

		depthStencilDesc.Width = s_ScreenWidth;
		depthStencilDesc.Height = s_ScreenHeight;
		depthStencilDesc.Usage = D3D11_USAGE_DEFAULT;
		depthStencilDesc.Format = DXGI_FORMAT_D24_UNORM_S8_UINT;
		depthStencilDesc.BindFlags = D3D11_BIND_DEPTH_STENCIL;
		depthStencilDesc.SampleDesc.Count = s_Samples;
		depthStencilDesc.SampleDesc.Quality = 0;
		depthStencilDesc.MipLevels = 1;
		depthStencilDesc.ArraySize = 1;

		D3D11_DEPTH_STENCIL_DESC depthStencilStateDesc;
		// Set up the description of the stencil state.
		ZeroMemory(&depthStencilStateDesc, sizeof(depthStencilStateDesc));
		depthStencilStateDesc.StencilReadMask = 0xFF;
		depthStencilStateDesc.StencilWriteMask = 0xFF;
		depthStencilStateDesc.DepthEnable = true;
		depthStencilStateDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ALL;
		depthStencilStateDesc.DepthFunc = D3D11_COMPARISON_LESS;
		depthStencilStateDesc.StencilEnable = true;
		// Front facing pixels
		depthStencilStateDesc.FrontFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
		depthStencilStateDesc.FrontFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
		depthStencilStateDesc.FrontFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
		depthStencilStateDesc.FrontFace.StencilDepthFailOp = D3D11_STENCIL_OP_INCR;
		// Back-facing pixels
		depthStencilStateDesc.BackFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
		depthStencilStateDesc.BackFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
		depthStencilStateDesc.BackFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
		depthStencilStateDesc.BackFace.StencilDepthFailOp = D3D11_STENCIL_OP_DECR;
		HRESULT result = s_pDevice->CreateTexture2D(&depthStencilDesc, nullptr, &s_pDepthStencilBuffer);
		if (FAILED(result)) 
		{
			//	OutputDebugString(L"\nFAILED TO CREATE TEXTURE2D FOR DEPTH STENCIL BUFFER\n");
			std::cout << "Could not create Texture2D for Depth stencil buffer" << std::endl;
			//::MessageBox(nullptr, "Failed to create Depth-Stencil buffer\nApp must now exit", "Depth-Stencil Fail", MB_ICONEXCLAMATION | MB_OK);
			return false;
		}

		ID3D11DepthStencilState* depthStencilState;
		// Create
		result = s_pDevice->CreateDepthStencilState(&depthStencilStateDesc, &depthStencilState);
		if (FAILED(result))
		{
			std::cout << "Could not create Depth-Stencil state" << std::endl;
			//::MessageBox(nullptr, "Failed to create Depth-Stencil state\nApp must now exit", "Depth-Stencil state Fail", MB_ICONEXCLAMATION | MB_OK);
			return false;
		}
		// Set
		s_pDeviceContext->OMSetDepthStencilState(depthStencilState, 1);

		D3D11_DEPTH_STENCIL_VIEW_DESC descDSV;
		ZeroMemory(&descDSV, sizeof(descDSV));
		descDSV.Format = depthStencilDesc.Format;
		descDSV.ViewDimension = D3D11_DSV_DIMENSION_TEXTURE2DMS;
		descDSV.Texture2D.MipSlice = 0;

		result = s_pDevice->CreateDepthStencilView(s_pDepthStencilBuffer, &descDSV, &s_pDepthStencilView);
		if (FAILED(result))
		{
			std::cout << "Could not create Depth Stencil View" << std::endl;
			//::MessageBox(nullptr, "Failed to create Depth-Stencil view\nApp must now exit", "Depth-Stencil view Fail", MB_ICONEXCLAMATION | MB_OK);
			return false;
		}

		return true;
	}

	bool DXRenderer::CreateBlendStates()
	{
		D3D11_BLEND_DESC blendStateDesc;
		ZeroMemory(&blendStateDesc, sizeof(D3D11_BLEND_DESC));
		blendStateDesc.RenderTarget[0].BlendEnable = TRUE;
		// Source blend params
		blendStateDesc.RenderTarget[0].SrcBlend = D3D11_BLEND_SRC_ALPHA;
		blendStateDesc.RenderTarget[0].SrcBlendAlpha = D3D11_BLEND_ONE;
		// Blend operations
		blendStateDesc.RenderTarget[0].BlendOp = D3D11_BLEND_OP_ADD;
		blendStateDesc.RenderTarget[0].BlendOpAlpha = D3D11_BLEND_OP_ADD;
		// Destination blend params
		blendStateDesc.RenderTarget[0].DestBlend = D3D11_BLEND_INV_SRC_ALPHA;
		blendStateDesc.RenderTarget[0].DestBlendAlpha = D3D11_BLEND_ZERO;
		// Mask
		blendStateDesc.RenderTarget[0].RenderTargetWriteMask = D3D11_COLOR_WRITE_ENABLE_ALL;
		// Create and set state
		s_pDevice->CreateBlendState(&blendStateDesc, &s_pBlendState);
		s_pDeviceContext->OMSetBlendState(s_pBlendState, nullptr, 0x0F);
		return true;
	}

	bool DXRenderer::SetUpRasterizerState()
	{
		ZeroMemory(&s_RasterizerDesc, sizeof(D3D11_RASTERIZER_DESC));
		s_RasterizerDesc.FillMode = D3D11_FILL_MODE::D3D11_FILL_SOLID;
		s_RasterizerDesc.CullMode = D3D11_CULL_MODE::D3D11_CULL_NONE;
		s_RasterizerDesc.MultisampleEnable = TRUE;
		s_RasterizerDesc.DepthClipEnable = TRUE;
		s_pDevice->CreateRasterizerState(&s_RasterizerDesc, &s_pRasteriserState);
		s_pDeviceContext->RSSetState(s_pRasteriserState);
		return true;
	}


}