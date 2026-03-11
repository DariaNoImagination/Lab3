using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab2
{
    public partial class Compiler : Form
    {
        string? filename; //Путь к текущему файлу
        OpenFileDialog openDialog = new OpenFileDialog();
        SaveFileDialog saveDialog = new SaveFileDialog();
        private Stack<string> redoStack = new Stack<string>();
        private Stack<string> undoStack = new Stack<string>();
        bool isProgrammaticChange = false; // Флаг для предотвращения зацикливания при Undo/Redo

        public Compiler()
        {
            InitializeComponent();
            openDialog.Filter = "Text files(*.txt)|*.txt|CSV files(*.csv)|*.csv|Word files(*.doc;*.docx)|*.doc;*.docx|All files(*.*)|*.*";
            saveDialog.Filter = "Text files(*.txt)|*.txt|CSV files(*.csv)|*.csv|Word files(*.doc;*.docx)|*.doc;*.docx|All files(*.*)|*.*";
        }

        private void aboutProgrammButton_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm("aboutProgramm");
            helpForm.ShowDialog();
        }
        private void helpButton_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm("help");
            helpForm.ShowDialog();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            saveDialog.Title = "Создание файла";

            if (saveDialog.ShowDialog() == DialogResult.Cancel)
                return;

            filename = saveDialog.FileName; //Сохраняем имя файла из диалогового окна

            try
            {
                //Создаем новый файл
                File.Create(filename).Close();
                fileInformationTextBox.Text = "";

                if (File.Exists(filename)) //Проверяем, создан ли файл
                    MessageBox.Show($"Файл создан: {Path.GetFileName(filename)}", "Создание файла",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Произошла ошибка при создании");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании файла: {ex.Message}");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

            try
            {
                // Проверяем, что имя файла указано
                if (string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show("Сначала создайте или откройте файл", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Сохраняем текст в файл
                File.WriteAllText(filename, fileInformationTextBox.Text);

                MessageBox.Show("Изменения успешно сохранены", "Сохранение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void fileOpenButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем наличие несохраненных изменений в текущем файле
                if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
                {
                    string currentContent = File.ReadAllText(filename);
                    if (currentContent != fileInformationTextBox.Text)
                    {
                        DialogResult result = MessageBox.Show(
                            "Последние изменения не были сохранены. Сохранить изменения?",
                            "Открыть файл", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            File.WriteAllText(filename, fileInformationTextBox.Text);
                        }
                        else if (result == DialogResult.Cancel)
                        {
                            return; // Отменяем открытие нового файла
                        }
                    }
                }


                if (openDialog.ShowDialog() == DialogResult.Cancel)
                    return;


                filename = openDialog.FileName;
                string fileText = File.ReadAllText(filename);


                redoStack.Clear();
                undoStack.Clear();


                undoStack.Push(fileText);


                isProgrammaticChange = true;
                fileInformationTextBox.Text = fileText;
                isProgrammaticChange = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void saveAsButton_Click(object sender, EventArgs e)
        {
            try
            {
                saveDialog.Title = "Сохранить как";

                if (saveDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                filename = saveDialog.FileName;
                File.WriteAllText(filename, fileInformationTextBox.Text);

                if (File.Exists(filename))
                    MessageBox.Show("Файл успешно сохранен", "Сохранение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Произошла ошибка при сохранении", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {

            // Проверяем наличие несохраненных изменений
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                string currentContent = File.ReadAllText(filename);
                if (currentContent != fileInformationTextBox.Text)
                {
                    DialogResult result = MessageBox.Show(
                        "Последние изменения не были сохранены. Сохранить изменения перед выходом?",
                        "Выход из приложения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        File.WriteAllText(filename, fileInformationTextBox.Text);
                        this.Close();
                    }
                    else if (result == DialogResult.No)
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }



        private void cutButton_Click(object sender, EventArgs e)
        {

            string selectedText = fileInformationTextBox.SelectedText;

            if (!string.IsNullOrEmpty(selectedText))
            {
                // Если есть выделенный текст - копируем его в буфер
                Clipboard.SetText(selectedText);
                fileInformationTextBox.SelectedText = "";
            }
            else
            {
                // Если нет выделения - копируем весь текст
                if (!string.IsNullOrEmpty(fileInformationTextBox.Text))
                {
                    Clipboard.SetText(fileInformationTextBox.Text);
                    fileInformationTextBox.Text = "";
                }
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            string selectedText = fileInformationTextBox.SelectedText;

            if (!string.IsNullOrEmpty(selectedText))
            {
                // Копируем выделенный текст
                Clipboard.SetText(selectedText);
            }
            else if (!string.IsNullOrEmpty(fileInformationTextBox.Text))
            {
                // Если нет выделения - копируем весь текст
                Clipboard.SetText(fileInformationTextBox.Text);
            }

        }

        private void pasteButton_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
                fileInformationTextBox.SelectedText = Clipboard.GetText();

        }

        private void selectAllButton_Click(object sender, EventArgs e)
        {
            fileInformationTextBox.SelectAll();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (fileInformationTextBox.SelectionLength > 0)
            {
                fileInformationTextBox.SelectedText = "";
            }
            else
            {
                fileInformationTextBox.Text = "";
            }
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 1)
            {
                isProgrammaticChange = true;
                redoStack.Push(fileInformationTextBox.Text);
                undoStack.Pop();
                fileInformationTextBox.Text = undoStack.Peek();

                isProgrammaticChange = false;
            }
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                isProgrammaticChange = true;

                string text = redoStack.Pop();
                undoStack.Push(text);
                fileInformationTextBox.Text = text;

                isProgrammaticChange = false;
            }
        }

        private void fileInformationTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!isProgrammaticChange)
            {
                undoStack.Push(fileInformationTextBox.Text);

                redoStack.Clear();
            }


        }

        private void programmButton_Click(object sender, EventArgs e)
        {
            List<Lexeme> lexemes = Scanner.Analyze(fileInformationTextBox.Text);
            dataGridView1.Rows.Clear();
            foreach (var lexeme in lexemes)
            {
                dataGridView1.Rows.Add(lexeme.Code, lexeme.TypeColumn, lexeme.Value, lexeme.Position);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
               
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                
                if (row.Cells["TypeColumn"].Value != null)
                {
                    string? value = row.Cells["TypeColumn"].Value.ToString();

                    if (value == "Ошибка")
                    {
                       
                        if (row.Cells["Position"].Value != null)
                        {
                            string? position = row.Cells["Position"].Value.ToString();

                           
                            string? errorValue = "";
                            if (row.Cells["Value"].Value != null)
                            {
                                errorValue = row.Cells["Value"].Value.ToString();
                            }
                            MoveToErrorPosition(fileInformationTextBox, position, errorValue);
                        }
                    }
                }
            }
        }

        private void MoveToErrorPosition(System.Windows.Forms.TextBox txtBox, string? positionString, string? errorValue) //Функция перехода к ошибке
        {
            if (txtBox == null || string.IsNullOrEmpty(positionString))
                return;

           
                txtBox.HideSelection = false;
              
                int lineNumber = -1; 
                int startPos = -1; 
                int endPos = -1;

               
                string[] parts = positionString.Split(new[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries); //Разделение строки на части 

                foreach (string part in parts)
                {
                    if (part == "Строка") 
                        continue;

                    if (int.TryParse(part, out int num)) //Получаем номер строки и индексы начала и конца лексемы из строки "Местоположение"
                    {
                        if (lineNumber == -1)
                            lineNumber = num;
                        else if (startPos == -1)
                            startPos = num;
                        else if (endPos == -1)
                            endPos = num;
                    }
                }

                
                if (lineNumber < 1 || lineNumber > txtBox.Lines.Length)
                {
                    Console.WriteLine($"Некорректный номер строки: {lineNumber}");
                    return;
                }

               
                int lineStartIndex = txtBox.GetFirstCharIndexFromLine(lineNumber - 1); //Получаем индекс первого символа строки

                if (lineStartIndex >= 0)
                {
                    string lineText = txtBox.Lines[lineNumber - 1]; //Получем текст строки

                   
                    if (startPos > 0)
                    {
                        
                        int selectionStart = lineStartIndex + (startPos - 1);  //Позиция начала выделения текста

                        
                        int selectionLength = 0;

                        if (endPos > startPos)
                        {
                            selectionLength = endPos - startPos; //Длина выделенного текста
                        }
                        else 
                        {
                            
                            selectionLength = 1;
                        }

                    
                        if (selectionStart >= 0 && selectionStart < txtBox.TextLength)
                        {
                            
                            if (selectionStart + selectionLength > txtBox.TextLength)
                            {
                                selectionLength = txtBox.TextLength - selectionStart;  //Если выделение выходит за пределы текста,обрезаем его
                            }

                          
                            txtBox.SelectionStart = selectionStart; //Установка выделения
                            txtBox.SelectionLength = selectionLength;

                        
                            txtBox.ScrollToCaret(); //Прокрутка к выделенному тексту

                            txtBox.Focus();


                        }
                    }
                    else //Если startPos некорректный, выделяем всю строку
                    {
                       
                        txtBox.SelectionStart = lineStartIndex;
                        txtBox.SelectionLength = lineText.Length;
                        txtBox.ScrollToCaret();
                        txtBox.Focus();


                    }
                }
       
        }
    }
}
