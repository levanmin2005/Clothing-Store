using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clothing_Store
{
    public partial class Manager : Form
    {
        public Manager()
        {
            InitializeComponent();
        }

        private void btnproduct_Click(object sender, EventArgs e)
        {
            Manager_Store productManagerment = new Manager_Store();
            productManagerment.ShowDialog();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            Employee Employee = new Employee();
            Employee.ShowDialog();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {

        }
    }
}
