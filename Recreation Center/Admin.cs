using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Recreation_Center
{
    public partial class Admin : Form
    {
        private string filePath = "PriceInformation.json";

        public Admin()
        {
            InitializeComponent();
            p = new Price();
        }
        Price p;
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login lf = new Login();
                lf.Show();
            }
        }
        private void exportpricebtn_Click(object sender, EventArgs e)
        {
            try
            {
                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                //Add the Header row for CSV file.
                foreach (DataGridViewColumn column in pricedataGridView.Columns)
                {
                    csv += column.HeaderText + ',';
                }
                //Add new line.
                csv += "\r\n";

                //Adding the Rows

                foreach (DataGridViewRow row in pricedataGridView.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                        {
                            //Add the Data rows.
                            csv += cell.Value.ToString().TrimEnd(',').Replace(",", ";") + ',';
                        }
                        // break;
                    }
                    //Add new line.
                    csv += "\r\n";
                }

                //Exporting to CSV.
                string folderPath = "..\\..\\..\\Recreation Center\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                File.WriteAllText(folderPath + "Price.csv", csv);
                MessageBox.Show("Price information saved.");
            }
            catch
            {
                MessageBox.Show("Error Price information already Exported");
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

        private void rhclearbtn_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void pricesavebtn_Click(object sender, EventArgs e)
        {
            string price1 = child1.Text;
            string price2 = child2.Text;
            string price3 = child3.Text;
            string price4 = child4.Text;

            string price5 = youth1.Text;
            string price6 = youth2.Text;
            string price7 = youth3.Text;
            string price8 = youth4.Text;

            string price9 = adult1.Text;
            string price10 = adult2.Text;
            string price11 = adult3.Text;
            string price12 = adult4.Text;

            string price13 = senior1.Text;
            string price14 = senior2.Text;
            string price15 = senior3.Text;
            string price16 = senior4.Text;

            string price17 = group1.Text;
            string price18 = group2.Text;
            string price19 = group3.Text;
            string price20 = weekdays.Text;
            string price21 = weekend.Text;

            if (Validate(price1, price2, price3, price4, price5, price6, price7, price8, price9, price10, price11, price12, price13, price14, price15, price16, price17, price18, price19, price20, price21))
            {
                if (MessageBox.Show("Are you sure??", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)       // If User Confirms then Execute Below
                {
                    p.child1to2hr = price1;
                    p.child2to4hr = price2;
                    p.child4to6hrs = price3;
                    p.childwholeday = price4;

                    p.youth1to2hrs = price5;
                    p.youth2to4hrs = price6;
                    p.youth4to6hrs = price7;
                    p.youthwholeday = price8;

                    p.adult1to2hrs = price9;
                    p.adult2to4hrs = price10;
                    p.adult4to6hrs = price11;
                    p.adultwholeday = price12;

                    p.senior1to2hrs = price13;
                    p.senior2to4hrs = price14;
                    p.senior4to6hrs = price15;
                    p.seniorwholeday = price16;

                    p.group2to5 = price17;
                    p.group5to10 = price18;
                    p.groupover10 = price19;

                    p.weekdays = price20;
                    p.weekend = price21;

                    InsertValuesToTable();
                    MessageBox.Show("Price Information Added");
                    clear();                         // To Clear after Submitting
                }
            }
        }
        private void InsertValuesToTable()                          // Enters data to Table
        {
            List<Price> Pricelist = new List<Price>();
            Pricelist.Add(p);
            DataTable dataTable = Tools.ToTable(Pricelist);
            pricedataGridView.DataSource = dataTable;                 // Source is given as datatable which contains all the inforamtion of the price
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

        private void importpricebtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to export pre-saved data??", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)       // If User Confirms then Execute Below
            {
                
                string file = Tools.ReadFromTextFile(filePath);
                if (file != null)
                {
                    List<Price> pl  = JsonConvert.DeserializeObject<List<Price>>(file);
                    p = pl[0];
                }
               
                InsertValuesToTable();
                MessageBox.Show("Price Data Imported.");
            }
        }
        private Boolean Validate(string price1, string price2, string price3, string price4, string price5, string price6, string price7, string price8,string price9,string price10, string price11, string price12, string price13, string price14, string price15, string price16, string price17, string price18,string price19, string price20, string price21)
        {
            if (String.IsNullOrEmpty(price1))
            {
                child1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price2))
            {
                child2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price3))
            {
                child3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price4))
            {
                child4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price5))
            {
                youth1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price6))
            {
                youth2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price7))
            {
                youth3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price8))
            {
                youth4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price9))
            {
                adult1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price10))
            {
                adult2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price11))
            {
                adult3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price12))
            {
                adult4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price13))
            {
                senior1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price14))
            {
                senior2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price15))
            {
                senior3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price16))
            {
                senior4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price17))
            {
                group1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price18))
            {
                group2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price19))
            {
                group3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price20))
            {
                weekdays.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrEmpty(price21))
            {
                weekend.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Price field empty!", "Empty data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //text input error validation
            else if (price1.All(Char.IsLetter))
            {
                child1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price2.All(Char.IsLetter))
            {
                child2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price3.All(Char.IsLetter))
            {
                child3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price4.All(Char.IsLetter))
            {
                child4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price5.All(Char.IsLetter))
            {
                youth1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price6.All(Char.IsLetter))
            {
                youth2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price7.All(Char.IsLetter))
            {
                youth3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price8.All(Char.IsLetter))
            {
                youth4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price9.All(Char.IsLetter))
            {
                adult1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price10.All(Char.IsLetter))
            {
                adult2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price11.All(Char.IsLetter))
            {
                adult3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price12.All(Char.IsLetter))
            {
                adult4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price13.All(Char.IsLetter))
            {
                senior1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price14.All(Char.IsLetter))
            {
                senior2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price15.All(Char.IsLetter))
            {
                senior3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price16.All(Char.IsLetter))
            {
                senior4.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price17.All(Char.IsLetter))
            {
                group1.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price18.All(Char.IsLetter))
            {
                group2.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price19.All(Char.IsLetter))
            {
                group3.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price20.All(Char.IsLetter))
            {
                weekdays.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (price21.All(Char.IsLetter))
            {
                weekend.BackColor = System.Drawing.Color.LightPink;
                MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                child1.BackColor = System.Drawing.Color.White;
                child2.BackColor = System.Drawing.Color.White;
                child3.BackColor = System.Drawing.Color.White;
                child4.BackColor = System.Drawing.Color.White;
                youth1.BackColor = System.Drawing.Color.White;
                youth2.BackColor = System.Drawing.Color.White;
                youth3.BackColor = System.Drawing.Color.White;
                youth4.BackColor = System.Drawing.Color.White;
                adult1.BackColor = System.Drawing.Color.White;
                adult2.BackColor = System.Drawing.Color.White;
                adult3.BackColor = System.Drawing.Color.White;
                adult4.BackColor = System.Drawing.Color.White;
                senior1.BackColor = System.Drawing.Color.White;
                senior2.BackColor = System.Drawing.Color.White;
                senior3.BackColor = System.Drawing.Color.White;
                senior4.BackColor = System.Drawing.Color.White;
                group1.BackColor = System.Drawing.Color.White;
                group2.BackColor = System.Drawing.Color.White;
                group3.BackColor = System.Drawing.Color.White;
                weekdays.BackColor = System.Drawing.Color.White;
                weekend.BackColor = System.Drawing.Color.White;

                return true; // validating price data !
            }
            return false;
        }

    }
}
