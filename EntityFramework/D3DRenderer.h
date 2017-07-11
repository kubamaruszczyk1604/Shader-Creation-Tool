#pragma once
#include "StdIncludes.h"
#include "Camera.h"
#include "RendererModes.h"
#include "LightBase.h"

namespace KLM_FRAMEWORK
{
	struct UniformBufferStructure
	{
		Mat4 ModelViewProjectionMatrix;
		Mat4 InvTranspWorldMatrix;
		Mat4 WorldMatrix;
		Vec4 Ambient;
		Vec4 Diffuse;
		Vec4 Specular;
		Vec4 Time__NumLights__AlphaPart;
		Vec4 CameraPosition;
	};

	class Entity;
	class DXRenderer 
	{

	private:
		static int s_ScreenHeight;
		static int s_ScreenWidth;
		static bool s_DepthStencilEnabled;
		static int s_Samples;
		static bool s_WireframeGreen;

		//GlobalMatrices
		static Mat4 s_IdentityMatrix;

		static HWND m_hWnd;
		static ID3D11Device*             s_pDevice;
		static ID3D11DeviceContext*      s_pDeviceContext;
		static IDXGISwapChain*           s_pSwapChain;
		static ID3D11RenderTargetView*   s_pRenderTargetView;
		static ID3D11Buffer*             s_pUniformBuffer;
		static ID3D11DepthStencilState*  s_pDepthStencilState;    // TO DO
		static ID3D11DepthStencilView*   s_pDepthStencilView;
		static ID3D11Texture2D*          s_pDepthStencilBuffer;
		static ID3D11RasterizerState*    s_pRasteriserState;      // TO DO
		static ID3D11BlendState*         s_pBlendState;
		static ID3D11BlendState*         s_pNoBlendState; // TO DO
		static ID3D11SamplerState*       s_pSamplerState;

		static D3D11_RASTERIZER_DESC     s_RasterizerDesc;
		static D3D_DRIVER_TYPE           s_DriverType;
		static D3D_FEATURE_LEVEL         s_FeatureLevel;
		static D3D11_VIEWPORT            s_Viewport;

		static ID3D11ShaderResourceView* s_Srv;
		static ID3D11Buffer* s_LightBuffer;


		static Camera* s_CurrentCamera;


	private:

		static void CreateUniformBuffer(const UINT size, void* const data, const bool dynamic, const bool CPUupdates, ID3D11Buffer** output);
		static void UpdateUniformBuffer(ID3D11Buffer* buffer, const UINT dataSize, void* const data, const UINT slotInSHADER);
		void UpdateUniformBuffer(const UniformBufferStructure& buffer) const;
		static void UpdateDynamicBuffer(ID3D11Buffer* buffer, const UINT dataSize, void* const data);

		static HRESULT CreateStructuredBuffer(
			const UINT                  iNumElements,
			const bool                  isCpuWritable,
			const bool                  isGpuWritable,
			ID3D11Buffer**              ppBuffer,
			ID3D11ShaderResourceView**  ppSRV,
			ID3D11UnorderedAccessView** ppUAV,
			void*                    pInitialData,
			UINT data_size);

		//Specific
		static void CreateLightBuffer();




		//void UpdateUniformBuffer(const UniformBufferStructure& buffer);
		static bool CreateDepthBuffer();
		static bool CreateBlendStates();
		static bool SetUpRasterizerState();

	public:

		DXRenderer() = delete;
		DXRenderer(const DXRenderer&) = delete;
		DXRenderer& operator=(const DXRenderer&) = delete;

		static bool Initialize(const int width, const int height, const HWND handle);
		static void Render(Entity* entity);
		static void Update(const float deltaTime, const float totalTime);
		static void ClearScreen(const Colour& colour);
		static void SwapBuffers() { s_pSwapChain->Present(0, 0); }
		static void SetCullMode(const CullMode mode);
		static void SetFillMode(const FillMode mode);
		static ID3D11Device* GetDevice() { return s_pDevice; }
		static ID3D11DeviceContext* GetDeviceContext() { return s_pDeviceContext; }

		static void SetActiveCamera(Camera* camera) { s_CurrentCamera = camera; }




	};

}