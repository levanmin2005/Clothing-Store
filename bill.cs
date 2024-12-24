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
    public partial class bill : Form
    {
        SqlConnection connection;

        public bill()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string insertOrderQuery = "INSERT INTO Sales_Orders (customer_id, employee_id, total_revenue, payment_method, status) VALUES (@customer_id, @employee_id, @total_revenue, @payment_method, @status)";
                SqlCommand cmdOrder = new SqlCommand(insertOrderQuery, connection);

                cmdOrder.Parameters.AddWithValue("@customer_id", txtCustomerID.Text);
                cmdOrder.Parameters.AddWithValue("@employee_id", txtEmployeeID.Text);
                cmdOrder.Parameters.AddWithValue("@total_revenue", Convert.ToDecimal(txtTotalRevenue.Text));
                cmdOrder.Parameters.AddWithValue("@payment_method", txtPaymentMethod.Text);
                cmdOrder.Parameters.AddWithValue("@status", txtStatus.Text);

                connection.Open();
                cmdOrder.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Order added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                connection.Close(); // Đảm bảo đóng kết nối nếu có lỗi
            }
        }

        private void btnLoadOrders_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM Sales_Orders";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this order?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string deleteQuery = "DELETE FROM Sales_Orders WHERE order_id = @order_id";
                    SqlCommand cmd = new SqlCommand(deleteQuery, connection);
                    cmd.Parameters.AddWithValue("@order_id", txtOrderID.Text);

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("Order deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Delete failed.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    connection.Close(); // Đảm bảo đóng kết nối nếu có lỗi
                }
            }
        }
    }
}