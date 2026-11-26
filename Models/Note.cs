namespace YMH219_NotepadApp.Models
{
    /// <summary>
    /// Bir notun başlık, içerik, kategori ve zaman damgası bilgilerini tutan model sınıfı.
    /// </summary>
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? RtfContent { get; set; } = null; // Zengin metin biçimlendirmesi (RTF)
        public string Category { get; set; } = "Genel";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Note() { }

        public Note(string title, string content, string category = "Genel", string? rtfContent = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Content = content;
            Category = category;
            RtfContent = rtfContent;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// .txt formatında dışa aktarma için biçimlendirilmiş metin.
        /// </summary>
        public string ToPlainTextExport()
        {
            return $"================================================================================\n" +
                   $"BAŞLIK   : {Title}\n" +
                   $"KATEGORİ : {Category}\n" +
                   $"OLUŞTURMA: {CreatedAt:yyyy-MM-dd HH:mm:ss}\n" +
                   $"GÜNCELLEME: {UpdatedAt:yyyy-MM-dd HH:mm:ss}\n" +
                   $"================================================================================\n\n" +
                   $"{Content}\n";
        }

        public override string ToString()
        {
            string dateStr = UpdatedAt.ToString("dd.MM.yyyy HH:mm");
            return $"[{Category}] {Title} ({dateStr})";
        }
    }
}
