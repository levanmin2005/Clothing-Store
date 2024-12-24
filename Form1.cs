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

namespace Clothing_Store
{
    public partial class Customers : Form
    {
        SqlConnection connection;
        public Customers()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");
            
        }

  
        private void Customers_Load(object sender, EventArgs e)
        {
            connection.Open();
            FillData();
        }
        public void FillData()
        {
            string query = " select * from product";
            DataTable tbl = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, connection);
            ad.Fill(tbl);
            dataGridView1.DataSource = tbl;
            connection.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Please enter a search keyword.");
                    return;
                }

                // Câu lệnh SQL tìm kiếm theo Product ID hoặc Product Name
                string searchQuery = "SELECT * FROM product WHERE product_id LIKE @keyword OR product_name LIKE @keyword";

                SqlCommand cmd = new SqlCommand(searchQuery, connection);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();

                connection.Open();
                ad.Fill(tbl);
                connection.Close();

                dataGridView1.DataSource = tbl;

                if (tbl.Rows.Count == 0)
                {
                    MessageBox.Show("No products found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Dispose();
        }
    }
}
