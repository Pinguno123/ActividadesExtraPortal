namespace ActividadesExtraPortal
{
    partial class Asociaciones
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
            panel1 = new Panel();
            label4 = new Label();
            label3 = new Label();
            flpAsociacionTarjeta = new FlowLayoutPanel();
            PanelHeader.SuspendLayout();
            panel1.SuspendLayout();
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
            PanelHeader.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Location = new Point(0, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(302, 684);
            panel2.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(232, 247, 255);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(116, 99);
            panel1.Name = "panel1";
            panel1.Size = new Size(948, 50);
            panel1.TabIndex = 10;
            // 
            // label4
            // 
            label4.BackColor = Color.FromArgb(232, 247, 255);
            label4.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(73, 80, 87);
            label4.Location = new Point(16, 10);
            label4.Name = "label4";
            label4.Size = new Size(929, 30);
            label4.TabIndex = 0;
            label4.Text = "Anímate a participar y ser miembro de algunas de las asociaciones estudiantiles que existen en la Universidad Don Bosco.";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Font = new Font("Times New Roman", 24F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(19, 50, 136);
            label3.Location = new Point(121, 53);
            label3.Name = "label3";
            label3.Size = new Size(943, 43);
            label3.TabIndex = 11;
            label3.Text = "Asociaciones";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flpAsociacionTarjeta
            // 
            flpAsociacionTarjeta.AutoScroll = true;
            flpAsociacionTarjeta.Location = new Point(116, 155);
            flpAsociacionTarjeta.Name = "flpAsociacionTarjeta";
            flpAsociacionTarjeta.Size = new Size(948, 466);
            flpAsociacionTarjeta.TabIndex = 12;
            // 
            // Asociaciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(flpAsociacionTarjeta);
            Controls.Add(panel1);
            Controls.Add(label3);
            Controls.Add(PanelHeader);
            MaximumSize = new Size(1200, 800);
            MinimumSize = new Size(1200, 800);
            Name = "Asociaciones";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Actividades Extra-Académicas - Asociaciones";
            FormClosing += Forms_FormClosing;
            PanelHeader.ResumeLayout(false);
            PanelHeader.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Panel PanelHeader;
        private Panel panel2;
        private Panel panel1;
        private Label label4;
        private Label label3;
        private FlowLayoutPanel flpAsociacionTarjeta;
    }
}