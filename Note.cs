using System;

namespace PersonalOrganizer
{
    public class Note
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Note()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Title = "Yeni Not"; // Varsayılan başlık
        }

        // ListBox'ta gösterilecek metin
        public string DisplayText
        {
            get
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    return $"{Title} ({CreatedAt.ToString("dd.MM.yyyy HH:mm")})";
                }
                else
                {
                    string previewText = Content;
                    if (previewText != null && previewText.Length > 30)
                    {
                        previewText = previewText.Substring(0, 30) + "...";
                    }
                    return $"{previewText} ({CreatedAt.ToString("dd.MM.yyyy HH:mm")})";
                }
            }
        }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}