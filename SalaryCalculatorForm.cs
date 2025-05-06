using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    public partial class SalaryCalculatorForm : Form
    {
        private User _currentUser;
        private SalaryService _salaryService;
        private SalaryInfo _currentSalary;
        private bool _isEditMode = false;

        // BMO temel katsayıları - gerçek değerlerle değiştirilmeli
        private const decimal BaseAmount = 25000; // Temel ücret

        // Eğitim katsayıları
        private readonly Dictionary<string, decimal> _educationFactors = new Dictionary<string, decimal>
        {
            { "Lisans", 1.0m },
            { "Yüksek Lisans", 1.25m },
            { "Doktora", 1.5m }
        };

        // Pozisyon katsayıları
        private readonly Dictionary<string, decimal> _positionFactors = new Dictionary<string, decimal>
        {
            { "Bilgisayar Mühendisi", 1.0m },
            { "Kıdemli Bilgisayar Mühendisi", 1.3m },
            { "Uzman Bilgisayar Mühendisi", 1.6m },
            { "Yönetici Bilgisayar Mühendisi", 1.9m }
        };

        public SalaryCalculatorForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _salaryService = new SalaryService();

            // Manuel olarak olayları bağla
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.lstSalaries.SelectedIndexChanged += new EventHandler(lstSalaries_SelectedIndexChanged);
            this.btnNew.Click += new EventHandler(btnNew_Click);
            this.btnEdit.Click += new EventHandler(btnEdit_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnCalculate.Click += new EventHandler(btnCalculate_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            // Form yüklenirken olayını tanımla
            this.Load += SalaryCalculatorForm_Load;
        }

        // Form yüklendiğinde çalışacak kod
        private void SalaryCalculatorForm_Load(object sender, EventArgs e)
        {
            // Eğitim seviyesi
            cmbEducationLevel.Items.AddRange(new string[] { "Lisans", "Yüksek Lisans", "Doktora" });
            cmbEducationLevel.SelectedIndex = 0; // Varsayılan Lisans

            // Pozisyon kategorisi
            cmbPosition.Items.AddRange(new string[] {
                "Bilgisayar Mühendisi",
                "Kıdemli Bilgisayar Mühendisi",
                "Uzman Bilgisayar Mühendisi",
                "Yönetici Bilgisayar Mühendisi"
            });
            cmbPosition.SelectedIndex = 0;

            // Çalışan tipi
            cmbEmployeeType.Items.AddRange(new string[] { "Tam Zamanlı", "Yarı Zamanlı" });
            cmbEmployeeType.SelectedIndex = 0;

            // Varsayılan değerler
            txtExperienceYears.Text = "0";
            txtTaxRate.Text = "15";
            txtInsuranceDeduction.Text = "0";
            txtOtherDeductions.Text = "0";

            // Bugünün tarihi
            dtpCalculationDate.Value = DateTime.Today;

            // Çalışan tipini kullanıcı tipine göre ayarla
            if (_currentUser.UserType == UserType.PartTime)
            {
                cmbEmployeeType.SelectedIndex = 1;
                cmbEmployeeType.Enabled = false; // Part-time kullanıcı değiştiremez
            }

            // Maaş listesini yükle
            LoadSalaryList();

            // Form öğelerini devre dışı bırak
            DisableSalaryForm();
        }

        private void LoadSalaryList()
        {
            try
            {
                // Listeyi temizle
                lstSalaries.Items.Clear();

                // Maaş kayıtlarını al
                List<SalaryInfo> salaries = _salaryService.GetSalariesByUserId(_currentUser.Id);

                if (salaries == null || salaries.Count == 0)
                {
                    UpdateButtonStates();
                    return;
                }

                // ListView'a ekle
                foreach (var salary in salaries)
                {
                    try
                    {
                        ListViewItem item = new ListViewItem(salary.Position);
                        item.SubItems.Add(_currentUser.UserType == UserType.PartTime ? "Yarı Zamanlı" : "Tam Zamanlı");
                        item.SubItems.Add(salary.CalculationDate.ToShortDateString());
                        item.SubItems.Add(salary.FinalSalary.ToString("N2"));
                        item.Tag = salary;

                        lstSalaries.Items.Add(item);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Maaş kaydı eklenirken hata: {ex.Message}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Buton durumlarını güncelle
                UpdateButtonStates();

                // Eğer öğe varsa ilkini seç
                if (lstSalaries.Items.Count > 0)
                {
                    lstSalaries.Items[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Maaş listesi yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = lstSalaries.SelectedItems.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void EnableSalaryForm()
        {
            // Tüm form elemanlarını etkinleştir
            txtTitle.Enabled = true;
            cmbEmployeeType.Enabled = (_currentUser.UserType != UserType.PartTime);
            cmbEducationLevel.Enabled = true;
            txtExperienceYears.Enabled = true;
            cmbPosition.Enabled = true;

            // Sertifikalar
            chkSoftwareDev.Enabled = true;
            chkSystemAdmin.Enabled = true;
            chkNetworkSecurity.Enabled = true;
            chkDatabaseAdmin.Enabled = true;

            // Kesintiler
            txtTaxRate.Enabled = true;
            txtInsuranceDeduction.Enabled = true;
            txtOtherDeductions.Enabled = true;

            dtpCalculationDate.Enabled = true;

            // İşlemle ilgili butonlar
            btnCalculate.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            // Form başlangıç değerlerini ayarla
            if (!_isEditMode)
            {
                // Yeni kayıt oluşturuluyorsa varsayılanları ata
                txtExperienceYears.Text = "0";
                txtTaxRate.Text = "15";
                txtInsuranceDeduction.Text = "0";
                txtOtherDeductions.Text = "0";
            }
        }

        private void DisableSalaryForm()
        {
            txtTitle.Enabled = false;
            cmbEmployeeType.Enabled = false;
            cmbEducationLevel.Enabled = false;
            txtExperienceYears.Enabled = false;
            cmbPosition.Enabled = false;

            chkSoftwareDev.Enabled = false;
            chkSystemAdmin.Enabled = false;
            chkNetworkSecurity.Enabled = false;
            chkDatabaseAdmin.Enabled = false;

            txtTaxRate.Enabled = false;
            txtInsuranceDeduction.Enabled = false;
            txtOtherDeductions.Enabled = false;

            dtpCalculationDate.Enabled = false;

            btnCalculate.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            ClearSalaryForm();
        }

        private void ClearSalaryForm()
        {
            txtTitle.Text = "";
            txtExperienceYears.Text = "0";
            txtTaxRate.Text = "15";
            txtInsuranceDeduction.Text = "0";
            txtOtherDeductions.Text = "0";

            // ComboBox'ları sıfırla
            cmbEducationLevel.SelectedIndex = 0;
            cmbPosition.SelectedIndex = 0;

            // CheckBox'ları temizle
            chkSoftwareDev.Checked = false;
            chkSystemAdmin.Checked = false;
            chkNetworkSecurity.Checked = false;
            chkDatabaseAdmin.Checked = false;

            dtpCalculationDate.Value = DateTime.Today;

            // Sonuçlar
            lblGrossSalary.Text = "0.00";
            lblTaxAmount.Text = "0.00";
            lblTotalDeductions.Text = "0.00";
            lblNetSalary.Text = "0.00";

            _currentSalary = null;
        }

        private void DisplaySalary(SalaryInfo salary)
        {
            _currentSalary = salary;

            if (salary != null)
            {
                txtTitle.Text = salary.Position;

                // Eğitim seviyesi
                if (!string.IsNullOrEmpty(salary.EducationLevel))
                {
                    int index = cmbEducationLevel.FindStringExact(salary.EducationLevel);
                    cmbEducationLevel.SelectedIndex = index >= 0 ? index : 0;
                }

                // Deneyim yılı
                txtExperienceYears.Text = salary.YearsOfExperience.ToString();

                // Pozisyon - ayrı bir alan olmadığı için başlıktan ayırıyoruz
                // Gerçek uygulamada SalaryInfo'ya bir PositionCategory alanı eklenmelidir

                // Çalışan tipi
                cmbEmployeeType.SelectedIndex = _currentUser.UserType == UserType.PartTime ? 1 : 0;

                // Sonuçları göster
                lblGrossSalary.Text = salary.CalculatedSalary.ToString("N2");
                decimal taxAmount = salary.CalculatedSalary * 0.15m; // Varsayılan vergi
                lblTaxAmount.Text = taxAmount.ToString("N2");
                lblTotalDeductions.Text = (salary.CalculatedSalary - salary.FinalSalary).ToString("N2");
                lblNetSalary.Text = salary.FinalSalary.ToString("N2");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _isEditMode = false;
            ClearSalaryForm();
            EnableSalaryForm();

            // Kullanıcı tipine göre çalışan tipini otomatik seç
            if (_currentUser.UserType == UserType.PartTime)
            {
                cmbEmployeeType.SelectedIndex = 1; // Yarı zamanlı
                cmbEmployeeType.Enabled = false;
            }
            else
            {
                cmbEmployeeType.SelectedIndex = 0; // Tam zamanlı
                cmbEmployeeType.Enabled = true;
            }

            txtTitle.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstSalaries.SelectedItems.Count > 0)
            {
                _isEditMode = true;
                EnableSalaryForm();
                txtTitle.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstSalaries.SelectedItems.Count > 0)
            {
                SalaryInfo salaryToDelete = (SalaryInfo)lstSalaries.SelectedItems[0].Tag;

                DialogResult result = MessageBox.Show(
                    $"'{salaryToDelete.Position}' maaş kaydını silmek istediğinize emin misiniz?",
                    "Silme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _salaryService.DeleteSalary(salaryToDelete.Id);
                        LoadSalaryList();
                        ClearSalaryForm();
                        MessageBox.Show("Maaş kaydı başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Maaş kaydı silinirken hata oluştu: {ex.Message}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void lstSalaries_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();

            if (lstSalaries.SelectedItems.Count > 0)
            {
                SalaryInfo selectedSalary = (SalaryInfo)lstSalaries.SelectedItems[0].Tag;
                DisplaySalary(selectedSalary);
            }
            else
            {
                ClearSalaryForm();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableSalaryForm();
            if (lstSalaries.SelectedItems.Count > 0)
            {
                SalaryInfo selectedSalary = (SalaryInfo)lstSalaries.SelectedItems[0].Tag;
                DisplaySalary(selectedSalary);
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // Veri doğrulama
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Lütfen bir başlık girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
                    return;
                }

                // Deneyim yılı kontrolü
                if (!int.TryParse(txtExperienceYears.Text, out int experienceYears) || experienceYears < 0)
                {
                    MessageBox.Show("Lütfen geçerli bir deneyim yılı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtExperienceYears.Focus();
                    return;
                }

                // Vergi oranı kontrolü
                if (!decimal.TryParse(txtTaxRate.Text, out decimal taxRate) || taxRate < 0 || taxRate > 100)
                {
                    MessageBox.Show("Lütfen %0-100 arasında geçerli bir vergi oranı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTaxRate.Focus();
                    return;
                }

                // Sigorta ve diğer kesintiler kontrolü
                if (!decimal.TryParse(txtInsuranceDeduction.Text, out decimal insuranceDeduction) || insuranceDeduction < 0)
                {
                    MessageBox.Show("Lütfen geçerli bir sigorta kesintisi girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtInsuranceDeduction.Focus();
                    return;
                }

                if (!decimal.TryParse(txtOtherDeductions.Text, out decimal otherDeductions) || otherDeductions < 0)
                {
                    MessageBox.Show("Lütfen geçerli diğer kesintileri girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtOtherDeductions.Focus();
                    return;
                }

                // Eğitim faktörü
                string educationLevel = cmbEducationLevel.SelectedItem.ToString();
                decimal educationFactor = _educationFactors[educationLevel];

                // Deneyim faktörü
                decimal experienceFactor = CalculateExperienceFactor(experienceYears);

                // Pozisyon faktörü
                string position = cmbPosition.SelectedItem.ToString();
                decimal positionFactor = _positionFactors[position];

                // Sertifika faktörü
                decimal certificateFactor = CalculateCertificateFactor();

                // BMO formülüne göre brüt maaş hesaplaması
                decimal grossSalary = BaseAmount * educationFactor * experienceFactor * positionFactor * certificateFactor;

                // Vergi ve kesintiler
                decimal taxAmount = grossSalary * (taxRate / 100);
                decimal totalDeductions = taxAmount + insuranceDeduction + otherDeductions;
                decimal netSalary = grossSalary - totalDeductions;

                // Part-time indirim kontrolü
                if (cmbEmployeeType.SelectedIndex == 1) // Yarı Zamanlı
                {
                    netSalary *= 0.6m; // %40 indirim
                }

                // Sonuçları göster
                lblGrossSalary.Text = grossSalary.ToString("N2");
                lblTaxAmount.Text = taxAmount.ToString("N2");
                lblTotalDeductions.Text = totalDeductions.ToString("N2");
                lblNetSalary.Text = netSalary.ToString("N2");

                // SalaryInfo nesnesini oluştur/güncelle
                if (_currentSalary == null)
                {
                    _currentSalary = new SalaryInfo();
                    _currentSalary.Id = Guid.NewGuid();
                    _currentSalary.UserId = _currentUser.Id;
                }

                // Bilgileri SalaryInfo nesnesine aktar
                _currentSalary.Position = $"{position} - {txtTitle.Text}"; // Pozisyon bilgisini başlığa ekle
                _currentSalary.CalculatedSalary = grossSalary;
                _currentSalary.FinalSalary = netSalary;
                _currentSalary.CalculationDate = dtpCalculationDate.Value;
                _currentSalary.YearsOfExperience = experienceYears;
                _currentSalary.EducationLevel = educationLevel;

                // Sonuçları mesaj olarak göster
                string userTypeInfo = cmbEmployeeType.SelectedIndex == 1 ? " (Yarı zamanlı için %40 indirim uygulandı)" : "";

                MessageBox.Show(
                    $"BMO Maaş Hesaplama Sonuçları:\n\n" +
                    $"Eğitim Seviyesi ({educationLevel}): {educationFactor:F2}\n" +
                    $"Deneyim Yılı ({experienceYears} yıl): {experienceFactor:F2}\n" +
                    $"Pozisyon ({position}): {positionFactor:F2}\n" +
                    $"Sertifika/Uzmanlık: {certificateFactor:F2}\n\n" +
                    $"Brüt Maaş: {grossSalary:N2} TL\n" +
                    $"Vergi Miktarı: {taxAmount:N2} TL\n" +
                    $"Toplam Kesintiler: {totalDeductions:N2} TL\n" +
                    $"Net Maaş: {netSalary:N2} TL{userTypeInfo}",
                    "BMO Maaş Hesaplama",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hesaplama sırasında bir hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal CalculateExperienceFactor(int experienceYears)
        {
            if (experienceYears <= 2)
                return 1.0m;
            else if (experienceYears <= 5)
                return 1.2m;
            else if (experienceYears <= 10)
                return 1.4m;
            else
                return 1.6m;
        }

        private decimal CalculateCertificateFactor()
        {
            int certificateCount = 0;
            if (chkSoftwareDev.Checked) certificateCount++;
            if (chkSystemAdmin.Checked) certificateCount++;
            if (chkNetworkSecurity.Checked) certificateCount++;
            if (chkDatabaseAdmin.Checked) certificateCount++;

            return 1.0m + (certificateCount * 0.05m); // Her sertifika %5 ekstra
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Hesaplanmış değerler var mı kontrolü
                decimal grossSalary;
                if (!decimal.TryParse(lblGrossSalary.Text, out grossSalary) || grossSalary <= 0)
                {
                    MessageBox.Show("Lütfen önce hesaplama yapın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnCalculate.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Lütfen bir başlık girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
                    return;
                }

                // Hesaplama sonuçlarını al
                decimal finalSalary;
                decimal.TryParse(lblNetSalary.Text, out finalSalary);

                // Yeni bir SalaryInfo oluştur veya mevcut olanı güncelle
                if (!_isEditMode)
                {
                    // Yeni kayıt
                    try
                    {
                        // Veritabanına kaydet
                        _salaryService.CreateSalary(_currentSalary);
                        MessageBox.Show("Maaş kaydı başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Maaş kaydı eklenirken hata: {ex.Message}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // Mevcut kaydı güncelle
                    try
                    {
                        // Veritabanını güncelle
                        _salaryService.UpdateSalary(_currentSalary);
                        MessageBox.Show("Maaş kaydı başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Maaş kaydı güncellenirken hata: {ex.Message}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Form durumunu resetle
                _isEditMode = false;

                try
                {
                    // Listeyi yenile
                    LoadSalaryList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Maaş listesi yüklenirken hata: {ex.Message}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Formu devre dışı bırak
                DisableSalaryForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme sırasında bir hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}