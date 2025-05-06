using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PersonalOrganizer
{
    public class CsvHelper<T> where T : new()
    {
        private string _dataDirectory;
        private string _fileName;
        private string _filePath;

        public CsvHelper(string fileName)
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _fileName = fileName;
            _filePath = Path.Combine(_dataDirectory, _fileName);

            // Ensure directory exists
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            // Create file if it doesn't exist
            if (!File.Exists(_filePath))
            {
                using (var writer = new StreamWriter(_filePath, false))
                {
                    // Write header
                    var properties = typeof(T).GetProperties();
                    writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));
                }
            }
        }

        public List<T> ReadAll()
        {
            var result = new List<T>();

            if (!File.Exists(_filePath))
                return result;

            using (var reader = new StreamReader(_filePath))
            {
                // Skip header
                reader.ReadLine();

                // Read data
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var values = ParseCsvLine(line);
                    var item = ConvertToObject(values);
                    result.Add(item);
                }
            }

            return result;
        }

        public void Write(List<T> items)
        {
            using (var writer = new StreamWriter(_filePath, false))
            {
                // Write header
                var properties = typeof(T).GetProperties();
                writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));

                // Write data
                foreach (var item in items)
                {
                    var values = ConvertToValues(item);
                    writer.WriteLine(string.Join(",", values));
                }
            }
        }

        public void Append(T item)
        {
            using (var writer = new StreamWriter(_filePath, true))
            {
                // Write data
                var values = ConvertToValues(item);
                writer.WriteLine(string.Join(",", values));
            }
        }

        private string[] ParseCsvLine(string line)
        {
            // Simple parser for comma separated values
            // Note: This is a simplified version that doesn't handle quoted values with commas properly
            return line.Split(',');
        }

        private T ConvertToObject(string[] values)
        {
            var properties = typeof(T).GetProperties();
            var item = new T();
            for (int i = 0; i < Math.Min(values.Length, properties.Length); i++)
            {
                var property = properties[i];
                var value = values[i];
                if (string.IsNullOrEmpty(value))
                    continue;
                try
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(item, value);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(item, int.Parse(value));
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        property.SetValue(item, decimal.Parse(value));
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(item, bool.Parse(value));
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        try
                        {
                            // Önce varsayılan kültürle dene
                            property.SetValue(item, DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        catch
                        {
                            try
                            {
                                // Türkçe tarih formatıyla dene
                                property.SetValue(item, DateTime.Parse(value, new System.Globalization.CultureInfo("tr-TR")));
                            }
                            catch
                            {
                                // En kötü durumda şu anki tarih
                                property.SetValue(item, DateTime.Now);
                            }
                        }
                    }
                    else if (property.PropertyType == typeof(Guid))
                    {
                        property.SetValue(item, Guid.Parse(value));
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        property.SetValue(item, Enum.Parse(property.PropertyType, value));
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda sessizce geç, bu alanı atla
                    Console.WriteLine($"Conversion error for {property.Name}: {ex.Message}");
                }
            }
            return item;
        }

        private string[] ConvertToValues(T item)
        {
            var properties = typeof(T).GetProperties();
            var values = new string[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var value = property.GetValue(item);

                if (value == null)
                {
                    values[i] = string.Empty;
                }
                else
                {
                    values[i] = value.ToString();
                }
            }

            return values;
        }
    }
}