using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Manegement_System___KMC
{
    public partial class SelectProject : Form
    {
        public SelectProject()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        public string SelectedProjectID = null;
        public bool AddNew = false, AddNewEnabled = true;

        private void BtnClose_Click(object sender, EventArgs e)
        {
            SelectedProjectID = null;
            this.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {            
            AddNew form = new AddNew();
            form.Show();
            AddNew = true;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataGridViewProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SelectedProjectID = DataGridViewProject.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            catch { }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            DataGridViewProject.Rows.Clear();
            if (!(sqlFunctions.RetrieveDataTable("SELECT Project.ProjectID,Project.Discription,Estimator.ZoneNo " +
                "FROM Project,Estimator WHERE Project.EstimatorNO=Estimator.EstimatorNO AND Project.Discription LIKE N'%" + txtDescription.Text + "%'", DataGridViewProject))) 
            {
                sqlFunctions.ErrorMessage();
            }
        }

        private void SelectProject_Load(object sender, EventArgs e)
        {
            if (!AddNewEnabled)
                btnAddNew.Visible = false;

            if (!(sqlFunctions.RetrieveDataTable("SELECT Project.ProjectID,Project.Discription,Estimator.ZoneNo " +
                "FROM Project,Estimator WHERE Project.EstimatorNO=Estimator.EstimatorNO", DataGridViewProject)))
            {
                sqlFunctions.ErrorMessage();
            }
        }
    }
}
