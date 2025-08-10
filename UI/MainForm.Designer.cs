namespace UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanelForm = new TableLayoutPanel();
            labelTrackName = new Label();
            tableLayoutPanelGenresOperations = new TableLayoutPanel();
            buttonLike = new Button();
            buttonDislike = new Button();
            comboBoxGenres = new ComboBox();
            comboBoxRandomMode = new ComboBox();
            buttonRandom = new Button();
            buttonDisableGenre = new Button();
            tableLayouTrackOperations = new TableLayoutPanel();
            trackBarVolume = new TrackBar();
            labelVolume = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            buttonEditGenres = new Button();
            buttonStartStop = new Button();
            labelProgress = new Label();
            tableLayoutPanelForm.SuspendLayout();
            tableLayoutPanelGenresOperations.SuspendLayout();
            tableLayouTrackOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelForm
            // 
            tableLayoutPanelForm.ColumnCount = 1;
            tableLayoutPanelForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelForm.Controls.Add(labelTrackName, 0, 0);
            tableLayoutPanelForm.Controls.Add(tableLayoutPanelGenresOperations, 0, 1);
            tableLayoutPanelForm.Controls.Add(tableLayouTrackOperations, 0, 2);
            tableLayoutPanelForm.Controls.Add(tableLayoutPanel1, 0, 3);
            tableLayoutPanelForm.Dock = DockStyle.Fill;
            tableLayoutPanelForm.Location = new Point(0, 0);
            tableLayoutPanelForm.Name = "tableLayoutPanelForm";
            tableLayoutPanelForm.RowCount = 4;
            tableLayoutPanelForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanelForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanelForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanelForm.Size = new Size(954, 206);
            tableLayoutPanelForm.TabIndex = 0;
            // 
            // labelTrackName
            // 
            labelTrackName.Anchor = AnchorStyles.None;
            labelTrackName.AutoSize = true;
            labelTrackName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelTrackName.Location = new Point(428, 32);
            labelTrackName.Name = "labelTrackName";
            labelTrackName.Size = new Size(97, 21);
            labelTrackName.TabIndex = 0;
            labelTrackName.Text = "Track name";
            // 
            // tableLayoutPanelGenresOperations
            // 
            tableLayoutPanelGenresOperations.ColumnCount = 6;
            tableLayoutPanelGenresOperations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.00001F));
            tableLayoutPanelGenresOperations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.9999924F));
            tableLayoutPanelGenresOperations.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanelGenresOperations.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanelGenresOperations.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanelGenresOperations.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanelGenresOperations.Controls.Add(buttonLike, 5, 0);
            tableLayoutPanelGenresOperations.Controls.Add(buttonDislike, 4, 0);
            tableLayoutPanelGenresOperations.Controls.Add(comboBoxGenres, 0, 0);
            tableLayoutPanelGenresOperations.Controls.Add(comboBoxRandomMode, 1, 0);
            tableLayoutPanelGenresOperations.Controls.Add(buttonRandom, 2, 0);
            tableLayoutPanelGenresOperations.Controls.Add(buttonDisableGenre, 3, 0);
            tableLayoutPanelGenresOperations.Dock = DockStyle.Fill;
            tableLayoutPanelGenresOperations.Location = new Point(3, 89);
            tableLayoutPanelGenresOperations.Name = "tableLayoutPanelGenresOperations";
            tableLayoutPanelGenresOperations.RowCount = 1;
            tableLayoutPanelGenresOperations.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelGenresOperations.Size = new Size(948, 34);
            tableLayoutPanelGenresOperations.TabIndex = 1;
            // 
            // buttonLike
            // 
            buttonLike.Dock = DockStyle.Top;
            buttonLike.Location = new Point(868, 3);
            buttonLike.Name = "buttonLike";
            buttonLike.Size = new Size(77, 23);
            buttonLike.TabIndex = 0;
            buttonLike.Text = "Like";
            buttonLike.UseVisualStyleBackColor = true;
            buttonLike.Click += buttonLike_Click;
            // 
            // buttonDislike
            // 
            buttonDislike.Dock = DockStyle.Top;
            buttonDislike.Location = new Point(787, 3);
            buttonDislike.Name = "buttonDislike";
            buttonDislike.Size = new Size(75, 23);
            buttonDislike.TabIndex = 1;
            buttonDislike.Text = "Dislike";
            buttonDislike.UseVisualStyleBackColor = true;
            buttonDislike.Click += buttonDislike_Click;
            // 
            // comboBoxGenres
            // 
            comboBoxGenres.Dock = DockStyle.Fill;
            comboBoxGenres.FormattingEnabled = true;
            comboBoxGenres.Location = new Point(3, 3);
            comboBoxGenres.Name = "comboBoxGenres";
            comboBoxGenres.Size = new Size(299, 23);
            comboBoxGenres.TabIndex = 4;
            comboBoxGenres.SelectedIndexChanged += comboBoxGenres_SelectedIndexChanged;
            // 
            // comboBoxRandomMode
            // 
            comboBoxRandomMode.Dock = DockStyle.Fill;
            comboBoxRandomMode.FormattingEnabled = true;
            comboBoxRandomMode.Location = new Point(308, 3);
            comboBoxRandomMode.Name = "comboBoxRandomMode";
            comboBoxRandomMode.Size = new Size(298, 23);
            comboBoxRandomMode.TabIndex = 5;
            comboBoxRandomMode.SelectedIndexChanged += comboBoxRandomMode_SelectedIndexChanged;
            // 
            // buttonRandom
            // 
            buttonRandom.Dock = DockStyle.Top;
            buttonRandom.Location = new Point(612, 3);
            buttonRandom.Name = "buttonRandom";
            buttonRandom.Size = new Size(75, 23);
            buttonRandom.TabIndex = 2;
            buttonRandom.Text = "Random";
            buttonRandom.UseVisualStyleBackColor = true;
            buttonRandom.Click += buttonRandom_Click;
            // 
            // buttonDisableGenre
            // 
            buttonDisableGenre.Dock = DockStyle.Top;
            buttonDisableGenre.Location = new Point(693, 3);
            buttonDisableGenre.Name = "buttonDisableGenre";
            buttonDisableGenre.Size = new Size(88, 23);
            buttonDisableGenre.TabIndex = 3;
            buttonDisableGenre.Text = "Disable Genre";
            buttonDisableGenre.UseVisualStyleBackColor = true;
            buttonDisableGenre.Click += buttonDisableGenre_Click;
            // 
            // tableLayouTrackOperations
            // 
            tableLayouTrackOperations.ColumnCount = 2;
            tableLayouTrackOperations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayouTrackOperations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayouTrackOperations.Controls.Add(trackBarVolume, 1, 0);
            tableLayouTrackOperations.Controls.Add(labelVolume, 0, 0);
            tableLayouTrackOperations.Dock = DockStyle.Fill;
            tableLayouTrackOperations.Location = new Point(3, 129);
            tableLayouTrackOperations.Name = "tableLayouTrackOperations";
            tableLayouTrackOperations.RowCount = 1;
            tableLayouTrackOperations.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayouTrackOperations.Size = new Size(948, 34);
            tableLayouTrackOperations.TabIndex = 2;
            // 
            // trackBarVolume
            // 
            trackBarVolume.Anchor = AnchorStyles.Left;
            trackBarVolume.Location = new Point(477, 3);
            trackBarVolume.Maximum = 100;
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Size = new Size(342, 28);
            trackBarVolume.TabIndex = 1;
            trackBarVolume.Value = 50;
            trackBarVolume.Scroll += trackBarVolume_Scroll;
            // 
            // labelVolume
            // 
            labelVolume.Anchor = AnchorStyles.Right;
            labelVolume.AutoSize = true;
            labelVolume.Location = new Point(421, 9);
            labelVolume.Name = "labelVolume";
            labelVolume.Size = new Size(50, 15);
            labelVolume.TabIndex = 2;
            labelVolume.Text = "Volume:";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(buttonEditGenres, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonStartStop, 2, 0);
            tableLayoutPanel1.Controls.Add(labelProgress, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 169);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(948, 34);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonEditGenres
            // 
            buttonEditGenres.Anchor = AnchorStyles.Left;
            buttonEditGenres.Location = new Point(3, 5);
            buttonEditGenres.Name = "buttonEditGenres";
            buttonEditGenres.Size = new Size(80, 23);
            buttonEditGenres.TabIndex = 1;
            buttonEditGenres.Text = "Edit Genres";
            buttonEditGenres.UseVisualStyleBackColor = true;
            buttonEditGenres.Click += buttonEditGenres_Click;
            // 
            // buttonStartStop
            // 
            buttonStartStop.Anchor = AnchorStyles.Right;
            buttonStartStop.Location = new Point(870, 5);
            buttonStartStop.Name = "buttonStartStop";
            buttonStartStop.Size = new Size(75, 23);
            buttonStartStop.TabIndex = 0;
            buttonStartStop.Text = "Play\\Stop";
            buttonStartStop.UseVisualStyleBackColor = true;
            buttonStartStop.Click += buttonStartStop_Click;
            // 
            // labelProgress
            // 
            labelProgress.Anchor = AnchorStyles.None;
            labelProgress.AutoSize = true;
            labelProgress.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelProgress.ForeColor = Color.ForestGreen;
            labelProgress.Location = new Point(472, 6);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(0, 21);
            labelProgress.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 206);
            Controls.Add(tableLayoutPanelForm);
            Name = "MainForm";
            Text = "Player - genre name - sub genre name";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            tableLayoutPanelForm.ResumeLayout(false);
            tableLayoutPanelForm.PerformLayout();
            tableLayoutPanelGenresOperations.ResumeLayout(false);
            tableLayouTrackOperations.ResumeLayout(false);
            tableLayouTrackOperations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanelForm;
        private Label labelTrackName;
        private TableLayoutPanel tableLayoutPanelGenresOperations;
        private TableLayoutPanel tableLayouTrackOperations;
        private Button buttonStartStop;
        private TrackBar trackBarVolume;
        private Button buttonLike;
        private Button buttonDislike;
        private Button buttonRandom;
        private Button buttonDisableGenre;
        private ComboBox comboBoxGenres;
        private ComboBox comboBoxRandomMode;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonEditGenres;
        private Label labelProgress;
        private Label labelVolume;
    }
}
