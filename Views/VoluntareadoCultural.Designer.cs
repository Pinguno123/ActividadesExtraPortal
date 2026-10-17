namespace ActividadesExtraPortal.Views
{
    partial class VoluntareadoCultural
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
            rbOfertados = new RadioButton();
            rbInscritos = new RadioButton();
            label1 = new Label();
            dgvVoluntareado = new DataGridView();
            btnRetirar = new Button();
            btnInscribir = new Button();
            pnBanner.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVoluntareado).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 48, 135);
            panel1.Controls.Add(rbOfertados);
            panel1.Controls.Add(rbInscritos);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(118, 155);
            panel1.Name = "panel1";
            panel1.Size = new Size(948, 50);
            panel1.TabIndex = 15;
            // 
            // rbOfertados
            // 
            rbOfertados.AutoSize = true;
            rbOfertados.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbOfertados.ForeColor = Color.White;
            rbOfertados.Location = new Point(103, 13);
            rbOfertados.Name = "rbOfertados";
            rbOfertados.Size = new Size(93, 24);
            rbOfertados.TabIndex = 16;
            rbOfertados.TabStop = true;
            rbOfertados.Text = "Ofertados";
            rbOfertados.UseVisualStyleBackColor = true;
            // 
            // rbInscritos
            // 
            rbInscritos.AutoSize = true;
            rbInscritos.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbInscritos.ForeColor = Color.White;
            rbInscritos.Location = new Point(16, 13);
            rbInscritos.Name = "rbInscritos";
            rbInscritos.Size = new Size(81, 24);
            rbInscritos.TabIndex = 15;
            rbInscritos.TabStop = true;
            rbInscritos.Text = "Inscritos";
            rbInscritos.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(10, 16);
            label1.Name = "label1";
            label1.Size = new Size(0, 20);
            label1.TabIndex = 0;
            // 
            // dgvVoluntareado
            // 
            dgvVoluntareado.BackgroundColor = SystemColors.Control;
            dgvVoluntareado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVoluntareado.Location = new Point(118, 211);
            dgvVoluntareado.Name = "dgvVoluntareado";
            dgvVoluntareado.Size = new Size(948, 462);
            dgvVoluntareado.TabIndex = 16;
            // 
            // btnRetirar
            // 
            btnRetirar.Location = new Point(760, 679);
            btnRetirar.Name = "btnRetirar";
            btnRetirar.Size = new Size(150, 50);
            btnRetirar.TabIndex = 22;
            btnRetirar.Text = "Retirar";
            btnRetirar.UseVisualStyleBackColor = true;
            // 
            // btnInscribir
            // 
            btnInscribir.Location = new Point(916, 679);
            btnInscribir.Name = "btnInscribir";
            btnInscribir.Size = new Size(150, 50);
            btnInscribir.TabIndex = 21;
            btnInscribir.Text = "Inscribirse";
            btnInscribir.UseVisualStyleBackColor = true;
            // 
            // VoluntareadoCultural
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(btnRetirar);
            Controls.Add(btnInscribir);
            Controls.Add(panel1);
            Controls.Add(dgvVoluntareado);
            Name = "VoluntareadoCultural";
            Text = "VoluntareadoCultural";
            Controls.SetChildIndex(lblNombreOpcion, 0);
            Controls.SetChildIndex(pnBanner, 0);
            Controls.SetChildIndex(dgvVoluntareado, 0);
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(btnInscribir, 0);
            Controls.SetChildIndex(btnRetirar, 0);
            pnBanner.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVoluntareado).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private RadioButton rbOfertados;
        private RadioButton rbInscritos;
        private Label label1;
        private DataGridView dgvVoluntareado;
        private Button btnRetirar;
        private Button btnInscribir;
    }
}