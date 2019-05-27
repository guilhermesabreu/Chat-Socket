using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnipComunication.BD
{
    public class ConnBD
    {
        public void Conn()
        {
            string connetionString = null;
            MySqlConnection cnn;
            connetionString = "server=den1.mysql2.gear.host;database=userscompanychat;uid=userscompanychat;pwd=Ha744L!!ne91;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
                MessageBox.Show("Connection Open ! ");
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }
    }
}
