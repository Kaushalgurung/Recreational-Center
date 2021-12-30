﻿using System;
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
            checkoutbtn.Visible = false;
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
            string entry = checkin.Text;
            if (String.IsNullOrEmpty(Nametxt.Text))
            {
                Nametxt.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Empty data field!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(Phonetxt.Text))
            {
                Phonetxt.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Empty data field!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(agetxt.Text))
            {
                agetxt.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Empty data field!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(grouptxt.Text))
            {
                MessageBox.Show("Check indivisual checkbox or enter group no.", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    v.EntryTime = entry;
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

        private void VisitordataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (VisitordataGridView.SelectedRows.Count > 0)
            {
                visitorID = Int32.Parse(VisitordataGridView.CurrentRow.Cells[0].Value.ToString());
                IDtxt.Text = visitorID.ToString();
                Nametxt.Text = VisitordataGridView.CurrentRow.Cells[1].Value.ToString();
                Phonetxt.Text = VisitordataGridView.CurrentRow.Cells[2].Value.ToString();
                agetxt.Text = VisitordataGridView.CurrentRow.Cells[3].Value.ToString();
                checkin.Text = VisitordataGridView.CurrentRow.Cells[5].Value.ToString();
                grouptxt.Text = VisitordataGridView.CurrentRow.Cells[7].Value.ToString();

                IDtxt.ReadOnly = true;
                Nametxt.ReadOnly = true;
                Phonetxt.ReadOnly = true;
                agetxt.ReadOnly = true;
                grouptxt.ReadOnly = true;
                checkin.ReadOnly = true;
                indivisualcheck.Visible = false;
                Checkinbtn.Visible = false;
                clearbtn.Visible = false;
                checkoutbtn.Visible = true;
                label6.Visible = true;
                checkin.Visible = true;
                checkout.Visible = true;
                label7.Visible = true;

            }
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
                checkout.Text = v.ExitTime;
                grouptxt.Text = v.Groupno;
                v.Edit(v);
                InsertVisitorsValueToTable();
                MessageBox.Show("Visitor Checked-Out");
                clear();
            }
            else
            {
                MessageBox.Show("Select a Visitor from the Visitor's table to checkout!", "No data to check-out!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
