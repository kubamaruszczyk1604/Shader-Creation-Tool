#if CSharp

namespace ShaderCreationTool
{
#endif


enum
#if __cplusplus

        class
#endif
        ShaderVariableType
    {
        Vector4,
        Vector3,
        Vector2,
        Single,
        Texture2D
    };

public delegate void OnWndProcUpdate();

#if CSharp
}
#endif
