using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;



namespace UnipComunication
{
    public partial class Main : Form
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
        
        public Main()
        {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();
        }

        public Main(string prNome,string prIp)
        {
            InitializeComponent();
            txtServidorIP.Text = prIp;
            txtUsuario.Text = prNome; 
        }
        public string Port{ get; set; }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
          
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            EnviaMensagem();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            txtMensagem.Text = "";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Friends friends = new Friends();
            friends.Show();
            this.Hide();
        }

        private void conversar_Click(object sender, EventArgs e)
        {
            // se não esta conectando aguarda a conexão
            if (Conectado == false)
            {
                // Inicializa a conexão
                InicializaConexao();
            }
            else // Se esta conectado entao desconecta
            {
                FechaConexao("Desconectado a pedido do usuário.");
            }
        }

        private void InicializaConexao()
        {
            try
            {
                // Trata o endereço IP informado em um objeto IPAdress
                enderecoIP = IPAddress.Parse(txtServidorIP.Text);
                // Inicia uma nova conexão TCP com o servidor chat
                tcpServidor = new TcpClient();
                tcpServidor.Connect(enderecoIP, 2502);

                // AJuda a verificar se estamos conectados ou não
                Conectado = true;

                // Prepara o formulário
                NomeUsuario = txtUsuario.Text;

                // Desabilita e habilita os campos apropriados
                txtServidorIP.Enabled = false;
                txtUsuario.Enabled = false;
                txtMensagem.Enabled = true;
                btnConectar.Text = "Desconectado";

                // Envia o nome do usuário ao servidor
                stwEnviador = new StreamWriter(tcpServidor.GetStream());
                stwEnviador.WriteLine(txtUsuario.Text);
                stwEnviador.Flush();

                //Inicia a thread para receber mensagens e nova comunicação
                mensagemThread = new Thread(new ThreadStart(RecebeMensagens));
                mensagemThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message, "Erro na conexão com servidor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtMensagem_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se pressionou a tecla Enter
            if (e.KeyChar == (char)13)
            {
                EnviaMensagem();
            }
        }

        // Envia a mensagem para o servidor
        private void EnviaMensagem()
        {
            if (txtMensagem.Lines.Length >= 1)
            {
                stwEnviador.WriteLine(txtMensagem.Text);
                stwEnviador.Flush();
                txtMensagem.Lines = null;
            }
            txtMensagem.Text = "";
        }

        // Fecha a conexão com o servidor
        private void FechaConexao(string Motivo)
        {
            // Mostra o motivo porque a conexão encerrou
            txtLog.AppendText(Motivo + "\r\n");
            // Habilita e desabilita os controles apropriados no formulario
            txtServidorIP.Enabled = true;
            txtUsuario.Enabled = true;
            txtMensagem.Enabled = false;
            btnConectar.Text = "Conectado";

            // Fecha os objetos
            Conectado = false;
            stwEnviador.Close();
            strReceptor.Close();
            tcpServidor.Close();
        }

        private void RecebeMensagens()
        {
            // recebe a resposta do servidor
            strReceptor = new StreamReader(tcpServidor.GetStream());
            string ConResposta = strReceptor.ReadLine();
            // Se o primeiro caracater da resposta é 1 a conexão foi feita com sucesso
            if (ConResposta[0] == '1')
            {
                // Atualiza o formulário para informar que esta conectado
                this.Invoke(new AtualizaLogCallBack(this.AtualizaLog), new object[] { "Conectado com sucesso!" });
            }
            else // Se o primeiro caractere não for 1 a conexão falhou
            {
                string Motivo = "Não Conectado: ";
                // Extrai o motivo da mensagem resposta. O motivo começa no 3o caractere
                Motivo += ConResposta.Substring(2, ConResposta.Length - 2);
                // Atualiza o formulário como o motivo da falha na conexão
                this.Invoke(new FechaConexaoCallBack(this.FechaConexao), new object[] { Motivo });
                // Sai do método
                return;
            }

            // Enquanto estiver conectado le as linhas que estão chegando do servidor
            while (Conectado)
            {
                // exibe mensagems no Textbox
                this.Invoke(new AtualizaLogCallBack(this.AtualizaLog), new object[] { strReceptor.ReadLine() });
            }
        }

        private void AtualizaLog(string strMensagem)
        {
            // Anexa texto ao final de cada linha
            txtLog.AppendText(strMensagem + "\r\n");
        }

        // O tratador de evento para a saida da aplicação
        public void OnApplicationExit(object sender, EventArgs e)
        {
            if (Conectado == true)
            {
                // Fecha as conexões, streams, etc...
                Conectado = false;
                stwEnviador.Close();
                strReceptor.Close();
                tcpServidor.Close();
            }
        }
    }
}
