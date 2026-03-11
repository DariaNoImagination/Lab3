using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{

    public partial class Lexeme //Данные о лексеме
    {
        public string Code { get; set; }
        public string TypeColumn { get; set; }
        public string Value { get; set; }
        public string Position { get; set; }

        public Lexeme(string type, string code, string value, int line, int start, int end)
        {
            Code = code;
            TypeColumn = type;
            Value = value;

            if (start == end)
                Position = $"Строка {line}, {start}";
            else
                Position = $"Строка {line}, {start}-{end}";
        }
    }

    static partial class Scanner //Сканнер
    {
        static public List<Lexeme> Analyze(string text)
        {
            List<Lexeme> lexemes = new List<Lexeme>();

            int index = 0;
            int str = 1; //Номер строки
            int positionInLine = 1;

            bool previousCharLetterDigit = false; //Предыдущий символ строка или цифра
            bool previousCharSpace = false; //Предыдущий символ пробел

            while (index < text.Length)
            {
                char currentChar = text[index];
                string Lexeme = "";
                int lexemeStartPos = positionInLine;


                if (currentChar == '\r' || currentChar == '\n')
                {
                    if (currentChar == '\n') //При переходе на новую строку сбрасываем все значения
                    {
                        str++;
                        positionInLine = 1;
                        previousCharLetterDigit = false;
                        previousCharSpace = false;
                    }

                    index++;
                    continue;
                }


                if (currentChar == '\t')
                {
                    positionInLine += 4;
                    index++;
                    continue;
                }


                if (char.IsDigit(currentChar))
                {

                    while (index < text.Length && char.IsDigit(text[index]))
                    {
                        Lexeme += text[index];
                        index++;
                        positionInLine++;
                    }


                    if (index < text.Length && text[index] == '.')
                    {
                        if (index + 1 < text.Length && char.IsDigit(text[index + 1]))
                        {
                            Lexeme += text[index];
                            index++;
                            positionInLine++;


                            while (index < text.Length && char.IsDigit(text[index]))
                            {
                                Lexeme += text[index];
                                index++;
                                positionInLine++;
                            }

                            lexemes.Add(new Lexeme(
                                "Число с плавающей точкой",
                                "9",
                                Lexeme,
                                str,
                                lexemeStartPos,
                                positionInLine - 1
                            ));
                        }
                        else
                        {
                            lexemes.Add(new Lexeme(
                                "Целое число",
                                "8",
                                Lexeme,
                                str,
                                lexemeStartPos,
                                positionInLine - 1
                            ));
                        }
                    }
                    else
                    {
                        lexemes.Add(new Lexeme(
                            "Целое число",
                            "8",
                            Lexeme,
                            str,
                            lexemeStartPos,
                            positionInLine - 1
                        ));
                    }

                    previousCharLetterDigit = true;
                    previousCharSpace = false;
                    continue;
                }


                else if (char.IsLetter(currentChar))
                {
                    while (index < text.Length && char.IsLetter(text[index]))
                    {
                        Lexeme += text[index];
                        index++;
                        positionInLine++;
                    }

                    switch (Lexeme)
                    {
                        case "struct":
                            lexemes.Add(new Lexeme(
                               "Ключевое слово",
                               "2",
                               Lexeme,
                               str,
                               lexemeStartPos,
                               positionInLine - 1
                           ));
                            break;
                        case "public":
                            lexemes.Add(new Lexeme(
                               "Ключевое слово",
                               "3",
                               Lexeme,
                               str,
                               lexemeStartPos,
                               positionInLine - 1
                           ));
                            break;
                        case "string":
                            lexemes.Add(new Lexeme(
                               "Ключевое слово",
                               "4",
                               Lexeme,
                               str,
                               lexemeStartPos,
                               positionInLine - 1
                           ));
                            break;
                        case "int":
                            lexemes.Add(new Lexeme(
                               "Ключевое слово",
                               "5",
                               Lexeme,
                               str,
                               lexemeStartPos,
                               positionInLine - 1
                           ));
                            break;
                        case "void":
                            lexemes.Add(new Lexeme(
                               "Ключевое слово",
                               "6",
                               Lexeme,
                               str,
                               lexemeStartPos,
                               positionInLine - 1
                           ));
                            break;
                        case "this":
                            lexemes.Add(new Lexeme(
                               "Ключевое слово",
                               "7",
                               Lexeme,
                               str,
                               lexemeStartPos,
                               positionInLine - 1
                           ));
                            break;
                        default:
                            lexemes.Add(new Lexeme(
                                "Идентификатор",
                                "1",
                                Lexeme,
                                str,
                                lexemeStartPos,
                                positionInLine - 1
                            ));
                            break;
                    }

                    previousCharLetterDigit = true;
                    previousCharSpace = false;
                    continue;
                }


                else if (currentChar == ' ')
                {

                    if (previousCharLetterDigit && !previousCharSpace)
                    {
                        int nextIndex = index + 1;
                        while (nextIndex < text.Length && (text[nextIndex] == ' ' || text[nextIndex] == '\t'))
                        {
                            nextIndex++;
                        }

                        if (nextIndex < text.Length && (char.IsLetterOrDigit(text[nextIndex]) || text[nextIndex] == '_'))
                        {
                            lexemes.Add(new Lexeme(
                                "Разделитель (пробел)",
                                "19",
                                " ",
                                str,
                                positionInLine,
                                positionInLine
                            ));
                            previousCharSpace = true;
                        }
                    }

                    positionInLine++;
                    index++;
                    continue;
                }


                else if (currentChar == '{' || currentChar == '}' || currentChar == ';' ||
                         currentChar == '(' || currentChar == ')' || currentChar == '=' ||
                         currentChar == '.' || currentChar == ',' || currentChar == ':')
                {
                    Lexeme = currentChar.ToString();
                    switch (Lexeme)
                    {
                        case "{":
                            lexemes.Add(new Lexeme(
                            "Начало выражения",
                            "10",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case "}":
                            lexemes.Add(new Lexeme(
                            "Конец выражения",
                            "11",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case ";":
                            lexemes.Add(new Lexeme(
                            "Конец оператора",
                            "12",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case "(":
                            lexemes.Add(new Lexeme(
                            "Начало перечисления аргументов",
                            "13",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case ")":
                            lexemes.Add(new Lexeme(
                            "Конец перечисления аргументов",
                            "14",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case "=":
                            lexemes.Add(new Lexeme(
                            "Оператор присваивания",
                            "15",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case ".":
                            lexemes.Add(new Lexeme(
                            "Синтаксический знак",
                            "16",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case ",":
                            lexemes.Add(new Lexeme(
                            "Синтаксический знак",
                            "17",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                        case ":":
                            lexemes.Add(new Lexeme(
                            "Синтаксический знак",
                            "18",
                            Lexeme,
                            str,
                            positionInLine,
                            positionInLine
                        ));
                            break;
                    }

                    positionInLine++;
                    index++;
                    continue;
                }


                else
                {
                    lexemes.Add(new Lexeme(
                        "Ошибка",
                        "20",
                        currentChar.ToString(),
                        str,
                        positionInLine,
                        positionInLine
                    ));
                    previousCharLetterDigit = false;
                    previousCharSpace = false;
                    positionInLine++;
                    index++;
                    continue;
                }
            }

            return lexemes;
        }
    }
}