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

namespace Recreation_Center
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

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
            string price15 = seniro3.Text;
            string price16 = senior4.Text;

            string price17 = group1.Text;
            string price18 = group2.Text;
            string price19 = group3.Text;
            string price20 = weekdays.Text;
            string price21 = weekend.Text;

            if (MessageBox.Show("Are you sure??", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)       // If User Confirms then Execute Below
                {
                    Price newPrice = new Price();
                    newPrice.child1to2hr = price1;
                    newPrice.child2to4hr = price2;
                    newPrice.child4to6hrs = price3;
                    newPrice.childwholeday = price4;

                    newPrice.youth1to2hrs = price5;
                    newPrice.youth2to4hrs = price6;
                    newPrice.youth4to6hrs = price7;
                    newPrice.youthwholeday = price8;

                    newPrice.adult1to2hrs = price9;
                    newPrice.adult2to4hrs = price10;
                    newPrice.adult4to6hrs = price11;
                    newPrice.adultwholeday = price12;

                    newPrice.senior1to2hrs = price13;
                    newPrice.senior2to4hrs = price14;
                    newPrice.senior4to6hrs = price15;
                    newPrice.seniorwholeday = price16;

                    newPrice.group1to5 = price17;
                    newPrice.group5to10 = price18;
                    newPrice.groupover10 = price19;

                    newPrice.weekdays = price20;
                    newPrice.weekend = price21;

                    newPrice.Add(newPrice);
                    InsertValuesToTable();
                    MessageBox.Show("Price Information Added");
                    clear();                         // To Clear after Submitting
                }
            }
        private void InsertValuesToTable()                          // Enters data to Table
        {
            Price p = new Price();
            List<Price> Pricelist = p.List();
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
                InsertValuesToTable();
                MessageBox.Show("Price Data Exported.");
            }
        }

    }
}
