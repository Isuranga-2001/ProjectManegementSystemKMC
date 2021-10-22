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
    public partial class SelectDate : Form
    {
        public SelectDate()
        {
            InitializeComponent();
            //this.TransparencyKey = Color.Silver;
        }

        public bool DateChangeEnabled = false;

        private void SelectDate_Load(object sender, EventArgs e)
        {
            //Calander.SelectionStart = CurrentDate;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DateChangeEnabled = true;
            this.Close();
        }
    }
}
