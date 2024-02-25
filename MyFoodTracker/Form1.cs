using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFoodTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Helper.GetMealList(cmbMeal);
       
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Helper.CreateFoodEntry(txtFood.Text, 1, dtpDate.Value);
        }
    }
}
