namespace PersonalOrganizer
{
    partial class SalaryCalculatorForm
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lstSalaries = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlListButtons = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpSalaryForm = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEmployeeType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbEducationLevel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExperienceYears = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbPosition = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSoftwareDev = new System.Windows.Forms.CheckBox();
            this.chkSystemAdmin = new System.Windows.Forms.CheckBox();
            this.chkNetworkSecurity = new System.Windows.Forms.CheckBox();
            this.chkDatabaseAdmin = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpCalculationDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTaxRate = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtInsuranceDeduction = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOtherDeductions = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblGrossSalary = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTaxAmount = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblTotalDeductions = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblNetSalary = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlListButtons.SuspendLayout();
            this.grpSalaryForm.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pnlListButtons);
            this.splitContainer.Panel1.Controls.Add(this.lstSalaries);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpSalaryForm);
            this.splitContainer.Size = new System.Drawing.Size(982, 603);
            this.splitContainer.SplitterDistance = 349;
            this.splitContainer.TabIndex = 0;
            // 
            // lstSalaries
            // 
            this.lstSalaries.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lstSalaries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstSalaries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSalaries.FullRowSelect = true;
            this.lstSalaries.HideSelection = false;
            this.lstSalaries.Location = new System.Drawing.Point(0, 0);
            this.lstSalaries.MultiSelect = false;
            this.lstSalaries.Name = "lstSalaries";
            this.lstSalaries.Size = new System.Drawing.Size(345, 599);
            this.lstSalaries.TabIndex = 2;
            this.lstSalaries.UseCompatibleStateImageBehavior = false;
            this.lstSalaries.View = System.Windows.Forms.View.Details;
            this.lstSalaries.Click += new System.EventHandler(this.lstSalaries_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Başlık";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Çalışan Tipi";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Tarih";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Net Maaş";
            this.columnHeader4.Width = 80;
            // 
            // pnlListButtons
            // 
            this.pnlListButtons.Controls.Add(this.btnDelete);
            this.pnlListButtons.Controls.Add(this.btnEdit);
            this.pnlListButtons.Controls.Add(this.btnNew);
            this.pnlListButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlListButtons.Location = new System.Drawing.Point(0, 499);
            this.pnlListButtons.Name = "pnlListButtons";
            this.pnlListButtons.Size = new System.Drawing.Size(345, 100);
            this.pnlListButtons.TabIndex = 3;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnNew.Location = new System.Drawing.Point(10, 39);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(91, 31);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Yeni";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnEdit.Location = new System.Drawing.Point(121, 39);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(81, 31);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Düzenle";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDelete.Location = new System.Drawing.Point(232, 39);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(79, 31);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Sil";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grpSalaryForm
            // 
            this.grpSalaryForm.Controls.Add(this.btnCancel);
            this.grpSalaryForm.Controls.Add(this.btnSave);
            this.grpSalaryForm.Controls.Add(this.btnCalculate);
            this.grpSalaryForm.Controls.Add(this.groupBox3);
            this.grpSalaryForm.Controls.Add(this.groupBox2);
            this.grpSalaryForm.Controls.Add(this.dtpCalculationDate);
            this.grpSalaryForm.Controls.Add(this.label6);
            this.grpSalaryForm.Controls.Add(this.groupBox1);
            this.grpSalaryForm.Controls.Add(this.cmbPosition);
            this.grpSalaryForm.Controls.Add(this.label5);
            this.grpSalaryForm.Controls.Add(this.txtExperienceYears);
            this.grpSalaryForm.Controls.Add(this.label4);
            this.grpSalaryForm.Controls.Add(this.cmbEducationLevel);
            this.grpSalaryForm.Controls.Add(this.label3);
            this.grpSalaryForm.Controls.Add(this.cmbEmployeeType);
            this.grpSalaryForm.Controls.Add(this.label2);
            this.grpSalaryForm.Controls.Add(this.txtTitle);
            this.grpSalaryForm.Controls.Add(this.label1);
            this.grpSalaryForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSalaryForm.Location = new System.Drawing.Point(0, 0);
            this.grpSalaryForm.Name = "grpSalaryForm";
            this.grpSalaryForm.Size = new System.Drawing.Size(625, 599);
            this.grpSalaryForm.TabIndex = 0;
            this.grpSalaryForm.TabStop = false;
            this.grpSalaryForm.Text = "BMO Maaş Hesaplama Formu";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Başlık";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(150, 35);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(122, 25);
            this.txtTitle.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Çalışan Tipi";
            // 
            // cmbEmployeeType
            // 
            this.cmbEmployeeType.FormattingEnabled = true;
            this.cmbEmployeeType.Location = new System.Drawing.Point(151, 68);
            this.cmbEmployeeType.Name = "cmbEmployeeType";
            this.cmbEmployeeType.Size = new System.Drawing.Size(121, 26);
            this.cmbEmployeeType.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Eğitim Seviyesi";
            // 
            // cmbEducationLevel
            // 
            this.cmbEducationLevel.FormattingEnabled = true;
            this.cmbEducationLevel.Location = new System.Drawing.Point(151, 99);
            this.cmbEducationLevel.Name = "cmbEducationLevel";
            this.cmbEducationLevel.Size = new System.Drawing.Size(121, 26);
            this.cmbEducationLevel.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Deneyim Yılı";
            // 
            // txtExperienceYears
            // 
            this.txtExperienceYears.Location = new System.Drawing.Point(151, 131);
            this.txtExperienceYears.Name = "txtExperienceYears";
            this.txtExperienceYears.Size = new System.Drawing.Size(121, 25);
            this.txtExperienceYears.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Pozisyon";
            // 
            // cmbPosition
            // 
            this.cmbPosition.FormattingEnabled = true;
            this.cmbPosition.Location = new System.Drawing.Point(151, 167);
            this.cmbPosition.Name = "cmbPosition";
            this.cmbPosition.Size = new System.Drawing.Size(121, 26);
            this.cmbPosition.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDatabaseAdmin);
            this.groupBox1.Controls.Add(this.chkNetworkSecurity);
            this.groupBox1.Controls.Add(this.chkSystemAdmin);
            this.groupBox1.Controls.Add(this.chkSoftwareDev);
            this.groupBox1.Location = new System.Drawing.Point(351, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 135);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sertifikalar/Uzmanlıklar";
            // 
            // chkSoftwareDev
            // 
            this.chkSoftwareDev.AutoSize = true;
            this.chkSoftwareDev.Location = new System.Drawing.Point(6, 24);
            this.chkSoftwareDev.Name = "chkSoftwareDev";
            this.chkSoftwareDev.Size = new System.Drawing.Size(156, 22);
            this.chkSoftwareDev.TabIndex = 0;
            this.chkSoftwareDev.Text = "Yazılım Geliştirme";
            this.chkSoftwareDev.UseVisualStyleBackColor = true;
            // 
            // chkSystemAdmin
            // 
            this.chkSystemAdmin.AutoSize = true;
            this.chkSystemAdmin.Location = new System.Drawing.Point(9, 52);
            this.chkSystemAdmin.Name = "chkSystemAdmin";
            this.chkSystemAdmin.Size = new System.Drawing.Size(144, 22);
            this.chkSystemAdmin.TabIndex = 1;
            this.chkSystemAdmin.Text = "Sistem Yönetimi";
            this.chkSystemAdmin.UseVisualStyleBackColor = true;
            // 
            // chkNetworkSecurity
            // 
            this.chkNetworkSecurity.AutoSize = true;
            this.chkNetworkSecurity.Location = new System.Drawing.Point(6, 81);
            this.chkNetworkSecurity.Name = "chkNetworkSecurity";
            this.chkNetworkSecurity.Size = new System.Drawing.Size(121, 22);
            this.chkNetworkSecurity.TabIndex = 2;
            this.chkNetworkSecurity.Text = "Ağ Güvenliği";
            this.chkNetworkSecurity.UseVisualStyleBackColor = true;
            // 
            // chkDatabaseAdmin
            // 
            this.chkDatabaseAdmin.AutoSize = true;
            this.chkDatabaseAdmin.Location = new System.Drawing.Point(4, 108);
            this.chkDatabaseAdmin.Name = "chkDatabaseAdmin";
            this.chkDatabaseAdmin.Size = new System.Drawing.Size(174, 22);
            this.chkDatabaseAdmin.TabIndex = 3;
            this.chkDatabaseAdmin.Text = "Veri Tabanı Yönetimi";
            this.chkDatabaseAdmin.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 18);
            this.label6.TabIndex = 11;
            this.label6.Text = "Hesaplama Tarihi";
            // 
            // dtpCalculationDate
            // 
            this.dtpCalculationDate.Location = new System.Drawing.Point(150, 222);
            this.dtpCalculationDate.Name = "dtpCalculationDate";
            this.dtpCalculationDate.Size = new System.Drawing.Size(200, 25);
            this.dtpCalculationDate.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtOtherDeductions);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtInsuranceDeduction);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtTaxRate);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox2.Location = new System.Drawing.Point(9, 258);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(446, 124);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ek Kesintiler";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(6, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Vergi Oranı(%)";
            // 
            // txtTaxRate
            // 
            this.txtTaxRate.Location = new System.Drawing.Point(142, 26);
            this.txtTaxRate.Name = "txtTaxRate";
            this.txtTaxRate.Size = new System.Drawing.Size(100, 28);
            this.txtTaxRate.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(6, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 18);
            this.label8.TabIndex = 2;
            this.label8.Text = "Sigorta Kesintisi";
            // 
            // txtInsuranceDeduction
            // 
            this.txtInsuranceDeduction.Location = new System.Drawing.Point(142, 60);
            this.txtInsuranceDeduction.Name = "txtInsuranceDeduction";
            this.txtInsuranceDeduction.Size = new System.Drawing.Size(100, 28);
            this.txtInsuranceDeduction.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.Location = new System.Drawing.Point(6, 89);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 18);
            this.label9.TabIndex = 4;
            this.label9.Text = "Diğer Kesintiler";
            // 
            // txtOtherDeductions
            // 
            this.txtOtherDeductions.Location = new System.Drawing.Point(142, 90);
            this.txtOtherDeductions.Name = "txtOtherDeductions";
            this.txtOtherDeductions.Size = new System.Drawing.Size(100, 28);
            this.txtOtherDeductions.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblNetSalary);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.lblTotalDeductions);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lblTaxAmount);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lblGrossSalary);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(9, 388);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(606, 129);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Hesaplama Sonuçları";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 18);
            this.label10.TabIndex = 0;
            this.label10.Text = "Brüt Maaş:";
            // 
            // lblGrossSalary
            // 
            this.lblGrossSalary.AutoSize = true;
            this.lblGrossSalary.Location = new System.Drawing.Point(120, 32);
            this.lblGrossSalary.Name = "lblGrossSalary";
            this.lblGrossSalary.Size = new System.Drawing.Size(0, 18);
            this.lblGrossSalary.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 18);
            this.label11.TabIndex = 2;
            this.label11.Text = "Vergi Miktarı:";
            // 
            // lblTaxAmount
            // 
            this.lblTaxAmount.AutoSize = true;
            this.lblTaxAmount.Location = new System.Drawing.Point(139, 63);
            this.lblTaxAmount.Name = "lblTaxAmount";
            this.lblTaxAmount.Size = new System.Drawing.Size(0, 18);
            this.lblTaxAmount.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(333, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 18);
            this.label12.TabIndex = 4;
            this.label12.Text = "Toplam Kesintiler:";
            // 
            // lblTotalDeductions
            // 
            this.lblTotalDeductions.AutoSize = true;
            this.lblTotalDeductions.Location = new System.Drawing.Point(475, 30);
            this.lblTotalDeductions.Name = "lblTotalDeductions";
            this.lblTotalDeductions.Size = new System.Drawing.Size(0, 18);
            this.lblTotalDeductions.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(350, 67);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 18);
            this.label13.TabIndex = 6;
            this.label13.Text = "Net Maaş:";
            // 
            // lblNetSalary
            // 
            this.lblNetSalary.AutoSize = true;
            this.lblNetSalary.Location = new System.Drawing.Point(462, 70);
            this.lblNetSalary.Name = "lblNetSalary";
            this.lblNetSalary.Size = new System.Drawing.Size(0, 18);
            this.lblNetSalary.TabIndex = 7;
            // 
            // btnCalculate
            // 
            this.btnCalculate.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnCalculate.Location = new System.Drawing.Point(52, 537);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(88, 32);
            this.btnCalculate.TabIndex = 15;
            this.btnCalculate.Text = "Hesapla";
            this.btnCalculate.UseVisualStyleBackColor = false;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSave.Location = new System.Drawing.Point(207, 538);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 31);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancel.Location = new System.Drawing.Point(345, 537);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 32);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SalaryCalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 603);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SalaryCalculatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SalaryCalculatorForm";
            this.Load += new System.EventHandler(this.SalaryCalculatorForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlListButtons.ResumeLayout(false);
            this.grpSalaryForm.ResumeLayout(false);
            this.grpSalaryForm.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListView lstSalaries;
        private System.Windows.Forms.Panel pnlListButtons;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.GroupBox grpSalaryForm;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbPosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExperienceYears;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbEducationLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbEmployeeType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpCalculationDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkDatabaseAdmin;
        private System.Windows.Forms.CheckBox chkNetworkSecurity;
        private System.Windows.Forms.CheckBox chkSystemAdmin;
        private System.Windows.Forms.CheckBox chkSoftwareDev;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblTaxAmount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblGrossSalary;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOtherDeductions;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtInsuranceDeduction;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTaxRate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Label lblNetSalary;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblTotalDeductions;
        private System.Windows.Forms.Label label12;
    }
}