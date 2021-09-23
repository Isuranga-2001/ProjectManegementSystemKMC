using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Manegement_System_KMC_Water
{
    public partial class SelectExpenditureHead : Form
    {
        public SelectExpenditureHead()
        {
            InitializeComponent();
        }
        SQLFunctions sqlFunctions = new SQLFunctions();

        private void SelectExpenditureHead_Load(object sender, EventArgs e)
        {
            UpdateDatGridView();
        }

        void UpdateDatGridView()
        {
            if (!(sqlFunctions.RetrieveDataOnDataGridView("SELECT * FROM ExpenditureHead WHERE Active='Yes'", EHDataGridView)))
            {
                sqlFunctions.ErrorMessage();
            }
        }

        private void EHDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Properties.Settings.Default.SelectedEHID = EHDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.Close();
        }

        private void EHDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Properties.Settings.Default.SelectedEHID = EHDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {/*
            Settings form = new Settings();            
            form.btnEditData.Checked = true;
            form.btnEditDataEH.Checked = true; 
            form.BeforeLoad();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
            UpdateDatGridView();*/
        }
    }
}
