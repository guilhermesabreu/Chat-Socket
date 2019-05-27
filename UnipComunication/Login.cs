using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace UnipComunication
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "pql6GYRVaYbSsgpMEomUlQUhx6KgMCaFgxP4BVCi", 
            BasePath = "https://theusers-8f35b.firebaseio.com/"
        };

        IFirebaseClient client;

        private void Login_Load(object sender, EventArgs e)
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

            FirebaseResponse res = client.Get(@"Users/" + txtUsername.Text);
            MyUser ResUser = res.ResultAs<MyUser>();

            MyUser CurUser = new MyUser()
            {
                Username = txtUsername.Text,
                Password = pswUser.Text
            };

            if (MyUser.IsEqual(ResUser, CurUser))
            {
                Friends friends = new Friends();
                friends.Show();
                this.Hide();
            }
            else
            {
                MyUser.ShowError();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Registration rg = new Registration();
            rg.Show();
            this.Hide();
        }
    }
}
