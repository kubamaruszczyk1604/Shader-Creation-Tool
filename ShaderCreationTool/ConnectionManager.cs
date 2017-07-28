using System;
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


namespace ShaderCreationTool
{
    static class ConnectionManager
    {

        static List<Connection> s_ConnectionList = new List<Connection>();

        static public void Draw(Graphics graphics)
        {
            foreach (Connection c in s_ConnectionList)
            {
                c.Draw(graphics);
            }
        }

        static public void UpdateOnObjectMoved()
        {
            foreach (Connection c in s_ConnectionList)
            {
                c.UpdateOnObjectMoved();
            }
        }

        static public void AddConnecion(Connection connection)
        {
            s_ConnectionList.Add(connection);
        }

        static public void RemoveConnection(Connection connection)
        {
            s_ConnectionList.Remove(connection);
        }

        static public Connection GetConnection(int index)
        {
            return s_ConnectionList[index];
        }

    }
}
