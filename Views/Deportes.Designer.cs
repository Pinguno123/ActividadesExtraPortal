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
            panel1 = new Panel();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            button1 = new Button();
            panel3 = new Panel();
            label4 = new Label();
            flpDeportesTarjeta = new FlowLayoutPanel();
            pnBanner.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(118, 165);
            panel1.Name = "panel1";
            panel1.Size = new Size(726, 79);
            panel1.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F);
            label3.Location = new Point(487, 9);
            label3.Name = "label3";
            label3.Size = new Size(224, 60);
            label3.TabIndex = 16;
            label3.Text = "Prof. Juan Manuel Medina SS.CC.\r\ne-mail: juan.medina@udb.edu.sv\r\nTel. 2251-8216";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F);
            label2.Location = new Point(16, 30);
            label2.Name = "label2";
            label2.Size = new Size(454, 20);
            label2.TabIndex = 15;
            label2.Text = "Departamento de Asistencia Estudiantil. Edificio CDIU, primer nivel.";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 9);
            label1.Name = "label1";
            label1.Size = new Size(77, 21);
            label1.TabIndex = 0;
            label1.Text = "Contacto";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 48, 135);
            button1.Font = new Font("Segoe UI", 11F);
            button1.ForeColor = Color.White;
            button1.Location = new Point(850, 167);
            button1.Name = "button1";
            button1.Size = new Size(216, 79);
            button1.TabIndex = 15;
            button1.Text = "Mis Participaciones";
            button1.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            panel3.Controls.Add(label4);
            panel3.Location = new Point(118, 260);
            panel3.Name = "panel3";
            panel3.Size = new Size(948, 62);
            panel3.TabIndex = 16;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(0, 48, 135);
            label4.Location = new Point(5, 13);
            label4.Name = "label4";
            label4.Size = new Size(298, 37);
            label4.TabIndex = 17;
            label4.Text = "Selecciones Deportivas";
            // 
            // flpDeportesTarjeta
            // 
            flpDeportesTarjeta.Location = new Point(118, 328);
            flpDeportesTarjeta.Name = "flpDeportesTarjeta";
            flpDeportesTarjeta.Size = new Size(948, 406);
            flpDeportesTarjeta.TabIndex = 17;
            // 
            // Deportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(flpDeportesTarjeta);
            Controls.Add(panel3);
            Controls.Add(button1);
            Controls.Add(panel1);
            Name = "Deportes";
            Text = "Actividades Extra-Académicas - Deportes";
            Load += Deportes_Load;
            Controls.SetChildIndex(lblNombreOpcion, 0);
            Controls.SetChildIndex(pnBanner, 0);
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(button1, 0);
            Controls.SetChildIndex(panel3, 0);
            Controls.SetChildIndex(flpDeportesTarjeta, 0);
            pnBanner.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Label label1;
        private Label label3;
        private Button button1;
        private Panel panel3;
        private Label label4;
        private FlowLayoutPanel flpDeportesTarjeta;
    }
}