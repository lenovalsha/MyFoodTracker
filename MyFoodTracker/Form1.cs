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
            Helper.CreateFoodEntry(txtFood.Text, cmbMeal.Text, dtpDate.Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvData.Columns.Add("Id", "ID");      // Add a column for Id
            dgvData.Columns.Add("FoodDate", "Date");
            dgvData.Columns.Add("Name", "Name");
            dgvData.Columns.Add("MealId", "Meal");
            dgvData.Columns[0].Visible = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Helper.GetAllFoodList(dgvData);

        }
    }
}
