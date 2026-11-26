using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using YMH219_NotepadApp.Models;

namespace YMH219_NotepadApp.Services
{
    /// <summary>
    /// Not koleksiyonunu, JSON serileştirme/yükleme ve metin dışa aktarma işlemlerini yöneten servis sınıfı.
    /// </summary>
    public class NoteManager
    {
        private readonly List<Note> _notes = new();
        private readonly string _storageFilePath;

        public bool AutoSave { get; set; } = true;
        public IReadOnlyList<Note> Notes => _notes.AsReadOnly();

        /// <summary>
        /// Son yüklemede kullanıcıya gösterilmesi gereken bir uyarı varsa (ör. bozuk not dosyası) metnini tutar.
        /// </summary>
        public string? LoadWarning { get; private set; }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) // Türkçe karakterlerin düzgün görünmesi için
        };

        public NoteManager(string? customPath = null)
        {
            if (string.IsNullOrWhiteSpace(customPath))
            {
                _storageFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notes.json");
            }
            else
            {
                _storageFilePath = customPath;
            }

            LoadFromJson();
        }

        /// <summary>
        /// Yeni bir not ekler ve otomatik kaydeder.
        /// </summary>
        public void AddNote(Note note)
        {
            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;
            _notes.Insert(0, note); // En yeni not en üstte

            if (AutoSave) SaveToJson();
        }

        /// <summary>
        /// Mevcut bir notu günceller.
        /// </summary>
        public bool UpdateNote(Guid id, string newTitle, string newContent, string newCategory, string? newRtf = null)
        {
            var existing = _notes.FirstOrDefault(n => n.Id == id);
            if (existing == null) return false;

            existing.Title = newTitle;
            existing.Content = newContent;
            existing.Category = newCategory;
            existing.RtfContent = newRtf;
            existing.UpdatedAt = DateTime.Now;

            if (AutoSave) SaveToJson();
            return true;
        }

        /// <summary>
        /// Belirtilen notu siler.
        /// </summary>
        public bool DeleteNote(Guid id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note == null) return false;

            _notes.Remove(note);
            if (AutoSave) SaveToJson();
            return true;
        }

        /// <summary>
        /// Notları başlık, içerik veya kategoriye göre filtreler.
        /// </summary>
        public IEnumerable<Note> FilterNotes(string? searchTerm, string? category)
        {
            var query = _notes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(category) && category != "Tümü")
            {
                query = query.Where(n => n.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim().ToLowerInvariant();
                query = query.Where(n =>
                    n.Title.ToLowerInvariant().Contains(term) ||
                    n.Content.ToLowerInvariant().Contains(term) ||
                    n.Category.ToLowerInvariant().Contains(term));
            }

            return query.OrderByDescending(n => n.UpdatedAt);
        }

        /// <summary>
        /// Mevcut tüm kategorilerin benzersiz listesini döner.
        /// </summary>
        public List<string> GetCategories()
        {
            var categories = _notes.Select(n => n.Category)
                                   .Where(c => !string.IsNullOrWhiteSpace(c))
                                   .Distinct(StringComparer.OrdinalIgnoreCase)
                                   .ToList();

            // Varsayılan kategoriler, büyük/küçük harf farkı gözetilmeden yoksa eklenir
            // ("genel" adlı bir kategori varken ikinci bir "Genel" eklenmez)
            var comparer = StringComparer.OrdinalIgnoreCase;
            if (!categories.Contains("Genel", comparer)) categories.Insert(0, "Genel");
            if (!categories.Contains("Ders", comparer)) categories.Add("Ders");
            if (!categories.Contains("Kişisel", comparer)) categories.Add("Kişisel");
            if (!categories.Contains("İş", comparer)) categories.Add("İş");

            return categories;
        }

        /// <summary>
        /// Notları JSON formatında dosyaya kaydeder.
        /// </summary>
        public void SaveToJson()
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(_notes, _jsonOptions);
                File.WriteAllText(_storageFilePath, jsonString, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new IOException($"Notlar JSON dosyasına kaydedilirken hata oluştu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// JSON dosyasından notları yükler. Dosya yoksa veya bozuksa güvenli şekilde kurtarır.
        /// </summary>
        public void LoadFromJson()
        {
            LoadWarning = null;

            try
            {
                if (!File.Exists(_storageFilePath))
                {
                    // Dosya henüz yoksa varsayılan örnek karşılama notları ekle
                    CreateDefaultWelcomeNotes();
                    SaveToJson();
                    return;
                }

                string jsonString = File.ReadAllText(_storageFilePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(jsonString)) return;

                var loaded = JsonSerializer.Deserialize<List<Note>>(jsonString, _jsonOptions);
                _notes.Clear();
                if (loaded != null)
                {
                    _notes.AddRange(loaded);
                }
            }
            catch (JsonException)
            {
                // Bozuk JSON durumu (Föy hata yönetimi kriteri): Yedekle ve temiz başlat
                string backup = $"{_storageFilePath}.bak_{DateTime.Now:yyyyMMddHHmmss}";
                bool backedUp = false;
                try
                {
                    File.Copy(_storageFilePath, backup, true);
                    backedUp = true;
                }
                catch { }

                _notes.Clear();
                CreateDefaultWelcomeNotes();

                // Kullanıcının notları sessizce örnek notlarla değişmesin; arayüz bu uyarıyı gösterir
                LoadWarning = backedUp
                    ? $"Not dosyası (notes.json) bozuk olduğu için okunamadı.\n\nEski dosya şu adla yedeklendi:\n{backup}\n\nUygulama örnek notlarla açıldı."
                    : "Not dosyası (notes.json) bozuk olduğu için okunamadı ve yedeklenemedi.\n\nUygulama örnek notlarla açıldı.";
            }
            catch (Exception ex)
            {
                throw new IOException($"Notlar yüklenirken bir hata oluştu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tek bir notu düz metin (.txt) dosyası olarak dışa aktarır.
        /// </summary>
        public void ExportNoteToTxt(Note note, string targetFilePath)
        {
            try
            {
                using var writer = new StreamWriter(targetFilePath, false, Encoding.UTF8);
                writer.Write(note.ToPlainTextExport());
            }
            catch (Exception ex)
            {
                throw new IOException($"Not .txt formatında dışa aktarılırken hata oluştu: {ex.Message}", ex);
            }
        }

        private void CreateDefaultWelcomeNotes()
        {
            _notes.Add(new Note(
                "YMH 219 Nesne Tabanlı Programlama",
                "Bu not defteri uygulaması, JSON serileştirme ve kalıcı dosya I/O işlemlerini test etmek amacıyla geliştirilmiştir.\n\n" +
                "Özellikler:\n- Not Ekleme / Düzenleme / Silme\n- Otomatik JSON Kaydetme\n- .txt Olarak Dışa Aktarma\n- Kategori ve Zengin Metin Desteği",
                "Ders"));

            _notes.Add(new Note(
                "Haftalık Planım",
                "1. Proje teslim kurallarını kontrol et\n2. GitHub repolarını doğrula\n3. README ekran görüntülerini hazırla",
                "Kişisel"));
        }
    }
}
