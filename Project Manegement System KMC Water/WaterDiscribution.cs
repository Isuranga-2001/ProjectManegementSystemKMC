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
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Diagnostics;

namespace Project_Manegement_System_KMC_Water
{
    public partial class WaterDiscribution : Form
    {
        public WaterDiscribution()
        {
            InitializeComponent();
        }

        SQLFunctions sqlFunctions = new SQLFunctions();
        MultiFunctions multiFunctions = new MultiFunctions();

        List<Control> ListAvailableSeriesTag = new List<Control> { };
        bool ChartDataShowAsAverage = true;
        int StartMonth = -12;

        List<string> XValueList = new List<string>();

        private void WaterDiscribution_Load(object sender, EventArgs e)
        {
            PanelAvailableChartSeries.Visible = ListBoxSearchResults.Visible = false;
            btnMonthlyConsumption.Checked = true;
            ChartAreaViseConsumption.Series.Clear();

            AddToChartAreaViseConsumption("TOWN", true);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(1280, 720);
        }

        void DrawChartProductionAndConsumption()
        {
            ChartProductionHistory.Series[0].Points.Clear();
            ChartProductionHistory.Series[1].Points.Clear();
            XValueList = new List<string> { };

            List<string> VariationProductionCapacity;

            for (int j = StartMonth; j < 0; j++)
            {
                XValueList.Add(DateTime.Today.AddMonths(j).Year.ToString() + "-" + DateTime.Today.AddMonths(j).Month.ToString());

                // Draw Production Series

                VariationProductionCapacity = sqlFunctions.SQLRead(
                    "SELECT SUM(Capacity) AS TotalProduction FROM Production " +
                    "WHERE Date LIKE '" + DateTime.Today.AddMonths(j).Year + "-" +
                    DateTime.Today.AddMonths(j).Month + "-%'", "TotalProduction");
                                
                if (VariationProductionCapacity != null)
                {
                    if (VariationProductionCapacity[0].Trim() != "")
                    {
                        if (ChartDataShowAsAverage)
                        {
                            ChartProductionHistory.Series[0].Points.AddXY(
                                DateTime.Today.AddMonths(j).Year.ToString() + "-" +
                                DateTime.Today.AddMonths(j).Month.ToString(),
                                Convert.ToDouble(VariationProductionCapacity[0].Trim()) /
                                multiFunctions.NoOfDays(DateTime.Today.AddMonths(j).Month));
                        }
                        else
                        {
                            ChartProductionHistory.Series[0].Points.AddXY(
                                DateTime.Today.AddMonths(j).Year.ToString() + "-" + 
                                DateTime.Today.AddMonths(j).Month.ToString(), 
                                Convert.ToDouble(VariationProductionCapacity[0].Trim()));
                        }
                    }
                    else
                        AddZeroIntoSeries(0);
                }
                else
                    AddZeroIntoSeries(0);

                // Draw Comsumption Series

                VariationProductionCapacity = sqlFunctions.SQLRead(
                    "SELECT SUM(Capacity) AS TotalConsumption FROM Consumption " +
                    "WHERE Month LIKE '" + DateTime.Today.AddMonths(j).Year + "-" +
                    DateTime.Today.AddMonths(j).Month + "%'", "TotalConsumption");

                if (VariationProductionCapacity != null)
                {
                    if (VariationProductionCapacity[0].Trim() != "")
                    {
                        if (ChartDataShowAsAverage)
                        {
                            ChartProductionHistory.Series[1].Points.AddXY(
                                DateTime.Today.AddMonths(j).Year.ToString() + "-" +
                                DateTime.Today.AddMonths(j).Month.ToString(),
                                Convert.ToDouble(VariationProductionCapacity[0].Trim()) /
                                multiFunctions.NoOfDays(DateTime.Today.AddMonths(j).Month));
                        }
                        else
                        {
                            ChartProductionHistory.Series[1].Points.AddXY(
                                DateTime.Today.AddMonths(j).Year.ToString() + "-" +
                                DateTime.Today.AddMonths(j).Month.ToString(),
                                Convert.ToDouble(VariationProductionCapacity[0].Trim()));
                        }
                    }
                    else
                        AddZeroIntoSeries(1);
                }
                else
                    AddZeroIntoSeries(1);

                void AddZeroIntoSeries(int SeriesID)
                {
                    ChartProductionHistory.Series[SeriesID].Points.AddXY(
                        DateTime.Today.AddMonths(j).Year.ToString() +
                        "-" + DateTime.Today.AddMonths(j).Month.ToString(), 1);
                }
            }
        }

        private void btnChart_CheckedChanged(object sender, EventArgs e)
        {
            Guna2Button btnChartDataShowType = (Guna2Button)sender;

            if (btnChartDataShowType.Checked)
            {
                btnChartAverage.ShadowDecoration.Enabled =
                    btnChartTotal.ShadowDecoration.Enabled = false;

                btnChartDataShowType.ShadowDecoration.Enabled = true;

                if (btnChartDataShowType.Text == btnChartAverage.Text)
                    ChartDataShowAsAverage = true;
                else
                    ChartDataShowAsAverage = false;

                DrawChartProductionAndConsumption();
            }                       
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            ImportFile form = new ImportFile();
            form.ShowDialog();
        }

        private void btnPageNavigation_CheckChanged(object sender, EventArgs e)
        {
            Guna2Button btnPageNavigation = (Guna2Button)sender;

            if (btnPageNavigation.Checked)
            {
                btnShowHidePanelSeries.Visible = ListBoxSearchResults.Visible = false;

                if (btnPageNavigation.Tag.ToString() == "Total")
                {
                    pagedControl1.SelectedPage = page1;
                    btnChartAverage.Checked = true;
                    btnShowHidePanelSeries.Checked = false;

                    ChartProductionHistory.Titles[0].Text =
                        "Monthly Water Production And Consumption In Last " + (StartMonth * -1).ToString() + " Months";
                }
                    
                else
                {
                    pagedControl1.SelectedPage = page2;
                    txtAreaSearch.Clear();
                    btnShowHidePanelSeries.Visible = true;

                    ResetAreaViseConsumptionChart();
                }
            }
        }

        private void ChartProductionHistory_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) 
            {
                MainContextMenuStrip.Show();
                MainContextMenuStrip.Left = MousePosition.X;
                MainContextMenuStrip.Top = MousePosition.Y;
            }
        }

        private void MenuStripChangeStartTime_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            StartMonth = Convert.ToInt32(e.ClickedItem.Tag);

            if (pagedControl1.SelectedPage == page1)
            {
                DrawChartProductionAndConsumption();
                ChartProductionHistory.Titles[0].Text =
                    "Monthly Water Production And Consumption In Last " + (StartMonth * -1).ToString() + " Months";
            }
            else
            {
                ResetAreaViseConsumptionChart();
            }
        }

        void ResetAreaViseConsumptionChart()
        {
            ChartAreaViseConsumption.Series.Clear();

            foreach (Control AvailableTagChip in ListAvailableSeriesTag)
            {
                AddToChartAreaViseConsumption(AvailableTagChip.Tag.ToString(), false);
            }

            if (ChartDataShowAsAverage)
                ChartAreaViseConsumption.Titles[0].Text = "Average ";
            else
                ChartAreaViseConsumption.Titles[0].Text = "Monthly ";


            ChartAreaViseConsumption.Titles[0].Text += "Area Vise Water Consumption In Last " +
                (StartMonth * -1).ToString() + " Months";
        }

        private void txtAreaSearch_TextChanged(object sender, EventArgs e)
        {
            ListBoxSearchResults.Visible = true;
            if (txtAreaSearch.Text != "") 
            {
                ListBoxSearchResults.DataSource = sqlFunctions.SQLRead("SELECT LocationName " +
                    "FROM Area Where LocationName Like '%" + txtAreaSearch.Text.Trim() + "%'",
                    "LocationName");
            }
            else
            {
                ListBoxSearchResults.Visible = false;
            }
        }

        private void txtAreaSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (ListBoxSearchResults.Items.Count > 1)
            {
                if (e.KeyCode == Keys.Down && ListBoxSearchResults.SelectedIndex + 1 != ListBoxSearchResults.Items.Count) 
                {
                    ListBoxSearchResults.SelectedIndex += 1;
                }   

                if (ListBoxSearchResults.SelectedIndex > 0 && e.KeyCode == Keys.Up)
                {
                    ListBoxSearchResults.SelectedIndex -= 1;
                }

                if (e.KeyCode == Keys.Enter)
                {
                    ListBoxSearchResults_Click(ListBoxSearchResults, null);
                }
            }
        }

        private void ListBoxSearchResults_Click(object sender, EventArgs e)
        {
            txtAreaSearch.Text = "";
            ListBoxSearchResults.Visible = false;
            AddToChartAreaViseConsumption((ListBoxSearchResults.SelectedItem).ToString().Trim(), true);
        }

        void AddToChartAreaViseConsumption(string AreaName, bool ShowTag)
        {
            try
            {
                ChartAreaViseConsumption.Series.Add(AreaName);
                ChartAreaViseConsumption.Series[AreaName].ChartType = SeriesChartType.Spline;
                ChartAreaViseConsumption.Series[AreaName].MarkerStyle = MarkerStyle.Circle;
                ChartAreaViseConsumption.Series[AreaName].ToolTip = AreaName;

                if (ChartAreaViseConsumption.Series.Count == 1 && pagedControl1.SelectedPage == page2)
                    XValueList = new List<string> { };

                List<string> VariationProductionCapacity;

                string AreaID = sqlFunctions.SQLRead("SELECT AreaID FROM Area " +
                    "WHERE LocationName='" + AreaName.Trim() + "'", "AreaID")[0];

                for (int j = StartMonth; j < 0; j++)
                {
                    if (ChartAreaViseConsumption.Series.Count == 1 && pagedControl1.SelectedPage == page2)
                        XValueList.Add(DateTime.Today.AddMonths(j).Year.ToString() + "-" + DateTime.Today.AddMonths(j).Month.ToString());

                    VariationProductionCapacity = sqlFunctions.SQLRead("SELECT Capacity FROM Consumption " +
                        "WHERE AreaID='" + AreaID + "' AND Month='" + DateTime.Today.AddMonths(j).Year + "-" +
                        DateTime.Today.AddMonths(j).Month + "'", "Capacity");

                    if (VariationProductionCapacity != null)
                    {
                        if (VariationProductionCapacity[0].Trim() != "")
                        {
                            if (ChartDataShowAsAverage)
                            {
                                ChartAreaViseConsumption.Series[AreaName].Points.AddXY(
                                    DateTime.Today.AddMonths(j).Year.ToString() + "-" +
                                    DateTime.Today.AddMonths(j).Month.ToString(),
                                    Convert.ToDouble(VariationProductionCapacity[0].Trim()) /
                                    multiFunctions.NoOfDays(DateTime.Today.AddMonths(j).Month));
                            }
                            else
                            {
                                ChartAreaViseConsumption.Series[AreaName].Points.AddXY(
                                    DateTime.Today.AddMonths(j).Year.ToString() + "-" +
                                    DateTime.Today.AddMonths(j).Month.ToString(),
                                    Convert.ToDouble(VariationProductionCapacity[0].Trim()));
                            }

                            continue;
                        }
                    }

                    ChartAreaViseConsumption.Series[AreaName].Points.AddXY(
                        DateTime.Today.AddMonths(j).Year.ToString() +
                        "-" + DateTime.Today.AddMonths(j).Month.ToString(), 0);
                }

                if (ShowTag)
                {
                    Guna2Chip ChipAvailableSeries = new Guna2Chip();
                    ChipAvailableSeries.AutoSize = true;
                    ChipAvailableSeries.BorderColor = Color.FromArgb(0, 71, 115);
                    ChipAvailableSeries.FillColor = Color.FromArgb(0, 110, 179);
                    ChipAvailableSeries.Font = new Font("Microsoft Sans Serif", 9.75F);
                    ChipAvailableSeries.TextAlign = HorizontalAlignment.Left;
                    ChipAvailableSeries.TextOffset = new Point(8, 0);
                    ChipAvailableSeries.Tag = ChipAvailableSeries.Text = AreaName;
                    ChipAvailableSeries.BorderRadius = 13;
                    ChipAvailableSeries.Margin = new Padding(20, 20, 10, 10);
                    ListAvailableSeriesTag.Add(ChipAvailableSeries);
                    FlowLayoutPanelAvailableSeries.Controls.Add(ChipAvailableSeries);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnShowHidePanelSeries_CheckedChanged(object sender, EventArgs e)
        {
            if (btnShowHidePanelSeries.Checked)
            {
                btnShowHidePanelSeries.Text = "▼";
                PanelAvailableChartSeries.Visible = true;
            }
            else
            {
                btnShowHidePanelSeries.Text = "▲";  
                PanelAvailableChartSeries.Visible = false;
            }
        }

        private void FlowLayoutPanelAvailableSeries_ControlRemoved(object sender, ControlEventArgs e)
        {
            ListAvailableSeriesTag.Remove(e.Control);
            ChartAreaViseConsumption.Series.Remove(ChartAreaViseConsumption.Series[e.Control.Tag.ToString()]);            
        }

        private void FormNavigationButton_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btnNavigation = (Guna2CircleButton)sender;
            multiFunctions.NavigateTo(btnNavigation.Tag.ToString(), this);
        }

        private void MenuStripDataShowType_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Total") 
            {
                btnChartTotal.Checked = true;
                btnChartAverage.Checked = false;
            }
            else
            {
                btnChartAverage.Checked = true;
                btnChartTotal.Checked = false;
            }  
            
            if (pagedControl1.SelectedPage == page2)
            {
                ResetAreaViseConsumptionChart();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (pagedControl1.SelectedPage == page1)
                ExportData(ChartProductionHistory);
            else
                ExportData(ChartAreaViseConsumption);
        }

        void ExportData(Chart SelectedChart)
        {
            if (SelectedChart.Series.Count > 0)
            {
                // create save file
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";

                if (SelectedChart.Name == ChartProductionHistory.Name)
                {
                    string DataType = "Total";
                    if (ChartDataShowAsAverage)
                        DataType = "Avarage";

                    sfd.FileName = DataType + " Production And Consumption Capacity From " + XValueList[0]
                        + " To " + XValueList[XValueList.Count - 1] + " .csv";
                }
                else
                {
                    sfd.FileName = "Area Wise Consumption Capacity From " + XValueList[0]
                        + " To " + XValueList[XValueList.Count - 1] + " .csv";
                }

                bool fileError = false;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." +
                                ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            string[] outputCsv = new string[SelectedChart.Series[0].Points.Count + 1];

                            outputCsv[0] += "Date,";
                            for (int i = 0; i < SelectedChart.Series.Count; i++)
                            {
                                outputCsv[0] += SelectedChart.Series[i].Name + ",";
                            }

                            for (int i = 0; i < SelectedChart.Series[0].Points.Count; i++) 
                            {
                                outputCsv[i + 1] += XValueList[i] + ",";

                                for (int j = 0; j < SelectedChart.Series.Count; j++)
                                {
                                    if (SelectedChart.Series[j].Points[i].YValues[0] != 1d) 
                                        outputCsv[i + 1] += SelectedChart.Series[j].Points[i].YValues[0] + ",";
                                    else
                                        outputCsv[i + 1] += "N/A,";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);

                            if (MessageBox.Show("Data exported successfully. Do you want to open it?", "Successfully Exported",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start(sfd.FileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export!, Chart Is Empty", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            multiFunctions.NavigateTo(btnClose.Tag.ToString(), this);
        }
    }
}
