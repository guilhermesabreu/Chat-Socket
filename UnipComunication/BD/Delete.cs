using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnipComunication.BD
{
    class Delete
    {
        public void Deletar(string _name)
        {
            MySqlConnection con;
            try
            {
                con = new MySqlConnection("server=den1.mysql2.gear.host;database=userscompanychat;uid=userscompanychat;pwd=Ha744L!!ne91;");

                string sql = "DELETE FROM users WHERE name_users = '"+ _name+"'";
      
                MySqlCommand cmd = new MySqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
                con.Close();
            
        }
    }
}
