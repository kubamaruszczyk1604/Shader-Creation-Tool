
//#if CSharp
//using System.Windows.Forms;
//namespace ShaderCreationTool
//{
//#endif

#if __cplusplus
 using SUINT = System::UInt64;
#else 
using SUINT = System.UInt64;
public
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
public delegate void OnWndProcMessage(SUINT message, SUINT wParam, SUINT lParam);

//#if CSharp
//}
//#endif
