using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PersonalOrganizer
{
    public class SalaryService
    {
        private readonly string _salaryFilePath;

        public SalaryService()
        {
            // Uygulama dizini altında oluştur
            _salaryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "salaries.csv");
        }

        public List<SalaryInfo> GetSalariesByUserId(Guid userId)
        {
            var allSalaries = GetAllSalaries();
            return allSalaries.Where(s => s.UserId == userId).ToList();
        }

        public void CreateSalary(SalaryInfo salaryInfo)
        {
            var allSalaries = GetAllSalaries();
            allSalaries.Add(salaryInfo);
            SaveSalaries(allSalaries);
        }

        public void UpdateSalary(SalaryInfo salaryInfo)
        {
            var allSalaries = GetAllSalaries();
            var existingSalary = allSalaries.FirstOrDefault(s => s.Id == salaryInfo.Id);

            if (existingSalary != null)
            {
                allSalaries.Remove(existingSalary);
                allSalaries.Add(salaryInfo);
                SaveSalaries(allSalaries);
            }
        }

        public void DeleteSalary(Guid salaryId)
        {
            var allSalaries = GetAllSalaries();
            var salaryToDelete = allSalaries.FirstOrDefault(s => s.Id == salaryId);

            if (salaryToDelete != null)
            {
                allSalaries.Remove(salaryToDelete);
                SaveSalaries(allSalaries);
            }
        }

        private List<SalaryInfo> GetAllSalaries()
        {
            List<SalaryInfo> salaries = new List<SalaryInfo>();

            if (!File.Exists(_salaryFilePath))
            {
                return salaries;
            }

            try
            {
                string[] lines = File.ReadAllLines(_salaryFilePath);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Noktalı virgül kullan, virgül değil
                    string[] parts = line.Split(';');

                    if (parts.Length >= 7)
                    {
                        SalaryInfo salary = new SalaryInfo
                        {
                            Id = Guid.Parse(parts[0]),
                            UserId = Guid.Parse(parts[1]),
                            Position = parts[2],
                            CalculatedSalary = decimal.Parse(parts[3]),
                            FinalSalary = decimal.Parse(parts[4]),
                            CalculationDate = DateTime.Parse(parts[5]),
                            YearsOfExperience = int.Parse(parts[6]),
                            EducationLevel = parts.Length > 7 ? parts[7] : "Bachelor"
                        };

                        salaries.Add(salary);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kayıt tut ve boş liste döndür
                Console.WriteLine($"Maaş verileri okunurken hata oluştu: {ex.Message}");
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt"),
                    $"{DateTime.Now}: Maaş verileri okunurken hata - {ex.Message}\n");
            }

            return salaries;
        }

        private void SaveSalaries(List<SalaryInfo> salaries)
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (var salary in salaries)
                {
                    // Noktalı virgül kullan, virgül değil
                    string line = $"{salary.Id};{salary.UserId};{salary.Position};{salary.CalculatedSalary};" +
                                 $"{salary.FinalSalary};{salary.CalculationDate};{salary.YearsOfExperience};{salary.EducationLevel}";
                    lines.Add(line);
                }

                File.WriteAllLines(_salaryFilePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Maaş verileri kaydedilirken hata oluştu: {ex.Message}");
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt"),
                    $"{DateTime.Now}: Maaş verileri kaydedilirken hata - {ex.Message}\n");

                // Hatayı kullanıcıya göster
                System.Windows.Forms.MessageBox.Show($"Maaş verileri kaydedilirken hata oluştu: {ex.Message}",
                    "Kayıt Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}