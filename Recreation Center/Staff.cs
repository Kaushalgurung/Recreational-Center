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
    public partial class Staff : Form
        
    {
        int visitorID;
        public Staff()
        {
            InitializeComponent();
            label7.Visible = false;
            checkout.Visible = false;
        }


        private void StaffLogoutbtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login lf = new Login();
                lf.Show();
            }
        }
        public void clear()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }

        private void clearbtn_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void Staff_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            currenttimelbl.Text = DateTime.Now.ToLongTimeString();
            this.checkin.Text = DateTime.Now.ToString("hh:mm");
            this.checkout.Text = DateTime.Now.ToString("hh:mm"); 
        }
        protected override void OnClosed(EventArgs e)
        {
            if (MessageBox.Show("Do you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login lf = new Login();
                lf.Show();
            }
        }

        private void getprice_Click(object sender, EventArgs e)
        {
            try
            {
                InsertValuesToTable();
                MessageBox.Show("Price Data Exported!", "Sucessful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Coudn't get price information !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InsertValuesToTable()                          // Enters  price data to Table
        {
            Price p = new Price();
            List<Price> Pricelist = p.List();
            DataTable dataTable = Tools.ToTable(Pricelist);
            pricedataGridView.DataSource = dataTable;                                       // Source is given as datatable which contains all the price rate
        }

        private void InsertVisitorsValueToTable()
        {
            Visitors v = new Visitors();
            List<Visitors> listVisitors = v.List();
            DataTable dataTable = Tools.ToTable(listVisitors);
            VisitordataGridView.DataSource = dataTable;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (indivisualcheck.Checked)
            {
                this.grouptxt.Text = "1";
                grouptxt.ReadOnly = true;
            }
            else
            {
                this.grouptxt.Text = "";
                grouptxt.ReadOnly = false;
            }
        }

        private void Checkinbtn_Click(object sender, EventArgs e)
        {
            string name = Nametxt.Text;
            string phone = Phonetxt.Text;
            string age = agetxt.Text;
            string group = grouptxt.Text;
            if (String.IsNullOrEmpty(Nametxt.Text))
            {
                Nametxt.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(Phonetxt.Text))
            {
                Phonetxt.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(agetxt.Text))
            {
                agetxt.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                Nametxt.BackColor = System.Drawing.Color.White;
                Phonetxt.BackColor = System.Drawing.Color.White;
                agetxt.BackColor = System.Drawing.Color.White;
                if (MessageBox.Show("Are you sure??", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    Visitors v = new Visitors();
                    v.Name = name;
                    v.Phone = phone;
                    v.Age = age;
                    v.Date = datetxt.Value.Date;
                    v.EntryTime = checkin.Text;
                    v.Groupno = group;
                    v.Add(v);
                    InsertVisitorsValueToTable();
                    MessageBox.Show("Visitor Checked-In");
                    clear();
                }
            }
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            InsertVisitorsValueToTable();
        }

        private void checkoutbtn_Click(object sender, EventArgs e)
        {
            if (IDtxt.Text != "")
            {
                Visitors v = new Visitors();
                Visitors vs = v.List().Where(visitorcheckout => visitorcheckout.VisitorID == visitorID).FirstOrDefault();
                IDtxt.Text = v.VisitorID.ToString();
                Nametxt.Text = v.Name;
                Phonetxt.Text = v.Phone;
                agetxt.Text = v.Age;
                datetxt.Value = v.Date;
                checkout.Text = v.ExitTime;
                grouptxt.Text = v.Groupno;
                checkin.Text = v.EntryTime;
                // xxxxxxxxxxxxxxxx=======Complete it=== incomplete code//

            }
        }

        private void VisitordataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (VisitordataGridView.SelectedRows.Count > 0)
            {
                Visitors v = new Visitors();
                Visitors vs = v.List().Where(visitorcheckout => visitorcheckout.VisitorID == visitorID).FirstOrDefault();
                visitorID = Int32.Parse(VisitordataGridView.CurrentRow.Cells[0].Value.ToString());
                IDtxt.Text = visitorID.ToString();
                Nametxt.Text = v.Name;
                Phonetxt.Text = v.Phone;
                agetxt.Text = v.Age;
                checkout.Text = v.ExitTime;
                grouptxt.Text = v.Groupno;
                checkin.Text = v.EntryTime;
            }
        }
    }
}
