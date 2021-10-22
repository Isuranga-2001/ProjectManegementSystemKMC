
namespace Project_Manegement_System___KMC
{
    partial class AddNewTableMode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewTableMode));
            this.TableResults = new Guna.UI2.WinForms.Guna2DataGridView();
            this.IndexNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstimatedMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Permission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MethodOfExecution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estimator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.ProgressBarWaiting = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.TableRightClickMenu = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            this.Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.Progress = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Search = new System.Windows.Forms.ToolStripMenuItem();
            this.Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemStartDate = new System.Windows.Forms.ToolStripMenuItem();
            this.StartDateMenu = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.SortByMenu = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDefaultData = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            this.btnSearch = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnProgress = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnResfesh = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnRemove = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnEdit = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnSave = new Guna.UI2.WinForms.Guna2CircleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TableResults)).BeginInit();
            this.TableRightClickMenu.SuspendLayout();
            this.StartDateMenu.SuspendLayout();
            this.SortByMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableResults
            // 
            this.TableResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.TableResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.TableResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TableResults.BackgroundColor = System.Drawing.Color.White;
            this.TableResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TableResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.TableResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TableResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.TableResults.ColumnHeadersHeight = 50;
            this.TableResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndexNo,
            this.Description,
            this.EstimatedMoney,
            this.Permission,
            this.MethodOfExecution,
            this.StartDate,
            this.Estimator,
            this.Column7,
            this.Column9});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TableResults.DefaultCellStyle = dataGridViewCellStyle3;
            this.TableResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.TableResults.EnableHeadersVisualStyles = false;
            this.TableResults.GridColor = System.Drawing.Color.LightSeaGreen;
            this.TableResults.Location = new System.Drawing.Point(0, 0);
            this.TableResults.MultiSelect = false;
            this.TableResults.Name = "TableResults";
            this.TableResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TableResults.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.TableResults.RowHeadersVisible = false;
            this.TableResults.RowTemplate.Height = 25;
            this.TableResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TableResults.Size = new System.Drawing.Size(984, 548);
            this.TableResults.TabIndex = 161;
            this.TableResults.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Teal;
            this.TableResults.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.LightBlue;
            this.TableResults.ThemeStyle.AlternatingRowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TableResults.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TableResults.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            this.TableResults.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.TableResults.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.TableResults.ThemeStyle.GridColor = System.Drawing.Color.LightSeaGreen;
            this.TableResults.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.Teal;
            this.TableResults.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.TableResults.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.TableResults.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.TableResults.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.TableResults.ThemeStyle.HeaderStyle.Height = 50;
            this.TableResults.ThemeStyle.ReadOnly = false;
            this.TableResults.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.PaleTurquoise;
            this.TableResults.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.TableResults.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.TableResults.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.TableResults.ThemeStyle.RowsStyle.Height = 25;
            this.TableResults.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            this.TableResults.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.TableResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TableResults_CellClick);
            this.TableResults.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.TableResults_CellMouseClick);
            this.TableResults.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.TableResults_RowsAdded);
            this.TableResults.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.TableResults_RowsRemoved);
            this.TableResults.Sorted += new System.EventHandler(this.TableResults_Sorted);
            // 
            // IndexNo
            // 
            this.IndexNo.HeaderText = "Index NO";
            this.IndexNo.Name = "IndexNo";
            this.IndexNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EstimatedMoney
            // 
            this.EstimatedMoney.HeaderText = "Estimated Money";
            this.EstimatedMoney.Name = "EstimatedMoney";
            this.EstimatedMoney.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Permission
            // 
            this.Permission.HeaderText = "Permission";
            this.Permission.Name = "Permission";
            this.Permission.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MethodOfExecution
            // 
            this.MethodOfExecution.HeaderText = "Method Of Execution";
            this.MethodOfExecution.Name = "MethodOfExecution";
            this.MethodOfExecution.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StartDate
            // 
            this.StartDate.HeaderText = "Start Date";
            this.StartDate.Name = "StartDate";
            this.StartDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estimator
            // 
            this.Estimator.HeaderText = "Estimator";
            this.Estimator.Name = "Estimator";
            this.Estimator.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Expenditure Head";
            this.Column7.Name = "Column7";
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Proposer";
            this.Column9.Name = "Column9";
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.Transparent;
            this.txtSearch.BorderRadius = 22;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.DisabledState.Parent = this.txtSearch;
            this.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.FillColor = System.Drawing.Color.PaleTurquoise;
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.FocusedState.Parent = this.txtSearch;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.HoverState.Parent = this.txtSearch;
            this.txtSearch.Location = new System.Drawing.Point(268, 554);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtSearch.PlaceholderText = "Search in Description ......";
            this.txtSearch.SelectedText = "";
            this.txtSearch.ShadowDecoration.BorderRadius = 22;
            this.txtSearch.ShadowDecoration.Enabled = true;
            this.txtSearch.ShadowDecoration.Parent = this.txtSearch;
            this.txtSearch.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 5, 5);
            this.txtSearch.Size = new System.Drawing.Size(491, 45);
            this.txtSearch.TabIndex = 165;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ProgressBarWaiting
            // 
            this.ProgressBarWaiting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBarWaiting.FillColor = System.Drawing.Color.White;
            this.ProgressBarWaiting.Location = new System.Drawing.Point(0, 532);
            this.ProgressBarWaiting.Name = "ProgressBarWaiting";
            this.ProgressBarWaiting.ProgressColor = System.Drawing.Color.Teal;
            this.ProgressBarWaiting.ProgressColor2 = System.Drawing.Color.LightSeaGreen;
            this.ProgressBarWaiting.ShadowDecoration.Parent = this.ProgressBarWaiting;
            this.ProgressBarWaiting.Size = new System.Drawing.Size(984, 15);
            this.ProgressBarWaiting.TabIndex = 166;
            this.ProgressBarWaiting.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.ProgressBarWaiting.Value = 100;
            this.ProgressBarWaiting.Visible = false;
            // 
            // TableRightClickMenu
            // 
            this.TableRightClickMenu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TableRightClickMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.TableRightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edit,
            this.Remove,
            this.Progress,
            this.toolStripSeparator1,
            this.Search,
            this.Refresh,
            this.ItemStartDate,
            this.Save});
            this.TableRightClickMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.TableRightClickMenu.Name = "SortByContextMenuStrip";
            this.TableRightClickMenu.RenderStyle.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(143)))), ((int)(((byte)(255)))));
            this.TableRightClickMenu.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
            this.TableRightClickMenu.RenderStyle.ColorTable = null;
            this.TableRightClickMenu.RenderStyle.RoundedEdges = true;
            this.TableRightClickMenu.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
            this.TableRightClickMenu.RenderStyle.SelectionBackColor = System.Drawing.Color.Teal;
            this.TableRightClickMenu.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.TableRightClickMenu.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
            this.TableRightClickMenu.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.TableRightClickMenu.Size = new System.Drawing.Size(223, 164);
            this.TableRightClickMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.TableRightClickMenu_ItemClicked);
            // 
            // Edit
            // 
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(222, 22);
            this.Edit.Tag = "Edit";
            this.Edit.Text = "Edit";
            // 
            // Remove
            // 
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(222, 22);
            this.Remove.Tag = "Remove";
            this.Remove.Text = "Remove Selected Row";
            // 
            // Progress
            // 
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(222, 22);
            this.Progress.Tag = "Progress";
            this.Progress.Text = "Show Progress Details";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.Gray;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // Search
            // 
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(222, 22);
            this.Search.Tag = "Search";
            this.Search.Text = "Search";
            // 
            // Refresh
            // 
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(222, 22);
            this.Refresh.Tag = "Refresh";
            this.Refresh.Text = "Refresh";
            // 
            // ItemStartDate
            // 
            this.ItemStartDate.DropDown = this.StartDateMenu;
            this.ItemStartDate.Name = "ItemStartDate";
            this.ItemStartDate.Size = new System.Drawing.Size(222, 22);
            this.ItemStartDate.Tag = "StartDate";
            this.ItemStartDate.Text = "Show Projects Start On";
            // 
            // StartDateMenu
            // 
            this.StartDateMenu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.StartDateMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.StartDateMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.StartDateMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.StartDateMenu.Name = "SortByContextMenuStrip";
            this.StartDateMenu.OwnerItem = this.ItemStartDate;
            this.StartDateMenu.RenderStyle.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(143)))), ((int)(((byte)(255)))));
            this.StartDateMenu.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
            this.StartDateMenu.RenderStyle.ColorTable = null;
            this.StartDateMenu.RenderStyle.RoundedEdges = true;
            this.StartDateMenu.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
            this.StartDateMenu.RenderStyle.SelectionBackColor = System.Drawing.Color.Teal;
            this.StartDateMenu.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.StartDateMenu.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
            this.StartDateMenu.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.StartDateMenu.Size = new System.Drawing.Size(180, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem1.Text = "All";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem2.Text = "This Year Only";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem4.Text = "This Month Only";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem5.Text = "Last Month Only";
            // 
            // SortByMenu
            // 
            this.SortByMenu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.SortByMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.SortByMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.SortByMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.SortByMenu.Name = "SortByContextMenuStrip";
            this.SortByMenu.RenderStyle.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(143)))), ((int)(((byte)(255)))));
            this.SortByMenu.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
            this.SortByMenu.RenderStyle.ColorTable = null;
            this.SortByMenu.RenderStyle.RoundedEdges = true;
            this.SortByMenu.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
            this.SortByMenu.RenderStyle.SelectionBackColor = System.Drawing.Color.Teal;
            this.SortByMenu.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.SortByMenu.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
            this.SortByMenu.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.SortByMenu.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            this.SortByMenu.Size = new System.Drawing.Size(180, 92);
            this.SortByMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SortByMenu_ItemClicked);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem6.Text = "All";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem7.Text = "This Year Only";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem8.Text = "This Month Only";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem9.Text = "Last Month Only";
            // 
            // Save
            // 
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(222, 22);
            this.Save.Tag = "Save";
            this.Save.Text = "Save Changes";
            // 
            // MenuDefaultData
            // 
            this.MenuDefaultData.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuDefaultData.ImageScalingSize = new System.Drawing.Size(16, 20);
            this.MenuDefaultData.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MenuDefaultData.Name = "MenuDefaultData";
            this.MenuDefaultData.RenderStyle.ArrowColor = System.Drawing.Color.Teal;
            this.MenuDefaultData.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
            this.MenuDefaultData.RenderStyle.ColorTable = null;
            this.MenuDefaultData.RenderStyle.RoundedEdges = true;
            this.MenuDefaultData.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
            this.MenuDefaultData.RenderStyle.SelectionBackColor = System.Drawing.Color.Teal;
            this.MenuDefaultData.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.MenuDefaultData.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
            this.MenuDefaultData.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.MenuDefaultData.Size = new System.Drawing.Size(61, 4);
            this.MenuDefaultData.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuDefaultData_ItemClicked);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.CheckedState.Parent = this.btnSearch;
            this.btnSearch.CustomImages.Parent = this.btnSearch;
            this.btnSearch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSearch.DisabledState.Parent = this.btnSearch;
            this.btnSearch.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.HoverState.Parent = this.btnSearch;
            this.btnSearch.Image = global::Project_Manegement_System___KMC.Properties.Resources.Search;
            this.btnSearch.ImageSize = new System.Drawing.Size(22, 22);
            this.btnSearch.Location = new System.Drawing.Point(714, 554);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PressedColor = System.Drawing.Color.Teal;
            this.btnSearch.ShadowDecoration.Depth = 40;
            this.btnSearch.ShadowDecoration.Enabled = true;
            this.btnSearch.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnSearch.ShadowDecoration.Parent = this.btnSearch;
            this.btnSearch.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 5, 5);
            this.btnSearch.Size = new System.Drawing.Size(45, 45);
            this.btnSearch.TabIndex = 164;
            this.btnSearch.Tag = "Search";
            this.btnSearch.UseTransparentBackground = true;
            this.btnSearch.Click += new System.EventHandler(this.btnOperation_Click);
            // 
            // btnProgress
            // 
            this.btnProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProgress.BackColor = System.Drawing.Color.Transparent;
            this.btnProgress.CheckedState.Parent = this.btnProgress;
            this.btnProgress.CustomImages.Parent = this.btnProgress;
            this.btnProgress.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnProgress.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnProgress.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnProgress.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnProgress.DisabledState.Parent = this.btnProgress;
            this.btnProgress.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnProgress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnProgress.ForeColor = System.Drawing.Color.White;
            this.btnProgress.HoverState.Parent = this.btnProgress;
            this.btnProgress.Image = global::Project_Manegement_System___KMC.Properties.Resources.Progress1;
            this.btnProgress.ImageSize = new System.Drawing.Size(29, 29);
            this.btnProgress.Location = new System.Drawing.Point(114, 554);
            this.btnProgress.Name = "btnProgress";
            this.btnProgress.PressedColor = System.Drawing.Color.Teal;
            this.btnProgress.ShadowDecoration.Depth = 40;
            this.btnProgress.ShadowDecoration.Enabled = true;
            this.btnProgress.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnProgress.ShadowDecoration.Parent = this.btnProgress;
            this.btnProgress.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 5, 5);
            this.btnProgress.Size = new System.Drawing.Size(45, 45);
            this.btnProgress.TabIndex = 163;
            this.btnProgress.Tag = "Progress";
            this.btnProgress.UseTransparentBackground = true;
            this.btnProgress.Click += new System.EventHandler(this.btnOperation_Click);
            // 
            // btnResfesh
            // 
            this.btnResfesh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResfesh.BackColor = System.Drawing.Color.Transparent;
            this.btnResfesh.CheckedState.Parent = this.btnResfesh;
            this.btnResfesh.CustomImages.Parent = this.btnResfesh;
            this.btnResfesh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnResfesh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnResfesh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnResfesh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnResfesh.DisabledState.Parent = this.btnResfesh;
            this.btnResfesh.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnResfesh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnResfesh.ForeColor = System.Drawing.Color.White;
            this.btnResfesh.HoverState.Parent = this.btnResfesh;
            this.btnResfesh.Image = global::Project_Manegement_System___KMC.Properties.Resources.Refresh;
            this.btnResfesh.ImageSize = new System.Drawing.Size(30, 30);
            this.btnResfesh.Location = new System.Drawing.Point(216, 554);
            this.btnResfesh.Name = "btnResfesh";
            this.btnResfesh.PressedColor = System.Drawing.Color.Teal;
            this.btnResfesh.ShadowDecoration.Depth = 40;
            this.btnResfesh.ShadowDecoration.Enabled = true;
            this.btnResfesh.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnResfesh.ShadowDecoration.Parent = this.btnResfesh;
            this.btnResfesh.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 5, 5);
            this.btnResfesh.Size = new System.Drawing.Size(45, 45);
            this.btnResfesh.TabIndex = 162;
            this.btnResfesh.Tag = "Refresh";
            this.btnResfesh.UseTransparentBackground = true;
            this.btnResfesh.Click += new System.EventHandler(this.btnOperation_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnRemove.CheckedState.Parent = this.btnRemove;
            this.btnRemove.CustomImages.Parent = this.btnRemove;
            this.btnRemove.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRemove.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRemove.DisabledState.Parent = this.btnRemove;
            this.btnRemove.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRemove.ForeColor = System.Drawing.Color.White;
            this.btnRemove.HoverState.Parent = this.btnRemove;
            this.btnRemove.Image = global::Project_Manegement_System___KMC.Properties.Resources.Remove1;
            this.btnRemove.ImageSize = new System.Drawing.Size(22, 22);
            this.btnRemove.Location = new System.Drawing.Point(63, 554);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.PressedColor = System.Drawing.Color.Teal;
            this.btnRemove.ShadowDecoration.Depth = 40;
            this.btnRemove.ShadowDecoration.Enabled = true;
            this.btnRemove.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnRemove.ShadowDecoration.Parent = this.btnRemove;
            this.btnRemove.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 5, 5);
            this.btnRemove.Size = new System.Drawing.Size(45, 45);
            this.btnRemove.TabIndex = 162;
            this.btnRemove.Tag = "Remove";
            this.btnRemove.UseTransparentBackground = true;
            this.btnRemove.Click += new System.EventHandler(this.btnOperation_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.CheckedState.Parent = this.btnEdit;
            this.btnEdit.CustomImages.Parent = this.btnEdit;
            this.btnEdit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEdit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEdit.DisabledState.Parent = this.btnEdit;
            this.btnEdit.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.HoverState.Parent = this.btnEdit;
            this.btnEdit.Image = global::Project_Manegement_System___KMC.Properties.Resources.Edit2;
            this.btnEdit.ImageSize = new System.Drawing.Size(22, 22);
            this.btnEdit.Location = new System.Drawing.Point(12, 554);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PressedColor = System.Drawing.Color.Teal;
            this.btnEdit.ShadowDecoration.Depth = 40;
            this.btnEdit.ShadowDecoration.Enabled = true;
            this.btnEdit.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnEdit.ShadowDecoration.Parent = this.btnEdit;
            this.btnEdit.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 5, 5);
            this.btnEdit.Size = new System.Drawing.Size(45, 45);
            this.btnEdit.TabIndex = 162;
            this.btnEdit.Tag = "Edit";
            this.btnEdit.UseTransparentBackground = true;
            this.btnEdit.Click += new System.EventHandler(this.btnOperation_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.CheckedState.Parent = this.btnSave;
            this.btnSave.CustomImages.Parent = this.btnSave;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.DisabledState.Parent = this.btnSave;
            this.btnSave.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.HoverState.Parent = this.btnSave;
            this.btnSave.Image = global::Project_Manegement_System___KMC.Properties.Resources.Save1;
            this.btnSave.ImageSize = new System.Drawing.Size(22, 22);
            this.btnSave.Location = new System.Drawing.Point(165, 554);
            this.btnSave.Name = "btnSave";
            this.btnSave.PressedColor = System.Drawing.Color.Teal;
            this.btnSave.ShadowDecoration.Depth = 40;
            this.btnSave.ShadowDecoration.Enabled = true;
            this.btnSave.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnSave.ShadowDecoration.Parent = this.btnSave;
            this.btnSave.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 5, 5);
            this.btnSave.Size = new System.Drawing.Size(45, 45);
            this.btnSave.TabIndex = 162;
            this.btnSave.Tag = "Save";
            this.btnSave.UseTransparentBackground = true;
            this.btnSave.Click += new System.EventHandler(this.btnOperation_Click);
            // 
            // AddNewTableMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.ProgressBarWaiting);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnProgress);
            this.Controls.Add(this.btnResfesh);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.TableResults);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(16, 100);
            this.Name = "AddNewTableMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddNewTableMode_FormClosing);
            this.Load += new System.EventHandler(this.AddNewTableMode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TableResults)).EndInit();
            this.TableRightClickMenu.ResumeLayout(false);
            this.StartDateMenu.ResumeLayout(false);
            this.SortByMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public Guna.UI2.WinForms.Guna2DataGridView TableResults;
        private Guna.UI2.WinForms.Guna2CircleButton btnSave;
        private Guna.UI2.WinForms.Guna2CircleButton btnEdit;
        private Guna.UI2.WinForms.Guna2CircleButton btnRemove;
        private Guna.UI2.WinForms.Guna2CircleButton btnResfesh;
        private Guna.UI2.WinForms.Guna2CircleButton btnProgress;
        private Guna.UI2.WinForms.Guna2CircleButton btnSearch;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2ProgressBar ProgressBarWaiting;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip TableRightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem ItemStartDate;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip StartDateMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem Refresh;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip SortByMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem Edit;
        private System.Windows.Forms.ToolStripMenuItem Remove;
        private System.Windows.Forms.ToolStripMenuItem Progress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Save;
        private System.Windows.Forms.ToolStripMenuItem Search;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip MenuDefaultData;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstimatedMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn Permission;
        private System.Windows.Forms.DataGridViewTextBoxColumn MethodOfExecution;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estimator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
    }
}