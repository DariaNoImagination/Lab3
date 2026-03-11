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

            
            bool inString = false;  // Находится ли в строке
            bool inInterpolation = false;  // Находится ли в строке с интерполяцией
            bool inInterpolationBrace = false;    // Находится ли в фигурных скобках интерполяции
            StringBuilder stringContent = new StringBuilder(); // Строковое содержимое
            int stringStartPosition = 1;         

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

    
                if (!inString && !inInterpolationBrace) //Вне строки и интерполяции
                {
                   
                    if (currentChar == '"')
                    {
                        lexemes.Add(new Lexeme(
                            "Синтаксический знак",
                            "20",
                            "\"",
                            str,
                            positionInLine,
                            positionInLine
                        ));

                        inString = true; // Вход в строковое содержимое
                        stringContent.Clear();
                        stringStartPosition = positionInLine + 1;

                        previousCharLetterDigit = false;
                        previousCharSpace = false;
                        positionInLine++;
                        index++;
                        continue;
                    }

                  
                    if (currentChar == '$')
                    {
                       
                        if (index + 1 < text.Length && text[index + 1] == '"') 
                        {
                            lexemes.Add(new Lexeme(
                                "Символ интерполяции",
                                 "18",
                                "$",
                                str,
                                positionInLine,
                                positionInLine
                            ));

                            inInterpolation = true; //Вход в интерполяцию
                            previousCharLetterDigit = false;
                            previousCharSpace = false;
                            positionInLine++;
                            index++;
                            continue;
                        }
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
                                    "21",
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
                             currentChar == '=' || currentChar == '.' || currentChar == ',' ||
                             currentChar == ':' || currentChar == ')' || currentChar == '(')
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
                                "19",
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
                            "22",
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

            
                else if (inString && !inInterpolationBrace) //Обработка в строке 
                {
                  
                    if (currentChar == '"') //Закрытие строки
                    {
                  
                        if (stringContent.Length > 0)
                        {
                            lexemes.Add(new Lexeme(
                                "Строковое содержимое",
                                "0",
                                $"\"{stringContent}\"",
                                str,
                                stringStartPosition,
                                positionInLine - 1
                            ));
                            stringContent.Clear();
                        }

                       
                        lexemes.Add(new Lexeme(
                            "Синтаксический знак",
                             "20",
                            "\"",
                            str,
                            positionInLine,
                            positionInLine
                        ));

                  
                        inString = false;
                        inInterpolation = false;

                        positionInLine++;
                        index++;
                        continue;
                    }

                    if (inInterpolation && currentChar == '{') //Обработка начала интерполяции
                    {
                        
                        if (stringContent.Length > 0)
                        {
                            lexemes.Add(new Lexeme(
                                "Строковое содержимое",
                                "0",
                                 $"\"{stringContent}\"",
                                str,
                                stringStartPosition,
                                positionInLine - 1
                            ));
                            stringContent.Clear();
                        }

                       
                        lexemes.Add(new Lexeme(
                            "Начало выражения интерполяции",
                            "10",
                            "{",
                            str,
                            positionInLine,
                            positionInLine
                        ));

                       
                        inInterpolationBrace = true;
                        inString = false;  
                        

                        positionInLine++;
                        index++;
                        continue;
                    }

                    stringContent.Append(currentChar);
                    positionInLine++;
                    index++;
                    continue;
                }

              
                else if (inInterpolationBrace) //Обработка внутри интерполяции
                {
                    
                    if (currentChar == '}')
                    {
                       
                        lexemes.Add(new Lexeme(
                            "Конец выражения интерполяции",
                            "11",
                            "}",
                            str,
                            positionInLine,
                            positionInLine
                        ));

                        
                        inInterpolationBrace = false;
                        inString = true;  
                        stringStartPosition = positionInLine + 1; 

                        positionInLine++;
                        index++;
                        continue;
                    }

                   
                    if (char.IsLetter(currentChar))
                    {
                        StringBuilder codeToken = new StringBuilder();
                        int codeTokenStart = positionInLine;

                        while (index < text.Length && char.IsLetter(text[index]))
                        {
                            codeToken.Append(text[index]);
                            index++;
                            positionInLine++;
                        }

                        lexemes.Add(new Lexeme(
                            "Идентификатор",
                            "1",
                            codeToken.ToString(),
                            str,
                            codeTokenStart,
                            positionInLine - 1
                        ));

                        continue;
                    }

                    
                    if (char.IsDigit(currentChar))
                    {
                        StringBuilder numberToken = new StringBuilder();
                        int numberTokenStart = positionInLine;

                        while (index < text.Length && char.IsDigit(text[index]))
                        {
                            numberToken.Append(text[index]);
                            index++;
                            positionInLine++;
                        }

                        lexemes.Add(new Lexeme(
                            "Целое число",
                            "8",
                            numberToken.ToString(),
                            str,
                            numberTokenStart,
                            positionInLine - 1
                        ));

                        continue;
                    }

                   
                    if (currentChar == '(' || currentChar == ')' || currentChar == '.' ||
                        currentChar == ',' || currentChar == '=' || currentChar == ';')
                    {
                        string tokenType = "";
                        string tokenCode = "";

                        switch (currentChar)
                        {
                            case '(': tokenType = "Начало перечисления аргументов"; tokenCode = "13"; break;
                            case ')': tokenType = "Конец перечисления аргументов"; tokenCode = "14"; break;
                            case '.': tokenType = "Синтаксический знак"; tokenCode = "16"; break;
                            case ',': tokenType = "Синтаксический знак"; tokenCode = "17"; break;
                            case '=': tokenType = "Оператор присваивания"; tokenCode = "15"; break;
                            case ';': tokenType = "Конец оператора"; tokenCode = "12"; break;
                        }

                        lexemes.Add(new Lexeme(
                            tokenType,
                            tokenCode,
                            currentChar.ToString(),
                            str,
                            positionInLine,
                            positionInLine
                        ));

                        positionInLine++;
                        index++;
                        continue;
                    }

                    if (char.IsWhiteSpace(currentChar) && currentChar != '\n' && currentChar != '\r')
                    {
                        positionInLine++;
                        index++;
                        continue;
                    }

                 
                    positionInLine++;
                    index++;
                }

                else
                {
                   
                    lexemes.Add(new Lexeme(
                        "Ошибка",
                        "22",
                        currentChar.ToString(),
                        str,
                        positionInLine,
                        positionInLine
                    ));
                    previousCharLetterDigit = false;
                    previousCharSpace = false;
                    positionInLine++;
                    index++;
                }
            }

            return lexemes;
        }
    }
}