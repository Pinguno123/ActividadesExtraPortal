namespace ActividadesExtraPortal.Views
{
    partial class DeportesTarjeta
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            lblDeporteTitulo = new Label();
            lblRama = new Label();
            lblDeporte = new Label();
            btnInscribir = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(300, 210);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblDeporteTitulo
            // 
            lblDeporteTitulo.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDeporteTitulo.ForeColor = Color.FromArgb(13, 110, 253);
            lblDeporteTitulo.Location = new Point(3, 217);
            lblDeporteTitulo.Name = "lblDeporteTitulo";
            lblDeporteTitulo.Size = new Size(294, 23);
            lblDeporteTitulo.TabIndex = 1;
            lblDeporteTitulo.Text = "Fútbol";
            lblDeporteTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRama
            // 
            lblRama.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRama.ForeColor = Color.Black;
            lblRama.Location = new Point(3, 252);
            lblRama.Name = "lblRama";
            lblRama.Size = new Size(151, 23);
            lblRama.TabIndex = 2;
            lblRama.Text = "Rama: Masculina";
            lblRama.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDeporte
            // 
            lblDeporte.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDeporte.ForeColor = Color.Black;
            lblDeporte.Location = new Point(160, 252);
            lblDeporte.Name = "lblDeporte";
            lblDeporte.Size = new Size(137, 23);
            lblDeporte.TabIndex = 3;
            lblDeporte.Text = "Deporte: Fútbol";
            lblDeporte.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnInscribir
            // 
            btnInscribir.BackColor = Color.FromArgb(13, 110, 253);
            btnInscribir.FlatStyle = FlatStyle.Flat;
            btnInscribir.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnInscribir.ForeColor = Color.White;
            btnInscribir.Location = new Point(90, 294);
            btnInscribir.Name = "btnInscribir";
            btnInscribir.Size = new Size(120, 36);
            btnInscribir.TabIndex = 4;
            btnInscribir.Text = "Inscribir";
            btnInscribir.UseVisualStyleBackColor = false;
            // 
            // DeportesTarjeta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnInscribir);
            Controls.Add(lblDeporte);
            Controls.Add(lblRama);
            Controls.Add(lblDeporteTitulo);
            Controls.Add(pictureBox1);
            Name = "DeportesTarjeta";
            Size = new Size(300, 350);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblDeporteTitulo;
        private Label lblRama;
        private Label lblDeporte;
        private Button btnInscribir;
    }
}
