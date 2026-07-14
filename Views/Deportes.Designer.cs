namespace ActividadesExtraPortal
{
    partial class Deportes
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
            label1 = new Label();
            PanelHeader = new Panel();
            panel2 = new Panel();
            PanelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = Cursors.Hand;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(122, 30);
            label1.TabIndex = 0;
            label1.Text = "Estudiantes";
            label1.Click += label1_Click;
            // 
            // PanelHeader
            // 
            PanelHeader.BackColor = Color.FromArgb(0, 48, 135);
            PanelHeader.Controls.Add(panel2);
            PanelHeader.Controls.Add(label1);
            PanelHeader.Location = new Point(0, 0);
            PanelHeader.Margin = new Padding(0);
            PanelHeader.Name = "PanelHeader";
            PanelHeader.Size = new Size(1185, 50);
            PanelHeader.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Location = new Point(0, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(302, 684);
            panel2.TabIndex = 1;
            // 
            // Deportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(PanelHeader);
            MaximumSize = new Size(1200, 800);
            MinimumSize = new Size(1200, 800);
            Name = "Deportes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Actividades Extra-Académicas - Deportes";
            FormClosing += Forms_FormClosing;
            Load += Deportes_Load;
            PanelHeader.ResumeLayout(false);
            PanelHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Panel PanelHeader;
        private Panel panel2;
    }
}