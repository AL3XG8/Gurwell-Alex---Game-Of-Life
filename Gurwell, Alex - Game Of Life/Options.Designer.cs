
namespace Gurwell__Alex___Game_Of_Life
{
    partial class Options
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
            this.Cancel = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.TickerInterval = new System.Windows.Forms.NumericUpDown();
            this.XCells = new System.Windows.Forms.NumericUpDown();
            this.YCells = new System.Windows.Forms.NumericUpDown();
            this.YCellsLabel = new System.Windows.Forms.Label();
            this.XCellsLabel = new System.Windows.Forms.Label();
            this.TickerIntervalLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TickerInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XCells)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YCells)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(33, 157);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 0;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            this.Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Save.Location = new System.Drawing.Point(153, 157);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            // 
            // TickerInterval
            // 
            this.TickerInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TickerInterval.Location = new System.Drawing.Point(177, 23);
            this.TickerInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.TickerInterval.Name = "TickerInterval";
            this.TickerInterval.Size = new System.Drawing.Size(81, 26);
            this.TickerInterval.TabIndex = 3;
            this.TickerInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TickerInterval.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // XCells
            // 
            this.XCells.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XCells.Location = new System.Drawing.Point(177, 63);
            this.XCells.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.XCells.Name = "XCells";
            this.XCells.Size = new System.Drawing.Size(81, 26);
            this.XCells.TabIndex = 4;
            this.XCells.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.XCells.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // YCells
            // 
            this.YCells.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YCells.Location = new System.Drawing.Point(177, 103);
            this.YCells.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.YCells.Name = "YCells";
            this.YCells.Size = new System.Drawing.Size(81, 26);
            this.YCells.TabIndex = 5;
            this.YCells.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.YCells.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // YCellsLabel
            // 
            this.YCellsLabel.AutoSize = true;
            this.YCellsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YCellsLabel.Location = new System.Drawing.Point(13, 105);
            this.YCellsLabel.Name = "YCellsLabel";
            this.YCellsLabel.Size = new System.Drawing.Size(106, 20);
            this.YCellsLabel.TabIndex = 6;
            this.YCellsLabel.Text = "Cells Height";
            // 
            // XCellsLabel
            // 
            this.XCellsLabel.AutoSize = true;
            this.XCellsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XCellsLabel.Location = new System.Drawing.Point(13, 65);
            this.XCellsLabel.Name = "XCellsLabel";
            this.XCellsLabel.Size = new System.Drawing.Size(99, 20);
            this.XCellsLabel.TabIndex = 7;
            this.XCellsLabel.Text = "Cells Width";
            // 
            // TickerIntervalLabel
            // 
            this.TickerIntervalLabel.AutoSize = true;
            this.TickerIntervalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TickerIntervalLabel.Location = new System.Drawing.Point(12, 25);
            this.TickerIntervalLabel.Name = "TickerIntervalLabel";
            this.TickerIntervalLabel.Size = new System.Drawing.Size(158, 20);
            this.TickerIntervalLabel.TabIndex = 8;
            this.TickerIntervalLabel.Text = "Timer Interval (ms)";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 192);
            this.Controls.Add(this.TickerIntervalLabel);
            this.Controls.Add(this.XCellsLabel);
            this.Controls.Add(this.YCellsLabel);
            this.Controls.Add(this.YCells);
            this.Controls.Add(this.XCells);
            this.Controls.Add(this.TickerInterval);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.TickerInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XCells)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YCells)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.NumericUpDown TickerInterval;
        private System.Windows.Forms.NumericUpDown XCells;
        private System.Windows.Forms.NumericUpDown YCells;
        private System.Windows.Forms.Label YCellsLabel;
        private System.Windows.Forms.Label XCellsLabel;
        private System.Windows.Forms.Label TickerIntervalLabel;
    }
}