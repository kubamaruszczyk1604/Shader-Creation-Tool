﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

using System.Runtime.InteropServices;


namespace ShaderCreationTool
{
    public static class ControlExtensions
    {
        public static T ShallowCopy<T>(this T controlToClone)
      where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;
        }

        public static T CopyAsSCTElement<T>(this T sourceInstance, bool enableAllChildreen)
        where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            T targetInstance = Activator.CreateInstance<T>();
         
            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(targetInstance, propInfo.GetValue(sourceInstance, null), null);
                }
            }
          
            foreach (Control control in sourceInstance.Controls)
            {
                if (control is Label)
                {
                    Label l = ((Label)control).CopyAsSCTElement(enableAllChildreen);
                    l.Parent = targetInstance;
                    l.Enabled = enableAllChildreen;
                    targetInstance.Controls.Add(l);
                }
                else if (control is Panel)
                {
                    Panel p = ((Panel)control).CopyAsSCTElement(enableAllChildreen);
                    p.Parent = targetInstance;
                    p.Enabled = enableAllChildreen;
                    targetInstance.Controls.Add(p);
                }
                else if (control is CheckBox)
                {
                    CheckBox cb = ((CheckBox)control).CopyAsSCTElement(enableAllChildreen);
                    cb.Parent = targetInstance;
                    cb.Enabled = enableAllChildreen;
                    targetInstance.Controls.Add(cb);
                }
                else if (control is Button)
                {
                    Button btn = ((Button)control).CopyAsSCTElement(enableAllChildreen);
                    btn.Parent = targetInstance;
                    btn.Enabled = enableAllChildreen;
                    targetInstance.Controls.Add(btn);
                }
                else if (control is ComboBox)
                {
                    ComboBox com = ((ComboBox)control).CopyAsSCTElement(enableAllChildreen);
                    com.Parent = targetInstance;
                    com.Enabled = enableAllChildreen;
                    targetInstance.Controls.Add(com);
                }
                else if (control is NumericUpDown)
                {
                    NumericUpDown n = ((NumericUpDown)control).CopyAsSCTElement(enableAllChildreen);
                    n.Parent = targetInstance;
                    n.Enabled = enableAllChildreen;
                    targetInstance.Controls.Add(n);
                }
                else if (control is TextBox)
                {
                    TextBox txtBox = ((TextBox)control).CopyAsSCTElement(enableAllChildreen);
                    txtBox.Parent = targetInstance;
                    txtBox.Enabled = enableAllChildreen;
                    targetInstance.Controls.Add(txtBox);
                }
            }
            targetInstance.Enabled = enableAllChildreen;
            return targetInstance;
        }




       public static List<Control> GetAllChildreenControls<T>(Control control)
            where T :Control
        {
            List<Control> outputList = new List<Control>();
            foreach (Control child in control.Controls)
            {
                if (child.Name.Equals("")) continue;
                if(child is T)
                {
                    outputList.Add(child);
                }
                outputList.AddRange(GetAllChildreenControls<T>(child));
            }
           return outputList;
        }

    }

    

}
