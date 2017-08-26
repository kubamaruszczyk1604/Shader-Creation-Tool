using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    static class AttribVariableStrings
    {
        // simple
        public static readonly string TIME_VARIABLE_NAME = "UnifTime";
        public static readonly string UV_VARIABLE_NAME = "UVs";


        // with space selection
        public static readonly string[] POSITION_VARIABLE_NAMES_ARRAY =  {
            "Position_WorldSpace",
            "Position_ObjectSpace",
            "Position_EyeSpace"
        };
        public static readonly string[] NORMAL_VARIABLE_NAMES_ARRAY = { "Normal_InverseWorld", "Normal_ObjectScpace" };
        public static readonly string[] CAMERA_POS_VARIABLE_NAMES_ARRAY = { "Camera_WorldSpace" };

        public static readonly string WORLD_MAT_VARIABLE_NAME = "WORLD";
        public static readonly string INVERSE_WORLD_MAT_VARIABLE_NAME = "WORLD_INVERSE";
        public static readonly string VIEW_MAT_VARIABLE_NAME = "VIEW";
        public static readonly string MVP_MAT_VARIABLE_NAME = "MVP";
    }
}
