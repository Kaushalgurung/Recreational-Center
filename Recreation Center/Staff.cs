using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recreation_Center
{
    public partial class Staff : Form

    {
        

        int visitorID;
        private string filePath = "VisitorInformation.json";
        private string pricefilePath = "PriceInformation.json";
        private string checkoutfilepath = "Visitorscheckout.json";

        List<Visitors> vs;
        List<Visitorscheckout> vsc;

        public Staff()
        {
            InitializeComponent();
            label7.Visible = false;
            checkout.Visible = false;
            pricebtn.Visible = false;
            weekendtxt.Visible = true;
            vs = new List<Visitors>();
            vsc = new List<Visitorscheckout>();
            p = new Price();


        }
        Price p;


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
            if (MessageBox.Show("Are you sure you want to export pre-saved data??", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)       // If User Confirms then Execute Below
            {

                string file = Tools.ReadFromTextFileMenu(pricefilePath);
                if (file != null)
                {
                    p = JsonConvert.DeserializeObject<Price>(file);
                }

                InsertValuesToTable();
                MessageBox.Show("Price Data Imported.");
            }
        }
        private void InsertValuesToTable()                          // Enters  price data to Table
        {
            string jsonString = JsonConvert.SerializeObject(p);
            File.WriteAllText(pricefilePath, jsonString);

            DataTable dataTable = Tools.ToTableSingle(p);
            pricedataGridView.DataSource = dataTable;
            // Source is given as datatable which contains all the price rate
        }

        private void InsertVisitorsValueToTable()
        {

            string file = Tools.ReadFromTextFile(filePath);
            if (file != null)
            {
                vs = JsonConvert.DeserializeObject<List<Visitors>>(file);

            }
            DataTable dataTable = Tools.ToTable(vs);
            VisitordataGridView.DataSource = dataTable;
        }
        private void InsertCheckoutValueToTable()
        {

            string file = Tools.ReadFromTextFile(checkoutfilepath);
            if (file != null)
            {
                vsc = JsonConvert.DeserializeObject<List<Visitorscheckout>>(file);

            }
            DataTable dataTable = Tools.ToTable(vsc);
            VisitordatacheckoutGridView.DataSource = dataTable;
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
                weekendtxt.ReadOnly = true;
            }

        }
        // checks in visitor and adds the data to the grid view
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
            else if (String.IsNullOrEmpty(weekendtxt.Text))
            {
                MessageBox.Show("Check weekend checkbox uncheck it.", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Nametxt.BackColor = System.Drawing.Color.White;
                Phonetxt.BackColor = System.Drawing.Color.White;
                agetxt.BackColor = System.Drawing.Color.White;
                if (MessageBox.Show("Are you sure??", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    Visitors v = new Visitors();
                    Random r = new Random();
                    v.VisitorID = r.Next(0, 1000000);
                    v.Name = name;
                    v.Phone = phone;
                    v.Age = age;
                    v.Date = datetxt.Value.Date;
                    v.EntryTime = entry;
                    v.Groupno = group;
                    v.Day = weekend;
                    vs.Add(v);
                    string data = JsonConvert.SerializeObject(v, Formatting.None);
                    Tools.WriteToTextFile(filePath, data);
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
                grouptxt.Text = VisitordataGridView.CurrentRow.Cells[7].Value.ToString();
                weekendtxt.Text = VisitordataGridView.CurrentRow.Cells[5].Value.ToString();
                weekendtxt.Visible = true;
                weekendtxt.ReadOnly = true;
                IDtxt.ReadOnly = true;
                Nametxt.ReadOnly = true;
                Phonetxt.ReadOnly = true;
                agetxt.ReadOnly = true;
                grouptxt.ReadOnly = true;
                checkin.ShowUpDown = true;
                indivisualcheck.Visible = false;
                Checkinbtn.Visible = false;
                clearbtn.Visible = false;
                pricebtn.Visible = true;
                label6.Visible = true;
                checkin.Visible = true;
                checkout.Visible = true;
                label7.Visible = true;
                isWeekend.Visible = false;

            }
        }
        private void checkoutbtn_Click(object sender, EventArgs e)
        {
            try {
                if (IDtxt.Text != "")
                {
                    DateTime dt = checkout.Value;
                    string exit = dt.ToLongTimeString();
                    Visitors v = vs.Where(visitorcheckout => visitorcheckout.VisitorID == visitorID).FirstOrDefault();
                    v.ExitTime = checkout.Text;
                    TimeSpan duration = this.checkin.Value - this.checkout.Value;
                    int d = duration.Hours;
                    //child price indivisual
                    if (int.Parse(v.Age) < 16)
                    {
                        if (d <= 2) //duration 1to 2 hr children
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekend) / 100));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekdays) / 100));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno)*(int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child1to2hr) * (int.Parse(p.group2to5) / 100)));
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child1to2hr) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child1to2hr) * (int.Parse(p.group5to10) / 100)));
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child1to2hr) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child1to2hr) * (int.Parse(p.groupover10) / 100)));
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child1to2hr) - (int.Parse(p.child1to2hr) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child1to2hr) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 2 && d < 4) //duration 2-4hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.child2to4hr) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "weekday")
                                {
                                    int price = int.Parse(p.child2to4hr) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child2to4hr) - (int.Parse(p.child2to4hr) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child2to4hr) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child2to4hr) - (int.Parse(p.child2to4hr) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child2to4hr) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child2to4hr) - (int.Parse(p.child2to4hr) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child2to4hr) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child2to4hr) - (int.Parse(p.child2to4hr) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child2to4hr) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child2to4hr) - (int.Parse(p.child2to4hr) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child2to4hr) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child2to4hr) - (int.Parse(p.child2to4hr) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child2to4hr) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 4 && d < 6) //duration 4to6 hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.child4to6hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.child1to2hr) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child4to6hrs) - (int.Parse(p.child4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child4to6hrs) - (int.Parse(p.child4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child4to6hrs) - (int.Parse(p.child4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child4to6hrs) - (int.Parse(p.child4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child4to6hrs) - (int.Parse(p.child4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.child4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.child4to6hrs) - (int.Parse(p.child4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.child4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 6) //duration wholeday
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.childwholeday) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.childwholeday) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.childwholeday) - (int.Parse(p.childwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.childwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.childwholeday) - (int.Parse(p.childwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.childwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.childwholeday) - (int.Parse(p.child4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.childwholeday) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.childwholeday) - (int.Parse(p.childwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.childwholeday) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.childwholeday) - (int.Parse(p.childwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.childwholeday) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.childwholeday) - (int.Parse(p.childwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.childwholeday) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                    }
                    //youth price
                    else if (int.Parse(v.Age) > 15 && int.Parse(v.Age) < 24)
                    {
                        if (d <= 2) //duration 1to2 hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.youth1to2hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.youth1to2hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth1to2hrs) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();

                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth1to2hrs) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth1to2hrs) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth1to2hrs) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth1to2hrs) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.youth1to2hrs) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth1to2hrs) * (int.Parse(p.groupover10) / 100));
                                    v.Price = price.ToString();
                                }
                            }

                        }
                        else if (d > 2 && d < 4) // duration 2to4hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.youth2to4hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "weekday")
                                {
                                    int price = int.Parse(p.youth2to4hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth2to4hrs) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth2to4hrs) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth2to4hrs) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth2to4hrs) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth2to4hrs) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth2to4hrs) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth2to4hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 4 && d < 6) //duration 4to6 hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.youth4to6hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.youth4to6hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth4to6hrs) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth4to6hrs) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth4to6hrs) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth4to6hrs) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth4to6hrs) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youth4to6hrs) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youth4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 6) // duratopn whole day
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.youthwholeday) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.youthwholeday) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youthwholeday) - (int.Parse(p.youthwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youthwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youthwholeday) - (int.Parse(p.youthwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youthwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youthwholeday) - (int.Parse(p.youthwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youthwholeday) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youthwholeday) - (int.Parse(p.youthwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youthwholeday) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youthwholeday) - (int.Parse(p.youthwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.youthwholeday) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.youthwholeday) - (int.Parse(p.youthwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.youthwholeday) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }

                    }
                    //adult price calcuation
                    else if (int.Parse(v.Age) > 23 && int.Parse(v.Age) < 65)
                    {
                        if (d <= 2) // duartion 1-2hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.adult1to2hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.adult1to2hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult1to2hrs) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult1to2hrs) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult1to2hrs) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult1to2hrs) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult1to2hrs) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult1to2hrs) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult1to2hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 2 && d < 4) //duration 2-4hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.adult2to4hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "weekday")
                                {
                                    int price = int.Parse(p.adult2to4hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult2to4hrs) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult2to4hrs) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult2to4hrs) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult2to4hrs) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult2to4hrs) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult2to4hrs) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult2to4hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 4 && d < 6) //duration 4-6hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.adult4to6hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.adult4to6hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult4to6hrs) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult4to6hrs) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult4to6hrs) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult4to6hrs) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult4to6hrs) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adult4to6hrs) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adult4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 6) //duration wholeday
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.adultwholeday) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.adultwholeday) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adultwholeday) - (int.Parse(p.adultwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adultwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adultwholeday) - (int.Parse(p.adultwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adultwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adultwholeday) - (int.Parse(p.adultwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adultwholeday) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adultwholeday) - (int.Parse(p.adultwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adultwholeday) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adultwholeday) - (int.Parse(p.adultwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.adultwholeday) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.adultwholeday) - (int.Parse(p.adultwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.adultwholeday) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }

                    }
                    //Senior price 
                    else if (int.Parse(v.Age) > 64 && int.Parse(v.Age) < 100)
                    {
                        if (d <= 2)
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.senior1to2hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.senior1to2hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior1to2hrs) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior1to2hrs) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior1to2hrs) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior1to2hrs) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior1to2hrs) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior1to2hrs) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior1to2hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }

                        }
                        else if (d > 2 && d < 4)
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.senior2to4hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "weekday")
                                {
                                    int price = int.Parse(p.senior2to4hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior2to4hrs) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior2to4hrs) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior2to4hrs) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior2to4hrs) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior2to4hrs) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior2to4hrs) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior2to4hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 4 && d < 6) //duration 4to6hrs
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.senior4to6hrs) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.senior4to6hrs) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior4to6hrs) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior4to6hrs) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior4to6hrs) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior4to6hrs) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior4to6hrs) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.senior4to6hrs) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.senior4to6hrs) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                        }
                        else if (d > 6) //duration whole day
                        {
                            if (int.Parse(v.Groupno) == 1)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(p.seniorwholeday) - int.Parse(p.weekend);
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.seniorwholeday) - int.Parse(p.weekdays);
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 1 && int.Parse(v.Groupno) <= 5)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.seniorwholeday) - (int.Parse(p.seniorwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.seniorwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.seniorwholeday) - (int.Parse(p.seniorwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.seniorwholeday) * (int.Parse(p.group2to5) / 100)));
                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 5 && int.Parse(v.Groupno) <= 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.seniorwholeday) - (int.Parse(p.seniorwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.seniorwholeday) * (int.Parse(p.group5to10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.seniorwholeday) - (int.Parse(p.seniorwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.seniorwholeday) * (int.Parse(p.group5to10) / 100)));

                                    v.Price = price.ToString();
                                }
                            }
                            else if (int.Parse(v.Groupno) > 10)
                            {
                                if (v.Day == "Weekend")
                                {
                                    int price = int.Parse(v.Groupno) * (int.Parse(p.seniorwholeday) - (int.Parse(p.seniorwholeday) * (int.Parse(p.weekend) / 100)) - (int.Parse(p.seniorwholeday) * (int.Parse(p.groupover10) / 100)));
                                    v.Price = price.ToString();
                                }
                                else if (v.Day == "Weekday")
                                {
                                    int price = int.Parse(p.seniorwholeday) - (int.Parse(p.seniorwholeday) * (int.Parse(p.weekdays) / 100)) - (int.Parse(p.seniorwholeday) * (int.Parse(p.groupover10) / 100));
                                    v.Price = price.ToString();
                                }
                            }
                        }

                    }
                    else if (int.Parse(v.Age) > 100)
                    {
                        int price = 0;
                        v.Price = price.ToString();
                    }
                    pricetxt.Text = v.Price;
                }
                else
                {
                    MessageBox.Show("Select a Visitor from the Visitor's table to checkout!", "No data to check-out!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Import price data in the price details!", "No price data", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    List<Visitors> visitorsList = vs;
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
            pricebtn.Visible = false;
            clear();
            IDtxt.ReadOnly = false;
            Nametxt.ReadOnly = false;
            Phonetxt.ReadOnly = false;
            agetxt.ReadOnly = false;
            grouptxt.ReadOnly = false;
            indivisualcheck.Visible = true;
            Checkinbtn.Visible = true;
            clearbtn.Visible = true;
            pricebtn.Visible = false;
            checkout.Visible = false;
            label6.Visible = true;
            checkin.Visible = true;
            checkout.Visible = false;
            label7.Visible = false;
            weekendtxt.Visible = true;
            isWeekend.Visible = true;

        }

        private void checkoutbtn_Click_1(object sender, EventArgs e)
        {
            if (pricetxt.Text == "")
            {
                MessageBox.Show("Price not calcuated!");
            }
            else if (pricetxt.Text != "")
            {
                string CheckoutId = IDtxt.Text;
                string Checkoutname = Nametxt.Text;
                string Checkoutphone = Phonetxt.Text;
                string Checkoutage = agetxt.Text;
                DateTime dt = checkin.Value;
                string CheckoutEntry = dt.ToLongTimeString();
                DateTime dt2 = checkout.Value;
                string CheckoutExit = dt2.ToLongTimeString();
                string Checkoutday = weekendtxt.Text;
                string Checkoutprice = pricetxt.Text;
                string Checkoutgroupno = grouptxt.Text;

                Visitorscheckout vc = new Visitorscheckout();
                vc.VisitorID = int.Parse(CheckoutId);
                vc.Name = Checkoutname;
                vc.Phone = Checkoutphone;
                vc.Age = Checkoutage;
                vc.Date = datetxt.Value.Date;
                vc.Day = Checkoutday;
                vc.EntryTime = CheckoutEntry;
                vc.ExitTime = CheckoutExit;
                vc.Price= Checkoutprice;
                vc.Groupno = Checkoutgroupno;
                vsc.Add(vc);
                string data = JsonConvert.SerializeObject(vc, Formatting.None);
                Tools.WriteToTextFile(checkoutfilepath, data);
                InsertCheckoutValueToTable();
                remove();
                MessageBox.Show("Visitor Checked-out");
                clear();
            }
        }

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            (VisitordataGridView.DataSource as DataTable).DefaultView.RowFilter =
                String.Format("visitorID like '%" + SearchIDtxt.Text + "%'");
        }
        public void remove()
        {
            int rowIndex = VisitordataGridView.CurrentCell.RowIndex;
            VisitordataGridView.Rows.RemoveAt(rowIndex);
        }

        void Showreport()
        {
            int total = 0;
            for (int i = 0; i < VisitordatacheckoutGridView.Rows.Count; ++i)
            {
                total += Convert.ToInt32(VisitordatacheckoutGridView.Rows[i].Cells[9].Value);
            }
            this.totaltxt.Text = total.ToString();
            
            int child = 0;
            for (int i = 0; i < (VisitordatacheckoutGridView.Rows.Count - 1); i++)
            {
                if ((VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "1") 
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "2")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "3")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "4")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "5")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "6")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "7")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "8")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "9")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "10")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "1")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "12")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "13")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "14")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "15")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "16")
                   )
                {
                    child += 1;
                }
            }

            this.totalchildtxt.Text = child.ToString();

            //youth total
            int youth = 0;
            for (int i = 0; i < (VisitordatacheckoutGridView.Rows.Count - 1); i++)
            {
                if ((VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "17")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "18")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "19")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "20")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "21")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "22")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "23")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "24")
                   )
                {
                    youth += 1;
                }
            }

            this.totalyouthtxt.Text = youth.ToString();

            //adult total
            int adult = 0;
            for (int i = 0; i < (VisitordatacheckoutGridView.Rows.Count - 1); i++)
            {
                if ((VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "25")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "26")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "27")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "28")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "29")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "30")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "31")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "32")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "33")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "34")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "35")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "36")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "37")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "38")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "39")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "40")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "41")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "42")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "43")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "44")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "45")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "46")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "47")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "48")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "49")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "50")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "51")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "52")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "53")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "54")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "55")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "56")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "57")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "58")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "59")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "60")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "61")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "62")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "63")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "64")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "65")
                    )
                {
                    adult += 1;
                }
            }

            this.totaladulttxt.Text = adult.ToString();

            int senior = 0;
            for (int i = 0; i < (VisitordatacheckoutGridView.Rows.Count - 1); i++)
            {
                if ((VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "66")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "67")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "68")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "69")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "70")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "71")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "72")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "73")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "74")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "75")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "76")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "77")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "78")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "79")
                    || (VisitordatacheckoutGridView.Rows[i].Cells["ageDataGridViewTextBoxColumn1"].Value.ToString() == "80")
                   )
                {
                    senior += 1;
                }
            }

            this.totalseniortxt.Text = senior.ToString();



        }

        private void Visitorsummary_Click(object sender, EventArgs e)
        {
            Showreport();
        }
    }

}
