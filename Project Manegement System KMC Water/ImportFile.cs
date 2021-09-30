using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Guna.UI2.WinForms;

namespace Project_Manegement_System_KMC_Water
{
    public partial class ImportFile : Form
    {
        public ImportFile()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();

        List<string> MonthNames = new List<string>
        {
            "January","February","March","April","May","June","July","August","September","October","November","December"
        };

        List<string> UpdatedMonths;

        private async void btnChooseFile_Click(object sender, EventArgs e)
        {
            if (!this.UseWaitCursor)
            {
                // DataTable has only special columns of ImportedDataSet's Table no 0 
                DataTable FilteredDataTable = new DataTable();

                List<string> ListImportantColumnsNames = new List<string> { "Area", "Consumption", "Month Amount" };
                Dictionary<string, int> ParallelIndexOfImportantColumnNames = new Dictionary<string, int> { };
                List<string> ValuesToAddDataTable = new List<string> { };

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV|*.csv|Microsoft Excel|*.xls";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (ofd.FileName.Split('.')[ofd.FileName.Split('.').Length - 1] == "csv")
                        {
                            try
                            {
                                bool IsDeleteErrorColumns = true;
                                string[] FirstLineOfFile = File.ReadAllLines(ofd.FileName)[0].Split(',');

                                if (FirstLineOfFile[1] == "" || FirstLineOfFile[2] == "" || FirstLineOfFile[3] == "") 
                                {
                                    IsDeleteErrorColumns = false;
                                    MessageBox.Show("Did not remove Name,Adress Coluumn in Selected file." +
                                        " You should delete Name,Address Columns in selected file to continue",
                                        "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                if (IsDeleteErrorColumns)
                                {
                                    int Index;
                                    foreach (string ImportedDataRow in File.ReadAllLines(ofd.FileName))
                                    {
                                        Index = 0;

                                        foreach (string CellValue in ImportedDataRow.Split(','))
                                        {
                                            // set column names
                                            if (FilteredDataTable.Columns.Count < 3)
                                            {
                                                foreach (string ImportantColumnsName in ListImportantColumnsNames)
                                                {
                                                    // check value of cell in first row equal to important column name 
                                                    if (CellValue.Trim() == ImportantColumnsName)
                                                    {
                                                        // add new column in filtered datatable
                                                        FilteredDataTable.Columns.Add(CellValue.Trim());

                                                        // add index to paralleldictionary
                                                        ParallelIndexOfImportantColumnNames.Add(CellValue.Trim(), Index);
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (KeyValuePair<string, int> SelectedIndex in ParallelIndexOfImportantColumnNames)
                                                {
                                                    if (Index == SelectedIndex.Value)
                                                    {
                                                        ValuesToAddDataTable.Add(CellValue);
                                                    }
                                                }
                                            }
                                            Index += 1;
                                        }

                                        if (ValuesToAddDataTable.Count == 3)
                                        {
                                            FilteredDataTable.Rows.Add(ValuesToAddDataTable[0].Trim(),
                                                ValuesToAddDataTable[1].Trim(), ValuesToAddDataTable[2].Trim());

                                        }
                                        ValuesToAddDataTable = new List<string> { };
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("Can't Import From Selected File. " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Can't Read Selected File. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    DataGridViewImportData.DataSource = FilteredDataTable;
                    string[] FileNameParts = ofd.FileName.Split()[0].Replace("\\", "#").Trim().Split('#');
                    txtPath.Text = FileNameParts[FileNameParts.Length - 1].Trim();
                    guna2ProgressBar2.Maximum = DataGridViewImportData.RowCount - 1;

                    this.UseWaitCursor = true;
                    await CovertLiterToSquareMeters(DataGridViewImportData, guna2ProgressBar2);
                    this.UseWaitCursor = false;
                }
            }
        }

        public static async Task CovertLiterToSquareMeters(Guna2DataGridView DataGridViewImportData, Guna2ProgressBar ImportingDataProgressBar)
        {
            await Task.Run(() =>
            {
                ImportingDataProgressBar.Value = 0;

                for (int i = 0; i < DataGridViewImportData.RowCount - 1; i++)
                {
                    try
                    {
                        DataGridViewImportData.Rows[i].Cells[1].Value = (Convert.ToDouble(
                            DataGridViewImportData.Rows[i].Cells[1].Value.ToString().Trim()) /
                            1000).ToString();
                    }
                    catch
                    {
                        continue;
                    }
                    ImportingDataProgressBar.Value += 1;
                }

                ImportingDataProgressBar.Value = 0;
            });
        }

        private void btnImport_Click(object sender, EventArgs e)
        {            
            if (DataGridViewImportData.Rows.Count > 0 && !this.UseWaitCursor)
            {
                this.UseWaitCursor = true;

                if (NotificationUpdatedStatus.Visible)
                {
                    if (MessageBox.Show("Cant't import data in to database. Because database is up to date." +
                        " If you want to remove updated data and import again", "Database is Up to date",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        string[] RelatedMonth = txtReletedDate.Tag.ToString().Split('-');
                        string SelectedMonth = Convert.ToInt32(RelatedMonth[1]).ToString();
                        if (SelectedMonth == "13")
                            SelectedMonth = "1";

                        if (sqlFunctions.ExecuteSQL(String.Format("DELETE FROM Consumption WHERE Month={0}", RelatedMonth[0] + "-" + SelectedMonth))) 
                        {
                            PassImportantDataToImportDataMethod();
                        }
                    }
                }
                else
                {
                    PassImportantDataToImportDataMethod();
                }
                
                async void PassImportantDataToImportDataMethod()
                {
                    await ImportData(DataGridViewImportData, txtReletedDate, guna2ProgressBar2);
                    this.Close();
                }                

                this.UseWaitCursor = false;
            }
        }

        public static async Task ImportData(Guna2DataGridView DataGridViewImportData, Guna2Chip txtReletedDate,Guna2ProgressBar ImportingDataProgressBar)
        {
            await Task.Run(() =>
            {
                string AreaID;
                int Passed = 0, Failed = 0;

                ImportingDataProgressBar.Value = 0;
                SQLFunctions sqlFunctions = new SQLFunctions();

                Dictionary<string, List<string>> DataDictionary = new Dictionary<string, List<string>> { };

                for (int i = 0; i < DataGridViewImportData.Rows.Count - 1; i++)
                {
                    try
                    {
                        List<string> ReturnList = sqlFunctions.SQLRead("SELECT AreaID FROM Area WHERE LocationName='" +
                            DataGridViewImportData.Rows[i].Cells[0].Value.ToString().Trim() + "'", "AreaID");

                        if (ReturnList != null)
                        {
                            if (ReturnList[0] != "")
                            {
                                AreaID = ReturnList[0];
                            }
                            else
                            {
                                if (!AreaIDNoAvailable())
                                    continue;
                            }
                        }
                        else
                        {
                            if (!AreaIDNoAvailable())
                                continue;
                        }
                    }
                    catch
                    {
                        if (!AreaIDNoAvailable())
                            continue;
                    }

                    bool AreaIDNoAvailable()
                    {
                        AreaID = sqlFunctions.SQLRead("SELECT MAX(AreaID) AS LastAreaID FROM Area", "LastAreaID")[0];

                        if (sqlFunctions.ExecuteSQL("INSERT INTO Area ([AreaID],[LocationName]) " +
                            "VALUES ('" + (Convert.ToInt32(AreaID) + 1).ToString() + "'," +
                            "'" + DataGridViewImportData.Rows[i].Cells[0].Value.ToString().Trim() + "')"))
                        {
                            AreaID = (Convert.ToInt32(AreaID) + 1).ToString();
                            return true;
                        }
                        else
                        {
                            Failed += 1;
                            return false;
                        }
                    }

                    try
                    {
                        DataDictionary.Add(AreaID, new List<string> {
                            DataGridViewImportData.Rows[i].Cells[1].Value.ToString().Trim(),
                            DataGridViewImportData.Rows[i].Cells[2].Value.ToString().Trim() });
                    }
                    catch
                    {
                        try
                        {
                            DataDictionary[AreaID][0] = (Convert.ToDouble(DataDictionary[AreaID][0]) +
                                Convert.ToDouble(DataGridViewImportData.Rows[i].Cells[1].Value.ToString().Trim())).ToString();

                            DataDictionary[AreaID][1] = (float.Parse(DataDictionary[AreaID][1]) +
                                float.Parse(DataGridViewImportData.Rows[i].Cells[2].Value.ToString().Trim())).ToString();
                        }
                        catch
                        {
                            Failed += 1;
                            continue;
                        }
                    }

                    ImportingDataProgressBar.Value += 1;
                    /*
                    if (i == 100)
                        break;*/

                }

                string ReletedMonth = txtReletedDate.Tag.ToString().Split('-')[0] + "-" +
                    (Convert.ToInt32(txtReletedDate.Tag.ToString().Split('-')[1]) + 1).ToString();

                foreach (KeyValuePair<string, List<string>> Data in DataDictionary)
                {
                    if (sqlFunctions.ExecuteSQL("INSERT INTO Consumption " +
                        "([AreaID],[Month],[Capacity],[MonthAmount]) " +
                        "VALUES ('" + Data.Key + "','" + ReletedMonth + "'," +
                        "'" + Data.Value[0] + "','" + Data.Value[0] + "')"))
                    {
                        Passed += 1;
                    }
                    else
                        Failed += 1;
                }

                MessageBox.Show("Data Rows Imported Successfully. Passed - " + Passed + " AND Failed - " + Failed + ".",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void btnChangeReletedMonth_Click(object sender, EventArgs e)
        {
            if (!this.UseWaitCursor)
            {
                Guna2CircleButton btnChangeReletedMonth = (Guna2CircleButton)sender;

                string[] CurrentDate = txtReletedDate.Tag.ToString().Split('-');

                if (btnChangeReletedMonth.Tag.ToString() == "Up")
                {
                    if (CurrentDate[1] != "11")
                        ChangeReletedMonth(Convert.ToInt32(CurrentDate[0]), Convert.ToInt32(CurrentDate[1]) + 1);
                    else
                        ChangeReletedMonth(Convert.ToInt32(CurrentDate[0]) + 1, 0);
                }
                else
                {
                    if (CurrentDate[1] != "0")
                        ChangeReletedMonth(Convert.ToInt32(CurrentDate[0]), Convert.ToInt32(CurrentDate[1]) - 1);
                    else
                        ChangeReletedMonth(Convert.ToInt32(CurrentDate[0]) - 1, 11);
                }
            }            
        }

        void ChangeReletedMonth(int SelectedYear, int SelectedMonth)
        {
            txtReletedDate.Text = SelectedYear + " " + MonthNames[SelectedMonth];
            txtReletedDate.Tag = SelectedYear + "-" + (SelectedMonth).ToString();
            CheckUpdatedStatusOfReletedMonth();
        }

        void CheckUpdatedStatusOfReletedMonth()
        {
            if (UpdatedMonths != null)
            {
                string CurrentMonth = txtReletedDate.Tag.ToString().Split('-')[0] + "-" + (Convert.ToInt32(txtReletedDate.Tag.ToString().Split('-')[1]) + 1).ToString();
                NotificationUpdatedStatus.Visible = false;

                foreach (string UpdatedMonth in UpdatedMonths)
                {
                    if (CurrentMonth == UpdatedMonth.Trim())
                    {
                        NotificationUpdatedStatus.Visible = true;
                        break;
                    }
                }
            }            
        }

        private void ImportFile_Load(object sender, EventArgs e)
        {
           UpdatedMonths = sqlFunctions.SQLRead("SELECT DISTINCT Month FROM Consumption", "Month");
           CheckUpdatedStatusOfReletedMonth();
        }
    }
}
