namespace ActividadesExtraPortal.Views
{
    partial class PlantillaAACD
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
            btnReturn = new Label();
            PanelHeader = new Panel();
            panel2 = new Panel();
            pnBanner = new Panel();
            lblBanner = new Label();
            lblNombreOpcion = new Label();
            PanelHeader.SuspendLayout();
            pnBanner.SuspendLayout();
            SuspendLayout();
            // 
            // btnReturn
            // 
            btnReturn.AutoSize = true;
            btnReturn.Cursor = Cursors.Hand;
            btnReturn.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReturn.ForeColor = Color.White;
            btnReturn.Location = new Point(12, 9);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(122, 30);
            btnReturn.TabIndex = 0;
            btnReturn.Text = "Estudiantes";
            btnReturn.Click += btnReturn_Click;
            // 
            // PanelHeader
            // 
            PanelHeader.BackColor = Color.FromArgb(0, 48, 135);
            PanelHeader.Controls.Add(panel2);
            PanelHeader.Controls.Add(btnReturn);
            PanelHeader.Location = new Point(0, 0);
            PanelHeader.Margin = new Padding(0);
            PanelHeader.Name = "PanelHeader";
            PanelHeader.Size = new Size(1185, 50);
            PanelHeader.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.Location = new Point(0, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(302, 684);
            panel2.TabIndex = 1;
            // 
            // pnBanner
            // 
            pnBanner.BackColor = Color.FromArgb(232, 247, 255);
            pnBanner.Controls.Add(lblBanner);
            pnBanner.Location = new Point(118, 99);
            pnBanner.Name = "pnBanner";
            pnBanner.Size = new Size(948, 50);
            pnBanner.TabIndex = 12;
            // 
            // lblBanner
            // 
            lblBanner.BackColor = Color.Transparent;
            lblBanner.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblBanner.ForeColor = Color.FromArgb(73, 80, 87);
            lblBanner.Location = new Point(10, 10);
            lblBanner.Name = "lblBanner";
            lblBanner.Size = new Size(929, 30);
            lblBanner.TabIndex = 0;
            lblBanner.Text = "Texto que va a ser el banner de la pagina";
            lblBanner.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNombreOpcion
            // 
            lblNombreOpcion.Font = new Font("Times New Roman", 24F, FontStyle.Bold);
            lblNombreOpcion.ForeColor = Color.FromArgb(19, 50, 136);
            lblNombreOpcion.Location = new Point(123, 53);
            lblNombreOpcion.Name = "lblNombreOpcion";
            lblNombreOpcion.Size = new Size(943, 43);
            lblNombreOpcion.TabIndex = 13;
            lblNombreOpcion.Text = "Opción escogida";
            lblNombreOpcion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PlantillaAACD
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(pnBanner);
            Controls.Add(lblNombreOpcion);
            Controls.Add(PanelHeader);
            MaximumSize = new Size(1200, 800);
            MinimumSize = new Size(1200, 800);
            Name = "PlantillaAACD";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PlantillaAACD";
            FormClosing += PlantillaAACD_FormClosing;
            PanelHeader.ResumeLayout(false);
            PanelHeader.PerformLayout();
            pnBanner.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label btnReturn;
        private Panel PanelHeader;
        private Panel panel2;
        protected Label lblBanner;
        protected Label lblNombreOpcion;
        protected Panel pnBanner;
    }
}