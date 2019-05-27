using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnipComunication.BD
{
    public class Insert
    {

        public void Salvar(string _name,string _ip, int _port)
        {
            try
            {
                MySqlConnection mConn;
                mConn = new MySqlConnection("server=den1.mysql2.gear.host;database=userscompanychat;uid=userscompanychat;pwd=Ha744L!!ne91;");
                // Abre a conexão
                mConn.Open();
                //Query SQL
                MySqlCommand command = new MySqlCommand("INSERT INTO users (name_users,ip_users,port_users)" +
                "VALUES('" + _name + "','" + _ip + "', '" + _port + "')", mConn);
                //Executa a Query SQL
                command.ExecuteNonQuery();
                // Fecha a conexão
                mConn.Close();
                //Mensagem de Sucesso
                MessageBox.Show("Gravado com Sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }  
    }
}
