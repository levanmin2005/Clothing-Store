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
    public partial class Manager_Store : Form
    {
        SqlConnection connection;
        public Manager_Store()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");
        
        }

    

        private void Manager_Store_Load(object sender, EventArgs e)
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

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int error = 0;

            // Lấy dữ liệu từ các textbox
            string id = txtID.Text.Trim();
            string name = txtName.Text.Trim();
            string price = txtPrice.Text.Trim();
            string quantity = txtQuantity.Text.Trim();

            // Kiểm tra lỗi nhập liệu
            if (id.Equals(""))
            {
                error++;
                lbidError.Text = "ID can't be blank";
            }
            else
            {
                lbidError.Text = "";
            }

            if (name.Equals(""))
            {
                error++;
                lbnameError.Text = "Name can't be blank";
            }
            else
            {
                lbnameError.Text = "";
            }

            if (price.Equals(""))
            {
                error++;
                lblpriceError.Text = "Price can't be blank";
            }
            else
            {
                lblpriceError.Text = "";
            }

            if (quantity.Equals(""))
            {
                error++;
                lbquantityError.Text = "Quantity can't be blank";
            }
            else
            {
                lbquantityError.Text = "";
            }

            // Nếu không có lỗi, tiếp tục kiểm tra ID trong database
            if (error == 0)
            {
                string queryCheck = "SELECT * FROM product WHERE product_id = @id";
                connection.Open();
                SqlCommand cmdCheck = new SqlCommand(queryCheck, connection);
                cmdCheck.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmdCheck.ExecuteReader();
                if (reader.HasRows)
                {
                    lbidError.Text = "This ID already exists, please choose another.";
                    connection.Close();
                    return;
                }
                connection.Close();

                // Nếu ID không tồn tại, thực hiện Insert
                string queryInsert = "INSERT INTO product (product_id, product_name, price, quantity) VALUES (@id, @name, @price, @quantity)";
                SqlCommand cmdInsert = new SqlCommand(queryInsert, connection);
                cmdInsert.Parameters.AddWithValue("@id", id);
                cmdInsert.Parameters.AddWithValue("@name", name);
                cmdInsert.Parameters.AddWithValue("@price", price);
                cmdInsert.Parameters.AddWithValue("@quantity", quantity);

                connection.Open();
                int rowsInserted = cmdInsert.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {
                    MessageBox.Show("Insert successful!");
                    FillData(); // Load lại dữ liệu vào DataGridView
                }
                else
                {
                    MessageBox.Show("Insert failed.");
                }
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to edit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Câu lệnh SQL để cập nhật dữ liệu
                    string update = "UPDATE product SET product_name = @name, price = @price, quantity = @quantity WHERE product_id = @id";

                    SqlCommand cmd = new SqlCommand(update, connection);

                    // Thêm các tham số với dữ liệu từ TextBox
                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@price", Convert.ToInt32(txtPrice.Text.Trim()));
                    cmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(txtQuantity.Text.Trim()));
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text.Trim()));

                    connection.Open();
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();

                    // Kiểm tra kết quả và thông báo cho người dùng
                    if (i > 0)
                    {
                        FillData(); // Cập nhật lại dữ liệu trên DataGridView
                        MessageBox.Show(this, "Update successful", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, "Update failed. Please check the Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    connection.Close(); // Đảm bảo đóng kết nối nếu có lỗi xảy ra
                }
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to delete?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string delete = "DELETE FROM product WHERE product_id = @id";
                    SqlCommand cmd = new SqlCommand(delete, connection);

                    // Thêm tham số ID
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text.Trim()));

                    connection.Open();
                    int rowsDeleted = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (rowsDeleted > 0)
                    {
                        FillData(); // Cập nhật lại DataGridView
                        MessageBox.Show(this, "Delete successful", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, "Delete failed. Please check the Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    connection.Close(); // Đảm bảo đóng kết nối nếu xảy ra lỗi
                }
            }
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

        private void btndelete_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to delete?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string delete = "DELETE FROM product WHERE product_id = @id";
                    SqlCommand cmd = new SqlCommand(delete, connection);

                    // Thêm tham số ID
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text.Trim()));

                    connection.Open();
                    int rowsDeleted = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (rowsDeleted > 0)
                    {
                        FillData(); // Cập nhật lại DataGridView
                        MessageBox.Show(this, "Delete successful", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, "Delete failed. Please check the Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    connection.Close(); // Đảm bảo đóng kết nối nếu xảy ra lỗi
                }
            }
        }
    }
}