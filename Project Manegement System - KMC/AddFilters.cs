using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Project_Manegement_System___KMC
{
    public partial class AddFilters : Form
    {
        public AddFilters()
        {
            InitializeComponent();
        }

        public List<Guna2CustomCheckBox> ActiveCheckBoxList = new List<Guna2CustomCheckBox> { };
        public List<string> ActiveColumns = new List<string> { };
        public string ReturnValueTest = "", ReturnValueTag = "";
        public int WindowHeightIndex = 0, FiltersCount = 0, NoOfReturnFilters = 5;
        Guna2Button SelectedBtnOperation = null;
        SQLFunctions sqlFunctions = new SQLFunctions { };
        Guna2TextBox SelectedValueTextBox = null;

        List<string> FilterConditions = new List<string>
        {
            "Equals", "Not Equals", "Greater Than", "Greater Than Or Equal", "Less Than", "Less Than Or Equal", "Begin With", "End With"
        };

        List<string> FilterConditionsTags = new List<string>
        {
            "='", "<>'", ">'", ">='", "<'", "<='", "LikeBegin", "Like '&"
        };

        static List<int> ListWindowHeight = new List<int>
        {
            190, 295, 405, 515, 620
        };

        private void btnOperation(object sender, EventArgs e)
        {
            Guna2Button btnOperaton = (Guna2Button)sender;
            SelectedBtnOperation = btnOperaton;
            btnAnd.Visible = btnOR.Visible = true;
            btnOperaton.Left = 252;
            if (btnOperaton.Text == btnAnd.Text)
            {
                btnAnd.Visible = false;
                btnOR.Top = btnOperaton.Top;
                btnOR.Left = 366;
            }
            else
            {
                btnOR.Visible = false;
                btnAnd.Top = btnOperaton.Top;
                btnAnd.Left = 366;
            }
            btnOperaton.Checked = true;
        }

        private void btnChooseOpearion(object sender, EventArgs e)
        {
            Guna2Button btnChooseOperaton = (Guna2Button)sender;
            SelectedBtnOperation.Text = btnChooseOperaton.Text;
            SelectedBtnOperation.Checked = false;
            SelectedBtnOperation.Left = 307;
            btnAnd.Visible = btnOR.Visible = false;
        }

        private void btnAddAnother_Click(object sender, EventArgs e)
        {
            if (WindowHeightIndex < NoOfReturnFilters) 
            {
                WindowHeightIndex += 1;
                this.Height = ListWindowHeight[WindowHeightIndex];
                LastOperationBtn(true);

                if (WindowHeightIndex == 4)
                {
                    btnAddAnother.Enabled = false;
                }
            }
            else
            {
                btnAddAnother.Enabled = false;
            }
        }

        private void btnRemoveLast(object sender, EventArgs e)
        {
            //WindowHeightIndex -= 1;
            this.Height = ListWindowHeight[WindowHeightIndex - 1];
            LastOperationBtn(false);
            WindowHeightIndex -= 1;

            if (WindowHeightIndex == 0)
            {
                btnRemove.Enabled = false;
            }
        }

        void LastOperationBtn(bool visible)
        {
            foreach (Control ctrl in Controls)
            {
                if (ctrl.Tag != null)
                {
                    if (ctrl.GetType() == typeof(Guna2Button))
                    {
                        Guna2Button Selectedbtn = (Guna2Button)ctrl;

                        if (Selectedbtn.Tag.ToString() == WindowHeightIndex.ToString())
                        {
                            Selectedbtn.Visible = visible;
                        }
                    }
                }
            }
        }

        private void AddFilters_SizeChanged(object sender, EventArgs e)
        {
            if (this.Height == 620)
            {
                btnAddAnother.Enabled = false;
            }
            else if (this.Height == 190)
            {
                btnRemove.Enabled = false;
            }
            else
            {
                btnRemove.Enabled = true;
                btnAddAnother.Enabled = true;
            }
        }

        private void TC_SelectedChange(object sender, EventArgs e)
        {
            Guna2ComboBox TC = (Guna2ComboBox)sender;
            List<int> SelectedIndex = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
            List<Guna2ComboBox> FCList = new List<Guna2ComboBox> { FC1, FC2, FC3, FC4, FC5 };
            List<Guna2ComboBox> TCList = new List<Guna2ComboBox> { TC2, TC3, TC4, TC5 };

            for (var i = 0; i < TCList.Count; i++)
            {
                if (TC.Name == TCList[i].Name)
                {
                    for (var j = 0; j <= i; j++)
                    {
                        FCList.RemoveAt(0);
                    }
                    break;
                }
            }

            List<string> ProjectEstimatorTableQueryColumnNames = new List<string>
            {
                "Index No","Project Description","Method Of Execution","Permission","Start Date","Estimator","Section","Estimated Money"
            };

            if (TC.SelectedItem.ToString() == "Expenditure Head" || TC.SelectedItem.ToString() == "Proposer")
            {
                SelectedIndex = new List<int> { 0, 1 };
                if (TC.SelectedItem.ToString() == "Proposer")
                {
                    SelectedIndex = new List<int> { 0, 1, 6, 7 };
                }                    
                if (TC == TC1)
                {
                    foreach (var SelectedTC in TCList)
                    {
                        SelectedTC.Items.Clear();
                        SelectedTC.Items.Add(TC.SelectedItem.ToString());
                    }
                }
            }
            else
            {
                for (var i = 0; i < ProjectEstimatorTableQueryColumnNames.Count; i++) 
                {
                    if (TC.SelectedItem.ToString() == ProjectEstimatorTableQueryColumnNames[i])
                    {
                        foreach (var j in new List<int> { 1, 2, 3, 4, 5 })
                        {
                            if (i == j)
                            {
                                SelectedIndex = new List<int> { 0, 1, 6, 7 };
                            }
                        }
                        if (TC == TC1)
                        {
                            foreach (var SelectedTC in TCList)
                            {
                                SelectedTC.Items.Clear();
                                foreach (var ColumnName in ProjectEstimatorTableQueryColumnNames)
                                {
                                    SelectedTC.Items.Add(ColumnName);
                                }
                            }
                        }
                        break;
                    }
                    else
                    {
                        if (TC.SelectedItem.ToString() == "Progress Details" || TC.SelectedItem.ToString() == "Progress")
                        {
                            if (TC.SelectedItem.ToString() == "Progress Details")
                            {
                                SelectedIndex = new List<int> { 0, 1, 6, 7 };
                            }
                            if (TC == TC1)
                            {
                                foreach (var SelectedTC in TCList)
                                {
                                    SelectedTC.Items.Clear();
                                    SelectedTC.Items.Add("Progress");
                                    SelectedTC.Items.Add("Progress Details");
                                }
                            }
                            break;
                        }
                    }
                }
            }

            foreach (var FC in FCList)
            {
                FC.Items.Clear();
                foreach (var Index in SelectedIndex)
                {
                    FC.Items.Add(FilterConditions[Index]);
                }
            }
        }

        private void V_Click(object sender, EventArgs e)
        {
            Guna2TextBox ValueTest = (Guna2TextBox)sender;
            SelectedValueTextBox = ValueTest;
            Guna2ComboBox TC;
            if (ValueTest == V1)
                TC = TC1;
            else if (ValueTest == V2)
                TC = TC2;
            else if (ValueTest == V3)
                TC = TC3;
            else if (ValueTest == V4)
                TC = TC4;
            else
                TC = TC5;

            if (TC.SelectedItem.ToString() == "Expenditure Head")
            {
                List<string> EHData = sqlFunctions.SQLRead("SELECT EH_Main,EH_Sub1,EH_Sub2,EH_Sub3 " +
                    "FROM ExpenditureHead WHERE Active='Yes'", "EH_Main EH_Sub1 EH_Sub2 EH_Sub3");

                ValueItemList.Items.Clear();
                for (var i = 0; i < EHData.Count; i++)
                {
                    try
                    {
                        string Item = EHData[i] + "-" + EHData[i + 1] + "-" + EHData[i + 2] + "-" + EHData[i + 3].Trim();
                        if (Item[Item.Length - 1].ToString() == "-")
                        {
                            Item = Item.Remove(Item.Length - 1);
                        }
                        ValueItemList.Items.Add(Item);
                    }
                    catch
                    {
                        ValueItemList.Items.Add(EHData[i] + "-" + EHData[i + 1] + "-" + EHData[i + 2]);
                    }
                    i += 3;
                }
                HelpMenuShow();
            }
            else if (TC.SelectedItem.ToString() == "Proposer")
            {
                AddDataToHelpMenu(sqlFunctions.SQLRead("SELECT Name FROM Person WHERE Active='Yes'", "Name"));
            }
            else if (TC.SelectedItem.ToString() == "Method Of Execution")
            {
                AddDataToHelpMenu(sqlFunctions.SQLRead("SELECT DISTINCT MethodOfExecution FROM Project", "MethodOfExecution"));   
            }
            else if (TC.SelectedItem.ToString() == "Permission")
            {
                AddDataToHelpMenu(sqlFunctions.SQLRead("SELECT DISTINCT Permission FROM Project", "Permission"));
            }
            else if (TC.SelectedItem.ToString() == "Section")
            {
                AddDataToHelpMenu(sqlFunctions.SQLRead("SELECT DISTINCT ZoneNo FROM Estimator WHERE Active='Yes'", "ZoneNo"));
            }
            else if (TC.SelectedItem.ToString() == "Start Date")
            {
                AddDataToHelpMenu(sqlFunctions.SQLRead("SELECT DISTINCT StartDate FROM Project", "StartDate"));
            }

            void AddDataToHelpMenu(List<string> DataList)
            {
                ValueItemList.Items.Clear();
                for (var i = 0; i < DataList.Count; i++)
                {
                    ValueItemList.Items.Add(DataList[i]);
                }
                HelpMenuShow();
            }

            void HelpMenuShow()
            {
                ValueItemList.Visible = true;
                ValueItemList.Left = MousePosition.X;
                ValueItemList.Top = MousePosition.Y;
            }
        }

        private void ValueItemList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SelectedValueTextBox.Text = e.ClickedItem.ToString().Trim();
        }

        private void BtnCondition(object sender, EventArgs e)
        {
            Guna2ComboBox SelectedComboBox = (Guna2ComboBox)sender;
            SelectedComboBox.Tag = FilterConditionsTags[SelectedComboBox.SelectedIndex];
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ReturnValueTag = "";
            ReturnValueTest = "";
            string EncodeFromIdentifysettingsInEditMode = "";
            bool FindErrorInput = false;

            if (this.Height >= ListWindowHeight[0])
            {
                CreateReturnValues(TC1, FC1, V1, "");
                FiltersCount += 1;
            }
            if (this.Height >= ListWindowHeight[1])
            {
                CreateReturnValues(TC2, FC2, V2, O12.Text);
                FiltersCount += 1;
            }                
            if (this.Height >= ListWindowHeight[2])
            {
                CreateReturnValues(TC3, FC3, V3, O23.Text);
                FiltersCount += 1;
            }
            if (this.Height >= ListWindowHeight[3])
            {
                CreateReturnValues(TC4, FC4, V4, O34.Text);
                FiltersCount += 1;
            }
            if (this.Height == ListWindowHeight[4])
            {
                CreateReturnValues(TC5, FC5, V5, O45.Text);
                FiltersCount += 1;
            }

            void CreateReturnValues(Guna2ComboBox TC, Guna2ComboBox FC, Guna2TextBox V, string O)
            {                
                if (FC.Tag.ToString() == "LikeBegin")
                    ReturnValueTag += O + " [" + sqlFunctions.DatabaseColumnName[TC.Text] + "] Like '" + V.Text + "%' ";
                else
                {
                    if (TC.SelectedItem.ToString() == "Expenditure Head")
                    {
                        string[] EHParts = V.Text.Split('-');
                        List<string> EHColumnNames = new List<string> { "EH_Main", "EH_Sub1", "EH_Sub2", "EH_Sub3" };
                        ReturnValueTag += O;
                        for (int i = 0; i < EHParts.Length; i++) 
                        {
                            ReturnValueTag += " [" + EHColumnNames[i] + "] " + FC.Tag.ToString() + EHParts[i] + "'";
                            if (i < EHParts.Length - 1) 
                                ReturnValueTag += " AND";
                        }
                    }
                    else
                    {
                        ReturnValueTag += O + " [" + sqlFunctions.DatabaseColumnName[TC.Text] + "] " + FC.Tag.ToString() + V.Text + "' ";
                    }

                }
                
                EncodeFromIdentifysettingsInEditMode += O + "#" + TC.Text + "#" + FC.Text + "#" + V.Text + "#";
                ReturnValueTest += O + " " + TC.Text + " " + FC.Text + " " + V.Text + " ";

                if (TC.Text == "" | FC.Text == "" | V.Text == "")
                    FindErrorInput = true;
            }
            ReturnValueTag = EncodeFromIdentifysettingsInEditMode + "~" + ReturnValueTag + "~" + FiltersCount.ToString();

            if (FindErrorInput)
            {
                MessageBox.Show("Can't Update Table", "Something Went Wrong !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Close();
            }
        }

        private void AddFilters_Load(object sender, EventArgs e)
        {
            this.Height = ListWindowHeight[0];
            btnAnd.Visible = btnOR.Visible = false;

            if (NoOfReturnFilters > 5)
                NoOfReturnFilters = 5;

            foreach (var item in ActiveCheckBoxList)
            {
                ActiveColumns.Add(item.Tag.ToString().Split('~')[0]);
            }

            foreach (Control ctrl in Controls)
            {
                if (ctrl.GetType() == typeof(Guna2ComboBox))
                {
                    Guna2ComboBox SelectedComboBox = (Guna2ComboBox)ctrl;

                    if (SelectedComboBox.Tag != null)
                    {
                        if (SelectedComboBox.Tag.ToString() == "TargetColumn")
                        {
                            foreach (string ColumnName in ActiveColumns)
                            {
                                SelectedComboBox.Items.Add(ColumnName);
                            }
                        }
                        else
                        {
                            foreach (string ColumnName in FilterConditions)
                            {
                                SelectedComboBox.Items.Add(ColumnName);
                            }
                        }
                    }                    
                }
            }

            if (ReturnValueTag != "" | ReturnValueTest != "")
            {
                string[] Tags = ReturnValueTag.Split('~');
                string EncodeFromIdentifysettingsInEditMode = Tags[0];
                ReturnValueTag = Tags[1];

                string[] ListOfWordsInTest = EncodeFromIdentifysettingsInEditMode.Split('#');
                List<Guna2ComboBox> ListTC = new List<Guna2ComboBox> { TC1, TC2, TC3, TC4, TC5 };
                List<Guna2ComboBox> ListFC = new List<Guna2ComboBox> { FC1, FC2, FC3, FC4, FC5 };
                List<Guna2TextBox> ListV = new List<Guna2TextBox> { V1, V2, V3, V4, V5 };
                List<Guna2Button> ListO = new List<Guna2Button> { O12, O23, O34, O45 };

                WindowHeightIndex = -1;

                for (var index = 1; index < ListOfWordsInTest.Length; index++)
                {
                    if (ListOfWordsInTest[index] == "")
                        continue;

                    switch (index % 4)
                    {
                        case 0:
                            {
                                ListO[0].Text = ListOfWordsInTest[index];
                                ListO[0].Visible = true;
                                ListO.RemoveAt(0);
                                break;
                            }
                        case 1:
                            {
                                ListTC[0].SelectedItem = ListOfWordsInTest[index];
                                ListTC.RemoveAt(0);
                                WindowHeightIndex += 1;
                                break;
                            }
                        case 2:
                            {
                                ListFC[0].SelectedItem = ListOfWordsInTest[index];
                                ListFC.RemoveAt(0);
                                break;
                            }
                        case 3:
                            {
                                ListV[0].Text = ListOfWordsInTest[index];
                                ListV.RemoveAt(0);
                                break;
                            }
                    }

                    this.Height = ListWindowHeight[WindowHeightIndex];
                }
            }

            if (WindowHeightIndex >= NoOfReturnFilters)
            {
                btnAddAnother.Enabled = false;
            }
        }
    }
}
