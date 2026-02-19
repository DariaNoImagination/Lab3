using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Laboratory1
{
    partial class Compiler
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Compiler));
            menuStrip = new MenuStrip();
            fileBtn = new ToolStripMenuItem();
            fileNewItem = new ToolStripMenuItem();
            fileOpenItem = new ToolStripMenuItem();
            fileSaveItem = new ToolStripMenuItem();
            fileSaveAsItem = new ToolStripMenuItem();
            fileExitItem = new ToolStripMenuItem();
            comandBtn = new ToolStripMenuItem();
            editUndoItem = new ToolStripMenuItem();
            editRedoItem = new ToolStripMenuItem();
            editCutItem = new ToolStripMenuItem();
            editCopyItem = new ToolStripMenuItem();
            editPasteItem = new ToolStripMenuItem();
            editDeleteItem = new ToolStripMenuItem();
            editSelectAllItem = new ToolStripMenuItem();
            textBtn = new ToolStripMenuItem();
            textTaskItem = new ToolStripMenuItem();
            textGrammarItem = new ToolStripMenuItem();
            textGrammarClassItem = new ToolStripMenuItem();
            textMethodItem = new ToolStripMenuItem();
            textTestItem = new ToolStripMenuItem();
            textLiteratureItem = new ToolStripMenuItem();
            textCodeItem = new ToolStripMenuItem();
            startBtn = new ToolStripMenuItem();
            helpBtn = new ToolStripMenuItem();
            helpCallItem = new ToolStripMenuItem();
            helpAboutItem = new ToolStripMenuItem();
            localizationBtn = new ToolStripMenuItem();
            viewBtn = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            newButton = new ToolStripButton();
            fileOpenButton = new ToolStripButton();
            saveButton = new ToolStripButton();
            undoButton = new ToolStripButton();
            redoButton = new ToolStripButton();
            copyButton = new ToolStripButton();
            cutButton = new ToolStripButton();
            pasteButton = new ToolStripButton();
            programmButton = new ToolStripButton();
            helpButton = new ToolStripButton();
            aboutProgrammButton = new ToolStripButton();
            splitContainer1 = new SplitContainer();
            fileInformationTextBox = new System.Windows.Forms.TextBox();
            textBox2 = new System.Windows.Forms.TextBox();
            menuStrip.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileBtn, comandBtn, textBtn, startBtn, helpBtn, localizationBtn, viewBtn });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(887, 28);
            menuStrip.TabIndex = 6;
            // 
            // fileBtn
            // 
            fileBtn.DropDownItems.AddRange(new ToolStripItem[] { fileNewItem, fileOpenItem, fileSaveItem, fileSaveAsItem, fileExitItem });
            fileBtn.Name = "fileBtn";
            fileBtn.Size = new Size(59, 24);
            fileBtn.Text = "Файл";
            // 
            // fileNewItem
            // 
            fileNewItem.Name = "fileNewItem";
            fileNewItem.Size = new Size(192, 26);
            fileNewItem.Text = "Создать";
            fileNewItem.Click += newButton_Click;
            // 
            // fileOpenItem
            // 
            fileOpenItem.Name = "fileOpenItem";
            fileOpenItem.Size = new Size(192, 26);
            fileOpenItem.Text = "Открыть";
            fileOpenItem.Click += fileOpenButton_Click;
            // 
            // fileSaveItem
            // 
            fileSaveItem.Name = "fileSaveItem";
            fileSaveItem.Size = new Size(192, 26);
            fileSaveItem.Text = "Сохранить";
            fileSaveItem.Click += saveButton_Click;
            // 
            // fileSaveAsItem
            // 
            fileSaveAsItem.Name = "fileSaveAsItem";
            fileSaveAsItem.Size = new Size(192, 26);
            fileSaveAsItem.Text = "Сохранить как";
            fileSaveAsItem.Click += saveAsButton_Click;
            // 
            // fileExitItem
            // 
            fileExitItem.Name = "fileExitItem";
            fileExitItem.Size = new Size(192, 26);
            fileExitItem.Text = "Выход";
            fileExitItem.Click += exitButton_Click;
            // 
            // comandBtn
            // 
            comandBtn.DropDownItems.AddRange(new ToolStripItem[] { editUndoItem, editRedoItem, editCutItem, editCopyItem, editPasteItem, editDeleteItem, editSelectAllItem });
            comandBtn.Name = "comandBtn";
            comandBtn.Size = new Size(74, 24);
            comandBtn.Text = "Правка";
            // 
            // editUndoItem
            // 
            editUndoItem.Name = "editUndoItem";
            editUndoItem.Size = new Size(186, 26);
            editUndoItem.Text = "Отменить";
            editUndoItem.Click += undoButton_Click;
            // 
            // editRedoItem
            // 
            editRedoItem.Name = "editRedoItem";
            editRedoItem.Size = new Size(186, 26);
            editRedoItem.Text = "Повторить";
            editRedoItem.Click += redoButton_Click;
            // 
            // editCutItem
            // 
            editCutItem.Name = "editCutItem";
            editCutItem.Size = new Size(186, 26);
            editCutItem.Text = "Вырезать";
            editCutItem.Click += cutButton_Click;
            // 
            // editCopyItem
            // 
            editCopyItem.Name = "editCopyItem";
            editCopyItem.Size = new Size(186, 26);
            editCopyItem.Text = "Копировать";
            editCopyItem.Click += copyButton_Click;
            // 
            // editPasteItem
            // 
            editPasteItem.Name = "editPasteItem";
            editPasteItem.Size = new Size(186, 26);
            editPasteItem.Text = "Вставить";
            editPasteItem.Click += pastetButton_Click;
            // 
            // editDeleteItem
            // 
            editDeleteItem.Name = "editDeleteItem";
            editDeleteItem.Size = new Size(186, 26);
            editDeleteItem.Text = "Удалить";
            editDeleteItem.Click += deleteButton_Click;
            // 
            // editSelectAllItem
            // 
            editSelectAllItem.Name = "editSelectAllItem";
            editSelectAllItem.Size = new Size(186, 26);
            editSelectAllItem.Text = "Выделить все";
            editSelectAllItem.Click += selectAllButton_Click;
            // 
            // textBtn
            // 
            textBtn.DropDownItems.AddRange(new ToolStripItem[] { textTaskItem, textGrammarItem, textGrammarClassItem, textMethodItem, textTestItem, textLiteratureItem, textCodeItem });
            textBtn.Name = "textBtn";
            textBtn.Size = new Size(59, 24);
            textBtn.Text = "Текст";
            // 
            // textTaskItem
            // 
            textTaskItem.Name = "textTaskItem";
            textTaskItem.Size = new Size(288, 26);
            textTaskItem.Text = "Постановка задачи";
            // 
            // textGrammarItem
            // 
            textGrammarItem.Name = "textGrammarItem";
            textGrammarItem.Size = new Size(288, 26);
            textGrammarItem.Text = "Грамматика";
            // 
            // textGrammarClassItem
            // 
            textGrammarClassItem.Name = "textGrammarClassItem";
            textGrammarClassItem.Size = new Size(288, 26);
            textGrammarClassItem.Text = "Классификация грамматики";
            // 
            // textMethodItem
            // 
            textMethodItem.Name = "textMethodItem";
            textMethodItem.Size = new Size(288, 26);
            textMethodItem.Text = "Метод анализа";
            // 
            // textTestItem
            // 
            textTestItem.Name = "textTestItem";
            textTestItem.Size = new Size(288, 26);
            textTestItem.Text = "Тестовый пример";
            // 
            // textLiteratureItem
            // 
            textLiteratureItem.Name = "textLiteratureItem";
            textLiteratureItem.Size = new Size(288, 26);
            textLiteratureItem.Text = "Список литературы";
            // 
            // textCodeItem
            // 
            textCodeItem.Name = "textCodeItem";
            textCodeItem.Size = new Size(288, 26);
            textCodeItem.Text = "Исходный код программы";
            // 
            // startBtn
            // 
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(55, 24);
            startBtn.Text = "Пуск";
            // 
            // helpBtn
            // 
            helpBtn.DropDownItems.AddRange(new ToolStripItem[] { helpCallItem, helpAboutItem });
            helpBtn.Name = "helpBtn";
            helpBtn.Size = new Size(81, 24);
            helpBtn.Text = "Справка";
            // 
            // helpCallItem
            // 
            helpCallItem.Name = "helpCallItem";
            helpCallItem.Size = new Size(197, 26);
            helpCallItem.Text = "Вызов справки";
            helpCallItem.Click += helpButton_Click;
            // 
            // helpAboutItem
            // 
            helpAboutItem.Name = "helpAboutItem";
            helpAboutItem.Size = new Size(197, 26);
            helpAboutItem.Text = "О программе";
            helpAboutItem.Click += aboutProgrammButton_Click;
            // 
            // localizationBtn
            // 
            localizationBtn.Name = "localizationBtn";
            localizationBtn.Size = new Size(115, 24);
            localizationBtn.Text = "Локализация";
            // 
            // viewBtn
            // 
            viewBtn.Name = "viewBtn";
            viewBtn.Size = new Size(49, 24);
            viewBtn.Text = "Вид";
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(50, 50);
            toolStrip1.Items.AddRange(new ToolStripItem[] { newButton, fileOpenButton, saveButton, undoButton, redoButton, copyButton, cutButton, pasteButton, programmButton, helpButton, aboutProgrammButton });
            toolStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
            toolStrip1.Location = new Point(0, 28);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(887, 57);
            toolStrip1.TabIndex = 7;
            toolStrip1.Text = "toolStrip1";
            // 
            // newButton
            // 
            newButton.Image = (Image)resources.GetObject("newButton.Image");
            newButton.Name = "newButton";
            newButton.Size = new Size(54, 54);
            newButton.ToolTipText = "Создать";
            newButton.Click += newButton_Click;
            // 
            // fileOpenButton
            // 
            fileOpenButton.Image = (Image)resources.GetObject("fileOpenButton.Image");
            fileOpenButton.Name = "fileOpenButton";
            fileOpenButton.Size = new Size(54, 54);
            fileOpenButton.ToolTipText = "Открыть";
            fileOpenButton.Click += fileOpenButton_Click;
            // 
            // saveButton
            // 
            saveButton.Image = (Image)resources.GetObject("saveButton.Image");
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(54, 54);
            saveButton.ToolTipText = "Сохранить";
            saveButton.Click += saveButton_Click;
            // 
            // undoButton
            // 
            undoButton.Image = (Image)resources.GetObject("undoButton.Image");
            undoButton.Name = "undoButton";
            undoButton.Size = new Size(54, 54);
            undoButton.ToolTipText = "Отменить";
            undoButton.Click += undoButton_Click;
            // 
            // redoButton
            // 
            redoButton.Image = (Image)resources.GetObject("redoButton.Image");
            redoButton.Name = "redoButton";
            redoButton.Size = new Size(54, 54);
            redoButton.ToolTipText = "Повторить";
            redoButton.Click += redoButton_Click;
            // 
            // copyButton
            // 
            copyButton.Image = (Image)resources.GetObject("copyButton.Image");
            copyButton.Name = "copyButton";
            copyButton.Size = new Size(54, 54);
            copyButton.ToolTipText = "Копировать";
            copyButton.Click += copyButton_Click;
            // 
            // cutButton
            // 
            cutButton.Image = (Image)resources.GetObject("cutButton.Image");
            cutButton.Name = "cutButton";
            cutButton.Size = new Size(54, 54);
            cutButton.ToolTipText = "Вырезать";
            cutButton.Click += cutButton_Click;
            // 
            // pasteButton
            // 
            pasteButton.Image = (Image)resources.GetObject("pasteButton.Image");
            pasteButton.Name = "pasteButton";
            pasteButton.Size = new Size(54, 54);
            pasteButton.ToolTipText = "Вставить";
            pasteButton.Click += pastetButton_Click;
            // 
            // programmButton
            // 
            programmButton.Image = (Image)resources.GetObject("programmButton.Image");
            programmButton.Name = "programmButton";
            programmButton.Size = new Size(54, 54);
            programmButton.ToolTipText = "Пуск";
            // 
            // helpButton
            // 
            helpButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            helpButton.Image = (Image)resources.GetObject("helpButton.Image");
            helpButton.ImageTransparentColor = Color.Magenta;
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(54, 54);
            helpButton.ToolTipText = "Вызов справки";
            helpButton.Click += helpButton_Click;
            // 
            // aboutProgrammButton
            // 
            aboutProgrammButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            aboutProgrammButton.Image = (Image)resources.GetObject("aboutProgrammButton.Image");
            aboutProgrammButton.ImageTransparentColor = Color.Magenta;
            aboutProgrammButton.Name = "aboutProgrammButton";
            aboutProgrammButton.Size = new Size(54, 54);
            aboutProgrammButton.ToolTipText = "О программе";
            aboutProgrammButton.Click += aboutProgrammButton_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 85);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(fileInformationTextBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(textBox2);
            splitContainer1.Size = new Size(887, 426);
            splitContainer1.SplitterDistance = 189;
            splitContainer1.TabIndex = 10;
            // 
            // fileInformationTextBox
            // 
            fileInformationTextBox.Dock = DockStyle.Fill;
            fileInformationTextBox.Font = new Font("Segoe UI", 14F);
            fileInformationTextBox.Location = new Point(0, 0);
            fileInformationTextBox.Multiline = true;
            fileInformationTextBox.Name = "fileInformationTextBox";
            fileInformationTextBox.ScrollBars = ScrollBars.Both;
            fileInformationTextBox.Size = new Size(887, 189);
            fileInformationTextBox.TabIndex = 8;
            fileInformationTextBox.TextChanged += fileInformationTextBox_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Font = new Font("Segoe UI", 14F);
            textBox2.Location = new Point(0, 0);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Both;
            textBox2.Size = new Size(887, 233);
            textBox2.TabIndex = 9;
            // 
            // Compiler
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(887, 511);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip);
            MinimumSize = new Size(600, 400);
            Name = "Compiler";
            Text = "Compiler";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip;
        private MenuStrip menuStrip;
        private ToolStrip toolStrip1;
        private ToolStripMenuItem fileBtn;
        private ToolStripMenuItem comandBtn;
        private ToolStripMenuItem textBtn;
        private ToolStripMenuItem startBtn;
        private ToolStripMenuItem helpBtn;
        private ToolStripMenuItem localizationBtn;
        private ToolStripMenuItem viewBtn;
        private ToolStripButton newButton;
        private ToolStripButton fileOpenButton;
        private ToolStripButton saveButton;
        private ToolStripButton undoButton;
        private ToolStripButton redoButton;
        private ToolStripButton copyButton;
        private ToolStripButton cutButton;
        private ToolStripButton pasteButton;
        private ToolStripButton helpButton;
        private ToolStripButton aboutProgrammButton;
        public ToolStripButton programmButton;
        private ToolStripMenuItem fileNewItem;
        private ToolStripMenuItem fileOpenItem;
        private ToolStripMenuItem fileSaveItem;
        private ToolStripMenuItem fileSaveAsItem;
        private ToolStripMenuItem fileExitItem;
        private ToolStripMenuItem editUndoItem;
        private ToolStripMenuItem editRedoItem;
        private ToolStripMenuItem editCutItem;
        private ToolStripMenuItem editCopyItem;
        private ToolStripMenuItem editPasteItem;
        private ToolStripMenuItem editDeleteItem;
        private ToolStripMenuItem editSelectAllItem;
        private ToolStripMenuItem textTaskItem;
        private ToolStripMenuItem textGrammarItem;
        private ToolStripMenuItem textGrammarClassItem;
        private ToolStripMenuItem textMethodItem;
        private ToolStripMenuItem textTestItem;
        private ToolStripMenuItem textLiteratureItem;
        private ToolStripMenuItem textCodeItem;
        private ToolStripMenuItem helpCallItem;
        private ToolStripMenuItem helpAboutItem;
        private SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox fileInformationTextBox;
        private System.Windows.Forms.TextBox textBox2;
    }
}