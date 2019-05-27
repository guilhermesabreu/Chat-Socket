using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnipComunication.BD;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace UnipComunication
{
    public partial class Friends : Form
    {
        public string NomeUsuario = "Desconhecido";
        public StreamWriter stwEnviador;
        public StreamReader strReceptor;
        public TcpClient tcpServidor;
        // Necessário para atualizar o formulário com mensagens da outra thread
        public delegate void AtualizaLogCallBack(string strMensagem);
        // Necessário para definir o formulário para o estado "disconnected" de outra thread
        public delegate void FechaConexaoCallBack(string strMotivo);
        public Thread mensagemThread;
        public IPAddress enderecoIP;
        public bool Conectado;


        public Friends()
        {
            InitializeComponent();

            listRegisters.FullRowSelect = true;
        }


        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login backToLogin = new Login();
            backToLogin.Show();
            this.Hide();
        }

        private void Friends_Load(object sender, EventArgs e)
        {

            MySqlConnection conn = new MySqlConnection("server=den1.mysql2.gear.host;database=userscompanychat;uid=userscompanychat;pwd=Ha744L!!ne91;");
            conn.Open();

            string sql = "SELECT * FROM users";
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = command.ExecuteReader();

            listRegisters.Items.Clear();

            while(reader.Read())
            {
                ListViewItem newList = new ListViewItem(reader.GetString(0).ToString());
                newList.SubItems.Add(reader.GetString(1).ToString());
                newList.SubItems.Add(reader.GetInt32(2).ToString());
        
                listRegisters.Items.Add(newList);
            }
            reader.Close();
            conn.Dispose();
            conn.Close();
        }

        private void connect_Click(object sender, EventArgs e)
        {
                Main lTelaMain = new Main(txtName.Text, txtIp.Text);

                lTelaMain.Port = txtPort.Text;
           
                lTelaMain.Show();
                this.Hide();
        }
            
        private void deleteList()
        {
            if (MessageBox.Show("Sure ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                listRegisters.Items.RemoveAt(listRegisters.SelectedIndices[0]);

                ///Limpando os campos digitados após deletado 
                txtName.Text = "";
                txtIp.Text = "";
                txtPort.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Delete delete = new Delete();
            delete.Deletar(txtName.Text);

            deleteList();
        }

        private void update()
        {
            if (MessageBox.Show("Sure ??", "UPDATE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                listRegisters.SelectedItems[0].SubItems[0].Text = txtName.Text;
                listRegisters.SelectedItems[0].SubItems[1].Text = txtIp.Text;
                listRegisters.SelectedItems[0].SubItems[2].Text = txtPort.Text;

                ///Limpando os campos digitados após atualizado  
                txtName.Text = "";
                txtIp.Text = "";
                txtPort.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Update lUpdate = new Update();
            lUpdate.updateUsers(txtName.Text,txtIp.Text,txtPort.Text);
            update();
        }

        private void add(String name, String ip, String port)
        {
            string[] row = { name, ip, port };

            ListViewItem item = new ListViewItem();

            listRegisters.Items.Add(item);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ConnBD connBD = new ConnBD();
            connBD.Conn();
            Insert insert = new Insert();
            add(txtName.Text, txtIp.Text, txtPort.Text);
            insert.Salvar(txtName.Text, txtIp.Text, Convert.ToInt32(txtPort.Text));

            ///Limpando os campos digitados após adcionado  
            txtName.Text = "";
            txtIp.Text = "";
            txtPort.Text = "";
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
           txtName.Text = listRegisters.SelectedItems[0].SubItems[0].Text;
           txtIp.Text = listRegisters.SelectedItems[0].SubItems[1].Text;
           txtPort.Text = listRegisters.SelectedItems[0].SubItems[2].Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("server=den1.mysql2.gear.host;database=userscompanychat;uid=userscompanychat;pwd=Ha744L!!ne91;");
            conn.Open();

            string sql = "SELECT * FROM users";
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = command.ExecuteReader();

            listRegisters.Items.Clear();

            while (reader.Read())
            {
                ListViewItem newList = new ListViewItem(reader.GetString(0).ToString());
                newList.SubItems.Add(reader.GetString(1).ToString());
                newList.SubItems.Add(reader.GetInt32(2).ToString());

                listRegisters.Items.Add(newList);
            }
            reader.Close();
            conn.Dispose();
            conn.Close();
        }
    }
}
