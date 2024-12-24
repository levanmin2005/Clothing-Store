using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Clothing_Store
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeCode.Text))
            {
                MessageBox.Show("Employee code cannot be empty.");
                return;
            }

            if (IsEmployeeCodeDuplicate(txtEmployeeCode.Text))
            {
                MessageBox.Show("Employee code already exists. Please use a different code.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
            {
                string query = @"INSERT INTO Employees (employee_code, name, position, authority, phone, email, address, date_of_birth) 
                         VALUES (@employee_code, @name, @position, @authority, @phone, @email, @address, @date_of_birth)";
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@employee_code", txtEmployeeCode.Text);
                cmd.Parameters.AddWithValue("@name", txtEmployeeName.Text);
                cmd.Parameters.AddWithValue("@position", txtPosition.Text);
                cmd.Parameters.AddWithValue("@authority", txtAuthority.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@date_of_birth", dtpDate0fBirth.Value);

                connection.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee added successfully.");
                LoadEmployees(); // Nạp lại dữ liệu sau khi thêm
            }
        }

        private bool IsEmployeeCodeDuplicate(string employeeCode)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
            {
                string query = "SELECT COUNT(*) FROM Employees WHERE employee_code = @employee_code";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@employee_code", employeeCode);

                connection.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
            {
                string query = @"UPDATE Employees 
                         SET name = @name, position = @position, authority = @authority, 
                             phone = @phone, email = @email, address = @address, 
                             date_of_birth = @date_of_birth 
                         WHERE employee_code = @employee_code";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@employee_code", txtEmployeeCode.Text);
                cmd.Parameters.AddWithValue("@name", txtEmployeeName.Text);
                cmd.Parameters.AddWithValue("@position", txtPosition.Text);
                cmd.Parameters.AddWithValue("@authority", txtAuthority.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@date_of_birth", dtpDate0fBirth.Value);

                connection.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee updated successfully.");
                LoadEmployees(); // Nạp lại dữ liệu sau khi cập nhật
            }
        }

        private void LoadEmployees()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
            {
                string query = "SELECT * FROM Employees"; // Lấy tất cả dữ liệu từ bảng Employees
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Hiển thị dữ liệu trong DataGridView
            }
        }

        private void Employee_Load(object sender, EventArgs e)
        {
            LoadEmployees(); // Nạp dữ liệu khi form được tải
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmployeeCode.Text))
            {
                MessageBox.Show("Please select an employee to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"))
                {
                    try
                    {
                        // Mở kết nối
                        connection.Open();

                        // Truy vấn xóa dữ liệu
                        string query = "DELETE FROM Employees WHERE employee_code = @employee_code";
                        SqlCommand cmd = new SqlCommand(query, connection);

                        // Gán giá trị mã nhân viên
                        cmd.Parameters.AddWithValue("@employee_code", txtEmployeeCode.Text);

                        // Thực thi lệnh
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Hiển thị thông báo
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployees(); // Tải lại danh sách nhân viên
                            ClearFields();  // Xóa dữ liệu trên các TextBox
                        }
                        else
                        {
                            MessageBox.Show("Employee not found.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị lỗi nếu có
                        MessageBox.Show("Error: " + ex.Message, "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        private void ClearFields()
        {
            txtEmployeeCode.Clear();      // Xóa mã nhân viên
            txtEmployeeName.Clear();      // Xóa tên nhân viên
            txtPosition.Clear();          // Xóa chức vụ
            txtAuthority.Clear();         // Xóa quyền hạn
            txtPhone.Clear();             // Xóa số điện thoại
            txtEmail.Clear();             // Xóa email
            txtAddress.Clear();           // Xóa địa chỉ
            dtpDate0fBirth.Value = DateTime.Now; // Đặt ngày sinh về ngày hiện tại
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