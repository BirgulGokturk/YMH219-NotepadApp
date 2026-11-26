using YMH219_NotepadApp.Models;
using YMH219_NotepadApp.Services;

namespace YMH219_NotepadApp;

public partial class Form1 : Form
{
    private readonly NoteManager _noteManager = new();
    private Note? _currentNote;
    private bool _isBinding = false;

    public Form1()
    {
        InitializeComponent();

        PopulateCategories();
        RefreshNotesList();

        if (lstNotes.Items.Count > 0)
        {
            lstNotes.SelectedIndex = 0;
        }
        else
        {
            CreateNewNote();
        }
    }

    /// <summary>
    /// Kategori listelerini yeniler. Kullanıcının seçtiği filtre ve düzenlenen notun kategorisi
    /// korunur; önceden her kayıtta filtre "Tümü"ye, not kategorisi ilk kategoriye dönüyordu.
    /// </summary>
    private void PopulateCategories()
    {
        bool wasBinding = _isBinding;
        _isBinding = true;

        string? selectedFilter = cmbCategoryFilter.SelectedItem?.ToString();
        string noteCategory = cmbNoteCategory.Text;
        var categories = _noteManager.GetCategories();

        // Kategori filtresi (Sol)
        cmbCategoryFilter.Items.Clear();
        cmbCategoryFilter.Items.Add("Tümü");
        foreach (var cat in categories)
        {
            cmbCategoryFilter.Items.Add(cat);
        }
        int filterIndex = selectedFilter == null ? 0 : cmbCategoryFilter.Items.IndexOf(selectedFilter);
        cmbCategoryFilter.SelectedIndex = filterIndex >= 0 ? filterIndex : 0;

        // Not kategorisi seçici (Sağ)
        cmbNoteCategory.Items.Clear();
        foreach (var cat in categories)
        {
            cmbNoteCategory.Items.Add(cat);
        }
        if (!string.IsNullOrWhiteSpace(noteCategory))
        {
            cmbNoteCategory.Text = noteCategory;
        }
        else if (cmbNoteCategory.Items.Count > 0)
        {
            cmbNoteCategory.SelectedIndex = 0;
        }

        _isBinding = wasBinding;
    }

    private void RefreshNotesList()
    {
        string? searchTerm = txtSearch.Text.Trim();
        string? category = cmbCategoryFilter.SelectedItem?.ToString();

        var filtered = _noteManager.FilterNotes(searchTerm, category).ToList();

        lstNotes.BeginUpdate();
        lstNotes.Items.Clear();

        foreach (var note in filtered)
        {
            lstNotes.Items.Add(note);
        }

        lstNotes.EndUpdate();
        lblNoteCount.Text = $"Toplam Not: {filtered.Count} adet";
    }

    /// <summary>
    /// Listeyi yeniler ve düzenlenmekte olan notu, editörü yeniden yüklemeden seçili hale getirir.
    /// (Editör yeniden yüklenirse yazı imleci başa atlardı.)
    /// </summary>
    private void RefreshNotesListKeepingSelection()
    {
        bool wasBinding = _isBinding;
        _isBinding = true;

        RefreshNotesList();
        if (_currentNote != null)
        {
            int index = lstNotes.Items.IndexOf(_currentNote);
            if (index >= 0) lstNotes.SelectedIndex = index;
        }

        _isBinding = wasBinding;
    }

    private void LstNotes_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Liste program tarafından yenilenirken editör yeniden yüklenmez
        if (_isBinding) return;

        if (lstNotes.SelectedItem is Note selected && selected != _currentNote)
        {
            LoadNoteToEditor(selected);
        }
    }

    private void LoadNoteToEditor(Note note)
    {
        _isBinding = true;
        _currentNote = note;

        txtTitle.Text = note.Title;
        cmbNoteCategory.Text = note.Category;

        if (!string.IsNullOrEmpty(note.RtfContent))
        {
            try
            {
                rtbContent.Rtf = note.RtfContent;
            }
            catch
            {
                rtbContent.Text = note.Content;
            }
        }
        else
        {
            rtbContent.Text = note.Content;
        }

        lblDateInfo.Text = $"Oluşturulma: {note.CreatedAt:dd.MM.yyyy HH:mm} | Son Güncelleme: {note.UpdatedAt:dd.MM.yyyy HH:mm}";
        UpdateWordCount();

        _isBinding = false;
    }

    private void CreateNewNote()
    {
        _isBinding = true;
        _currentNote = null;

        txtTitle.Clear();
        rtbContent.Clear();
        cmbNoteCategory.Text = "Genel";
        lblDateInfo.Text = "Yeni Not (Henüz kaydedilmedi)";
        txtTitle.Focus();

        lstNotes.ClearSelected();
        UpdateWordCount();

        _isBinding = false;
    }

    #region CRUD Buton Olayları

    private void BtnNewNote_Click(object? sender, EventArgs e)
    {
        CreateNewNote();
    }

    private void BtnSaveNote_Click(object? sender, EventArgs e)
    {
        SaveCurrentNote();
    }

    private void SaveCurrentNote()
    {
        string title = GetEditorTitle();
        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            // Başlık kutusuna yazılan "Başlıksız Not" otomatik kaydı tetiklemesin
            _isBinding = true;
            txtTitle.Text = title;
            _isBinding = false;
        }

        string content = rtbContent.Text;
        string? rtf = rtbContent.Rtf;
        string category = GetEditorCategory();

        try
        {
            if (_currentNote == null)
            {
                // Yeni Not Ekle
                var newNote = new Note(title, content, category, rtf);
                _noteManager.AddNote(newNote);
                _currentNote = newNote;
            }
            else
            {
                // Mevcut Notu Güncelle
                _noteManager.UpdateNote(_currentNote.Id, title, content, category, rtf);
            }

            PopulateCategories();

            // Kaydedilen notu listede seç
            RefreshNotesListKeepingSelection();

            lblStatus.Text = $"● Kaydedildi: {DateTime.Now:HH:mm:ss} (notes.json)";
            lblDateInfo.Text = $"Oluşturulma: {_currentNote.CreatedAt:dd.MM.yyyy HH:mm} | Son Güncelleme: {_currentNote.UpdatedAt:dd.MM.yyyy HH:mm}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Not kaydedilirken bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDeleteNote_Click(object? sender, EventArgs e)
    {
        if (_currentNote == null)
        {
            MessageBox.Show("Silinecek bir not seçili değil.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"\"{_currentNote.Title}\" başlıklı not silinecektir. Onaylıyor musunuz?",
            "Notu Sil",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            _noteManager.DeleteNote(_currentNote.Id);
            RefreshNotesList();

            if (lstNotes.Items.Count > 0)
            {
                lstNotes.SelectedIndex = 0;
            }
            else
            {
                CreateNewNote();
            }

            lblStatus.Text = "🗑️ Not silindi ve dosya güncellendi.";
        }
    }

    private void BtnExportTxt_Click(object? sender, EventArgs e)
    {
        if (_currentNote == null && string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(rtbContent.Text))
        {
            MessageBox.Show("Dışa aktarılacak bir not bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string defaultTitle = string.IsNullOrWhiteSpace(txtTitle.Text) ? "Not" : txtTitle.Text.Trim();
        string safeFileName = string.Join("_", defaultTitle.Split(Path.GetInvalidFileNameChars()));

        using var sfd = new SaveFileDialog
        {
            Filter = "Metin Dosyası (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*",
            FileName = $"{safeFileName}_{DateTime.Now:yyyyMMdd}.txt",
            Title = "Notu .txt Dosyası Olarak Kaydet"
        };

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var noteToExport = _currentNote ?? new Note(txtTitle.Text, rtbContent.Text, cmbNoteCategory.Text);
                _noteManager.ExportNoteToTxt(noteToExport, sfd.FileName);

                MessageBox.Show($"Not başarıyla .txt olarak dışa aktarıldı:\n{sfd.FileName}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = $"📄 Dışa aktarıldı: {Path.GetFileName(sfd.FileName)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dosya yazılırken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnReload_Click(object? sender, EventArgs e)
    {
        try
        {
            _noteManager.LoadFromJson();
            PopulateCategories();
            RefreshNotesList();
            if (lstNotes.Items.Count > 0) lstNotes.SelectedIndex = 0;
            lblStatus.Text = "🔄 Notlar dosyadan yeniden yüklendi.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Zengin Metin Biçimlendirme

    private void BtnBold_Click(object? sender, EventArgs e)
    {
        ToggleFontStyle(FontStyle.Bold);
    }

    private void BtnItalic_Click(object? sender, EventArgs e)
    {
        ToggleFontStyle(FontStyle.Italic);
    }

    private void BtnUnderline_Click(object? sender, EventArgs e)
    {
        ToggleFontStyle(FontStyle.Underline);
    }

    private void ToggleFontStyle(FontStyle style)
    {
        if (rtbContent.SelectionFont == null) return;

        Font current = rtbContent.SelectionFont;
        FontStyle newStyle = current.Style ^ style;
        rtbContent.SelectionFont = new Font(current.FontFamily, current.Size, newStyle);
    }

    private void BtnColor_Click(object? sender, EventArgs e)
    {
        using var cd = new ColorDialog();
        if (cd.ShowDialog() == DialogResult.OK)
        {
            rtbContent.SelectionColor = cd.Color;
        }
    }

    private void BtnBullet_Click(object? sender, EventArgs e)
    {
        rtbContent.SelectionBullet = !rtbContent.SelectionBullet;
    }

    #endregion

    #region Arama ve İstatistikler

    private void FilterChanged(object? sender, EventArgs e)
    {
        RefreshNotesList();
    }

    private void EditorContentChanged(object? sender, EventArgs e)
    {
        if (_isBinding) return;

        UpdateWordCount();

        if (!_noteManager.AutoSave) return;

        try
        {
            if (_currentNote == null)
            {
                // Yeni not: ilk karakter yazıldığında not oluşturulur. Önceden yeni notlar
                // yalnızca "Kaydet" ile kaydediliyordu; başka nota geçince veya uygulama
                // kapanınca yazılanlar kayboluyordu.
                if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(rtbContent.Text)) return;

                var newNote = new Note(GetEditorTitle(), rtbContent.Text, GetEditorCategory(), rtbContent.Rtf);
                _noteManager.AddNote(newNote);
                _currentNote = newNote;
                RefreshNotesListKeepingSelection();
            }
            else
            {
                // Başlık veya kategori değiştiyse listedeki görünen metin de güncellenir
                bool listTextChanged = _currentNote.Title != GetEditorTitle() || _currentNote.Category != GetEditorCategory();
                _noteManager.UpdateNote(_currentNote.Id, GetEditorTitle(), rtbContent.Text, GetEditorCategory(), rtbContent.Rtf);
                if (listTextChanged) RefreshNotesListKeepingSelection();
            }

            lblStatus.Text = $"● Otomatik Kaydedildi: {DateTime.Now:HH:mm:ss}";
            lblDateInfo.Text = $"Oluşturulma: {_currentNote.CreatedAt:dd.MM.yyyy HH:mm} | Son Güncelleme: {_currentNote.UpdatedAt:dd.MM.yyyy HH:mm}";
        }
        catch (IOException ex)
        {
            // Dosya kilitliyse veya yazılamıyorsa uygulama kapanmaz; durum çubuğunda bildirilir
            lblStatus.Text = $"⚠ Otomatik kayıt başarısız: {ex.Message}";
        }
    }

    private string GetEditorTitle() =>
        string.IsNullOrWhiteSpace(txtTitle.Text) ? "Başlıksız Not" : txtTitle.Text.Trim();

    private string GetEditorCategory() =>
        string.IsNullOrWhiteSpace(cmbNoteCategory.Text) ? "Genel" : cmbNoteCategory.Text.Trim();

    private void UpdateWordCount()
    {
        string text = rtbContent.Text;
        int charCount = text.Length;
        int wordCount = text.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

        lblWordCount.Text = $"Karakter: {charCount} | Kelime: {wordCount}";
    }

    #endregion

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        try
        {
            _noteManager.SaveToJson();
        }
        catch { }
    }
}
