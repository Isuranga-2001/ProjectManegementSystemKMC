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
using Manina.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Project_Manegement_System_KMC_Water
{
    public partial class WaterProduction : Form
    {
        public WaterProduction()
        {
            InitializeComponent();
        }

        MultiFunctions multiFunctions = new MultiFunctions();
        SQLFunctions sqlFunctions = new SQLFunctions();

        List<string> PlantNames = new List<string> { "Gatambe", "Roseneeth", "Dunumadalawa (STS)", "Waterboard" };
        List<Color> CustomColorList = new List<Color> 
        { 
            Color.FromArgb(100, 0, 110, 179), Color.FromArgb(100, 0, 71, 115), 
            Color.FromArgb(100, 125, 137, 149), Color.FromArgb(100, 0, 157, 255) 
        };

        private void WaterProduction_Load(object sender, EventArgs e)
        {
            ListOfBtnFlowUpDown = new List<Guna2CircleButton>
            {
                btnFlowUpDownGatambe, btnFlowUpDownRoseneeth, btnFlowUpDownDunumadalawa, btnFlowUpDownWaterboard
            };
            ListUpdateDataInputTools = new List<Control>
            {
                lblCostName, lblCostPerUnit, lblDetails, btnDefualt, btnClear, btnUpdate, txtDetails, txtCost, 
                lblCapacity, txtCapacity, TrackBarCapacity, btnUpdateTestResults
            };
            UpdatedStatusButton = new Dictionary<Guna2CircleButton, Guna2Button>
            {
                {btnFlowUpDownGatambe, UpdateStatusGatambe}, {btnFlowUpDownRoseneeth, UpdateStatusRoseneeth},
                {btnFlowUpDownDunumadalawa, UpdateStatusDunumadalawa},{btnFlowUpDownWaterboard, UpdateStatusWaterboard}
            };
            ListUpdateStatusList = new List<Guna2Button>
            {
                UpdateStatusGatambe, UpdateStatusRoseneeth, UpdateStatusDunumadalawa, UpdateStatusWaterboard
            };
            ListTestTabbtnPlant = new List<Guna2Button>
            {
                btnTestGatambe, btnTestRoseneeth, btnTestSTS, btnTestWaterboard
            };
            ListUserInputsForTestingParameters = new List<Guna2TextBox>
            {
                guna2TextBox5, guna2TextBox8, guna2TextBox24, guna2TextBox26, guna2TextBox25,
                guna2TextBox28, guna2TextBox27, guna2TextBox34, guna2TextBox35, guna2TextBox36
            };
            ListlblQualityCheck = new List<Guna2HtmlLabel>
            {
                guna2HtmlLabel46, guna2HtmlLabel48, guna2HtmlLabel49, guna2HtmlLabel50, guna2HtmlLabel51,
                guna2HtmlLabel52, guna2HtmlLabel53, guna2HtmlLabel55, guna2HtmlLabel54, guna2HtmlLabel56
            };
            ListMoreDetailsControl = new Dictionary<int, List<Control>>
            {
                { 0, new List<Control> { label63, label69, label74, label22, label29, label37, label38, label43, label24, label50, label51, label56, label57 } },
                { 1, new List<Control> { label65, label70, label75, label28, label30, label21, label39, label42, label44, label49, label52, label55, label58 } },
                { 2, new List<Control> { label66, label71, label76, label32, label31, label36, label40, label23, label45, label48, label25, label54, label59 } },      
                { 3, new List<Control> { label67, label72, label77, label33, label34, label35, label26, label41, label46, label47, label53, label27, label60 } }
            };
            ListMoreDetailsRatingStars = new List<Guna2RatingStar>
            {
                guna2RatingStar1, guna2RatingStar2, guna2RatingStar4, guna2RatingStar3
            };
            ListHomeProgressBars = new Dictionary<int, List<Guna2ProgressBar>>
            {
                { 0 , new List<Guna2ProgressBar> { guna2ProgressBar1 , guna2ProgressBar2 } },
                { 1 , new List<Guna2ProgressBar> { guna2ProgressBar7 , guna2ProgressBar8 } },
                { 2 , new List<Guna2ProgressBar> { guna2ProgressBar3 , guna2ProgressBar4 } },
                { 3 , new List<Guna2ProgressBar> { guna2ProgressBar5 , guna2ProgressBar6 } }
            };
            ListLabelsOfHomeProgressBarValues = new Dictionary<int, List<Guna2HtmlLabel>>
            {
                { 0 , new List<Guna2HtmlLabel> { guna2HtmlLabel69, guna2HtmlLabel68 } },
                { 1 , new List<Guna2HtmlLabel> { guna2HtmlLabel23, guna2HtmlLabel22 } },
                { 2 , new List<Guna2HtmlLabel> { guna2HtmlLabel74, guna2HtmlLabel73 } },
                { 3 , new List<Guna2HtmlLabel> { guna2HtmlLabel79, guna2HtmlLabel78 } }
            };
            ListbtnSelectMonth = new List<Guna2Button>
            {
                btnChangeMonthJan, guna2Button2, guna2Button3, guna2Button4, guna2Button5, guna2Button6,
                guna2Button7, guna2Button8, guna2Button9, guna2Button10, guna2Button11, guna2Button12
            };
            ListMainTabButtons = new List<Guna2Button>
            {
                TabBtnProduction, btnMoreDetails, guna2Button15, guna2Button16, TabBtnTest
            };

            PanelFlowUpDownUpdatePageGatambe.Size = MinSize;
            tableLayoutPanel3.ColumnStyles[10].Width = 
                tableLayoutPanel11.ColumnStyles[8].Width = tableLayoutPanel9.ColumnStyles[12].Width = 20F;
            ListbtnSelectMonth[DateTime.Today.AddMonths(-1).Month - 1].Checked = true;
            btnHistoryChartCapacity.Checked = true;
                        
            CheackUpdateForToday();
            IsUpdatedTestData();
            UpdateMoreDetailsPage();

            TabBtnProduction.Checked = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);
        }

        private void NavigationBarButton_Click(object sender, EventArgs e)
        {
            Guna2CircleButton NavigationBarButton = (Guna2CircleButton)sender;
            multiFunctions.NavigateTo(NavigationBarButton.Tag.ToString(), this);
        }

        private void RatingStarQualityOfWater_Click(object sender, EventArgs e)
        {
            Guna2RatingStar RatingStarQualityOfWater = (Guna2RatingStar)sender;
            RatingStarQualityOfWater.Value = float.Parse(RatingStarQualityOfWater.Tag.ToString());
        }

        string SelectedDate()
        {
            return txtDate.Value.Year.ToString() + "-" + txtDate.Value.Month.ToString() + "-" + txtDate.Value.Day.ToString();
        }

        private void txtDate_ValueChanged(object sender, EventArgs e)
        {
            if (pagedControl1.SelectedPage == page4)
            {
                for (int i = 0; i < ListUpdateStatusList.Count; i++)
                {
                    ISUpdatedMainData(i, ListUpdateStatusList[i]);
                }
                SetUpdatedData();
            }
            else if (pagedControl1.SelectedPage == page5)
            {
                ClearData();
                btnTestGatambe.Checked = true;
            }
            else if (pagedControl1.SelectedPage == page2)
            {
                UpdateMoreDetailsPage();
            }
        }

        double Average(string Field, int Plant)
        {
            if (Plant < 0)
            {
                try
                {
                    return Convert.ToDouble(sqlFunctions.SQLRead("SELECT SUM(" + Field + ") " +
                    "AS TotalProduction FROM Production", "TotalProduction")[0].Trim()) /
                    Convert.ToDouble(sqlFunctions.SQLRead("SELECT COUNT(" + Field + ") " +
                    "AS TotalDays FROM Production", "TotalDays")[0].Trim());
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                try
                {
                    return Convert.ToDouble(sqlFunctions.SQLRead("SELECT SUM(" + Field + ") " +
                    "AS TotalProduction FROM Production WHERE PlantID='" + Plant + "'",
                    "TotalProduction")[0].Trim()) /
                    Convert.ToDouble(sqlFunctions.SQLRead("SELECT COUNT(" + Field + ") " +
                    "AS TotalDays FROM Production WHERE PlantID='" + Plant + "'",
                    "TotalDays")[0].Trim());
                }
                catch
                {
                    return 0;
                }                
            }
        }

        void ClearData()
        {
            txtCapacity.Clear();
            txtCost.Clear();
            txtDetails.Clear();
            TrackBarCapacity.Value = 0;

            for (int i = 0; i < ListlblQualityCheck.Count; i++)
            {
                ListUserInputsForTestingParameters[i].Clear();
                ListlblQualityCheck[i].Text = "";
            }
            lblPassed.Text = "0/10";
            lblFaild.Text = "10/10";
            lblRating.Text = "0.0";
            RatingStarQualityOfWaterSelectedDate.Value = 0;
            RatingStarQualityOfWaterSelectedDate.Tag = 0;
            lblDescribtion.Text = "";
        }

        /// Navigation Between Pages       

        List<Guna2Button> ListMainTabButtons;

        private void btnMoreDetails_Click(object sender, EventArgs e)
        {
            txtDate.Value = DateTime.Now.AddDays(-1);
            pagedControl1.SelectedPage = page2;
            btnMoreDetails.Checked = true;
        }

        private void btnTab_CheackedChange(object sender, EventArgs e)
        {
            txtDate.Enabled = true;
            Guna2Button btnTab = (Guna2Button)sender;
            if (btnTab.Tag.ToString() == "1")
            {
                pagedControl1.SelectedPage = page1;
                if (!NotificationTestResults.Visible && !NotificationUpdateData.Visible)
                    lblNotification.Visible = false;
                UpdateHomePageProductionValues();
            }                
            else if (btnTab.Tag.ToString() == "2")
            {
                UpdateMoreDetailsPage();
                pagedControl1.SelectedPage = page2;
            }                
            else if (btnTab.Tag.ToString() == "3")
            {
                txtDate.Enabled = false;
                pagedControl1.SelectedPage = page3;
            }               
            else if (btnTab.Tag.ToString() == "4")
            {
                CheackUpdateForToday();
                pagedControl1.SelectedPage = page4;
            }
            else if (btnTab.Tag.ToString() == "5")
            {
                btnTestGatambe.Checked = true;
                IsUpdatedTestData();
                pagedControl1.SelectedPage = page5;
            }

            foreach (Guna2Button MainTabButton in ListMainTabButtons)
            {
                MainTabButton.ShadowDecoration.Enabled = false;
            }

            //btnTab.ShadowDecoration.Enabled = true;
        }

        /// <summary>
        /// Animations and Flow UX of "update for today" page
        /// Content Of "Update For Today" Page
        /// </summary>
        /// <param name="MaxSize" value="(1090, 300)"></param>
        /// <param name="MinSize" value="(1090, 52)"></param>

        Size MaxSize = new Size(1090, 300), MinSize = new Size(1090, 52);
        List<Guna2CircleButton> ListOfBtnFlowUpDown;
        List<Control> ListUpdateDataInputTools;
        Dictionary<Guna2CircleButton, Guna2Button> UpdatedStatusButton;
        List<Guna2Button> ListUpdateStatusList;

        void CheackUpdateForToday()
        {
            txtDate.Value = DateTime.Now;

            int CountUpdated = 0;
            for (int i = 0; i < ListUpdateStatusList.Count; i++)
            {
                if (ISUpdatedMainData(i, ListUpdateStatusList[i]))
                {
                    CountUpdated += 1;
                }
            }

            if (CountUpdated == 4)
            {
                NotificationUpdateData.Visible = false;
            }
        }

        private void btnFlowUpDown_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnFlowUpDown = (Guna2CircleButton)sender;
            Guna2Panel PanelFlowUpDownUpdate = (Guna2Panel)btnFlowUpDown.Parent;

            if (PanelFlowUpDownUpdate.Size == MaxSize)
            {
                PanelFlowUpDownUpdate.Size = MinSize;
                ChangeBtnFlowUpDownPosition(btnFlowUpDown);
                UpdatedStatusButton[btnFlowUpDown].Visible = true;
            }
            else
            {
                // Compress all Panels
                PanelFlowUpDownUpdatePageGatambe.Size = PanelFlowUpDownUpdatePageRoseneeth.Size =
                    PanelFlowUpDownUpdateDunumadalawa.Size = PanelFlowUpDownUpdateWaterboard.Size = MinSize;

                // Expand Selected Panel
                PanelFlowUpDownUpdate.Size = MaxSize;
                ChangeBtnFlowUpDownPosition(btnFlowUpDown);

                // Reset Flow UpDown Control buttons and updated status
                foreach (Guna2CircleButton btnFlowUpDownListItem in ListOfBtnFlowUpDown)
                {
                    ChangeBtnFlowUpDownPosition(btnFlowUpDownListItem);
                    UpdatedStatusButton[btnFlowUpDownListItem].Visible = true;
                }

                // Change Positions of Update data input tools
                foreach (Control InputTool in ListUpdateDataInputTools)
                {
                    btnFlowUpDown.Parent.Controls.Add(InputTool);
                }

                // update Content in panel                
                UpdatedStatusButton[btnFlowUpDown].Visible = false;
                ChangedSelectedPlant();
                SetUpdatedData();                
            }

            for (int i = 0; i < ListUpdateStatusList.Count; i++)
            {
                ISUpdatedMainData(i, ListUpdateStatusList[i]);
            }
            lblCostPerUnit.Visible = true;

            btnUpdate.ShadowDecoration.Enabled = true;
        }

        private void ChangeBtnFlowUpDownPosition(Guna2CircleButton btnFlowUpDown)
        {
            if (btnFlowUpDown.Parent.Size == MinSize)
            {
                btnFlowUpDown.Text = "▼";
                btnFlowUpDown.TextOffset = new Point(1, 1);
                btnFlowUpDown.Top = 8;
            }
            else
            {
                btnFlowUpDown.Text = "▲";
                btnFlowUpDown.TextOffset = new Point(1, -1);
                btnFlowUpDown.Top = 255;
            }
        }

        private void txtInputControl_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox txtInputControl = (Guna2TextBox)sender;
            txtInputControl.Text = multiFunctions.TypeCheckInput(txtInputControl.Text, "N.F", 2);
            txtInputControl.Select(txtInputControl.Text.Length, 0);

            if (txtCapacity.Text != "")
            {
                if (TrackBarCapacity.Maximum >= Convert.ToInt32(Convert.ToDouble(txtCapacity.Text)))
                {
                    TrackBarCapacity.Value = Convert.ToInt32(Convert.ToDouble(txtCapacity.Text));
                    TrackBarCapacity.ThumbColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(110)))), ((int)(((byte)(179)))));
                }
                else
                {
                    TrackBarCapacity.Value = TrackBarCapacity.Maximum;
                    TrackBarCapacity.ThumbColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(115)))));
                }
            }

            try
            {
                lblCostPerUnit.Text = lblCostPerUnit.Tag.ToString() +
                    Math.Round(Convert.ToDouble(txtCost.Text) / 
                    Convert.ToDouble(txtCapacity.Text), 2);
            }
            catch
            {
                lblCostPerUnit.Text = lblCostPerUnit.Tag.ToString() +
                    Math.Round(Average("Cost", txtInputControl.Parent.TabIndex) /
                    Average("Capacity", txtInputControl.Parent.TabIndex), 2);
            }
        }

        private void txtCost_DoubleClick(object sender, EventArgs e)
        {
            txtCost.Text = Math.Round(Convert.ToDouble(txtCapacity.Text) * 
                Math.Round(Average("Cost", txtCapacity.Parent.TabIndex) /
                Average("Capacity", txtCapacity.Parent.TabIndex), 2), 2).ToString();

            lblCostPerUnit.Visible = false;
        }
        
        void SetDefaults()
        {
            txtCapacity.Text = Average("Capacity", txtCapacity.Parent.TabIndex).ToString();
            txtCost.Text = Average("Cost", txtCapacity.Parent.TabIndex).ToString();
            txtDetails.Clear();
        }

        bool ISUpdatedMainData(int PlantID, Guna2Button UpdateStatus)
        {
            List<string> TodayCapacity = sqlFunctions.SQLRead("SELECT Capacity FROM Production " +
                "WHERE PlantID='" + PlantID.ToString() + "' AND " +
                "Date='" + SelectedDate() + "'", "Capacity");
            if (TodayCapacity == null) 
            {
                NotUpdated();
                return false;
            }
            else if (TodayCapacity[0].Trim() == "")
            {
                NotUpdated();
                return false;
            }
            else
            {
                UpdateStatus.CheckedState.FillColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(110)))), ((int)(((byte)(179)))));
                UpdateStatus.Text = "Updated";
                return true;
            }

            void NotUpdated()
            {
                UpdateStatus.CheckedState.FillColor = Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
                UpdateStatus.Text = "Not Updated";
            }
        }

        void SetUpdatedData()
        {
            if (ISUpdatedMainData(txtCapacity.Parent.TabIndex, UpdatedStatusButton[ListOfBtnFlowUpDown[txtCapacity.Parent.TabIndex]]))
            {
                try
                {
                    List<string> UpdatedData = sqlFunctions.SQLRead("SELECT Capacity,Cost,Details FROM Production " +
                        "WHERE PlantID='" + txtCapacity.Parent.TabIndex + "' AND Date='" + SelectedDate() + "'", "Capacity Cost Details");

                    txtCapacity.Text = UpdatedData[0].Trim();
                    txtCost.Text = UpdatedData[1].Trim();
                    txtDetails.Text = UpdatedData[2].Trim();
                }
                catch
                {
                    SetDefaults();
                }
            }
            else
            {
                SetDefaults();
            }
        }

        void ChangedSelectedPlant()
        {
            ClearData();
            try
            {
                TrackBarCapacity.Maximum = Convert.ToInt32(Convert.ToDouble(
                    sqlFunctions.SQLRead("SELECT MAX(Capacity) AS MaximumCapacity FROM Production " +
                    "WHERE PlantID='" + txtCapacity.Parent.TabIndex + "'", "MaximumCapacity")[0]));
            }
            catch
            {
                TrackBarCapacity.Maximum = 60000;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<string> CurrentStatus = sqlFunctions.SQLRead("SELECT Capacity " +
                "FROM Production WHERE PlantID='" + btnUpdate.Parent.TabIndex + "' " +
                "AND Date='" + SelectedDate() + "'", "Capacity");

            if (CurrentStatus == null) 
            {
                if (sqlFunctions.ExecuteSQL("INSERT INTO Production ([PlantID], [Date], [Capacity], [Cost], [Details]) " +
                    "VALUES ('" + btnUpdate.Parent.TabIndex + "','" + SelectedDate() + "','" + txtCapacity.Text + "'," +
                    "'" + txtCost.Text + "',N'" + txtDetails.Text.Trim() + "') "))
                {
                    MessageBox.Show("Updated Data Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (sqlFunctions.ExecuteSQL("UPDATE Production SET [Capacity]='" + txtCapacity.Text + "'," +
                    "[Cost]='" + txtCost.Text + "',[Details]=N'" + txtDetails.Text.Trim() + "' " +
                    "WHERE [Date]='" + SelectedDate() + "' AND PlantID='" + txtCapacity.Parent.TabIndex + "'")) 
                {
                    MessageBox.Show("Updated Data Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnDefualt_Click(object sender, EventArgs e)
        {
            SetDefaults();
            lblCostPerUnit.Visible = true;
        }

        private void TrackBarCapacity_ValueChanged(object sender, EventArgs e)
        {
            if (txtCapacity.Text != "")
            {
                if (Math.Round(Convert.ToDouble(txtCapacity.Text), 0) != TrackBarCapacity.Value 
                    && Math.Round(Convert.ToDouble(txtCapacity.Text), 0) < TrackBarCapacity.Maximum) 
                {
                    txtCapacity.Text = TrackBarCapacity.Value.ToString();
                }
            }            
        }

        private void btnUpdateTestResults_Click(object sender, EventArgs e)
        {
            DateTime CurrentDate = txtDate.Value;
            TabBtnTest.Checked = true;
            txtDate.Value = CurrentDate;
            ListTestTabbtnPlant[btnUpdateTestResults.Parent.TabIndex].Checked = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// Content of Test page
        /// </summary>

        List<Guna2Button> ListTestTabbtnPlant;
        List<Guna2TextBox> ListUserInputsForTestingParameters;
        List<Guna2HtmlLabel> ListlblQualityCheck;

        private void btnSelectPlantForWaterTest_CheckChanged(object sender, EventArgs e)
        {
            ClearData();

            Guna2Button btnSelectedPlant = (Guna2Button)sender;

            List<string> CurrentTestData = sqlFunctions.SQLRead("SELECT PH,EC,TDS,Tur,DO,Al,Fe,RCI,Colour,Temp " +
                "FROM Production WHERE PlantID='" + btnSelectedPlant.Tag + "' AND Date='" + SelectedDate() + "'",
                "PH EC TDS Tur DO Al Fe RCI Colour Temp");

            if (CurrentTestData != null)
            {
                for (int i = 0; i < CurrentTestData.Count; i++) 
                {
                    ListUserInputsForTestingParameters[i].Text = CurrentTestData[i].Trim();
                }

                for (int i = 0; i < ListUserInputsForTestingParameters.Count; i++)
                {
                    if (ListUserInputsForTestingParameters[i].Text == "")
                        continue;

                    CheckWaterQuality();
                    break;
                }                
            }
        }

        Guna2Button SelectedTestTabButton()
        {
            foreach (Guna2Button SelectedButton in ListTestTabbtnPlant)
            {
                if (SelectedButton.Checked)
                    return SelectedButton;
            }
            return ListTestTabbtnPlant[0];
        }

        private void btnCheckWaterData_Click(object sender, EventArgs e)
        {
            if (CheckWaterQuality())
            {
                SumbitToDatabase();
            }
            else
            {
                if (MessageBox.Show("Some input are out of allowable limit." +
                    "Do you want to save data in database?", "Information",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SumbitToDatabase();
                }
            }

            void SumbitToDatabase()
            {
                List<string> CurrentTestData = sqlFunctions.SQLRead("SELECT PH,EC,TDS,Tur,DO,Al,Fe,RCI,Colour,Temp " +
                    "FROM Production WHERE PlantID='" + SelectedTestTabButton().Tag + "' AND Date='" + SelectedDate() + "'",
                    "PH EC TDS Tur DO Al Fe RCI Colour Temp");

                string Query;

                if (CurrentTestData == null)
                {
                    Query = "INSERT INTO Production ([PlantID], [Date], [PH], [EC], [TDS], [Tur], [DO], [Al], [Fe], [RCI], [Colour], [Temp])" +
                        " Values('" + SelectedTestTabButton().Tag + "','" + SelectedDate() + "',";

                    foreach (Guna2TextBox InputText in ListUserInputsForTestingParameters)
                    {
                        Query += "'" + InputText.Text.Trim() + "',";
                    }

                    Query = Query.Remove(Query.Length - 1) + ")";
                }
                else
                {
                    Query = "UPDATE Production SET ";

                    foreach (Guna2TextBox InputText in ListUserInputsForTestingParameters)
                    {
                        Query += "[" + InputText.Tag.ToString() + "]='" + InputText.Text.Trim() + "',";
                    }
                    Query = Query.Remove(Query.Length - 1) +
                        " WHERE PlantID='" + SelectedTestTabButton().Tag.ToString() +
                        "' AND Date='" + SelectedDate() + "'";
                }

                sqlFunctions.ExecuteSQL(Query);
            }
        }

        bool CheckWaterQuality()
        {
            RatingStarQualityOfWaterSelectedDate.Value = 0;
            RatingStarQualityOfWaterSelectedDate.Tag = 0;

            for (int i = 0; i < ListUserInputsForTestingParameters.Count; i++)
            {
                if (ListUserInputsForTestingParameters[i].Text.Trim() != "")
                {
                    CheckRange(ListUserInputsForTestingParameters[i].Tag.ToString(),
                        float.Parse(ListUserInputsForTestingParameters[i].Text),
                        ListlblQualityCheck[i], RatingStarQualityOfWaterSelectedDate, page5);
                }
                else
                {
                    ListlblQualityCheck[i].Text = "■";
                }
            }

            lblRating.Text = RatingStarQualityOfWaterSelectedDate.Value.ToString();
            lblPassed.Text = (float.Parse(lblRating.Text) * 2).ToString() + "/10";
            lblFaild.Text = (10 - float.Parse(lblRating.Text) * 2).ToString() + "/10";

            if (float.Parse(lblRating.Text) > 3)
            {
                lblDescribtion.Text = "Water is ready to distribution. <br>Quality of water sample is higher <br> than average quality.";
                return true;
            }
            else
            {
                lblDescribtion.Text = "Water unqualified to destribution!! <br>Check Parameters which are in <br>out of allowable limit.";
                return false;
            }
        }

        void CheckRange(string ParameterName, float Value, Control AffectedControl, Guna2RatingStar QualityOfWater,Page CurrentPage)
        {
            void ChangeLblText(string Place)
            {
                if (Place == null)
                {
                    AffectedControl.Text = "■";
                }
                else if (Place == "UP")
                {
                    AffectedControl.Text = "▲";
                }
                else if (Place == "DOWN") 
                {
                    AffectedControl.Text = "▼";
                }
                else
                {
                    AffectedControl.Text = "";
                    QualityOfWater.Value += 0.5f;
                    QualityOfWater.Tag = QualityOfWater.Value;
                }
            }

            void ChangeLblColor(string Place)
            {                
                if (Place == "UP")
                {
                    AffectedControl.ForeColor = Color.White;
                    AffectedControl.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(115)))));
                }
                else if (Place == "DOWN")
                {
                    AffectedControl.ForeColor = Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(25)))), ((int)(((byte)(49)))));
                    AffectedControl.BackColor = Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(185)))), ((int)(((byte)(246)))));
                }
                else
                {
                    AffectedControl.ForeColor = Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(25)))), ((int)(((byte)(49)))));
                    AffectedControl.BackColor = Color.Transparent;
                    QualityOfWater.Value += 0.5f;
                    QualityOfWater.Tag = QualityOfWater.Value;
                }
            }

            if (CurrentPage == page5)
            {
                ChangeLblText(multiFunctions.CheckRange(ParameterName, Value));
            }
            else if (CurrentPage == page2)
            {
                ChangeLblColor(multiFunctions.CheckRange(ParameterName, Value));
            }
        }

        void IsUpdatedTestData()
        {
            int UpdatedRowCount = 0;

            for (int i = 0; i < 4; i++)
            {
                txtDate.Value = DateTime.Now;

                List<string> CurrentTestData = sqlFunctions.SQLRead("SELECT PH,EC,TDS,Tur,DO,Al,Fe,RCI,Colour,Temp " +
                "FROM Production WHERE PlantID='" + i + "' AND Date='" + SelectedDate() + "'",
                "PH EC TDS Tur DO Al Fe RCI Colour Temp");

                if (CurrentTestData != null)
                {
                    foreach (string TestData in CurrentTestData)
                    {
                        if (TestData.Trim() != "")
                        {
                            UpdatedRowCount += 1;
                            break;
                        }
                    }
                }
            }

            if (UpdatedRowCount == 4)
            {
                NotificationTestResults.Visible = false;
            }
        }

        private void txtInputTestResults_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox txtInputTestResults = (Guna2TextBox)sender;
            txtInputTestResults.Text = multiFunctions.TypeCheckInput(txtInputTestResults.Text, "N.F", 3);
        }

        /// <summary>
        /// Content of More Details page
        /// </summary>

        Dictionary<int, List<Control>> ListMoreDetailsControl;
        List<Guna2RatingStar> ListMoreDetailsRatingStars;
         
        void UpdateMoreDetailsPage()
        {
            lblTotalDailyCapacity.Text = lblTotalDailyCost.Text = "0";

            foreach (Guna2RatingStar item in ListMoreDetailsRatingStars)
            {
                item.Value = 0;
                item.Tag = 0;
            }


            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j < 13; j++)
                {
                    ListMoreDetailsControl[i][j].ForeColor = Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(25)))), ((int)(((byte)(49)))));
                    ListMoreDetailsControl[i][j].BackColor = Color.Transparent;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                List<string> ProductionDetails = sqlFunctions.SQLRead(
                    "SELECT Capacity,Cost,Details,PH,EC,TDS,Tur,DO,Al,Fe,RCI,Colour,Temp " +
                    "FROM Production WHERE PlantID='" + i + "' AND Date='" + SelectedDate() + "'",
                    "Capacity Cost Details PH EC TDS Tur DO Al Fe RCI Colour Temp");

                if (ProductionDetails != null)
                {
                    for (int j = 0; j < ProductionDetails.Count; j++)
                    {
                        if (ProductionDetails[j].Trim() == "" && j != 2) 
                        {
                            ListMoreDetailsControl[i][j].Text = "N/A";
                        }
                        else
                        {
                            ListMoreDetailsControl[i][j].Text = ProductionDetails[j].Trim();
                                                        
                            if (j == 0)
                            {
                                lblTotalDailyCapacity.Text = (float.Parse(lblTotalDailyCapacity.Text) +
                                    float.Parse(ProductionDetails[j].Trim())).ToString();

                                ListMoreDetailsControl[i][j].Text = multiFunctions.SplitText(ListMoreDetailsControl[i][j].Text) + " m<sup>3</sup>";

                            }
                            else if (j == 1)
                            {
                                lblTotalDailyCost.Text = (float.Parse(lblTotalDailyCost.Text) +
                                    float.Parse(ProductionDetails[j].Trim())).ToString();


                                ListMoreDetailsControl[i][j].Text = "Rs. " + multiFunctions.SplitText(ListMoreDetailsControl[i][j].Text);
                            }
                            else if (j >= 3)
                            {
                                CheckRange(ListMoreDetailsControl[i][j].Tag.ToString(), 
                                    float.Parse(ListMoreDetailsControl[i][j].Text), 
                                    ListMoreDetailsControl[i][j], 
                                    ListMoreDetailsRatingStars[i], page2);
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 13; j++)
                    {
                        ListMoreDetailsControl[i][j].Text = "N/A";
                    }
                }
            }

            lblTotalDailyCapacity.Text = multiFunctions.SplitText(lblTotalDailyCapacity.Text) + " m<sup>3</sup>";
            lblTotalDailyCost.Text = "Rs. " + multiFunctions.SplitText(lblTotalDailyCost.Text);
        }

        /// <summary>
        /// UI,UX and content of production history page
        /// </summary>
        /// <param SelectedMonth="null"></param>
        /// <param SelectedParameter="null"></param>
        /// <param SelectedSection="null"></param>

        string SelectedMonth = "1", SelectedParameter = "PH", SelectedSection = "Capacity";
        List<Guna2Button> ListbtnSelectMonth;

        private void btnHistoryTabButton_CheckChange(object sender, EventArgs e)
        {
            Guna2Button btnHistoryTabButton = (Guna2Button)sender;
            HistoryChartNavigationButtonReset();
            SelectedSection = btnHistoryTabButton.Tag.ToString();

            switch (SelectedSection)
            {
                case "Capacity":
                    {
                        btnHistoryChartCapacity.CustomizableEdges.BottomLeft = false;
                        btnHistoryChartCapacity.CustomizableEdges.BottomRight = false;
                        PanelLeftMid.CustomizableEdges.BottomLeft = true;
                        PanelLeft.CustomizableEdges.BottomRight = true;
                        ChartProductionHistory.Dock = DockStyle.Fill;
                        DrawProgressHistoryChart();
                        break;
                    }
                case "Cost":
                    {
                        btnHistoryChartCost.CustomizableEdges.BottomLeft = false;
                        btnHistoryChartCost.CustomizableEdges.BottomRight = false;
                        PanelRightMid.CustomizableEdges.BottomLeft = true;
                        PanelLeftMid.CustomizableEdges.BottomRight = true;
                        ChartProductionHistory.Dock = DockStyle.Fill;
                        DrawProgressHistoryChart();
                        break;
                    }
                case "TestResults":
                    {
                        btnHistoryChartAvg.CustomizableEdges.BottomLeft = false;
                        btnHistoryChartAvg.CustomizableEdges.BottomRight = false;
                        PanelRight.CustomizableEdges.BottomLeft = true;
                        PanelRightMid.CustomizableEdges.BottomRight = true;
                        ChartProductionHistory.Dock = DockStyle.Right;
                        btnSelectedTestingParameterPH.Checked = true;
                        break;
                    }
            }
        }

        void HistoryChartNavigationButtonReset()
        {
            btnHistoryChartAvg.CustomizableEdges.BottomLeft = btnHistoryChartAvg.CustomizableEdges.BottomRight =
                btnHistoryChartCost.CustomizableEdges.BottomLeft = btnHistoryChartCost.CustomizableEdges.BottomRight =
                btnHistoryChartCapacity.CustomizableEdges.BottomLeft = btnHistoryChartCapacity.CustomizableEdges.BottomRight = true;

            PanelLeft.CustomizableEdges.BottomRight = PanelLeftMid.CustomizableEdges.BottomLeft =
                PanelLeftMid.CustomizableEdges.BottomRight = PanelRightMid.CustomizableEdges.BottomLeft =
                PanelRightMid.CustomizableEdges.BottomRight = PanelRight.CustomizableEdges.BottomLeft = false;
        }

        private void btnChangeYearButton_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnChangeYear = (Guna2CircleButton)sender;

            if (btnChangeYear.Tag.ToString() == "Pre")
            {
                if (Convert.ToInt32(SelectedYear.Text) >= 2021)
                {
                    SelectedYear.Text = (Convert.ToInt32(SelectedYear.Text) - 1).ToString();
                    DrawProgressHistoryChart();
                }
            }
            else
            {
                if (Convert.ToInt32(SelectedYear.Text) <= 2029)
                {
                    SelectedYear.Text = (Convert.ToInt32(SelectedYear.Text) + 1).ToString();
                    DrawProgressHistoryChart();
                }
            }
        }

        private void btnChangeSelectedMonth_CheckedChanged(object sender, EventArgs e)
        {
            Guna2Button btnChangeSelectedMonth = (Guna2Button)sender;
            SelectedMonth = btnChangeSelectedMonth.Tag.ToString();

            try
            {
                lblTotalCapacity.Text = multiFunctions.SplitText(sqlFunctions.SQLRead("SELECT " +
                                "SUM(Capacity) AS TotalProduction FROM Production " +
                                "WHERE Date LIKE '" + SelectedYear.Text + "-" + SelectedMonth + "-%'",
                                "TotalProduction")[0].Trim()) + " m<sup>3</sup>";

                lblTotalCost.Text = "Rs. " + multiFunctions.SplitText(sqlFunctions.SQLRead("SELECT " +
                    "SUM(Cost) AS TotalProduction FROM Production " +
                    "WHERE Date LIKE '" + SelectedYear.Text + "-" + SelectedMonth + "-%'",
                    "TotalProduction")[0].Trim());
            }
            catch
            {
                lblTotalCapacity.Text = "0 m<sup>3</sup>";
                lblTotalCost.Text = "Rs. 0";
            }

            DrawProgressHistoryChart();
        }

        private void btnSelectedTestingParameter_CheckedChanged(object sender, EventArgs e)
        {
            Guna2Button btnSelectedTestingParameter = (Guna2Button)sender;
            SelectedParameter = btnSelectedTestingParameter.Text.Trim();
            DrawProgressHistoryChart();
        }

        void DrawProgressHistoryChart()
        {
            ChartProductionHistory.Series.Clear();

            void AddDataIntoChart(string SeriesName, SortedDictionary<string, float> AffectedDictionary, List<string> DataList, int SeriesIndex)
            {
                if (SeriesIndex >= ChartProductionHistory.Series.Count && SeriesIndex != 0) 
                    SeriesIndex = ChartProductionHistory.Series.Count;

                // Add Series to chart
                ChartProductionHistory.Series.Add(SeriesName);
                ChartProductionHistory.Series[SeriesIndex].ChartType = SeriesChartType.Spline;

                // Add production deatils to dictionary ad count total
                for (int i = 0; i < DataList.Count; i += 2)
                {
                    if (DataList[i + 1].Trim() == "" | DataList[i].Trim() == "")
                    {
                        continue;
                    }

                    try
                    {
                        AffectedDictionary.Add(DataList[i + 1].Trim(), float.Parse(DataList[i].Trim()));
                    }
                    catch
                    {
                        AffectedDictionary[DataList[i + 1].Trim()] += float.Parse(DataList[i].Trim());
                    }
                }

                // Enter data into chart form dictionary 
                string IndexDate;

                for (int i = 1; i <= multiFunctions.NoOfDays(Convert.ToInt32(SelectedMonth)); i++)
                {
                    IndexDate = SelectedYear.Text + "-" + SelectedMonth + "-" + i.ToString();

                    try
                    {
                        ChartProductionHistory.Series[SeriesIndex].Points.AddXY(IndexDate, AffectedDictionary[IndexDate]);
                    }
                    catch
                    {
                        ChartProductionHistory.Series[SeriesIndex].Points.AddXY(IndexDate, 0);
                    }
                }

                // Change chart style
                ChartProductionHistory.Series[SeriesIndex].ShadowColor = Color.Gainsboro;
                ChartProductionHistory.Series[SeriesIndex].ShadowOffset = 1;
                ChartProductionHistory.Series[SeriesIndex].MarkerStyle = MarkerStyle.Circle;
            }

            if (SelectedSection == "Capacity" || SelectedSection == "Cost") 
            {
                List<string> MonthlyProductionDetails = sqlFunctions.SQLRead("SELECT " + SelectedSection + ",Date FROM Production " +
                    "WHERE Date LIKE '" + SelectedYear.Text + "-" + SelectedMonth + "-%'", SelectedSection + " Date");

                SortedDictionary<string, float> MonthlyProductionDetailsByDate = new SortedDictionary<string, float> { };

                if (MonthlyProductionDetails != null)
                {
                    AddDataIntoChart("Production " + SelectedSection, 
                        MonthlyProductionDetailsByDate, MonthlyProductionDetails, 0);
                }
            }
            else
            {
                List<string> VariationTestingParameter;
                SortedDictionary<string, float> VariationTestingParameterByDate;

                for (int i = 0; i < 4; i++)
                {
                    VariationTestingParameter = sqlFunctions.SQLRead("SELECT " + SelectedParameter + ",Date FROM Production " +
                        "WHERE Date LIKE '" + SelectedYear.Text + "-" + SelectedMonth + "-%' " +
                        "AND PlantID='" + i + "'", SelectedParameter + " Date");

                    if (VariationTestingParameter != null)
                    {
                        VariationTestingParameterByDate = new SortedDictionary<string, float> { };

                        AddDataIntoChart(PlantNames[i], VariationTestingParameterByDate, VariationTestingParameter, i);
                    }
                }
            }
        }

        /// <summary>
        /// Home Page content
        /// </summary>

        Dictionary<int, List<Guna2ProgressBar>> ListHomeProgressBars;
        Dictionary<int, List<Guna2HtmlLabel>> ListLabelsOfHomeProgressBarValues;

        void UpdateHomePageProductionValues()
        {
            // Set Date as yesterday
            txtDate.Value = DateTime.Today.AddDays(-1);

            // update summery details
            lblYesterdayProducedCapacity.Text = "&nbsp; &nbsp; 0 m<sup>3</sup>";
            lblDifference.Text = "-100%";

            double AverageProductionCapacity = Math.Round(Average("Capacity", -1));
            lblAverageProduction.Text = multiFunctions.SplitText((AverageProductionCapacity).ToString()) + "m<sup>3</sup>";

            List<string> TotalProduction = sqlFunctions.SQLRead("SELECT SUM(Capacity) AS TotalProduction " +
                "FROM Production WHERE Date='" + SelectedDate() + "'", "TotalProduction");
            if (TotalProduction != null)
            {
                if (TotalProduction[0] != "")
                {
                    lblYesterdayProducedCapacity.Text = 
                        multiFunctions.SplitText((Math.Round(Convert.ToDouble(TotalProduction[0]))).ToString()) + " m<sup>3</sup>";

                    lblDifference.Text = "&nbsp;" + Math.Round(
                        (Convert.ToDouble(TotalProduction[0]) - AverageProductionCapacity) /
                        AverageProductionCapacity * 100).ToString() + "%";

                    if (lblDifference.Text.Trim() == "0")
                        lblDifference.Text = "&nbsp; &nbsp; 0%";
                }
            }

            // update details about Production plant wise
            double AveragePlantProduction, SelectedDatePlantProduction, MaxValue;
            List<string> ReturnListSDPP; // SDPP = Selected Date Palnt Production

            for (int i = 0; i < 4; i++)
            {
                AveragePlantProduction = Average("Capacity", i);
                ReturnListSDPP = sqlFunctions.SQLRead("SELECT Capacity FROM Production " +
                    "WHERE PlantID='" + i + "' AND Date='" + SelectedDate() + "'", "Capacity");

                if (ReturnListSDPP != null)
                {
                    if (ReturnListSDPP[0] != "")
                    {
                        SelectedDatePlantProduction = Convert.ToDouble(ReturnListSDPP[0]);

                        if (AveragePlantProduction > SelectedDatePlantProduction)
                            MaxValue = AveragePlantProduction;
                        else
                            MaxValue = SelectedDatePlantProduction;

                        ListHomeProgressBars[i][0].Maximum = 
                            ListHomeProgressBars[i][1].Maximum = Convert.ToInt32(MaxValue);

                        ListHomeProgressBars[i][0].Value = Convert.ToInt32(AveragePlantProduction);
                        ListHomeProgressBars[i][1].Value = Convert.ToInt32(SelectedDatePlantProduction);

                        ListLabelsOfHomeProgressBarValues[i][0].Text = multiFunctions.SplitText(
                            ListHomeProgressBars[i][0].Value.ToString()) + " m<sup>3</sup>";
                        ListLabelsOfHomeProgressBarValues[i][1].Text = multiFunctions.SplitText(
                            ListHomeProgressBars[i][1].Value.ToString()) + " m<sup>3</sup>";

                        continue;
                    }
                }

                ListHomeProgressBars[i][0].Maximum = ListHomeProgressBars[i][0].Value = Convert.ToInt32(AveragePlantProduction);
                ListHomeProgressBars[i][1].Value = 0;

                ListLabelsOfHomeProgressBarValues[i][0].Text = multiFunctions.SplitText(
                            ListHomeProgressBars[i][0].Value.ToString());

                ListLabelsOfHomeProgressBarValues[i][1].Text = "N/A";
            }

            DrowChartPlantWiseProductionCapacityHistory();
        }

        void DrowChartPlantWiseProductionCapacityHistory()
        {
            for (int j = 0; j < 4; j++)
                ChartPlantWiseProductionHistory.Series[j].Points.Clear();

            Dictionary<string, List<string>> ChartPoints = new Dictionary<string, List<string>> { };
            List<string> ReturnData;

            for (int i = -7; i < 0; i++)
            {
                txtDate.Value = DateTime.Today.AddDays(i);
                ChartPoints.Add(SelectedDate(), new List<string> { "0", "0", "0", "0" });

                for (int j = 0; j < 4; j++) 
                {
                    ReturnData = sqlFunctions.SQLRead("SELECT Capacity FROM Production " +
                        "WHERE PlantID='" + j + "' AND Date='" + SelectedDate() + "'", "Capacity");

                    if (ReturnData != null)
                    {
                        if (ReturnData[0] != "")
                        {
                            ChartPoints[SelectedDate()][j] = ReturnData[0];
                        }
                    }
                }
            }

            for (int j = 0; j < 4; j++)
            {
                foreach (KeyValuePair<string, List<string>> ChartPointIndex in ChartPoints)
                {
                    ChartPlantWiseProductionHistory.Series[j].Points.AddXY(ChartPointIndex.Key, ChartPointIndex.Value[j]);
                }

                ChartPlantWiseProductionHistory.Series[j].MarkerStyle = MarkerStyle.Circle;
            }

            txtDate.Value = DateTime.Today.AddDays(-1);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            multiFunctions.NavigateTo(btnClose.Tag.ToString(), this);
        }
    }
}
