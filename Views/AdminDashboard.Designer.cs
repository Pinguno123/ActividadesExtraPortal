using System.Windows.Forms;
using System.Drawing;

namespace ActividadesExtraPortal
{
    partial class AdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            PanelHeader = new Panel();
            btnVolver = new Button();
            lblTitle = new Label();
            tabAdminCrud = new TabControl();
            tabCursos = new TabPage();
            gridCursos = new DataGridView();
            lblCursoId = new Label();
            txtCursoId = new TextBox();
            lblCursoNombre = new Label();
            txtCursoNombre = new TextBox();
            lblCursoDesc = new Label();
            txtCursoDesc = new TextBox();
            lblCursoInicio = new Label();
            dtpCursoInicio = new DateTimePicker();
            lblCursoFin = new Label();
            dtpCursoFin = new DateTimePicker();
            lblCursoHorario = new Label();
            txtCursoHorario = new TextBox();
            lblCursoCupo = new Label();
            txtCursoCupo = new TextBox();
            lblCursoInstructor = new Label();
            cboCursoInstructor = new ComboBox();
            lblCursoEstado = new Label();
            txtCursoEstado = new TextBox();
            btnCrearCurso = new Button();
            btnActualizarCurso = new Button();
            tabAsociaciones = new TabPage();
            gridAsociaciones = new DataGridView();
            lblAsoAcronimo = new Label();
            txtAsoAcronimo = new TextBox();
            lblAsoNombre = new Label();
            txtAsoNombre = new TextBox();
            lblAsoAnio = new Label();
            txtAsoAnio = new TextBox();
            lblAsoDesc = new Label();
            txtAsoDesc = new TextBox();
            lblAsoFormulario = new Label();
            txtAsoFormulario = new TextBox();
            lblAsoRutaImg = new Label();
            txtAsoRutaImg = new TextBox();
            btnAsoBuscarImg = new Button();
            btnCrearAso = new Button();
            tabDeportes = new TabPage();
            gridDeportes = new DataGridView();
            lblDepDisciplina = new Label();
            txtDepDisciplina = new TextBox();
            lblDepRama = new Label();
            txtDepRama = new TextBox();
            lblDepEquipo = new Label();
            txtDepEquipo = new TextBox();
            lblDepRutaImg = new Label();
            txtDepRutaImg = new TextBox();
            btnDepBuscarImg = new Button();
            btnCrearDeporte = new Button();
            tabArte = new TabPage();
            gridArte = new DataGridView();
            lblArteCatId = new Label();
            cboArteCategoria = new ComboBox();
            lblArteNombre = new Label();
            txtArteNombre = new TextBox();
            lblArteDesc = new Label();
            txtArteDesc = new TextBox();
            lblArteInstructor = new Label();
            cboArteInstructor = new ComboBox();
            btnCrearArte = new Button();
            PanelHeader.SuspendLayout();
            tabAdminCrud.SuspendLayout();
            tabCursos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCursos).BeginInit();
            tabAsociaciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridAsociaciones).BeginInit();
            tabDeportes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridDeportes).BeginInit();
            tabArte.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridArte).BeginInit();
            SuspendLayout();
            // 
            // PanelHeader
            // 
            PanelHeader.BackColor = Color.FromArgb(0, 48, 135);
            PanelHeader.Controls.Add(btnVolver);
            PanelHeader.Controls.Add(lblTitle);
            PanelHeader.Location = new Point(0, 0);
            PanelHeader.Name = "PanelHeader";
            PanelHeader.Size = new Size(1185, 50);
            PanelHeader.TabIndex = 0;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(24, 131, 81);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(1020, 10);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(140, 30);
            btnVolver.TabIndex = 0;
            btnVolver.Text = "< Volver al Portal";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(243, 30);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Panel de Administración";
            // 
            // tabAdminCrud
            // 
            tabAdminCrud.Controls.Add(tabCursos);
            tabAdminCrud.Controls.Add(tabAsociaciones);
            tabAdminCrud.Controls.Add(tabDeportes);
            tabAdminCrud.Controls.Add(tabArte);
            tabAdminCrud.Font = new Font("Segoe UI", 10F);
            tabAdminCrud.Location = new Point(15, 65);
            tabAdminCrud.Name = "tabAdminCrud";
            tabAdminCrud.SelectedIndex = 0;
            tabAdminCrud.Size = new Size(1150, 680);
            tabAdminCrud.TabIndex = 1;
            // 
            // tabCursos
            // 
            tabCursos.Controls.Add(gridCursos);
            tabCursos.Controls.Add(lblCursoId);
            tabCursos.Controls.Add(txtCursoId);
            tabCursos.Controls.Add(lblCursoNombre);
            tabCursos.Controls.Add(txtCursoNombre);
            tabCursos.Controls.Add(lblCursoDesc);
            tabCursos.Controls.Add(txtCursoDesc);
            tabCursos.Controls.Add(lblCursoInicio);
            tabCursos.Controls.Add(dtpCursoInicio);
            tabCursos.Controls.Add(lblCursoFin);
            tabCursos.Controls.Add(dtpCursoFin);
            tabCursos.Controls.Add(lblCursoHorario);
            tabCursos.Controls.Add(txtCursoHorario);
            tabCursos.Controls.Add(lblCursoCupo);
            tabCursos.Controls.Add(txtCursoCupo);
            tabCursos.Controls.Add(lblCursoInstructor);
            tabCursos.Controls.Add(cboCursoInstructor);
            tabCursos.Controls.Add(lblCursoEstado);
            tabCursos.Controls.Add(txtCursoEstado);
            tabCursos.Controls.Add(btnCrearCurso);
            tabCursos.Controls.Add(btnActualizarCurso);
            tabCursos.Location = new Point(4, 26);
            tabCursos.Name = "tabCursos";
            tabCursos.Size = new Size(1142, 650);
            tabCursos.TabIndex = 0;
            tabCursos.Text = "Cursos";
            // 
            // gridCursos
            // 
            gridCursos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridCursos.Location = new Point(20, 20);
            gridCursos.MultiSelect = false;
            gridCursos.Name = "gridCursos";
            gridCursos.ReadOnly = true;
            gridCursos.Size = new Size(700, 600);
            gridCursos.TabIndex = 0;
            gridCursos.RowHeaderMouseClick += gridCursos_RowHeaderMouseClick;
            // 
            // lblCursoId
            // 
            lblCursoId.Location = new Point(740, 20);
            lblCursoId.Name = "lblCursoId";
            lblCursoId.Size = new Size(150, 20);
            lblCursoId.TabIndex = 1;
            lblCursoId.Text = "Id Curso (Numérico):";
            // 
            // txtCursoId
            // 
            txtCursoId.Enabled = false;
            txtCursoId.Location = new Point(740, 40);
            txtCursoId.Name = "txtCursoId";
            txtCursoId.Size = new Size(370, 25);
            txtCursoId.TabIndex = 2;
            // 
            // lblCursoNombre
            // 
            lblCursoNombre.Location = new Point(740, 75);
            lblCursoNombre.Name = "lblCursoNombre";
            lblCursoNombre.Size = new Size(150, 20);
            lblCursoNombre.TabIndex = 3;
            lblCursoNombre.Text = "Nombre del Curso:";
            // 
            // txtCursoNombre
            // 
            txtCursoNombre.Location = new Point(740, 95);
            txtCursoNombre.Name = "txtCursoNombre";
            txtCursoNombre.Size = new Size(370, 25);
            txtCursoNombre.TabIndex = 4;
            // 
            // lblCursoDesc
            // 
            lblCursoDesc.Location = new Point(740, 130);
            lblCursoDesc.Name = "lblCursoDesc";
            lblCursoDesc.Size = new Size(150, 20);
            lblCursoDesc.TabIndex = 5;
            lblCursoDesc.Text = "Descripción:";
            // 
            // txtCursoDesc
            // 
            txtCursoDesc.Location = new Point(740, 150);
            txtCursoDesc.Name = "txtCursoDesc";
            txtCursoDesc.Size = new Size(370, 25);
            txtCursoDesc.TabIndex = 6;
            // 
            // lblCursoInicio
            // 
            lblCursoInicio.Location = new Point(740, 185);
            lblCursoInicio.Name = "lblCursoInicio";
            lblCursoInicio.Size = new Size(150, 20);
            lblCursoInicio.TabIndex = 7;
            lblCursoInicio.Text = "Fecha de Inicio:";
            // 
            // dtpCursoInicio
            // 
            dtpCursoInicio.Location = new Point(740, 205);
            dtpCursoInicio.Name = "dtpCursoInicio";
            dtpCursoInicio.Size = new Size(370, 25);
            dtpCursoInicio.TabIndex = 8;
            // 
            // lblCursoFin
            // 
            lblCursoFin.Location = new Point(740, 240);
            lblCursoFin.Name = "lblCursoFin";
            lblCursoFin.Size = new Size(150, 20);
            lblCursoFin.TabIndex = 9;
            lblCursoFin.Text = "Fecha de Fin:";
            // 
            // dtpCursoFin
            // 
            dtpCursoFin.Location = new Point(740, 260);
            dtpCursoFin.Name = "dtpCursoFin";
            dtpCursoFin.Size = new Size(370, 25);
            dtpCursoFin.TabIndex = 10;
            // 
            // lblCursoHorario
            // 
            lblCursoHorario.Location = new Point(740, 295);
            lblCursoHorario.Name = "lblCursoHorario";
            lblCursoHorario.Size = new Size(250, 20);
            lblCursoHorario.TabIndex = 11;
            lblCursoHorario.Text = "Horario (ej. Sábado 8:00 - 12:00):";
            // 
            // txtCursoHorario
            // 
            txtCursoHorario.Location = new Point(740, 315);
            txtCursoHorario.Name = "txtCursoHorario";
            txtCursoHorario.Size = new Size(370, 25);
            txtCursoHorario.TabIndex = 12;
            // 
            // lblCursoCupo
            // 
            lblCursoCupo.Location = new Point(740, 350);
            lblCursoCupo.Name = "lblCursoCupo";
            lblCursoCupo.Size = new Size(150, 20);
            lblCursoCupo.TabIndex = 13;
            lblCursoCupo.Text = "Cupo Máximo:";
            // 
            // txtCursoCupo
            // 
            txtCursoCupo.Location = new Point(740, 370);
            txtCursoCupo.Name = "txtCursoCupo";
            txtCursoCupo.Size = new Size(370, 25);
            txtCursoCupo.TabIndex = 14;
            // 
            // lblCursoInstructor
            // 
            lblCursoInstructor.Location = new Point(740, 405);
            lblCursoInstructor.Name = "lblCursoInstructor";
            lblCursoInstructor.Size = new Size(200, 20);
            lblCursoInstructor.TabIndex = 15;
            lblCursoInstructor.Text = "Instructor (Opcional):";
            // 
            // cboCursoInstructor
            // 
            cboCursoInstructor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCursoInstructor.Location = new Point(740, 425);
            cboCursoInstructor.Name = "cboCursoInstructor";
            cboCursoInstructor.Size = new Size(370, 25);
            cboCursoInstructor.TabIndex = 16;
            // 
            // lblCursoEstado
            // 
            lblCursoEstado.Location = new Point(740, 460);
            lblCursoEstado.Name = "lblCursoEstado";
            lblCursoEstado.Size = new Size(200, 20);
            lblCursoEstado.TabIndex = 17;
            lblCursoEstado.Text = "Estado (Activo, Inactivo):";
            // 
            // txtCursoEstado
            // 
            txtCursoEstado.Location = new Point(740, 480);
            txtCursoEstado.Name = "txtCursoEstado";
            txtCursoEstado.Size = new Size(370, 25);
            txtCursoEstado.TabIndex = 18;
            txtCursoEstado.Text = "Activo";
            // 
            // btnCrearCurso
            // 
            btnCrearCurso.BackColor = Color.FromArgb(0, 48, 135);
            btnCrearCurso.FlatStyle = FlatStyle.Flat;
            btnCrearCurso.ForeColor = Color.White;
            btnCrearCurso.Location = new Point(740, 530);
            btnCrearCurso.Name = "btnCrearCurso";
            btnCrearCurso.Size = new Size(175, 40);
            btnCrearCurso.TabIndex = 19;
            btnCrearCurso.Text = "Crear Curso";
            btnCrearCurso.UseVisualStyleBackColor = false;
            btnCrearCurso.Click += btnCrearCurso_Click;
            // 
            // btnActualizarCurso
            // 
            btnActualizarCurso.BackColor = Color.FromArgb(24, 131, 81);
            btnActualizarCurso.FlatStyle = FlatStyle.Flat;
            btnActualizarCurso.ForeColor = Color.White;
            btnActualizarCurso.Location = new Point(935, 530);
            btnActualizarCurso.Name = "btnActualizarCurso";
            btnActualizarCurso.Size = new Size(175, 40);
            btnActualizarCurso.TabIndex = 20;
            btnActualizarCurso.Text = "Actualizar Curso";
            btnActualizarCurso.UseVisualStyleBackColor = false;
            btnActualizarCurso.Click += btnActualizarCurso_Click;
            // 
            // tabAsociaciones
            // 
            tabAsociaciones.Controls.Add(gridAsociaciones);
            tabAsociaciones.Controls.Add(lblAsoAcronimo);
            tabAsociaciones.Controls.Add(txtAsoAcronimo);
            tabAsociaciones.Controls.Add(lblAsoNombre);
            tabAsociaciones.Controls.Add(txtAsoNombre);
            tabAsociaciones.Controls.Add(lblAsoAnio);
            tabAsociaciones.Controls.Add(txtAsoAnio);
            tabAsociaciones.Controls.Add(lblAsoDesc);
            tabAsociaciones.Controls.Add(txtAsoDesc);
            tabAsociaciones.Controls.Add(lblAsoFormulario);
            tabAsociaciones.Controls.Add(txtAsoFormulario);
            tabAsociaciones.Controls.Add(lblAsoRutaImg);
            tabAsociaciones.Controls.Add(txtAsoRutaImg);
            tabAsociaciones.Controls.Add(btnAsoBuscarImg);
            tabAsociaciones.Controls.Add(btnCrearAso);
            tabAsociaciones.Location = new Point(4, 26);
            tabAsociaciones.Name = "tabAsociaciones";
            tabAsociaciones.Size = new Size(1142, 650);
            tabAsociaciones.TabIndex = 1;
            tabAsociaciones.Text = "Asociaciones";
            // 
            // gridAsociaciones
            // 
            gridAsociaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridAsociaciones.Location = new Point(20, 20);
            gridAsociaciones.Name = "gridAsociaciones";
            gridAsociaciones.ReadOnly = true;
            gridAsociaciones.Size = new Size(700, 600);
            gridAsociaciones.TabIndex = 0;
            // 
            // lblAsoAcronimo
            // 
            lblAsoAcronimo.Location = new Point(740, 20);
            lblAsoAcronimo.Name = "lblAsoAcronimo";
            lblAsoAcronimo.Size = new Size(200, 20);
            lblAsoAcronimo.TabIndex = 1;
            lblAsoAcronimo.Text = "Acrónimo (ej. ASEICC):";
            // 
            // txtAsoAcronimo
            // 
            txtAsoAcronimo.Location = new Point(740, 40);
            txtAsoAcronimo.Name = "txtAsoAcronimo";
            txtAsoAcronimo.Size = new Size(370, 25);
            txtAsoAcronimo.TabIndex = 2;
            // 
            // lblAsoNombre
            // 
            lblAsoNombre.Location = new Point(740, 80);
            lblAsoNombre.Name = "lblAsoNombre";
            lblAsoNombre.Size = new Size(200, 20);
            lblAsoNombre.TabIndex = 3;
            lblAsoNombre.Text = "Nombre Completo:";
            // 
            // txtAsoNombre
            // 
            txtAsoNombre.Location = new Point(740, 100);
            txtAsoNombre.Name = "txtAsoNombre";
            txtAsoNombre.Size = new Size(370, 25);
            txtAsoNombre.TabIndex = 4;
            // 
            // lblAsoAnio
            // 
            lblAsoAnio.Location = new Point(740, 140);
            lblAsoAnio.Name = "lblAsoAnio";
            lblAsoAnio.Size = new Size(200, 20);
            lblAsoAnio.TabIndex = 5;
            lblAsoAnio.Text = "Año de Fundación:";
            // 
            // txtAsoAnio
            // 
            txtAsoAnio.Location = new Point(740, 160);
            txtAsoAnio.Name = "txtAsoAnio";
            txtAsoAnio.Size = new Size(370, 25);
            txtAsoAnio.TabIndex = 6;
            // 
            // lblAsoDesc
            // 
            lblAsoDesc.Location = new Point(740, 200);
            lblAsoDesc.Name = "lblAsoDesc";
            lblAsoDesc.Size = new Size(200, 20);
            lblAsoDesc.TabIndex = 7;
            lblAsoDesc.Text = "Descripción:";
            // 
            // txtAsoDesc
            // 
            txtAsoDesc.Location = new Point(740, 220);
            txtAsoDesc.Multiline = true;
            txtAsoDesc.Name = "txtAsoDesc";
            txtAsoDesc.Size = new Size(370, 100);
            txtAsoDesc.TabIndex = 8;
            // 
            // lblAsoFormulario
            // 
            lblAsoFormulario.Location = new Point(740, 330);
            lblAsoFormulario.Name = "lblAsoFormulario";
            lblAsoFormulario.Size = new Size(200, 20);
            lblAsoFormulario.TabIndex = 9;
            lblAsoFormulario.Text = "Formulario de Inscripción:";
            // 
            // txtAsoFormulario
            // 
            txtAsoFormulario.Location = new Point(740, 350);
            txtAsoFormulario.Multiline = true;
            txtAsoFormulario.Name = "txtAsoFormulario";
            txtAsoFormulario.Size = new Size(370, 80);
            txtAsoFormulario.TabIndex = 10;
            // 
            // lblAsoRutaImg
            // 
            lblAsoRutaImg.Location = new Point(740, 440);
            lblAsoRutaImg.Name = "lblAsoRutaImg";
            lblAsoRutaImg.Size = new Size(200, 20);
            lblAsoRutaImg.TabIndex = 11;
            lblAsoRutaImg.Text = "Imagen de la Asociación:";
            // 
            // txtAsoRutaImg
            // 
            txtAsoRutaImg.Location = new Point(740, 460);
            txtAsoRutaImg.Name = "txtAsoRutaImg";
            txtAsoRutaImg.ReadOnly = true;
            txtAsoRutaImg.Size = new Size(270, 25);
            txtAsoRutaImg.TabIndex = 12;
            // 
            // btnAsoBuscarImg
            // 
            btnAsoBuscarImg.Location = new Point(1020, 460);
            btnAsoBuscarImg.Name = "btnAsoBuscarImg";
            btnAsoBuscarImg.Size = new Size(90, 25);
            btnAsoBuscarImg.TabIndex = 13;
            btnAsoBuscarImg.Text = "Buscar...";
            btnAsoBuscarImg.UseVisualStyleBackColor = true;
            btnAsoBuscarImg.Click += btnAsoBuscarImg_Click;
            // 
            // btnCrearAso
            // 
            btnCrearAso.BackColor = Color.FromArgb(0, 48, 135);
            btnCrearAso.FlatStyle = FlatStyle.Flat;
            btnCrearAso.ForeColor = Color.White;
            btnCrearAso.Location = new Point(740, 500);
            btnCrearAso.Name = "btnCrearAso";
            btnCrearAso.Size = new Size(370, 40);
            btnCrearAso.TabIndex = 14;
            btnCrearAso.Text = "Crear Asociación";
            btnCrearAso.UseVisualStyleBackColor = false;
            btnCrearAso.Click += btnCrearAso_Click;
            // 
            // tabDeportes
            // 
            tabDeportes.Controls.Add(gridDeportes);
            tabDeportes.Controls.Add(lblDepDisciplina);
            tabDeportes.Controls.Add(txtDepDisciplina);
            tabDeportes.Controls.Add(lblDepRama);
            tabDeportes.Controls.Add(txtDepRama);
            tabDeportes.Controls.Add(lblDepEquipo);
            tabDeportes.Controls.Add(txtDepEquipo);
            tabDeportes.Controls.Add(lblDepRutaImg);
            tabDeportes.Controls.Add(txtDepRutaImg);
            tabDeportes.Controls.Add(btnDepBuscarImg);
            tabDeportes.Controls.Add(btnCrearDeporte);
            tabDeportes.Location = new Point(4, 26);
            tabDeportes.Name = "tabDeportes";
            tabDeportes.Size = new Size(1142, 650);
            tabDeportes.TabIndex = 2;
            tabDeportes.Text = "Deportes";
            // 
            // gridDeportes
            // 
            gridDeportes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridDeportes.Location = new Point(20, 20);
            gridDeportes.Name = "gridDeportes";
            gridDeportes.ReadOnly = true;
            gridDeportes.Size = new Size(700, 600);
            gridDeportes.TabIndex = 0;
            // 
            // lblDepDisciplina
            // 
            lblDepDisciplina.Location = new Point(740, 20);
            lblDepDisciplina.Name = "lblDepDisciplina";
            lblDepDisciplina.Size = new Size(250, 20);
            lblDepDisciplina.TabIndex = 1;
            lblDepDisciplina.Text = "Disciplina (ej. Fútbol, Ajedrez):";
            // 
            // txtDepDisciplina
            // 
            txtDepDisciplina.Location = new Point(740, 40);
            txtDepDisciplina.Name = "txtDepDisciplina";
            txtDepDisciplina.Size = new Size(370, 25);
            txtDepDisciplina.TabIndex = 2;
            // 
            // lblDepRama
            // 
            lblDepRama.Location = new Point(740, 80);
            lblDepRama.Name = "lblDepRama";
            lblDepRama.Size = new Size(250, 20);
            lblDepRama.TabIndex = 3;
            lblDepRama.Text = "Rama (Masculina, Femenina, Mixto):";
            // 
            // txtDepRama
            // 
            txtDepRama.Location = new Point(740, 100);
            txtDepRama.Name = "txtDepRama";
            txtDepRama.Size = new Size(370, 25);
            txtDepRama.TabIndex = 4;
            // 
            // lblDepEquipo
            // 
            lblDepEquipo.Location = new Point(740, 140);
            lblDepEquipo.Name = "lblDepEquipo";
            lblDepEquipo.Size = new Size(200, 20);
            lblDepEquipo.TabIndex = 5;
            lblDepEquipo.Text = "Nombre del Equipo:";
            // 
            // txtDepEquipo
            // 
            txtDepEquipo.Location = new Point(740, 160);
            txtDepEquipo.Name = "txtDepEquipo";
            txtDepEquipo.Size = new Size(370, 25);
            txtDepEquipo.TabIndex = 6;
            // 
            // lblDepRutaImg
            // 
            lblDepRutaImg.Location = new Point(740, 200);
            lblDepRutaImg.Name = "lblDepRutaImg";
            lblDepRutaImg.Size = new Size(200, 20);
            lblDepRutaImg.TabIndex = 7;
            lblDepRutaImg.Text = "Imagen del Equipo:";
            // 
            // txtDepRutaImg
            // 
            txtDepRutaImg.Location = new Point(740, 220);
            txtDepRutaImg.Name = "txtDepRutaImg";
            txtDepRutaImg.ReadOnly = true;
            txtDepRutaImg.Size = new Size(270, 25);
            txtDepRutaImg.TabIndex = 8;
            // 
            // btnDepBuscarImg
            // 
            btnDepBuscarImg.Location = new Point(1020, 220);
            btnDepBuscarImg.Name = "btnDepBuscarImg";
            btnDepBuscarImg.Size = new Size(90, 25);
            btnDepBuscarImg.TabIndex = 9;
            btnDepBuscarImg.Text = "Buscar...";
            btnDepBuscarImg.UseVisualStyleBackColor = true;
            btnDepBuscarImg.Click += btnDepBuscarImg_Click;
            // 
            // btnCrearDeporte
            // 
            btnCrearDeporte.BackColor = Color.FromArgb(0, 48, 135);
            btnCrearDeporte.FlatStyle = FlatStyle.Flat;
            btnCrearDeporte.ForeColor = Color.White;
            btnCrearDeporte.Location = new Point(740, 260);
            btnCrearDeporte.Name = "btnCrearDeporte";
            btnCrearDeporte.Size = new Size(370, 40);
            btnCrearDeporte.TabIndex = 10;
            btnCrearDeporte.Text = "Crear Selección Deportiva";
            btnCrearDeporte.UseVisualStyleBackColor = false;
            btnCrearDeporte.Click += btnCrearDeporte_Click;
            // 
            // tabArte
            // 
            tabArte.Controls.Add(gridArte);
            tabArte.Controls.Add(lblArteCatId);
            tabArte.Controls.Add(cboArteCategoria);
            tabArte.Controls.Add(lblArteNombre);
            tabArte.Controls.Add(txtArteNombre);
            tabArte.Controls.Add(lblArteDesc);
            tabArte.Controls.Add(txtArteDesc);
            tabArte.Controls.Add(lblArteInstructor);
            tabArte.Controls.Add(cboArteInstructor);
            tabArte.Controls.Add(btnCrearArte);
            tabArte.Location = new Point(4, 26);
            tabArte.Name = "tabArte";
            tabArte.Size = new Size(1142, 650);
            tabArte.TabIndex = 3;
            tabArte.Text = "Arte y Cultura (DAC)";
            // 
            // gridArte
            // 
            gridArte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridArte.Location = new Point(20, 20);
            gridArte.Name = "gridArte";
            gridArte.ReadOnly = true;
            gridArte.Size = new Size(700, 600);
            gridArte.TabIndex = 0;
            // 
            // lblArteCatId
            // 
            lblArteCatId.Location = new Point(740, 20);
            lblArteCatId.Name = "lblArteCatId";
            lblArteCatId.Size = new Size(350, 20);
            lblArteCatId.TabIndex = 1;
            lblArteCatId.Text = "Categoría DAC:";
            // 
            // cboArteCategoria
            // 
            cboArteCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboArteCategoria.Location = new Point(740, 40);
            cboArteCategoria.Name = "cboArteCategoria";
            cboArteCategoria.Size = new Size(370, 25);
            cboArteCategoria.TabIndex = 2;
            // 
            // lblArteNombre
            // 
            lblArteNombre.Location = new Point(740, 80);
            lblArteNombre.Name = "lblArteNombre";
            lblArteNombre.Size = new Size(250, 20);
            lblArteNombre.TabIndex = 3;
            lblArteNombre.Text = "Nombre del Taller / Actividad:";
            // 
            // txtArteNombre
            // 
            txtArteNombre.Location = new Point(740, 100);
            txtArteNombre.Name = "txtArteNombre";
            txtArteNombre.Size = new Size(370, 25);
            txtArteNombre.TabIndex = 4;
            // 
            // lblArteDesc
            // 
            lblArteDesc.Location = new Point(740, 140);
            lblArteDesc.Name = "lblArteDesc";
            lblArteDesc.Size = new Size(200, 20);
            lblArteDesc.TabIndex = 5;
            lblArteDesc.Text = "Descripción:";
            // 
            // txtArteDesc
            // 
            txtArteDesc.Location = new Point(740, 160);
            txtArteDesc.Name = "txtArteDesc";
            txtArteDesc.Size = new Size(370, 25);
            txtArteDesc.TabIndex = 6;
            // 
            // lblArteInstructor
            // 
            lblArteInstructor.Location = new Point(740, 200);
            lblArteInstructor.Name = "lblArteInstructor";
            lblArteInstructor.Size = new Size(250, 20);
            lblArteInstructor.TabIndex = 7;
            lblArteInstructor.Text = "Instructor (Opcional):";
            // 
            // cboArteInstructor
            // 
            cboArteInstructor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboArteInstructor.Location = new Point(740, 220);
            cboArteInstructor.Name = "cboArteInstructor";
            cboArteInstructor.Size = new Size(370, 25);
            cboArteInstructor.TabIndex = 8;
            // 
            // btnCrearArte
            // 
            btnCrearArte.BackColor = Color.FromArgb(0, 48, 135);
            btnCrearArte.FlatStyle = FlatStyle.Flat;
            btnCrearArte.ForeColor = Color.White;
            btnCrearArte.Location = new Point(740, 270);
            btnCrearArte.Name = "btnCrearArte";
            btnCrearArte.Size = new Size(370, 40);
            btnCrearArte.TabIndex = 9;
            btnCrearArte.Text = "Crear Actividad DAC";
            btnCrearArte.UseVisualStyleBackColor = false;
            btnCrearArte.Click += btnCrearArte_Click;
            // 
            // AdminDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(tabAdminCrud);
            Controls.Add(PanelHeader);
            MaximumSize = new Size(1200, 800);
            MinimumSize = new Size(1200, 800);
            Name = "AdminDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Administración - Gestión de Actividades Extra-Académicas";
            FormClosing += AdminDashboard_FormClosing;
            Load += AdminDashboard_Load;
            PanelHeader.ResumeLayout(false);
            PanelHeader.PerformLayout();
            tabAdminCrud.ResumeLayout(false);
            tabCursos.ResumeLayout(false);
            tabCursos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridCursos).EndInit();
            tabAsociaciones.ResumeLayout(false);
            tabAsociaciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridAsociaciones).EndInit();
            tabDeportes.ResumeLayout(false);
            tabDeportes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridDeportes).EndInit();
            tabArte.ResumeLayout(false);
            tabArte.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridArte).EndInit();
            ResumeLayout(false);
        }

        private Panel PanelHeader;
        private Button btnVolver;
        private Label lblTitle;
        private TabControl tabAdminCrud;
        
        // Tab Cursos Controls
        private TabPage tabCursos;
        private DataGridView gridCursos;
        private Label lblCursoId;
        private TextBox txtCursoId;
        private Label lblCursoNombre;
        private TextBox txtCursoNombre;
        private Label lblCursoDesc;
        private TextBox txtCursoDesc;
        private Label lblCursoInicio;
        private DateTimePicker dtpCursoInicio;
        private Label lblCursoFin;
        private DateTimePicker dtpCursoFin;
        private Label lblCursoHorario;
        private TextBox txtCursoHorario;
        private Label lblCursoCupo;
        private TextBox txtCursoCupo;
        private Label lblCursoInstructor;
        private ComboBox cboCursoInstructor;
        private Label lblCursoEstado;
        private TextBox txtCursoEstado;
        private Button btnCrearCurso;
        private Button btnActualizarCurso;

        // Tab Asociaciones Controls
        private TabPage tabAsociaciones;
        private DataGridView gridAsociaciones;
        private Label lblAsoAcronimo;
        private TextBox txtAsoAcronimo;
        private Label lblAsoNombre;
        private TextBox txtAsoNombre;
        private Label lblAsoAnio;
        private TextBox txtAsoAnio;
        private Label lblAsoDesc;
        private TextBox txtAsoDesc;
        private Label lblAsoFormulario;
        private TextBox txtAsoFormulario;
        private Label lblAsoRutaImg;
        private TextBox txtAsoRutaImg;
        private Button btnAsoBuscarImg;
        private Button btnCrearAso;

        // Tab Deportes Controls
        private TabPage tabDeportes;
        private DataGridView gridDeportes;
        private Label lblDepDisciplina;
        private TextBox txtDepDisciplina;
        private Label lblDepRama;
        private TextBox txtDepRama;
        private Label lblDepEquipo;
        private TextBox txtDepEquipo;
        private Label lblDepRutaImg;
        private TextBox txtDepRutaImg;
        private Button btnDepBuscarImg;
        private Button btnCrearDeporte;

        // Tab Arte Controls
        private TabPage tabArte;
        private DataGridView gridArte;
        private Label lblArteCatId;
        private ComboBox cboArteCategoria;
        private Label lblArteNombre;
        private TextBox txtArteNombre;
        private Label lblArteDesc;
        private TextBox txtArteDesc;
        private Label lblArteInstructor;
        private ComboBox cboArteInstructor;
        private Button btnCrearArte;
    }
}
