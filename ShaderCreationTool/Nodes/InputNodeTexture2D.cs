﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ShaderCreationTool
{
    class InputNodeTexture2D : ISCTNode, IInputNode, IDisposable
    {
        private const string s_PlainTexturePath = "../Assets/Textures/bkg.bmp";

        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private List<Connector> m_InputConnectors;
        private string m_ImgPath = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;
        private NodeInputError p_ErrorCallback;
        private TextBox m_NameTextbox;
        private TextBox m_FileTextbox;
        private Panel m_ImagePanel;
        private string m_Name;
        ShaderTextureVariable m_ShaderVariable;
        private static bool s_ButtonsLocked = false;
        private static int s_InstanceCounter = 0;

        //Remember last choice of path for user convenience
        private static string s_LastTexturePath = string.Empty;


        public InputNodeTexture2D(Panel nodeTemplate, Point location)
        {

            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

            m_OutputConnectors = new List<Connector>();
            m_InputConnectors = new List<Connector>();
            int tbCounter = 0;

            List<Control> allControlls = ControlExtensions.GetAllChildreenControls<Control>(m_SctElement).Cast<Control>().ToList();
            foreach (Control control in allControlls)
            {
                if (control.Name.Equals("")) continue;

                control.Click += AnyElement_Click;

                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    if (checkBox.Name.Contains(Connector.s_OutSlotSequenceID))
                    {
                        Connector tempCon = new Connector(checkBox, ShaderVariableType.Vector4, this);
                        m_OutputConnectors.Add(tempCon);
                    }
                    else if (checkBox.Name.Contains(Connector.s_InSlotSequenceID))
                    {
                        Connector tempCon = new Connector(checkBox, ShaderVariableType.Vector2, this);
                        m_InputConnectors.Add(tempCon);
                    }
                }

                else if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    if (textBox.Name.Contains("Name"))
                    {
                        SCTConsole.Instance.PrintLine("Name is:" + textBox.Name);
                        textBox.Validated += TextBox_LostFocus;
                        textBox.KeyPress += TextBox_KeyPress;
                        m_NameTextbox = textBox;
                        m_Name = "SCT_TEXTURE2D_" + s_InstanceCounter.ToString();
                        m_NameTextbox.Text = m_Name;
                        tbCounter++;
                    }
                    else if(textBox.Name.Contains("File"))
                    {
                        m_FileTextbox = textBox;
                        m_FileTextbox.MouseDoubleClick += FileTextBox_MouseDoubleClick;
                        tbCounter++;
                    }
                }
                else if (control is Panel)
                {
                    Panel p = (Panel)control;
                    if (p.Name.Contains("Image"))
                    {
                        m_ImagePanel = p;
                        m_ImagePanel.DoubleClick += ImagePanel_Click;
                    }
                }
                else if (control is Label)
                {
                    Label l = (Label)control;
                    if (l.Name.Contains("Title"))
                    {
                        l.MouseDown += TitleLabel_MouseDown;
                        l.MouseMove += TitleLabel_MouseMove;
                    }
                }
                else if (control is Button)
                {
                    Button button = (Button)control;
                    button.Click += CloseButton_Click;
                }
            }
            if (tbCounter < 2)
            {
                SCTConsole.Instance.PrintLine("ERROR: No Texboxes in Input TEXTURE Node");
                throw new Exception("ERROR: No Texboxes in Input TEXTURE  Node");
            }

            s_InstanceCounter++;
            ShowNode(1);

            m_ShaderVariable = new ShaderTextureVariable(s_PlainTexturePath, m_Name);
            Bridge.SetVariable(m_ShaderVariable);
        }

        static public void LockButtons()
        {
            s_ButtonsLocked = true;
        }

        static public void UnlockButtons()
        {
            s_ButtonsLocked = false;
        }
        ////////////////////////  ADD CALLBACK METHODS ///////////////

        public void AddOnMovedCallback(ObjectMovedCallback onMovedCallback)
        {
            m_Mover.AddObjectMovedEventListener(onMovedCallback);
        }

        public void AddOnBeginConnectionCallback(BeginConnectionCallback onBeginConnection)
        {
            foreach (Connector c in m_OutputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
        }

        public void AddOnBreakConnectionCallback(BreakConnectionCallback onBreakConnection)
        {
            foreach (Connector c in m_OutputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
        }

        public void AddOnCloseCallback(NodeCloseButtonCallback callback)
        {
            p_CloseCallback += callback;
        }

        public void AddInputErrorCallback(NodeInputError callback)
        {
            p_ErrorCallback += callback;
        }

        public Connector GetConnector(ConnectionDirection type, int index)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors[index];
            else if (type == ConnectionDirection.In) return m_InputConnectors[index];
            else return null;
        }

        public List<Connector> GetAllConnectors(ConnectionDirection type)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors;
            else if (type == ConnectionDirection.In) return m_InputConnectors;
            else return null;
        }

        public List<Connector> GetAllConnectors()
        {
            List<Connector> outList = new List<Connector>();
            outList.AddRange(m_OutputConnectors);
            outList.AddRange(m_InputConnectors);
            return outList;
        }


        //////////////////// PUBLIC UTIL METHODS /////////////////////////


        public void Dispose()
        {
            List<Connector> connectors = GetAllConnectors();
            foreach (Connector c in connectors)
            {
                if (!c.Connected) continue;
                if (!ConnectionManager.ContainsConncetion(c.ParentConnection)) continue;
                ConnectionManager.RemoveConnection(c.ParentConnection);
            }

            m_SctElement.Parent.Controls.Remove(m_SctElement);
            //new code
            foreach (Control c in m_SctElement.Controls)
            {
                c.Dispose();
            }
        }

        ////// UTIL FOR ASYNC
        private async void ShowNode(int delay)
        {
            await Task.Delay(delay);
            m_SctElement.Visible = true;
            m_SctElement.Update();
        }

        ////////////////// UI EVENTS ////////////////
        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (s_ButtonsLocked) return;
            if (p_CloseCallback != null)
            {
                p_CloseCallback(this);
            }

        }

        private void AnyElement_Click(object sender, EventArgs e)
        {
            m_SctElement.SuspendLayout();
            m_SctElement.BringToFront();
            ((Control)sender).Focus();
            m_SctElement.Update();
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            m_SctElement.BringToFront();
            m_SctElement.Focus();
        }


        private void TitleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            m_Mover.MoveControlMouseCapture(m_SctElement, e);
        }

        private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            m_Mover.MoveControlMouseMove(m_SctElement, e);
        }


        private void ImagePanel_Click(object sender, EventArgs e)
        {
            string pathStr = (m_ImgPath == string.Empty) ? "-- no file selected yet --" : m_ImgPath;
            ImagePreviewForm cd = new ImagePreviewForm(m_ImagePanel.BackgroundImage,m_Name,pathStr);
            DialogResult result = cd.ShowDialog();
            if (result == DialogResult.OK)
            {     
              

            }
            cd.Dispose();

        }
        private void FileTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            if (s_LastTexturePath == string.Empty)
            {
                ofd.InitialDirectory = @"C:\";
            }
            else
            {
                ofd.InitialDirectory = s_LastTexturePath;
            }
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                SCTConsole.Instance.PrintLine("Opening Texture file: " + ofd.FileName);
                try
                {
                    m_ImagePanel.BackgroundImage = Image.FromFile(ofd.FileName);
                    m_FileTextbox.Text = ofd.FileName;
                    m_ImgPath = ofd.FileName;
                    s_LastTexturePath = ofd.FileName;
                    this.TextureChanged(m_ImgPath);
                    m_ShaderVariable.SetPath(m_ImgPath);
                }
                catch(Exception ex)
                {
                    SCTConsole.Instance.PrintLine(ex.Message);
                    p_ErrorCallback("Texture: "+ ofd.FileName + " has incorrect format\n",this);
                }
            }
            ofd.Dispose();
        }

        ///////////////////   TEXTURE FILE CHANGED //////////////////////

        private void TextureChanged(string fileName)
        {
            SCTConsole.Instance.PrintLine("Texture change detected!\n New file:" + fileName);
        } 

        //////////////////// VARIABLE NAME HANDLING  ////////////////////
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            TextChanged(((TextBox)sender).Text);
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                TextChanged(((TextBox)sender).Text);
            }
        }

        private void TextChanged(string newText)
        {
            if (m_Name == newText) return;

            // Check for white characters error
            if (newText.Contains(" "))
            {
                // error - spaces
                SCTConsole.Instance.PrintLine("Error: White spaces not allowed in varable name!");
                m_NameTextbox.Text = m_Name; // use old name
                m_NameTextbox.Invalidate();
                if (p_ErrorCallback != null)
                {
                    p_ErrorCallback("Invalid Input: White Spaces not allowed!", this);
                }
                return;
            }

            //Check for empty string error
            if (newText == string.Empty)
            {
                SCTConsole.Instance.PrintLine("Error: Variable name must contain characters!");
                m_NameTextbox.Text = m_Name;
                m_NameTextbox.Invalidate();
                if (p_ErrorCallback != null)
                {
                    p_ErrorCallback("Invalid Input: Empty varaiable names are not allowed!", this);
                }
                return;
            }

            // Check for illegal symbols error
            var regexItem = new Regex("^[a-zA-Z0-9_]*$");
            if (!regexItem.IsMatch(newText))
            {
                // error - illegal signs
                SCTConsole.Instance.PrintLine("Error: Symbols not allowed in varable name!");
                if (p_ErrorCallback != null)
                {
                    m_NameTextbox.Text = m_Name;
                    m_NameTextbox.Invalidate();
                    p_ErrorCallback("Invalid Input: Symbols other than underscore ('_') are not allowed!", this);
                }
                return;
            }

            m_Name = newText;

            m_ShaderVariable.SetName(m_Name);
            SCTConsole.Instance.PrintLine("TEXT CHANGED to: " + newText);
   
        }

        

    }
}
