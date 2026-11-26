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

    private void PopulateCategories()
    {
        var categories = _noteManager.GetCategories();

        // Kategori filtresi (Sol)
        cmbCategoryFilter.Items.Clear();
        cmbCategoryFilter.Items.Add("Tümü");
        foreach (var cat in categories)
        {
            cmbCategoryFilter.Items.Add(cat);
        }
        cmbCategoryFilter.SelectedIndex = 0;

        // Not kategorisi seçici (Sağ)
        cmbNoteCategory.Items.Clear();
        foreach (var cat in categories)
        {
            cmbNoteCategory.Items.Add(cat);
        }
        if (cmbNoteCategory.Items.Count > 0) cmbNoteCategory.SelectedIndex = 0;
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

    private void LstNotes_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (lstNotes.SelectedItem is Note selected)
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
        string title = txtTitle.Text.Trim();
        if (string.IsNullOrEmpty(title))
        {
            title = "Başlıksız Not";
            txtTitle.Text = title;
        }

        string content = rtbContent.Text;
        string? rtf = rtbContent.Rtf;
        string category = string.IsNullOrWhiteSpace(cmbNoteCategory.Text) ? "Genel" : cmbNoteCategory.Text.Trim();

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
            RefreshNotesList();

            // Kaydedilen notu listede seç
            for (int i = 0; i < lstNotes.Items.Count; i++)
            {
                if (lstNotes.Items[i] is Note n && n.Id == _currentNote.Id)
                {
                    lstNotes.SelectedIndex = i;
                    break;
                }
            }

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

        // Otomatik kaydetme açık olduğunda içeriği anlık olarak modele yansıt
        if (_currentNote != null && _noteManager.AutoSave)
        {
            _currentNote.Title = string.IsNullOrWhiteSpace(txtTitle.Text) ? "Başlıksız Not" : txtTitle.Text.Trim();
            _currentNote.Content = rtbContent.Text;
            _currentNote.RtfContent = rtbContent.Rtf;
            _currentNote.Category = cmbNoteCategory.Text.Trim();
            _currentNote.UpdatedAt = DateTime.Now;
            _noteManager.SaveToJson();

            lblStatus.Text = $"● Otomatik Kaydedildi: {DateTime.Now:HH:mm:ss}";
        }
    }

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
