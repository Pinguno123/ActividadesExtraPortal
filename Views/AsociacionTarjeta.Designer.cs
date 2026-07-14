namespace ActividadesExtraPortal.Views
{
    partial class AsociacionTarjeta
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
            lblNombre = new Label();
            lblFunda = new Label();
            lblDesc = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblDesc
            // 
            lblDesc.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDesc.Location = new Point(3, 318);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(219, 71);
            lblDesc.TabIndex = 3;
            lblDesc.Text = "Descripción:  Asociciación de estudiantes de la facultad de humanidades de la escuela de";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(225, 206);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblNombre
            // 
            lblNombre.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNombre.Location = new Point(0, 209);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(225, 58);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Asociación de Comunicadores Universitarios Salesianos -";
            lblNombre.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFunda
            // 
            lblFunda.AutoSize = true;
            lblFunda.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFunda.Location = new Point(3, 285);
            lblFunda.Name = "lblFunda";
            lblFunda.Size = new Size(120, 20);
            lblFunda.TabIndex = 2;
            lblFunda.Text = "Fundación: XXXX";
            lblFunda.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AsociacionTarjeta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            Controls.Add(lblDesc);
            Controls.Add(lblFunda);
            Controls.Add(lblNombre);
            Controls.Add(pictureBox1);
            Name = "AsociacionTarjeta";
            Size = new Size(225, 400);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblNombre;
        private Label lblFunda;
        private Label lblDesc;
    }
}
