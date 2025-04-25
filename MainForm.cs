using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MediaMate
{
    public partial class MainForm : Form
    {
        private const string Version = "1.0";
        private TabControl tabControl;
        private TabPage renameTab;
        private TabPage moveTab;
        private TextBox seriesNameTextBox;
        private NumericUpDown seasonNumberUpDown;
        private Button renameButton;
        private Button moveButton;
        private TextBox folderPathTextBox;
        private Button browseButton;
        private Button startRenameButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel versionLabel;
        private TableLayoutPanel renameLayout;
        private NumericUpDown startNumberUpDown;
        private Action<Label> labelStyle;
        private Action<TextBox> textBoxStyle;
        private Action<Button> buttonStyle;
        private Action<NumericUpDown> numericStyle;

        // Couleurs personnalisées
        private readonly Color DarkBackground = Color.FromArgb(45, 45, 48);
        private readonly Color DarkerBackground = Color.FromArgb(37, 37, 38);
        private readonly Color AccentColor = Color.FromArgb(0, 122, 204);
        private readonly Color TextColor = Color.FromArgb(241, 241, 241);
        private readonly Color BorderColor = Color.FromArgb(63, 63, 70);
        private readonly Color ControlBackground = Color.FromArgb(55, 55, 58);

        public MainForm()
        {
            // Initialisation des champs
            tabControl = new TabControl();
            renameTab = new TabPage("Renommage");
            moveTab = new TabPage("Déplacement de fichiers");
            seriesNameTextBox = new TextBox();
            seasonNumberUpDown = new NumericUpDown();
            startNumberUpDown = new NumericUpDown();
            folderPathTextBox = new TextBox();
            browseButton = new Button();
            startRenameButton = new Button();
            statusStrip = new StatusStrip();
            versionLabel = new ToolStripStatusLabel();
            renameLayout = new TableLayoutPanel();
            renameButton = new Button();
            moveButton = new Button();

            // Initialisation des styles
            labelStyle = new Action<Label>(label => {
                label.ForeColor = TextColor;
                label.Font = new Font("Segoe UI", 10F);
                label.AutoSize = true;
                label.Margin = new Padding(0, 10, 0, 5);
            });

            textBoxStyle = new Action<TextBox>(textBox => {
                textBox.BackColor = ControlBackground;
                textBox.ForeColor = TextColor;
                textBox.Font = new Font("Segoe UI", 10F);
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.Margin = new Padding(0, 0, 10, 15);
                textBox.Padding = new Padding(5);
                textBox.MinimumSize = new Size(0, 30);
                textBox.Dock = DockStyle.Fill;
            });

            buttonStyle = new Action<Button>(button => {
                button.BackColor = AccentColor;
                button.ForeColor = TextColor;
                button.Font = new Font("Segoe UI", 9F);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Padding = new Padding(8, 5, 8, 5);
                button.MinimumSize = new Size(120, 35);
                button.Cursor = Cursors.Hand;
                
                button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(AccentColor.R + 20, AccentColor.G + 20, AccentColor.B + 20);
                button.MouseLeave += (s, e) => button.BackColor = AccentColor;
            });

            numericStyle = new Action<NumericUpDown>(numeric => {
                numeric.BackColor = ControlBackground;
                numeric.ForeColor = TextColor;
                numeric.Font = new Font("Segoe UI", 10F);
                numeric.BorderStyle = BorderStyle.FixedSingle;
                numeric.Margin = new Padding(0, 0, 0, 15);
                numeric.MinimumSize = new Size(100, 30);
                numeric.Size = new Size(120, 30);
            });

            // Configuration de base du formulaire
            this.Text = $"MediaMate v{Version}";
            this.MinimumSize = new Size(800, 600);
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = DarkBackground;
            this.ForeColor = TextColor;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // Initialisation des composants
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Configuration du TabControl
            tabControl.Dock = DockStyle.Fill;
            tabControl.BackColor = DarkBackground;
            tabControl.ForeColor = TextColor;
            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            tabControl.Padding = new Point(20, 8);
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(250, 40);
            tabControl.Margin = new Padding(0);

            // Configuration de la barre de statut
            statusStrip.BackColor = DarkerBackground;
            statusStrip.ForeColor = TextColor;
            statusStrip.Padding = new Padding(1);
            
            versionLabel.Text = $"Version {Version}";
            versionLabel.Alignment = ToolStripItemAlignment.Right;
            versionLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            statusStrip.Items.Add(versionLabel);

            // Onglet Renommage
            renameTab.BackColor = DarkBackground;
            renameTab.ForeColor = TextColor;
            renameTab.Padding = new Padding(20, 10, 20, 10);
            
            renameLayout.Dock = DockStyle.Fill;
            renameLayout.Padding = new Padding(20);
            renameLayout.RowCount = 9;
            renameLayout.ColumnCount = 2;
            renameLayout.BackColor = DarkBackground;
            renameLayout.AutoSize = true;
            renameLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Configuration des lignes avec espacement uniforme
            renameLayout.RowStyles.Clear();
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Description de l'onglet
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // RadioButtons
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // Label série/dossier
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // TextBox série/dossier
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // Label saison/numéro
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // NumericUpDown saison/numéro
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // Label dossier
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // TextBox + Button dossier
            renameLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Button renommer

            // Configuration des colonnes
            renameLayout.ColumnStyles.Clear();
            renameLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            renameLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));

            // Label descriptif de l'onglet
            var descLabel = new Label
            {
                Text = "Cet outil vous permet de renommer automatiquement vos fichiers ou dossiers.",
                AutoSize = true,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Margin = new Padding(3, 20, 3, 10),
                Padding = new Padding(10, 0, 10, 0)
            };
            renameLayout.Controls.Add(descLabel, 0, 0);
            renameLayout.SetColumnSpan(descLabel, 2);

            // Panel pour les RadioButtons avec meilleur alignement
            var radioPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 60,
                Margin = new Padding(10, 5, 10, 5)
            };

            var filesRadio = new RadioButton
            {
                Text = "Renommer les fichiers vidéo",
                Checked = true,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Location = new Point(0, 10),
                AutoSize = true
            };

            var foldersRadio = new RadioButton
            {
                Text = "Renommer les dossiers",
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Location = new Point(250, 10),
                AutoSize = true
            };

            radioPanel.Controls.Add(filesRadio);
            radioPanel.Controls.Add(foldersRadio);

            filesRadio.CheckedChanged += (s, e) => UpdateRenameInterface(filesRadio.Checked);
            foldersRadio.CheckedChanged += (s, e) => UpdateRenameInterface(!filesRadio.Checked);

            renameLayout.Controls.Add(radioPanel, 0, 1);
            renameLayout.SetColumnSpan(radioPanel, 2);

            // Label et TextBox pour le nom
            var nameLabel = new Label { Text = "Nom de la série:" };
            labelStyle(nameLabel);
            renameLayout.Controls.Add(nameLabel, 0, 2);
            renameLayout.SetColumnSpan(nameLabel, 2);

            seriesNameTextBox.Dock = DockStyle.Fill;
            textBoxStyle(seriesNameTextBox);
            renameLayout.Controls.Add(seriesNameTextBox, 0, 3);
            renameLayout.SetColumnSpan(seriesNameTextBox, 2);

            // Label et NumericUpDown pour la saison/numéro
            var seasonLabel = new Label { Text = "Numéro de saison:" };
            labelStyle(seasonLabel);
            renameLayout.Controls.Add(seasonLabel, 0, 4);
            renameLayout.SetColumnSpan(seasonLabel, 2);

            var startLabel = new Label { Text = "Numéro de départ:" };
            labelStyle(startLabel);
            renameLayout.Controls.Add(startLabel, 0, 4);
            renameLayout.SetColumnSpan(startLabel, 2);
            startLabel.Visible = false;

            seasonNumberUpDown.Minimum = 1;
            seasonNumberUpDown.Maximum = 99;
            seasonNumberUpDown.Value = 1;
            numericStyle(seasonNumberUpDown);
            renameLayout.Controls.Add(seasonNumberUpDown, 0, 5);

            startNumberUpDown.Minimum = 1;
            startNumberUpDown.Maximum = 999;
            startNumberUpDown.Value = 1;
            numericStyle(startNumberUpDown);
            renameLayout.Controls.Add(startNumberUpDown, 0, 5);
            startNumberUpDown.Visible = false;

            // Label pour le dossier
            var folderLabel = new Label { Text = "Dossier des fichiers à renommer:" };
            labelStyle(folderLabel);
            renameLayout.Controls.Add(folderLabel, 0, 6);
            renameLayout.SetColumnSpan(folderLabel, 2);

            // Configuration du TextBox et du bouton Parcourir dans l'onglet Renommage
            var folderPathPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            folderPathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            folderPathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));

            folderPathTextBox.Dock = DockStyle.Fill;
            textBoxStyle(folderPathTextBox);
            folderPathTextBox.Margin = new Padding(0, 0, 10, 15);
            folderPathPanel.Controls.Add(folderPathTextBox, 0, 0);

            browseButton.Text = "Parcourir...";
            buttonStyle(browseButton);
            browseButton.MinimumSize = new Size(120, 30);
            browseButton.MaximumSize = new Size(120, 30);
            browseButton.Margin = new Padding(0, 0, 0, 15);
            browseButton.Click += (s, e) => BrowseFolder(s, e);
            folderPathPanel.Controls.Add(browseButton, 1, 0);

            renameLayout.Controls.Add(folderPathPanel, 0, 7);
            renameLayout.SetColumnSpan(folderPathPanel, 2);

            // Bouton de renommage
            startRenameButton.Text = "Renommer les fichiers";
            buttonStyle(startRenameButton);
            startRenameButton.MinimumSize = new Size(200, 35);
            startRenameButton.Click += StartRename;
            startRenameButton.Margin = new Padding(0);
            startRenameButton.Dock = DockStyle.None;
            startRenameButton.Anchor = AnchorStyles.None;
            renameLayout.Controls.Add(startRenameButton, 0, 8);
            renameLayout.SetColumnSpan(startRenameButton, 2);

            renameTab.Controls.Add(renameLayout);

            // Onglet Déplacement
            moveTab.BackColor = DarkBackground;
            moveTab.ForeColor = TextColor;
            moveTab.Padding = new Padding(20, 10, 20, 10);
            
            var moveLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 6,
                ColumnCount = 2,
                BackColor = DarkBackground,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Configuration des lignes avec espacement uniforme
            moveLayout.RowStyles.Clear();
            moveLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));  // Description de l'onglet
            moveLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // Label source
            moveLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // TextBox + Button source
            moveLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // Label destination
            moveLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // TextBox + Button destination
            moveLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Button déplacer

            // Configuration des colonnes
            moveLayout.ColumnStyles.Clear();
            moveLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            moveLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));

            // Configuration des TextBox et boutons Parcourir dans l'onglet Déplacement
            var sourcePathPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            sourcePathPanel.ColumnStyles.Clear();
            sourcePathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            sourcePathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));  // Ajusté à 130F

            var sourcePathTextBox = new TextBox();
            textBoxStyle(sourcePathTextBox);
            sourcePathTextBox.Margin = new Padding(0, 0, 10, 15);
            sourcePathPanel.Controls.Add(sourcePathTextBox, 0, 0);

            var browseSourceButton = new Button { Text = "Parcourir..." };
            buttonStyle(browseSourceButton);
            browseSourceButton.MinimumSize = new Size(120, 30);
            browseSourceButton.MaximumSize = new Size(120, 30);
            browseSourceButton.Margin = new Padding(0, 0, 0, 15);
            browseSourceButton.Click += (s, e) => BrowseFolder(s, e);
            sourcePathPanel.Controls.Add(browseSourceButton, 1, 0);

            moveLayout.Controls.Add(sourcePathPanel, 0, 2);
            moveLayout.SetColumnSpan(sourcePathPanel, 2);

            var destFolderLabel = new Label
            {
                Text = "Dossier de destination:",
                AutoSize = true,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F)
            };
            labelStyle(destFolderLabel);
            moveLayout.Controls.Add(destFolderLabel, 0, 3);
            moveLayout.SetColumnSpan(destFolderLabel, 2);

            var destPathPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            destPathPanel.ColumnStyles.Clear();
            destPathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            destPathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));  // Ajusté à 130F

            var destPathTextBox = new TextBox();
            textBoxStyle(destPathTextBox);
            destPathTextBox.Margin = new Padding(0, 0, 10, 15);
            destPathPanel.Controls.Add(destPathTextBox, 0, 0);

            var browseDestButton = new Button { Text = "Parcourir..." };
            buttonStyle(browseDestButton);
            browseDestButton.MinimumSize = new Size(120, 30);
            browseDestButton.MaximumSize = new Size(120, 30);
            browseDestButton.Margin = new Padding(0, 0, 0, 15);
            browseDestButton.Click += (s, e) => BrowseFolder(s, e);
            destPathPanel.Controls.Add(browseDestButton, 1, 0);

            moveLayout.Controls.Add(destPathPanel, 0, 4);
            moveLayout.SetColumnSpan(destPathPanel, 2);

            // Pour les boutons principaux (Renommer et Déplacer)
            var mainButtonStyle = new Action<Button>(button => {
                buttonStyle(button);
                button.MinimumSize = new Size(200, 35);
                button.Margin = new Padding(0);
                button.Dock = DockStyle.None;
                button.Anchor = AnchorStyles.None;
            });

            moveButton = new Button { Text = "Déplacer les fichiers" };
            mainButtonStyle(moveButton);
            moveButton.Click += (s, e) => MoveFiles(sourcePathTextBox.Text, destPathTextBox.Text);
            moveLayout.Controls.Add(moveButton, 0, 5);
            moveLayout.SetColumnSpan(moveButton, 2);

            // Ajout du label descriptif pour l'onglet Déplacement
            var moveDescLabel = new Label
            {
                Text = "Cet outil vous permet de rassembler tous vos épisodes qui sont dispersés dans des sous-dossiers.\n" +
                      "Idéal pour extraire les épisodes de série qui sont dans des dossiers individuels.\n" +
                      "Formats supportés: MP4, MKV, AVI, WMV, MOV, FLV, WEBM, M4V, MPG, MPEG",
                AutoSize = true,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Margin = new Padding(3, 0, 3, 10),
                MaximumSize = new Size(600, 0)
            };
            moveLayout.Controls.Add(moveDescLabel, 0, 0);
            moveLayout.SetColumnSpan(moveDescLabel, 2);

            // Ajout des labels pour les dossiers source et destination
            var sourceFolderLabel = new Label
            {
                Text = "Dossier source contenant les fichiers:",
                AutoSize = true,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F)
            };
            labelStyle(sourceFolderLabel);
            moveLayout.Controls.Add(sourceFolderLabel, 0, 1);
            moveLayout.SetColumnSpan(sourceFolderLabel, 2);

            moveTab.Controls.Add(moveLayout);

            // Ajout des onglets
            tabControl.TabPages.Add(renameTab);
            tabControl.TabPages.Add(moveTab);

            // Configuration du formulaire
            this.Controls.Add(tabControl);
            this.Controls.Add(statusStrip);
        }

        private void BrowseFolder(object? sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Sélectionner le dossier contenant les fichiers";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                if (sender == browseButton)
                {
                    folderPathTextBox.Text = folderDialog.SelectedPath;
                }
                else if (sender is Button button)
                {
                    // Trouver le TextBox associé au bouton
                    var parent = button.Parent as TableLayoutPanel;
                    if (parent != null)
                    {
                        // Trouver le TextBox qui est dans la même ligne que le bouton
                        var row = parent.GetRow(button);
                        var textBox = parent.Controls.OfType<TextBox>()
                            .FirstOrDefault(t => parent.GetRow(t) == row);
                        if (textBox != null)
                        {
                            textBox.Text = folderDialog.SelectedPath;
                        }
                    }
                }
            }
        }

        private void UpdateRenameInterface(bool isFileMode)
        {
            if (seasonNumberUpDown != null && startNumberUpDown != null)
            {
                seasonNumberUpDown.Visible = isFileMode;
                startNumberUpDown.Visible = !isFileMode;
            }

            var seasonLabel = renameLayout?.Controls.OfType<Label>().FirstOrDefault(l => l.Text == "Numéro de saison:");
            var startLabel = renameLayout?.Controls.OfType<Label>().FirstOrDefault(l => l.Text == "Numéro de départ:");
            
            if (seasonLabel != null) seasonLabel.Visible = isFileMode;
            if (startLabel != null) startLabel.Visible = !isFileMode;

            if (startRenameButton != null)
            {
                startRenameButton.Text = isFileMode ? "Renommer les fichiers" : "Renommer les dossiers";
            }

            var nameLabel = renameLayout?.Controls.OfType<Label>().FirstOrDefault(l => l.Text.Contains("série") || l.Text.Contains("dossier"));
            if (nameLabel != null && nameLabel != renameLayout?.Controls.OfType<Label>().FirstOrDefault())
            {
                nameLabel.Text = isFileMode ? "Nom de la série:" : "Nom du dossier souhaité:";
            }
        }

        private void StartRename(object? sender, EventArgs e)
        {
            string folderPath = folderPathTextBox?.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Veuillez sélectionner un dossier", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string baseName = seriesNameTextBox?.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(baseName))
            {
                MessageBox.Show("Veuillez entrer un nom", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var radioButtons = renameLayout?.Controls.OfType<FlowLayoutPanel>()
                    .FirstOrDefault()?.Controls.OfType<RadioButton>();
                bool renameFiles = radioButtons?.FirstOrDefault()?.Checked ?? true;

                if (renameFiles)
                {
                    // Mode renommage de fichiers
                    int seasonNum = (int)(seasonNumberUpDown?.Value ?? 1);
                    var files = Directory.GetFiles(folderPath)
                        .Where(f => f.ToLower().EndsWith(".mp4") || 
                                  f.ToLower().EndsWith(".mkv") || 
                                  f.ToLower().EndsWith(".avi") ||
                                  f.ToLower().EndsWith(".wmv") ||
                                  f.ToLower().EndsWith(".mov") ||
                                  f.ToLower().EndsWith(".flv") ||
                                  f.ToLower().EndsWith(".webm") ||
                                  f.ToLower().EndsWith(".m4v") ||
                                  f.ToLower().EndsWith(".mpg") ||
                                  f.ToLower().EndsWith(".mpeg"))
                        .OrderBy(f => f)
                        .ToList();

                    for (int i = 0; i < files.Count; i++)
                    {
                        string oldPath = files[i];
                        string ext = Path.GetExtension(oldPath);
                        string newName = $"{baseName} S{seasonNum:D2}E{i + 1:D2}{ext}";
                        string newPath = Path.Combine(Path.GetDirectoryName(oldPath) ?? "", newName);
                        File.Move(oldPath, newPath);
                    }
                }
                else
                {
                    // Mode renommage de dossiers
                    int startNumber = (int)(startNumberUpDown?.Value ?? 1);
                    var directories = Directory.GetDirectories(folderPath)
                        .OrderBy(d => d)
                        .ToList();

                    for (int i = 0; i < directories.Count; i++)
                    {
                        string oldPath = directories[i];
                        string newName = $"{baseName} {startNumber + i:D2}";
                        string newPath = Path.Combine(Path.GetDirectoryName(oldPath) ?? "", newName);
                        Directory.Move(oldPath, newPath);
                    }
                }

                MessageBox.Show($"Les {(renameFiles ? "fichiers" : "dossiers")} ont été renommés avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MoveFiles(string sourcePath, string destPath)
        {
            if (string.IsNullOrEmpty(sourcePath))
            {
                MessageBox.Show("Veuillez sélectionner un dossier source", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(destPath))
            {
                MessageBox.Show("Veuillez sélectionner un dossier de destination", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var videoExtensions = new[] { ".mp4", ".mkv", ".avi", ".wmv", ".mov", ".flv", ".webm", ".m4v", ".mpg", ".mpeg" };
                var processedDirs = new HashSet<string>();

                // Récupérer tous les fichiers vidéo
                var videoFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)
                    .Where(f => videoExtensions.Contains(Path.GetExtension(f).ToLower()))
                    .ToList();

                foreach (string sourceFile in videoFiles)
                {
                    string fileName = Path.GetFileName(sourceFile);
                    string destFilePath = Path.Combine(destPath, fileName);
                    string sourceDir = Path.GetDirectoryName(sourceFile);

                    if (sourceDir != null)
                    {
                        processedDirs.Add(sourceDir);
                    }

                    // Si le fichier existe déjà, ajouter un suffixe
                    if (File.Exists(destFilePath))
                    {
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        string ext = Path.GetExtension(fileName);
                        int i = 1;
                        while (File.Exists(destFilePath))
                        {
                            destFilePath = Path.Combine(destPath, $"{nameWithoutExt}_{i}{ext}");
                            i++;
                        }
                    }

                    // Déplacer le fichier vidéo
                    File.Move(sourceFile, destFilePath);

                    // Supprimer les fichiers annexes dans le même dossier
                    if (sourceDir != null)
                    {
                        var otherFiles = Directory.GetFiles(sourceDir)
                            .Where(f => !videoExtensions.Contains(Path.GetExtension(f).ToLower()));
                        
                        foreach (var file in otherFiles)
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception)
                            {
                                // Ignorer les erreurs de suppression des fichiers annexes
                            }
                        }
                    }
                }

                // Supprimer les dossiers vides, en commençant par les plus profonds
                var dirsToCheck = processedDirs.OrderByDescending(d => d.Count(c => c == Path.DirectorySeparatorChar));
                foreach (var dir in dirsToCheck)
                {
                    try
                    {
                        // Vérifier si le dossier est vide (pas de fichiers et pas de sous-dossiers)
                        if (!Directory.EnumerateFileSystemEntries(dir).Any())
                        {
                            Directory.Delete(dir, false); // false = ne pas supprimer récursivement
                        }
                    }
                    catch (Exception)
                    {
                        // Ignorer les erreurs de suppression des dossiers
                    }
                }

                MessageBox.Show("Les fichiers ont été déplacés et les dossiers vides ont été nettoyés avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private GraphicsPath GetRoundedRect(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Width - radius * 2, rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Mettre à jour les régions arrondies des boutons
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    button.Region = new Region(GetRoundedRect(new RectangleF(Point.Empty, button.Size), 8));
                }
                if (control is TabControl tc)
                {
                    foreach (TabPage page in tc.TabPages)
                    {
                        foreach (Control pageControl in page.Controls)
                        {
                            if (pageControl is TableLayoutPanel panel)
                            {
                                foreach (Control panelControl in panel.Controls)
                                {
                                    if (panelControl is Button panelButton)
                                    {
                                        panelButton.Region = new Region(GetRoundedRect(new RectangleF(Point.Empty, panelButton.Size), 8));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
} 