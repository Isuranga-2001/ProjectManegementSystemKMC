﻿
namespace Project_Manegement_System___KMC
{
    partial class SelectProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectProject));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddNew = new Guna.UI2.WinForms.Guna2Button();
            this.btnOK = new Guna.UI2.WinForms.Guna2Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DataGridViewProject = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2Separator3 = new Guna.UI2.WinForms.Guna2Separator();
            this.txtDescription = new Guna.UI2.WinForms.Guna2TextBox();
            this.DragControl = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.ProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewProject)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnAddNew);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 464);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(777, 57);
            this.panel2.TabIndex = 4;
            // 
            // btnAddNew
            // 
            this.btnAddNew.BorderColor = System.Drawing.Color.Teal;
            this.btnAddNew.BorderRadius = 10;
            this.btnAddNew.BorderThickness = 2;
            this.btnAddNew.CheckedState.Parent = this.btnAddNew;
            this.btnAddNew.CustomImages.Parent = this.btnAddNew;
            this.btnAddNew.FillColor = System.Drawing.Color.White;
            this.btnAddNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnAddNew.ForeColor = System.Drawing.Color.Teal;
            this.btnAddNew.HoverState.BorderColor = System.Drawing.Color.LightSeaGreen;
            this.btnAddNew.HoverState.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnAddNew.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnAddNew.HoverState.Parent = this.btnAddNew;
            this.btnAddNew.Location = new System.Drawing.Point(555, 12);
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
            this.btnOK.BorderThickness = 2;
            this.btnOK.CheckedState.Parent = this.btnOK;
            this.btnOK.CustomImages.Parent = this.btnOK;
            this.btnOK.FillColor = System.Drawing.Color.Teal;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.HoverState.BorderColor = System.Drawing.Color.LightSeaGreen;
            this.btnOK.HoverState.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnOK.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnOK.HoverState.Parent = this.btnOK;
            this.btnOK.Location = new System.Drawing.Point(663, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.ShadowDecoration.Parent = this.btnOK;
            this.btnOK.Size = new System.Drawing.Size(102, 33);
            this.btnOK.TabIndex = 77;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Teal;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(777, 30);
            this.panel1.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FillColor = System.Drawing.Color.Teal;
            this.btnClose.HoverState.Parent = this.btnClose;
            this.btnClose.IconColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(732, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShadowDecoration.Parent = this.btnClose;
            this.btnClose.Size = new System.Drawing.Size(45, 30);
            this.btnClose.TabIndex = 48;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Project";
            // 
            // DataGridViewProject
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(223)))), ((int)(((byte)(219)))));
            this.DataGridViewProject.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridViewProject.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridViewProject.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewProject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataGridViewProject.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewProject.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridViewProject.ColumnHeadersHeight = 30;
            this.DataGridViewProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridViewProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjectID,
            this.Description,
            this.ZoneNo});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(233)))), ((int)(((byte)(231)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridViewProject.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewProject.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridViewProject.EnableHeadersVisualStyles = false;
            this.DataGridViewProject.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(222)))), ((int)(((byte)(218)))));
            this.DataGridViewProject.Location = new System.Drawing.Point(12, 165);
            this.DataGridViewProject.Name = "DataGridViewProject";
            this.DataGridViewProject.RowHeadersVisible = false;
            this.DataGridViewProject.RowTemplate.Height = 25;
            this.DataGridViewProject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridViewProject.Size = new System.Drawing.Size(753, 293);
            this.DataGridViewProject.TabIndex = 5;
            this.DataGridViewProject.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Teal;
            this.DataGridViewProject.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(223)))), ((int)(((byte)(219)))));
            this.DataGridViewProject.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.DataGridViewProject.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.DataGridViewProject.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.DataGridViewProject.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.DataGridViewProject.ThemeStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewProject.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(222)))), ((int)(((byte)(218)))));
            this.DataGridViewProject.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.DataGridViewProject.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataGridViewProject.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridViewProject.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridViewProject.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridViewProject.ThemeStyle.HeaderStyle.Height = 30;
            this.DataGridViewProject.ThemeStyle.ReadOnly = false;
            this.DataGridViewProject.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(233)))), ((int)(((byte)(231)))));
            this.DataGridViewProject.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.DataGridViewProject.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridViewProject.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.DataGridViewProject.ThemeStyle.RowsStyle.Height = 25;
            this.DataGridViewProject.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(185)))), ((int)(((byte)(175)))));
            this.DataGridViewProject.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.DataGridViewProject.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewProject_CellClick);
            // 
            // guna2Separator3
            // 
            this.guna2Separator3.FillColor = System.Drawing.Color.Teal;
            this.guna2Separator3.FillThickness = 2;
            this.guna2Separator3.Location = new System.Drawing.Point(12, 146);
            this.guna2Separator3.Name = "guna2Separator3";
            this.guna2Separator3.Size = new System.Drawing.Size(753, 13);
            this.guna2Separator3.TabIndex = 143;
            // 
            // txtDescription
            // 
            this.txtDescription.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.txtDescription.BorderRadius = 15;
            this.txtDescription.BorderThickness = 0;
            this.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDescription.DefaultText = "";
            this.txtDescription.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDescription.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDescription.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDescription.DisabledState.Parent = this.txtDescription;
            this.txtDescription.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDescription.FillColor = System.Drawing.Color.PaleTurquoise;
            this.txtDescription.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDescription.FocusedState.FillColor = System.Drawing.Color.PaleTurquoise;
            this.txtDescription.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txtDescription.FocusedState.Parent = this.txtDescription;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.ForeColor = System.Drawing.Color.Black;
            this.txtDescription.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDescription.HoverState.Parent = this.txtDescription;
            this.txtDescription.Location = new System.Drawing.Point(12, 45);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.PasswordChar = '\0';
            this.txtDescription.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtDescription.PlaceholderText = "Search Project Using Project Description";
            this.txtDescription.SelectedText = "";
            this.txtDescription.ShadowDecoration.Parent = this.txtDescription;
            this.txtDescription.Size = new System.Drawing.Size(753, 91);
            this.txtDescription.TabIndex = 144;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // DragControl
            // 
            this.DragControl.ContainerControl = this;
            this.DragControl.TargetControl = this.panel1;
            // 
            // ProjectID
            // 
            this.ProjectID.FillWeight = 25F;
            this.ProjectID.HeaderText = "IndexNo";
            this.ProjectID.Name = "ProjectID";
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            // 
            // ZoneNo
            // 
            this.ZoneNo.FillWeight = 15F;
            this.ZoneNo.HeaderText = "Section";
            this.ZoneNo.Name = "ZoneNo";
            // 
            // SelectProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(777, 521);
            this.ControlBox = false;
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.guna2Separator3);
            this.Controls.Add(this.DataGridViewProject);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SelectProject_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2Button btnAddNew;
        private Guna.UI2.WinForms.Guna2Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2DataGridView DataGridViewProject;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator3;
        private Guna.UI2.WinForms.Guna2TextBox txtDescription;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private Guna.UI2.WinForms.Guna2DragControl DragControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZoneNo;
    }
}