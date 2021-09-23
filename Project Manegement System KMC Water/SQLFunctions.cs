using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Guna.UI2.WinForms;
using System.Windows.Forms;

namespace Project_Manegement_System_KMC_Water
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
                                        try
                                        {
                                            Value = reader.GetInt32(reader.GetOrdinal(Name)).ToString();
                                        }
                                        catch
                                        {
                                            Value = reader.GetFloat(reader.GetOrdinal(Name)).ToString();
                                        }
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
            catch (Exception e)
            {
                conn.Close();
                return null;
            }
        }

        public bool RetrieveDataOnDataGridView(string query, Guna2DataGridView SelectedDataGridView)
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
            catch (Exception e)
            {
                conn.Close();
                return false;
            }
        }

        public DataTable RetrieveDataOnDataTable(string query)
        {
            try
            {
                DataTable Dt = new DataTable();

                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                Dt = new DataTable();
                sda.Fill(Dt);

                conn.Close();
                return Dt;
            }
            catch (Exception e)
            {
                conn.Close();
                return null;
            }
        }

        public void ErrorMessage()
        {
            MessageBox.Show("Can't Update Database", "Something Went Wrong !", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool ExecuteSQL(string query)
        {
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = '"
                + Properties.Settings.Default.DatabaseLocation + "'; Integrated Security = True");
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader SQL = cmd.ExecuteReader();

                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error found!" + e.Message, "Somthing Went Wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    } 
}
