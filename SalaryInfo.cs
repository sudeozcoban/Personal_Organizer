using System;

namespace PersonalOrganizer
{
    public class SalaryInfo
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Position { get; set; } = string.Empty;
        public decimal CalculatedSalary { get; set; }
        public decimal FinalSalary { get; set; }
        public DateTime CalculationDate { get; set; }
        public int YearsOfExperience { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
    }
}