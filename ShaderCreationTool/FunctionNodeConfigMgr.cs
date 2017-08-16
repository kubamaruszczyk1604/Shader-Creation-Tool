using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    static class FunctionNodeConfigMgr
    {

        static private List<FunctionNodeDescription> s_DescriptionList = new List<FunctionNodeDescription>();
        static private List<string> s_TitlesList = new List<string>();
        static private string s_Status = string.Empty;

        static public bool ReadNodesFromFile(string path)
        {

           bool success = ReaderXML.ReadInDescriptions(path, out s_DescriptionList, out s_Status);
           if(success) s_TitlesList = ReaderXML.GetNamesList(s_DescriptionList).ToList();
           return success;
        }

        static public bool AppendNodesFromFile(string path)
        {
            List<FunctionNodeDescription> temp;
            bool success = ReaderXML.ReadInDescriptions(path, out temp, out s_Status);
            s_DescriptionList.AddRange(temp);
            if (success) s_TitlesList = ReaderXML.GetNamesList(s_DescriptionList).ToList();
            return success;
        }

        static public bool ReadNodesFromFiles(string [] paths)
        {
            s_DescriptionList.Clear();
            s_TitlesList.Clear();
            s_Status = string.Empty;
            bool success = true;
            foreach (string path in paths)
            {
                string tempStatus;
                List<FunctionNodeDescription> temp;
                if(ReaderXML.ReadInDescriptions(path, out temp, out tempStatus))
                {
                    s_DescriptionList.AddRange(temp);
                    s_Status += tempStatus;
                }
                else
                {
                    success = false;
                    s_Status += tempStatus;
                }   
            
            }

            s_TitlesList = ReaderXML.GetNamesList(s_DescriptionList).ToList();
            return success;
        }

        static public string [] GetNodesList()
        {
            return ReaderXML.GetNamesList(s_DescriptionList);
        }

        static public List<string> NodeList { get { return s_TitlesList; } }

        static public string LastStatus { get { return s_Status; } }

        static public FunctionNodeDescription GetFunctionNodeDescription(int index)
        {
            return s_DescriptionList[index];
        }
        
    }
}
