using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Grocery_management_System
{
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            populate();
        }

        
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\GroceryDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string query = "select * from ItemTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ItemDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        int n = 0, GrdTotal = 0, Amount;
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (ItQtyTb.Text == "" || Convert.ToInt32(ItQtyTb.Text) > stock ||ItNameTb.Text == "")
            {
                MessageBox.Show("Enter Quantity");
            }
            else
            {
                int total = Convert.ToInt32(ItQtyTb.Text) * Convert.ToInt32(ItPriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n +1;
                newRow.Cells[1].Value = ItNameTb.Text;
                newRow.Cells[2].Value = ItPriceTb.Text;
                newRow.Cells[3].Value = ItQtyTb.Text;
                newRow.Cells[4].Value = total;
                BillDGV.Rows.Add(newRow);
                GrdTotal = GrdTotal + total;
                Amount = GrdTotal;
                TotalLb.Text = "Rs " + GrdTotal;
                n++;
                UpdateItem();
                Reset();
            }
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void UpdateItem()
        {
            try
            {
                int newQty = stock - Convert.ToInt32(ItQtyTb.Text);
                Con.Open();
                string query = "Update ItemTbl set ItQty = '" + newQty + "' where ItId=" + Key + ";";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item Updated Successfuly");
                Con.Close();
                populate();
                //Clear();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);

            }
        }
        public void Reset()
        {
            ItPriceTb.Text = "";
            ItQtyTb.Text = "";
            ClientNameTb.Text = "";
            ItNameTb.Text = "";
        }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ClientNameTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BillTbl values('" + EmployeeLbl.Text + "','" + ClientNameTb.Text + "'," + Amount + ")", Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bill Saved Successfuly");
                    Con.Close();
                    populate();
                   // Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600);
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            EmployeeLbl.Text = Login.EmployeeName;
        }
        int Itid, ItQty, ItPrice, total, pos = 60;

        private void label9_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        string ItName;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Grocery Shop", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(80));
            e.Graphics.DrawString("ID| ITEM | PRICE | QUANTITY | TOTAL", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                Itid = Convert.ToInt32(row.Cells["Column1"].Value);
                ItName = "" + row.Cells["Column2"].Value;
                ItPrice = Convert.ToInt32(row.Cells["Column3"].Value);
                ItQty = Convert.ToInt32(row.Cells["Column4"].Value);
                total = Convert.ToInt32(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + Itid, new Font("Century Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(26, pos));
                e.Graphics.DrawString("" + ItName, new Font("Century Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(50, pos));
                e.Graphics.DrawString("" + ItPrice, new Font("Century Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(120, pos));
                e.Graphics.DrawString("" + ItQty, new Font("Century Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(170, pos));
                e.Graphics.DrawString("" + total, new Font("Century Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(235, pos));
                pos = pos + 20; 
            }
            e.Graphics.DrawString("Grand Total: Rs " + Amount, new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(60, pos + 50));
            e.Graphics.DrawString("**********Grocery Shop**********", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 85));
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            Amount = 0;
            e.Graphics.DrawString("Thank You " + ClientNameTb.Text, new Font("Century Gothic", 12, FontStyle.Regular), Brushes.Black, new Point(60, pos + 120));
        }

        int stock = 0, Key = 0;
        private void ItemDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ItNameTb.Text = ItemDGV.SelectedRows[0].Cells[1].Value.ToString();
            ItPriceTb.Text = ItemDGV.SelectedRows[0].Cells[3].Value.ToString();
            if (ItNameTb.Text == "")
            {
                stock = 0;
                Key = 0;
            }
            else
            {
                stock = Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[2].Value.ToString());
                Key = Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[0].Value.ToString());
            }

        }
    }
}
