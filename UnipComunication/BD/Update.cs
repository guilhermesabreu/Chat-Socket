using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnipComunication.BD
{
    class Update
    {
        public void updateUsers(string _name, string _ip, string _port)
        {
            MySqlConnection con;
            try
            {
                String sql = "UPDATE users SET name_users= @name_users ,ip_users=@ip_users ,port_users=@port_users WHERE name_users = @name_users";
                con = new MySqlConnection("server=den1.mysql2.gear.host;database=userscompanychat;uid=userscompanychat;pwd=Ha744L!!ne91;");
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name_users", _name);
                cmd.Parameters.AddWithValue("@ip_users", _ip);
                cmd.Parameters.AddWithValue("@port_users", _port);
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
