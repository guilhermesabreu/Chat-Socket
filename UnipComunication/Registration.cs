using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnipComunication
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "pql6GYRVaYbSsgpMEomUlQUhx6KgMCaFgxP4BVCi",
            BasePath = "https://theusers-8f35b.firebaseio.com/"
        };

        IFirebaseClient client;

        private void Registration_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);

            }
            catch
            {
                MessageBox.Show("No Internet or Connection Problem");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region[Condition]
            if (string.IsNullOrWhiteSpace(txtUsername.Text) &&
               string.IsNullOrWhiteSpace(pswUser.Text))
            {
                MessageBox.Show("Please Fill All the Fields");
                return;
            }
            #endregion

            MyUser user = new MyUser()
            {
                Username = txtUsername.Text,
                Password = pswUser.Text
            };

            SetResponse set = client.Set(@"Users/"+txtUsername.Text,user);

            MessageBox.Show("Succefull registred !!!");
        }
    }
}
