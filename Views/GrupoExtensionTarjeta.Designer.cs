namespace ActividadesExtraPortal.Views
{
    partial class GrupoExtensionTarjeta
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
            panel1 = new Panel();
            lblNombre = new Label();
            label2 = new Label();
            lblInstructor = new Label();
            lblDescripcion = new Label();
            panel2 = new Panel();
            label1 = new Label();
            button1 = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(lblDescripcion);
            panel1.Controls.Add(lblInstructor);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(2, 37);
            panel1.Name = "panel1";
            panel1.Size = new Size(944, 211);
            panel1.TabIndex = 0;
            // 
            // lblNombre
            // 
            lblNombre.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.Location = new Point(14, 11);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(931, 23);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Grupo de DAC número X";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            lblNombre.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 15);
            label2.Name = "label2";
            label2.Size = new Size(80, 20);
            label2.TabIndex = 0;
            label2.Text = "Instructor:";
            label2.Click += label2_Click;
            // 
            // lblInstructor
            // 
            lblInstructor.AutoSize = true;
            lblInstructor.BackColor = Color.White;
            lblInstructor.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInstructor.Location = new Point(98, 15);
            lblInstructor.Name = "lblInstructor";
            lblInstructor.Size = new Size(245, 20);
            lblInstructor.TabIndex = 1;
            lblInstructor.Text = "Nombre Nombre Apellido Apellido";
            // 
            // lblDescripcion
            // 
            lblDescripcion.BackColor = Color.White;
            lblDescripcion.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDescripcion.Location = new Point(12, 44);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(450, 154);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Descripción:";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(255, 243, 205);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(594, 15);
            panel2.Name = "panel2";
            panel2.Size = new Size(334, 183);
            panel2.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(105, 81, 9);
            label1.Location = new Point(15, 11);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 0;
            label1.Text = "Horario";
            // 
            // button1
            // 
            button1.Location = new Point(855, 8);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Inscribir";
            button1.UseVisualStyleBackColor = true;
            // 
            // GrupoExtension
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 193, 7);
            Controls.Add(button1);
            Controls.Add(lblNombre);
            Controls.Add(panel1);
            ForeColor = Color.Black;
            Name = "GrupoExtension";
            Size = new Size(948, 250);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lblNombre;
        private Label label2;
        private Label lblDescripcion;
        private Label lblInstructor;
        private Panel panel2;
        private Label label1;
        private Button button1;
    }
}
