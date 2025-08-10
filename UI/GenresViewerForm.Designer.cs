namespace UI
{
    partial class GenresViewerForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            dataGridViewGenres = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            buttonRescanGenres = new Button();
            buttonSaveChanges = new Button();
            labelProcess = new Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewGenres).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dataGridViewGenres, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(869, 554);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridViewGenres
            // 
            dataGridViewGenres.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewGenres.Dock = DockStyle.Fill;
            dataGridViewGenres.Location = new Point(3, 3);
            dataGridViewGenres.Name = "dataGridViewGenres";
            dataGridViewGenres.Size = new Size(863, 508);
            dataGridViewGenres.TabIndex = 1;
            dataGridViewGenres.CellEndEdit += dataGridViewGenres_CellEndEdit;
            dataGridViewGenres.CellValidating += dataGridViewGenres_CellValidating;
            dataGridViewGenres.DataBindingComplete += dataGridViewGenres_DataBindingComplete;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(buttonRescanGenres, 0, 0);
            tableLayoutPanel2.Controls.Add(buttonSaveChanges, 2, 0);
            tableLayoutPanel2.Controls.Add(labelProcess, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 517);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(863, 34);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // buttonRescanGenres
            // 
            buttonRescanGenres.Location = new Point(3, 3);
            buttonRescanGenres.Name = "buttonRescanGenres";
            buttonRescanGenres.Size = new Size(126, 28);
            buttonRescanGenres.TabIndex = 0;
            buttonRescanGenres.Text = "Rescan genres";
            buttonRescanGenres.UseVisualStyleBackColor = true;
            buttonRescanGenres.Click += buttonRescanGenres_Click;
            // 
            // buttonSaveChanges
            // 
            buttonSaveChanges.Anchor = AnchorStyles.Right;
            buttonSaveChanges.Location = new Point(785, 5);
            buttonSaveChanges.Name = "buttonSaveChanges";
            buttonSaveChanges.Size = new Size(75, 23);
            buttonSaveChanges.TabIndex = 1;
            buttonSaveChanges.Text = "Save";
            buttonSaveChanges.UseVisualStyleBackColor = true;
            buttonSaveChanges.Click += buttonSaveChanges_Click;
            // 
            // labelProcess
            // 
            labelProcess.Anchor = AnchorStyles.None;
            labelProcess.AutoSize = true;
            labelProcess.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelProcess.ForeColor = Color.ForestGreen;
            labelProcess.Location = new Point(430, 6);
            labelProcess.Name = "labelProcess";
            labelProcess.Size = new Size(0, 21);
            labelProcess.TabIndex = 2;
            // 
            // GenresViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(869, 554);
            Controls.Add(tableLayoutPanel1);
            Name = "GenresViewerForm";
            Text = "GenresViewer";
            FormClosed += GenresViewerForm_FormClosed;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewGenres).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonRescanGenres;
        private DataGridView dataGridViewGenres;
        private TableLayoutPanel tableLayoutPanel2;
        private Button buttonSaveChanges;
        private Label labelProcess;
    }
}