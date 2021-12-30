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
            checkoutbtn.Visible = false;
            weekendtxt.Visible = false;

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
        private void isWeekend_CheckedChanged(object sender, EventArgs e)
        {
            if (isWeekend.Checked)
            {
                this.weekendtxt.Text = "Weekend";
                weekendtxt.ReadOnly = true;
            }
            else
            {
                this.weekendtxt.Text = "Weekday";
            }

        }

        private void Checkinbtn_Click(object sender, EventArgs e)
        {
            string name = Nametxt.Text;
            string phone = Phonetxt.Text;
            string age = agetxt.Text;
            string group = grouptxt.Text;
            DateTime dt = checkin.Value;
            string weekend = weekendtxt.Text;
            string entry = dt.ToLongTimeString();
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
            else
            {
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
                    v.Day = weekend;
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
                checkin.Text = VisitordataGridView.CurrentRow.Cells[6].Value.ToString();
                grouptxt.Text = VisitordataGridView.CurrentRow.Cells[8].Value.ToString();

                IDtxt.ReadOnly = true;
                Nametxt.ReadOnly = true;
                Phonetxt.ReadOnly = true;
                agetxt.ReadOnly = true;
                grouptxt.ReadOnly = true;
                checkin.ShowUpDown = true;
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
                DateTime dt = checkout.Value;
                string exit = dt.ToLongTimeString();

                Visitors v = new Visitors();
                Visitors vs = v.List().Where(visitorcheckout => visitorcheckout.VisitorID == visitorID).FirstOrDefault();
                IDtxt.Text = v.VisitorID.ToString();
                Nametxt.Text = v.Name;
                Phonetxt.Text = v.Phone;
                agetxt.Text = v.Age;
                grouptxt.Text = v.Groupno;
                checkin.Text = v.EntryTime;
                checkout.Text = v.ExitTime;
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

        private void sortbtn_Click(object sender, EventArgs e)
        {
            string sortOrder = SortcomboBox.Text;
            if (sortOrder == "")
            {
                MessageBox.Show("Select sorting order ", "Missing sort order!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!radioButtonName.Checked && !radioButtonDate.Checked)
                {
                    MessageBox.Show("Choose either of the option you want to sort.", "Choose Sorting of data.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Visitors visitor = new Visitors();
                    List<Visitors> newSortedList = new List<Visitors>();
                    List<Visitors> visitorsList = visitor.List();
                    int noOfVisitors = visitorsList.Count;
                    if (radioButtonName.Checked)
                    {
                        string[] visitorNameArray = new string[noOfVisitors];
                        for (int i = 0; i < noOfVisitors; i++)
                        {
                            visitorNameArray[i] = visitorsList[i].Name.Split(' ')[0];
                        }
                        for (int i = 0; i < noOfVisitors; i++)
                        {
                            for (int j = 0; j < noOfVisitors - 1; j++)
                            {
                                if (visitorNameArray[j].CompareTo(visitorNameArray[j + 1]) > 0)
                                {
                                    string temp = visitorNameArray[j];
                                    visitorNameArray[j] = visitorNameArray[j + 1];
                                    visitorNameArray[j + 1] = temp;
                                }
                            }
                        }
                        if (sortOrder.Equals("Decending"))
                        {
                            Array.Reverse(visitorNameArray);
                        }
                        for (int i = 0; 1 < noOfVisitors; i++)
                        {
                            for (int j = 0; j < noOfVisitors; j++)
                            {
                                if (visitorNameArray[i] == visitorsList[j].Name.Split(' ')[0])
                                {
                                    visitorsList.Remove(visitor);
                                    newSortedList.Add(visitorsList[j]);
                                }
                            }
                        }
                    }
                    else if (radioButtonDate.Checked)
                    {
                        DateTime[] dateTimeArray = new DateTime[noOfVisitors];
                        for (int i = 0; i < noOfVisitors; i++)
                        {
                            dateTimeArray[i] = visitorsList[i].Date;
                        }
                        for (int i = 0; i < noOfVisitors; i++)
                        {
                            for (int j = 0; j < noOfVisitors - 1; j++)
                            {
                                if (dateTimeArray[j].CompareTo(dateTimeArray[j + 1]) > 0)
                                {
                                    DateTime temp = dateTimeArray[j];
                                    dateTimeArray[j] = dateTimeArray[j + 1];
                                    dateTimeArray[j + 1] = temp;
                                }
                            }
                        }
                        if (sortOrder.Equals("Descending"))
                        {
                            Array.Reverse(dateTimeArray);
                        }
                        for (int i = 0; i < noOfVisitors; i++)
                        {
                            for (int j = 0; j < noOfVisitors; j++)
                            {
                                if (dateTimeArray[i] == visitorsList[j].Date)
                                {
                                    visitorsList.Remove(visitor);
                                    newSortedList.Add(visitorsList[j]);
                                }
                            }
                        }
                    }
                    DataTable dataTable = Tools.ToTable(newSortedList);
                    VisitordataGridView.DataSource = dataTable;
                }
            }
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            label7.Visible = false;
            checkout.Visible = false;
            checkoutbtn.Visible = false;
            clear();
            IDtxt.ReadOnly = false;
            Nametxt.ReadOnly = false;
            Phonetxt.ReadOnly = false;
            agetxt.ReadOnly = false;
            grouptxt.ReadOnly = false;
            indivisualcheck.Visible = true;
            Checkinbtn.Visible = true;
            clearbtn.Visible = true;
            checkoutbtn.Visible = false;
            label6.Visible = true;
            checkin.Visible = true;
            checkout.Visible = false;
            label7.Visible = false;

        }


    }
}
