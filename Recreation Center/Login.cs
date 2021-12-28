using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recreation_Center
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            string User, Password;
            User = Usertxt.Text;
            Password = Passtxt.Text;
                if (User == "" && Password == "")
                {
                    MessageBox.Show("Username or Password field empty !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    foreach (Control c in Controls)
                    {
                        if (c is TextBox)
                        {
                            c.Text = "";
                        }
                    }
                }
                else if (User == "admin" && Password == "admin")
                {
                    Admin af = new Admin();
                    af.Show();
                    this.Hide();
                }
                else if (User == "staff" && Password == "staff")
                {
                    Staff sf = new Staff();
                    sf.Show();
                    this.Hide();
                }
                else
                {
                     MessageBox.Show("Invalid Input!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    foreach (Control c in Controls)
                    {
                        if (c is TextBox)
                        {
                             c.Text = "";
                        }
                    }
                }
        }
        protected override void OnClosed(EventArgs e)
        {
            MessageBox.Show("Recreation ceneter form closing!.",
                "Close Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            base.OnClosed(e);
            Application.Exit();
        }
    }
}
