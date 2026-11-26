namespace YMH219_NotepadApp;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.ToolStrip toolStripMain;
    private System.Windows.Forms.ToolStripButton btnNewNote;
    private System.Windows.Forms.ToolStripButton btnSaveNote;
    private System.Windows.Forms.ToolStripButton btnDeleteNote;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton btnExportTxt;
    private System.Windows.Forms.ToolStripButton btnReload;

    private System.Windows.Forms.SplitContainer splitMain;

    // Sol Panel
    private System.Windows.Forms.Panel pnlSearch;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.ComboBox cmbCategoryFilter;
    private System.Windows.Forms.ListBox lstNotes;
    private System.Windows.Forms.Label lblNoteCount;

    // Sağ Panel
    private System.Windows.Forms.Panel pnlEditorHeader;
    private System.Windows.Forms.TextBox txtTitle;
    private System.Windows.Forms.ComboBox cmbNoteCategory;
    private System.Windows.Forms.ToolStrip toolStripFormat;
    private System.Windows.Forms.ToolStripButton btnBold;
    private System.Windows.Forms.ToolStripButton btnItalic;
    private System.Windows.Forms.ToolStripButton btnUnderline;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton btnColor;
    private System.Windows.Forms.ToolStripButton btnBullet;
    private System.Windows.Forms.RichTextBox rtbContent;
    private System.Windows.Forms.Panel pnlEditorFooter;
    private System.Windows.Forms.Label lblDateInfo;

    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    private System.Windows.Forms.ToolStripStatusLabel lblWordCount;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        toolStripMain = new System.Windows.Forms.ToolStrip();
        btnNewNote = new System.Windows.Forms.ToolStripButton();
        btnSaveNote = new System.Windows.Forms.ToolStripButton();
        btnDeleteNote = new System.Windows.Forms.ToolStripButton();
        toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        btnExportTxt = new System.Windows.Forms.ToolStripButton();
        btnReload = new System.Windows.Forms.ToolStripButton();

        splitMain = new System.Windows.Forms.SplitContainer();

        pnlSearch = new System.Windows.Forms.Panel();
        txtSearch = new System.Windows.Forms.TextBox();
        cmbCategoryFilter = new System.Windows.Forms.ComboBox();
        lstNotes = new System.Windows.Forms.ListBox();
        lblNoteCount = new System.Windows.Forms.Label();

        pnlEditorHeader = new System.Windows.Forms.Panel();
        txtTitle = new System.Windows.Forms.TextBox();
        cmbNoteCategory = new System.Windows.Forms.ComboBox();

        toolStripFormat = new System.Windows.Forms.ToolStrip();
        btnBold = new System.Windows.Forms.ToolStripButton();
        btnItalic = new System.Windows.Forms.ToolStripButton();
        btnUnderline = new System.Windows.Forms.ToolStripButton();
        toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        btnColor = new System.Windows.Forms.ToolStripButton();
        btnBullet = new System.Windows.Forms.ToolStripButton();

        rtbContent = new System.Windows.Forms.RichTextBox();
        pnlEditorFooter = new System.Windows.Forms.Panel();
        lblDateInfo = new System.Windows.Forms.Label();

        statusStrip = new System.Windows.Forms.StatusStrip();
        lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
        lblWordCount = new System.Windows.Forms.ToolStripStatusLabel();

        toolStripMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        pnlSearch.SuspendLayout();
        pnlEditorHeader.SuspendLayout();
        toolStripFormat.SuspendLayout();
        pnlEditorFooter.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();

        // 
        // toolStripMain
        // 
        toolStripMain.BackColor = Color.FromArgb(245, 248, 250);
        toolStripMain.Font = new Font("Segoe UI", 9.5F);
        toolStripMain.GripStyle = ToolStripGripStyle.Hidden;
        toolStripMain.ImageScalingSize = new Size(20, 20);
        toolStripMain.Items.AddRange(new ToolStripItem[] {
            btnNewNote,
            btnSaveNote,
            btnDeleteNote,
            toolStripSeparator1,
            btnExportTxt,
            btnReload
        });
        toolStripMain.Location = new Point(0, 0);
        toolStripMain.Name = "toolStripMain";
        toolStripMain.Padding = new Padding(6, 4, 6, 4);
        toolStripMain.Size = new Size(960, 35);
        toolStripMain.TabIndex = 0;

        // btnNewNote
        btnNewNote.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnNewNote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnNewNote.ForeColor = Color.DarkGreen;
        btnNewNote.Name = "btnNewNote";
        btnNewNote.Size = new Size(81, 24);
        btnNewNote.Text = "➕ Yeni Not";
        btnNewNote.Click += BtnNewNote_Click;

        // btnSaveNote
        btnSaveNote.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnSaveNote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSaveNote.ForeColor = Color.DarkBlue;
        btnSaveNote.Name = "btnSaveNote";
        btnSaveNote.Size = new Size(69, 24);
        btnSaveNote.Text = "💾 Kaydet";
        btnSaveNote.Click += BtnSaveNote_Click;

        // btnDeleteNote
        btnDeleteNote.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnDeleteNote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnDeleteNote.ForeColor = Color.DarkRed;
        btnDeleteNote.Name = "btnDeleteNote";
        btnDeleteNote.Size = new Size(44, 24);
        btnDeleteNote.Text = "🗑️ Sil";
        btnDeleteNote.Click += BtnDeleteNote_Click;

        // toolStripSeparator1
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(6, 27);

        // btnExportTxt
        btnExportTxt.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnExportTxt.Name = "btnExportTxt";
        btnExportTxt.Size = new Size(137, 24);
        btnExportTxt.Text = "📄 .txt Olarak Dışa Aktar";
        btnExportTxt.Click += BtnExportTxt_Click;

        // btnReload
        btnReload.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnReload.Name = "btnReload";
        btnReload.Size = new Size(65, 24);
        btnReload.Text = "🔄 Yenile";
        btnReload.Click += BtnReload_Click;

        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel1;
        splitMain.Location = new Point(0, 35);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1 (Sol - Liste & Filtre)
        // 
        splitMain.Panel1.Controls.Add(lstNotes);
        splitMain.Panel1.Controls.Add(lblNoteCount);
        splitMain.Panel1.Controls.Add(pnlSearch);
        splitMain.Panel1.Padding = new Padding(6);
        // 
        // splitMain.Panel2 (Sağ - Not Editörü)
        // 
        splitMain.Panel2.Controls.Add(rtbContent);
        splitMain.Panel2.Controls.Add(pnlEditorFooter);
        splitMain.Panel2.Controls.Add(toolStripFormat);
        splitMain.Panel2.Controls.Add(pnlEditorHeader);
        splitMain.Panel2.Padding = new Padding(6);
        splitMain.Size = new Size(960, 560);
        splitMain.SplitterDistance = 310;
        splitMain.TabIndex = 1;

        // pnlSearch
        pnlSearch.Controls.Add(txtSearch);
        pnlSearch.Controls.Add(cmbCategoryFilter);
        pnlSearch.Dock = DockStyle.Top;
        pnlSearch.Location = new Point(6, 6);
        pnlSearch.Name = "pnlSearch";
        pnlSearch.Size = new Size(298, 62);
        pnlSearch.TabIndex = 0;

        // txtSearch
        txtSearch.Dock = DockStyle.Top;
        txtSearch.Font = new Font("Segoe UI", 9.5F);
        txtSearch.Location = new Point(0, 0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "🔍 Notlarda ara...";
        txtSearch.Size = new Size(298, 24);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += FilterChanged;

        // cmbCategoryFilter
        cmbCategoryFilter.Dock = DockStyle.Bottom;
        cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategoryFilter.Font = new Font("Segoe UI", 9F);
        cmbCategoryFilter.FormattingEnabled = true;
        cmbCategoryFilter.Location = new Point(0, 34);
        cmbCategoryFilter.Name = "cmbCategoryFilter";
        cmbCategoryFilter.Size = new Size(298, 23);
        cmbCategoryFilter.TabIndex = 1;
        cmbCategoryFilter.SelectedIndexChanged += FilterChanged;

        // lstNotes
        lstNotes.BackColor = Color.White;
        lstNotes.BorderStyle = BorderStyle.FixedSingle;
        lstNotes.Dock = DockStyle.Fill;
        lstNotes.Font = new Font("Segoe UI", 9.5F);
        lstNotes.FormattingEnabled = true;
        lstNotes.IntegralHeight = false;
        lstNotes.ItemHeight = 17;
        lstNotes.Location = new Point(6, 68);
        lstNotes.Name = "lstNotes";
        lstNotes.Size = new Size(298, 464);
        lstNotes.TabIndex = 1;
        lstNotes.SelectedIndexChanged += LstNotes_SelectedIndexChanged;

        // lblNoteCount
        lblNoteCount.Dock = DockStyle.Bottom;
        lblNoteCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
        lblNoteCount.ForeColor = Color.DimGray;
        lblNoteCount.Location = new Point(6, 532);
        lblNoteCount.Name = "lblNoteCount";
        lblNoteCount.Size = new Size(298, 22);
        lblNoteCount.Text = "Toplam Not: 0";
        lblNoteCount.TextAlign = ContentAlignment.MiddleLeft;

        // pnlEditorHeader
        pnlEditorHeader.Controls.Add(txtTitle);
        pnlEditorHeader.Controls.Add(cmbNoteCategory);
        pnlEditorHeader.Dock = DockStyle.Top;
        pnlEditorHeader.Location = new Point(6, 6);
        pnlEditorHeader.Name = "pnlEditorHeader";
        pnlEditorHeader.Size = new Size(634, 38);
        pnlEditorHeader.TabIndex = 0;

        // txtTitle
        txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        txtTitle.Location = new Point(0, 3);
        txtTitle.Name = "txtTitle";
        txtTitle.PlaceholderText = "Not Başlığı Giriniz...";
        txtTitle.Size = new Size(475, 29);
        txtTitle.TabIndex = 0;
        txtTitle.TextChanged += EditorContentChanged;

        // cmbNoteCategory
        cmbNoteCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cmbNoteCategory.Font = new Font("Segoe UI", 9.5F);
        cmbNoteCategory.FormattingEnabled = true;
        cmbNoteCategory.Location = new Point(482, 5);
        cmbNoteCategory.Name = "cmbNoteCategory";
        cmbNoteCategory.Size = new Size(150, 24);
        cmbNoteCategory.TabIndex = 1;

        // toolStripFormat (Zengin Metin Araçları)
        toolStripFormat.BackColor = Color.FromArgb(248, 249, 250);
        toolStripFormat.GripStyle = ToolStripGripStyle.Hidden;
        toolStripFormat.Items.AddRange(new ToolStripItem[] {
            btnBold,
            btnItalic,
            btnUnderline,
            toolStripSeparator2,
            btnColor,
            btnBullet
        });
        toolStripFormat.Location = new Point(6, 44);
        toolStripFormat.Name = "toolStripFormat";
        toolStripFormat.Size = new Size(634, 25);
        toolStripFormat.TabIndex = 1;

        btnBold.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnBold.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnBold.Name = "btnBold";
        btnBold.Size = new Size(23, 22);
        btnBold.Text = "K";
        btnBold.ToolTipText = "Kalın (Ctrl+B)";
        btnBold.Click += BtnBold_Click;

        btnItalic.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnItalic.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        btnItalic.Name = "btnItalic";
        btnItalic.Size = new Size(23, 22);
        btnItalic.Text = "I";
        btnItalic.ToolTipText = "İtalik (Ctrl+I)";
        btnItalic.Click += BtnItalic_Click;

        btnUnderline.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnUnderline.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
        btnUnderline.Name = "btnUnderline";
        btnUnderline.Size = new Size(23, 22);
        btnUnderline.Text = "U";
        btnUnderline.ToolTipText = "Altı Çizili (Ctrl+U)";
        btnUnderline.Click += BtnUnderline_Click;

        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(6, 25);

        btnColor.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnColor.Name = "btnColor";
        btnColor.Size = new Size(90, 22);
        btnColor.Text = "🎨 Metin Rengi";
        btnColor.Click += BtnColor_Click;

        btnBullet.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnBullet.Name = "btnBullet";
        btnBullet.Size = new Size(88, 22);
        btnBullet.Text = "• Madde İmleri";
        btnBullet.Click += BtnBullet_Click;

        // rtbContent
        rtbContent.BackColor = Color.White;
        rtbContent.BorderStyle = BorderStyle.FixedSingle;
        rtbContent.Dock = DockStyle.Fill;
        rtbContent.Font = new Font("Segoe UI", 10.5F);
        rtbContent.Location = new Point(6, 69);
        rtbContent.Name = "rtbContent";
        rtbContent.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbContent.Size = new Size(634, 459);
        rtbContent.TabIndex = 2;
        rtbContent.Text = "";
        rtbContent.TextChanged += EditorContentChanged;

        // pnlEditorFooter
        pnlEditorFooter.Controls.Add(lblDateInfo);
        pnlEditorFooter.Dock = DockStyle.Bottom;
        pnlEditorFooter.Location = new Point(6, 528);
        pnlEditorFooter.Name = "pnlEditorFooter";
        pnlEditorFooter.Size = new Size(634, 26);
        pnlEditorFooter.TabIndex = 3;

        // lblDateInfo
        lblDateInfo.Dock = DockStyle.Fill;
        lblDateInfo.Font = new Font("Segoe UI", 8F);
        lblDateInfo.ForeColor = Color.DimGray;
        lblDateInfo.Location = new Point(0, 0);
        lblDateInfo.Name = "lblDateInfo";
        lblDateInfo.Size = new Size(634, 26);
        lblDateInfo.Text = "Oluşturulma: - | Son Güncelleme: -";
        lblDateInfo.TextAlign = ContentAlignment.MiddleRight;

        // statusStrip
        statusStrip.Items.AddRange(new ToolStripItem[] {
            lblStatus,
            lblWordCount
        });
        statusStrip.Location = new Point(0, 595);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(960, 22);
        statusStrip.TabIndex = 2;

        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(795, 17);
        lblStatus.Spring = true;
        lblStatus.Text = "● Hazır - Otomatik Kaydetme Aktif (notes.json)";
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;

        lblWordCount.ForeColor = Color.DarkSlateGray;
        lblWordCount.Name = "lblWordCount";
        lblWordCount.Size = new Size(150, 17);
        lblWordCount.Text = "Karakter: 0 | Kelime: 0";
        lblWordCount.TextAlign = ContentAlignment.MiddleRight;

        // Form1
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 617);
        Controls.Add(splitMain);
        Controls.Add(toolStripMain);
        Controls.Add(statusStrip);
        MinimumSize = new Size(820, 520);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "YMH219 - Not Defteri Uygulaması (JSON Serileştirme & Dosya I/O)";
        FormClosing += Form1_FormClosing;

        toolStripMain.ResumeLayout(false);
        toolStripMain.PerformLayout();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        splitMain.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        pnlSearch.ResumeLayout(false);
        pnlSearch.PerformLayout();
        pnlEditorHeader.ResumeLayout(false);
        pnlEditorHeader.PerformLayout();
        toolStripFormat.ResumeLayout(false);
        toolStripFormat.PerformLayout();
        pnlEditorFooter.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
