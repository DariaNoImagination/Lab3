using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{

    public partial class Token
    {
        public string Code { get; set; }      
        public string TypeColumn { get; set; } 
        public string Value { get; set; }    
        public string Position { get; set; }   

        public Token(string type, string code, string value, int line, int start, int end)
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

    static partial class Scanner
    {
        static public List<Token> Analyze(string text)
        {
            List<Token> tokens = new List<Token>(); 

            int index = 0;
            int str = 1; 
            int positionInLine = 1; 

            
            bool inString = false;             
            bool inInterpolation = false;       
            bool inInterpolationBrace = false;   
            bool inCode = false;                 
            StringBuilder stringContent = new StringBuilder(); 
            int stringStartPosition = 1;         

            bool previousCharWasLetterOrDigit = false; 
            bool previousCharWasSpace = false; 

            while (index < text.Length)
            {
                char currentChar = text[index];
                string token = "";
                int tokenStartPos = positionInLine; 


                if (currentChar == '\r' || currentChar == '\n')
                {
                    if (currentChar == '\n')
                    {
                        str++; 
                        positionInLine = 1; 
                        previousCharWasLetterOrDigit = false;
                        previousCharWasSpace = false;
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

    
                if (!inString && !inCode)
                {
                   
                    if (currentChar == '"')
                    {
                        tokens.Add(new Token(
                            "Синтаксический знак",
                            "20",
                            "\"",
                            str,
                            positionInLine,
                            positionInLine
                        ));

                        inString = true;
                        stringContent.Clear();
                        stringStartPosition = positionInLine + 1;

                        previousCharWasLetterOrDigit = false;
                        previousCharWasSpace = false;
                        positionInLine++;
                        index++;
                        continue;
                    }

                  
                    if (currentChar == '$')
                    {
                       
                        if (index + 1 < text.Length && text[index + 1] == '"')
                        {
                            tokens.Add(new Token(
                                "Символ интерполяции",
                                 "18",
                                "$",
                                str,
                                positionInLine,
                                positionInLine
                            ));

                            inInterpolation = true;
                            previousCharWasLetterOrDigit = false;
                            previousCharWasSpace = false;
                            positionInLine++;
                            index++;
                            continue;
                        }
                    }

                  
                    if (char.IsDigit(currentChar))
                    {
                        
                        while (index < text.Length && char.IsDigit(text[index]))
                        {
                            token += text[index];
                            index++;
                            positionInLine++;
                        }

                
                        if (index < text.Length && text[index] == '.')
                        {
                            if (index + 1 < text.Length && char.IsDigit(text[index + 1]))
                            {
                                token += text[index]; 
                                index++;
                                positionInLine++;

                             
                                while (index < text.Length && char.IsDigit(text[index]))
                                {
                                    token += text[index];
                                    index++;
                                    positionInLine++;
                                }

                                tokens.Add(new Token(
                                    "Число с плавающей точкой",
                                    "9",
                                    token,
                                    str,
                                    tokenStartPos,
                                    positionInLine - 1
                                ));
                            }
                            else
                            {
                                tokens.Add(new Token(
                                    "Целое число",
                                    "8",
                                    token,
                                    str,
                                    tokenStartPos,
                                    positionInLine - 1
                                ));
                            }
                        }
                        else
                        {
                            tokens.Add(new Token(
                                "Целое число",
                                "8",
                                token,
                                str,
                                tokenStartPos,
                                positionInLine - 1
                            ));
                        }

                        previousCharWasLetterOrDigit = true;
                        previousCharWasSpace = false;
                        continue;
                    }

                    
                    else if (char.IsLetter(currentChar))
                    {
                        while (index < text.Length && char.IsLetter(text[index]))
                        {
                            token += text[index];
                            index++;
                            positionInLine++;
                        }

                        switch (token)
                        {
                            case "struct":
                                tokens.Add(new Token(
                                   "Ключевое слово",
                                   "2",
                                   token,
                                   str,
                                   tokenStartPos,
                                   positionInLine - 1
                               ));
                                break;
                            case "public":
                                tokens.Add(new Token(
                                   "Ключевое слово",
                                   "3",
                                   token,
                                   str,
                                   tokenStartPos,
                                   positionInLine - 1
                               ));
                                break;
                            case "string":
                                tokens.Add(new Token(
                                   "Ключевое слово",
                                   "4",
                                   token,
                                   str,
                                   tokenStartPos,
                                   positionInLine - 1
                               ));
                                break;
                            case "int":
                                tokens.Add(new Token(
                                   "Ключевое слово",
                                   "5",
                                   token,
                                   str,
                                   tokenStartPos,
                                   positionInLine - 1
                               ));
                                break;
                            case "void":
                                tokens.Add(new Token(
                                   "Ключевое слово",
                                   "6",
                                   token,
                                   str,
                                   tokenStartPos,
                                   positionInLine - 1
                               ));
                                break;
                            case "this":
                                tokens.Add(new Token(
                                   "Ключевое слово",
                                   "7",
                                   token,
                                   str,
                                   tokenStartPos,
                                   positionInLine - 1
                               ));
                                break;
                            default:
                                tokens.Add(new Token(
                                    "Идентификатор",
                                    "1",
                                    token,
                                    str,
                                    tokenStartPos,
                                    positionInLine - 1
                                ));
                                break;
                        }

                        previousCharWasLetterOrDigit = true;
                        previousCharWasSpace = false;
                        continue;
                    }

                 
                    else if (currentChar == ' ')
                    {
               
                        if (previousCharWasLetterOrDigit && !previousCharWasSpace)
                        {
                            int nextIndex = index + 1;
                            while (nextIndex < text.Length && (text[nextIndex] == ' ' || text[nextIndex] == '\t'))
                            {
                                nextIndex++;
                            }

                            if (nextIndex < text.Length && (char.IsLetterOrDigit(text[nextIndex]) || text[nextIndex] == '_'))
                            {
                                tokens.Add(new Token(
                                    "Разделитель (пробел)",
                                    "21",
                                    " ",
                                    str,
                                    positionInLine,
                                    positionInLine
                                ));
                                previousCharWasSpace = true;
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
                        token = currentChar.ToString();
                        switch (token)
                        {
                            case "{":
                                tokens.Add(new Token(
                                "Начало выражения",
                                "10",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case "}":
                                tokens.Add(new Token(
                                "Конец выражения",
                                "11",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case ";":
                                tokens.Add(new Token(
                                "Конец оператора",
                                "12",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case "(":
                                tokens.Add(new Token(
                                "Начало перечисления аргументов",
                                "13",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case ")":
                                tokens.Add(new Token(
                                "Конец перечисления аргументов",
                                "14",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case "=":
                                tokens.Add(new Token(
                                "Оператор присваивания",
                                "15",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case ".":
                                tokens.Add(new Token(
                                "Синтаксический знак",
                                "16",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case ",":
                                tokens.Add(new Token(
                                "Синтаксический знак",
                                "17",
                                token,
                                str,
                                positionInLine,
                                positionInLine
                            ));
                                break;
                            case ":":
                                tokens.Add(new Token(
                                "Синтаксический знак",
                                "19",
                                token,
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
                        tokens.Add(new Token(
                            "Ошибка",
                            "21",
                            currentChar.ToString(),
                            str,
                            positionInLine,
                            positionInLine
                        ));
                        previousCharWasLetterOrDigit = false;
                        previousCharWasSpace = false;
                        positionInLine++;
                        index++;
                        continue;
                    }
                }

            
                else if (inString && !inCode)
                {
                  
                    if (currentChar == '"')
                    {
                  
                        if (stringContent.Length > 0)
                        {
                            tokens.Add(new Token(
                                "Строковое содержимое",
                                "0",
                                $"\"{stringContent}\"",
                                str,
                                stringStartPosition,
                                positionInLine - 1
                            ));
                            stringContent.Clear();
                        }

                       
                        tokens.Add(new Token(
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

                    if (inInterpolation && currentChar == '{')
                    {
                        
                        if (stringContent.Length > 0)
                        {
                            tokens.Add(new Token(
                                "Строковое содержимое",
                                "0",
                                 $"\"{stringContent}\"",
                                str,
                                stringStartPosition,
                                positionInLine - 1
                            ));
                            stringContent.Clear();
                        }

                       
                        tokens.Add(new Token(
                            "Начало выражения интерполяции",
                            "10",
                            "{",
                            str,
                            positionInLine,
                            positionInLine
                        ));

                       
                        inInterpolationBrace = true;
                        inString = false;  
                        inCode = true;      

                        positionInLine++;
                        index++;
                        continue;
                    }

                    stringContent.Append(currentChar);
                    positionInLine++;
                    index++;
                    continue;
                }

              
                else if (inCode && inInterpolationBrace)
                {
                    
                    if (currentChar == '}')
                    {
                       
                        tokens.Add(new Token(
                            "Конец выражения интерполяции",
                            "11",
                            "}",
                            str,
                            positionInLine,
                            positionInLine
                        ));

                        
                        inInterpolationBrace = false;
                        inCode = false;
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

                        tokens.Add(new Token(
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

                        tokens.Add(new Token(
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

                        tokens.Add(new Token(
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
                   
                    tokens.Add(new Token(
                        "Ошибка",
                        "22",
                        currentChar.ToString(),
                        str,
                        positionInLine,
                        positionInLine
                    ));
                    previousCharWasLetterOrDigit = false;
                    previousCharWasSpace = false;
                    positionInLine++;
                    index++;
                }
            }

            return tokens;
        }
    }
}