using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Project_Manegement_System_KMC_Water
{
    class MultiFunctions
    {
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

        public int NoOfDays(int Month)
        {
            foreach (int n in new List<int> { 1, 3, 5, 7, 8, 10, 12 })
            {
                if (Month == n)
                    return 31;
            }
            foreach (int n in new List<int> { 4, 6, 9, 11 })
            {
                if (Month == n)
                    return 30;
            }
            return 28;
        }

        public string TypeCheckInput(string InputString, string Type, int ValidNoOFDecimalPlaces)
        {
            string ReturnString = InputString;
            bool DecimalIndicated = false;
            int CountDecimalPlaces = 0;

            if (InputString.Trim() != "")
            {
                char[] originalText = InputString.ToCharArray();

                for (int i = 0; i < originalText.Length; i++)
                {
                    char c = originalText[i];

                    if (Type == "N" || Type == "N.F")
                    {
                        if (Type == "N.F" && c == '.' && !DecimalIndicated)
                        {
                            if (ReturnString.IndexOf(c) == 0)
                                return "";

                            DecimalIndicated = true;
                            continue;
                        }

                        if (!(Char.IsNumber(c)))
                        {
                            ReturnString = ReturnString.Remove(i);
                        }

                        if (DecimalIndicated)
                        {
                            CountDecimalPlaces += 1;

                            if (CountDecimalPlaces > ValidNoOFDecimalPlaces)
                            {
                                if (Char.IsNumber(c))
                                    ReturnString = ReturnString.Remove(i);

                                return ReturnString;
                            }                                
                        }
                    }
                }
            }

            return ReturnString;
        }

        public string CurrentDate()
        {
            return DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
        }

        public string SplitText(string InputText)
        {
            if (InputText == "")
            {
                return InputText;
            }

            string DecimalPart = "", InputTextAfterSplit = "";

            if (InputText.Split('.').Length > 1)
            {
                DecimalPart = "." + InputText.Split('.')[1];
            }
            InputText = InputText.Split('.')[0];

            int Count = InputText.Length / 3;

            for (var k = 0; k < Count; k++)
            {
                InputTextAfterSplit = "," + InputText.Substring(InputText.Length - 3) + InputTextAfterSplit;
                InputText = InputText.Remove(InputText.Length - 3);
            }

            InputTextAfterSplit = InputText + InputTextAfterSplit;

            if (InputTextAfterSplit[0] == ',') 
            {
                InputTextAfterSplit = InputTextAfterSplit.Remove(0, 1);
            }

            return InputTextAfterSplit + DecimalPart;
        }

        public void NavigateTo(string NavigateFormName, Form CurrentForm)
        {
            switch (NavigateFormName)
            {
                case "Distribution":
                    {
                        WaterDiscribution form = new WaterDiscribution();
                        form.Show();
                        break;
                    }
                case "WaterProduction":
                    {
                        WaterProduction form = new WaterProduction();
                        form.Show();
                        break;
                    }
                case "Add":
                    {
                        AddNew form = new AddNew();
                        form.Show();
                        break;
                    }
                case "Progress":
                    {
                        Progress form = new Progress();
                        form.Show();
                        break;
                    }
                case "ProgressHistory":
                    {
                        ProgressHistory form = new ProgressHistory();
                        form.Show();
                        break;
                    }
                case "Dashboard":
                    {
                        DashBoard form = new DashBoard();
                        form.Show();
                        break;
                    }
                case "Edit":
                    {
                        EditData form = new EditData();
                        form.Show();
                        break;
                    }
                case "Filter":
                    {
                        Filter form = new Filter();
                        form.Show();
                        break;
                    }
                case "Settings":
                    {
                        SettingsForm form = new SettingsForm();
                        form.Show();
                        break;
                    }
                case "Exit":
                    {
                        Application.Exit();
                        break;
                    }
                case "LogOut":
                    {
                        Login form = new Login();
                        form.Show();
                        break;
                    }
            }
            CurrentForm.Close();
        }

        public string CheckRange(string ParameterName, float Value)
        {
            Dictionary<string, List<float>> AllowableLimits = new Dictionary<string, List<float>>
            {
                { "PH", new List<float> {6.5f, 8.5f } }, { "EC", new List<float> {50, 192 } },
                { "TDS", new List<float> {50, 150 } }, { "Tur", new List<float> {-1, 1.0f } },
                { "DO", new List<float> {6.5f, 8.0f } }, { "Al", new List<float> {0.002f, 0.1f } },
                { "Fe", new List<float> {-1, 0.3f } }, { "RCI", new List<float> {0.2f, 2 } },
                { "Colour", new List<float> {5, 5 } }, { "Temp", new List<float> {24, 28 } }
            };

            if (Value == -1)
            {
                return null;
            }
            else if (Value > AllowableLimits[ParameterName][1])
            {
                return "UP";
            }
            else if (Value < AllowableLimits[ParameterName][0])
            {
                return "DOWN";
            }
            else
            {
                return "YES";
            }
        }
    }
}
