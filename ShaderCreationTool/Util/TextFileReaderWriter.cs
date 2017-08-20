using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShaderCreationTool
{
    static class TextFileReaderWriter
    {
        private static string s_LastError;
        public static string LastError { get { return s_LastError; } }

        static public bool Save(string path, string content)
        {
            try
            {
                StreamWriter writer = new StreamWriter(path);
                writer.Write(content);
                writer.Flush();
                writer.Close();
            }
            catch(Exception e)
            {
                s_LastError = e.Message;
                return false;
            }
            return true;
        }




    }
}
