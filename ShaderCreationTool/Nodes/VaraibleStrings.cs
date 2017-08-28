
namespace ShaderCreationTool
{
    static class AttribVariableStrings
    {
        //******************************  VERTEX OUTPUT VARIABLES  *********************//
        // PER-VERTEX VARIABLES
        public static readonly string O_UV_VARIABLE_NAME = "oUVs";
        public static readonly string[] O_POSITION_VAR_NAMES =  {
            "oPosition_WorldSpace",
            "oPosition_ObjectSpace",
            "oPosition_EyeSpace"
        };

        public static readonly string[] O_NORMAL_VAR_NAMES = { "oNormal_InvWorldSpace", "oNormal_ObjectScpace" };


        // STANDARD UNIFORM VARIABLES
        public static readonly string O_TIME_VARIABLE_NAME = "oTime";
        public static readonly string[] O_CAMERA_POS_VAR_NAMES = { "oCamera_WorldSpace" };



        //******************************  VERTEX INPUT VARIABLES  *********************//

        
        public static readonly string U_CAMERA_POSITION_VAR_NAME = "uCameraPosition";
        public static readonly string U_TIME_VAR_NAME = "uTime";

        public static readonly string U_WORLD_MAT_VAR_NAME = "uWORLD";
        public static readonly string U_INVERSE_WORLD_MAT_VAR_NAME = "uWORLD_INVERSE";
        public static readonly string U_VIEW_MAT_VAR_NAME = "uVIEW";
        public static readonly string U_MVP_MAT_VAR_NAME = "uMVP";


        public static readonly string IN_POSITION_VAR_STR = "vertex_position";
        public static readonly string IN_NORMAL_VAR_STR = "vertex_normal";
        public static readonly string IN_TANGENT_VAR_STR = "tangent";
        public static readonly string IN_UVS_VAR_STR = "uvs";

        public static readonly string SHADER_VERSION_STR = "#version 330";

    }
}
