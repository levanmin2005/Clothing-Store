using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Clothing_Store
{
    public partial class Login : Form
    {
        SqlConnection connection;
        public Login()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=Admin\MSSQLSERVER01;Initial Catalog=clothing_store;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
           string username = txtusername.Text;
            string password = txtpassword.Text;
            string query = "select * from account where username = @username and u_password = @password";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", SqlDbType.VarChar);
            cmd.Parameters["@username"].Value = username;
            cmd.Parameters.AddWithValue("@password",SqlDbType.VarChar);
            cmd.Parameters["@password"].Value=password;
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                string role = reader["u_role"].ToString();
                if (role.Equals("admin"))
                {
                    MessageBox.Show(this, "Login successfull ", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.Hide();
                    Manager manager = new Manager();
                    
                    manager.ShowDialog();
                    this.Dispose();
                }
                else if (role.Equals("user"))
                {
                    MessageBox.Show(this, "Login successfull ", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.Hide();
                    Customers vp = new Customers();
                    vp.ShowDialog();
                    this.Dispose();
                }
                else
                    lblerror.Text = "You are not allowed to access";
            }
            else
            {
                lblerror.Text = "Wrong username or password";
            }
            connection.Close();
        }

        private void btncanner_Click(object sender, EventArgs e)
        {
            if((MessageBox.Show(this,"Do you want to canner?","Question",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes))
            {
                Application.Exit();
            }
        }
    }
}
