using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Guna.UI2.WinForms;
using System.Windows.Forms;
using System.Collections;

namespace Project_Manegement_System___KMC
{
    class SQLFunctions
    {
        public SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = '"
                + Properties.Settings.Default.DatabaseLocation + "'; Integrated Security = True");

        public List<string> ProjectEstimatorQueryTableColumnNames = new List<string>
        {
            "ProjectID", "Discription", "EstimatedMoney", "Permission", "MethodOfExecution", "StartDate", "EstName", "ZoneNo"
        };
        public List<string> ProgressOfProjectQueryTableColumnNames = new List<string> { "Progress", "Details" };
        public List<string> ExpenditureHeadQueryTableColumnNames = new List<string> { "ExpenditureHead" };
        public List<string> PersonQueryTableColumnNames = new List<string> { "Name" };

        public Dictionary<string, string> DatabaseColumnName = new Dictionary<string, string>
        {
            { "Index No","ProjectID" },{ "Project Description","Discription" },{ "Start Date","StartDate" },{ "Estimated Money","EstimatedMoney" },
            { "Permission","Permission" },{ "Method Of Execution","MethodOfExecution" },{ "Expenditure Head","ExpenditureHead" },{ "Section","ZoneNo" },
            { "Estimator","EstName" },{ "Proposer","Name" },{ "Progress","Progress" },{ "Progress Details","Details" }
        };

        public List<string> SQLRead(string query, string FieldNames)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<string> ReturnList = new List<string> { };
                    while (reader.Read())
                    {
                        string Value;
                        string[] FieldNameList = FieldNames.Split();
                        foreach (var Name in FieldNameList)
                        {
                            try
                            {
                                try
                                {
                                    Value = reader.GetString(reader.GetOrdinal(Name)).ToString();
                                }
                                catch
                                {
                                    try
                                    {
                                        Value = reader.GetDouble(reader.GetOrdinal(Name)).ToString();
                                    }
                                    catch
                                    {
                                        Value = reader.GetInt32(reader.GetOrdinal(Name)).ToString();
                                    }
                                }
                            }
                            catch
                            {
                                Value = "";
                            }
                            ReturnList.Add(Value);
                        }
                    }
                    conn.Close();
                    return ReturnList;
                }
                conn.Close();
                return null;
            }
            catch
            {
                conn.Close();
                return null;
            }
        }

        public bool RetrieveDataTable(string query, Guna2DataGridView SelectedDataGridView)
        {
            try
            {
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable Dt = new DataTable();
                sda.Fill(Dt);

                Guna2DataGridView dataGridView = SelectedDataGridView;
                dataGridView.Rows.Clear();
                int n;
                foreach (DataRow items in Dt.Rows)
                {
                    n = dataGridView.Rows.Add();
                    for (var i = 0; i < dataGridView.ColumnCount; i++)
                    {
                        dataGridView.Rows[n].Cells[i].Value = items[i].ToString();
                    }
                }

                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
                return false;
            }
        }

        public void ErrorMessage()
        {
            MessageBox.Show("Can't Update Database", "Something Went Wrong !", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SignOut(Form CurrentForm)
        {
            if (MessageBox.Show("Do you want to signout ?", "SignOut", 
                MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Login form = new Login();
                form.Show();
                CurrentForm.Close();
            }
            else
            {
                Application.Exit();
            }
        }

        public void RemoveDuplicateRows(DataTable dataTable)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            foreach (DataRow drow in dataTable.Rows)
            {
                if (hTable.Contains(drow[dataTable.Columns[0].ColumnName] + "" + drow[dataTable.Columns[1].ColumnName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[dataTable.Columns[0].ColumnName] + "" + drow[dataTable.Columns[1].ColumnName], String.Empty);
            }
            foreach (DataRow dRow in duplicateList)
                dataTable.Rows.Remove(dRow);
        }
    } 
}
