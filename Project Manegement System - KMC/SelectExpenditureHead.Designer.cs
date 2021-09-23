
namespace Project_Manegement_System___KMC
{
    partial class SelectExpenditureHead
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectExpenditureHead));
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DragControl = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.EHDataGridView = new Guna.UI2.WinForms.Guna2DataGridView();
            this.EHID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EH_Main = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EH_Sub1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EH_Sub2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EH_Sub3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddNew = new Guna.UI2.WinForms.Guna2Button();
            this.btnOK = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EHDataGridView)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Teal;
            this.panel1.Controls.Add(this.guna2ControlBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(804, 30);
            this.panel1.TabIndex = 0;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Teal;
            this.guna2ControlBox1.HoverState.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(759, 0);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.ShadowDecoration.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.Size = new System.Drawing.Size(45, 30);
            this.guna2ControlBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Expenditure Head";
            // 
            // DragControl
            // 
            this.DragControl.ContainerControl = this;
            this.DragControl.TargetControl = this.panel1;
            // 
            // EHDataGridView
            // 
            this.EHDataGridView.AllowUserToAddRows = false;
            this.EHDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(223)))), ((int)(((byte)(219)))));
            this.EHDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.EHDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.EHDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.EHDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EHDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.EHDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkSlateGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.EHDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.EHDataGridView.ColumnHeadersHeight = 30;
            this.EHDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.EHDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EHID,
            this.EH_Main,
            this.EH_Sub1,
            this.EH_Sub2,
            this.EH_Sub3,
            this.Description});
            this.EHDataGridView.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(233)))), ((int)(((byte)(231)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.EHDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.EHDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EHDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.EHDataGridView.EnableHeadersVisualStyles = false;
            this.EHDataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(222)))), ((int)(((byte)(218)))));
            this.EHDataGridView.Location = new System.Drawing.Point(0, 30);
            this.EHDataGridView.MultiSelect = false;
            this.EHDataGridView.Name = "EHDataGridView";
            this.EHDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.EHDataGridView.RowHeadersVisible = false;
            this.EHDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.EHDataGridView.RowTemplate.Height = 25;
            this.EHDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.EHDataGridView.Size = new System.Drawing.Size(804, 331);
            this.EHDataGridView.TabIndex = 1;
            this.EHDataGridView.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Teal;
            this.EHDataGridView.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(223)))), ((int)(((byte)(219)))));
            this.EHDataGridView.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.EHDataGridView.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.EHDataGridView.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.EHDataGridView.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.EHDataGridView.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.EHDataGridView.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(222)))), ((int)(((byte)(218)))));
            this.EHDataGridView.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.EHDataGridView.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.EHDataGridView.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.EHDataGridView.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.EHDataGridView.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.EHDataGridView.ThemeStyle.HeaderStyle.Height = 30;
            this.EHDataGridView.ThemeStyle.ReadOnly = false;
            this.EHDataGridView.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(233)))), ((int)(((byte)(231)))));
            this.EHDataGridView.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.EHDataGridView.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.EHDataGridView.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.EHDataGridView.ThemeStyle.RowsStyle.Height = 25;
            this.EHDataGridView.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            this.EHDataGridView.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.EHDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.EHDataGridView_CellClick);
            this.EHDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.EHDataGridView_CellDoubleClick);
            // 
            // EHID
            // 
            this.EHID.DataPropertyName = "EHID";
            this.EHID.FillWeight = 1F;
            this.EHID.HeaderText = "EHID";
            this.EHID.Name = "EHID";
            this.EHID.Visible = false;
            // 
            // EH_Main
            // 
            this.EH_Main.FillWeight = 20F;
            this.EH_Main.HeaderText = "EH Main";
            this.EH_Main.Name = "EH_Main";
            // 
            // EH_Sub1
            // 
            this.EH_Sub1.FillWeight = 20F;
            this.EH_Sub1.HeaderText = "EH Sub1";
            this.EH_Sub1.Name = "EH_Sub1";
            // 
            // EH_Sub2
            // 
            this.EH_Sub2.FillWeight = 20F;
            this.EH_Sub2.HeaderText = "EH Sub2";
            this.EH_Sub2.Name = "EH_Sub2";
            // 
            // EH_Sub3
            // 
            this.EH_Sub3.FillWeight = 20F;
            this.EH_Sub3.HeaderText = "EH Sub3";
            this.EH_Sub3.Name = "EH_Sub3";
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAddNew);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 361);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(804, 50);
            this.panel2.TabIndex = 2;
            // 
            // btnAddNew
            // 
            this.btnAddNew.BorderColor = System.Drawing.Color.Teal;
            this.btnAddNew.BorderRadius = 10;
            this.btnAddNew.BorderThickness = 2;
            this.btnAddNew.CheckedState.Parent = this.btnAddNew;
            this.btnAddNew.CustomImages.Parent = this.btnAddNew;
            this.btnAddNew.FillColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnAddNew.ForeColor = System.Drawing.Color.Teal;
            this.btnAddNew.HoverState.BorderColor = System.Drawing.Color.LightSeaGreen;
            this.btnAddNew.HoverState.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnAddNew.HoverState.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddNew.HoverState.Parent = this.btnAddNew;
            this.btnAddNew.Location = new System.Drawing.Point(588, 11);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.ShadowDecoration.Parent = this.btnAddNew;
            this.btnAddNew.Size = new System.Drawing.Size(102, 33);
            this.btnAddNew.TabIndex = 78;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnOK
            // 
            this.btnOK.BorderColor = System.Drawing.Color.Teal;
            this.btnOK.BorderRadius = 12;
            this.btnOK.CheckedState.Parent = this.btnOK;
            this.btnOK.CustomImages.Parent = this.btnOK;
            this.btnOK.FillColor = System.Drawing.Color.Teal;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.HoverState.BorderColor = System.Drawing.Color.LightSeaGreen;
            this.btnOK.HoverState.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnOK.HoverState.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOK.HoverState.Parent = this.btnOK;
            this.btnOK.Location = new System.Drawing.Point(696, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.ShadowDecoration.Parent = this.btnOK;
            this.btnOK.Size = new System.Drawing.Size(102, 33);
            this.btnOK.TabIndex = 77;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SelectExpenditureHead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(804, 411);
            this.ControlBox = false;
            this.Controls.Add(this.EHDataGridView);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectExpenditureHead";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SelectExpenditureHead_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EHDataGridView)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2DragControl DragControl;
        private Guna.UI2.WinForms.Guna2DataGridView EHDataGridView;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2Button btnOK;
        private Guna.UI2.WinForms.Guna2Button btnAddNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn EHID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EH_Main;
        private System.Windows.Forms.DataGridViewTextBoxColumn EH_Sub1;
        private System.Windows.Forms.DataGridViewTextBoxColumn EH_Sub2;
        private System.Windows.Forms.DataGridViewTextBoxColumn EH_Sub3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
    }
}