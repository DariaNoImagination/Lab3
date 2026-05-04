using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lab2
{
    public enum CODE
    {
        ERROR, STRUCT, PUBLIC, STRING, INT, FLOAT, THIS,
        IDENTIFIER, TYPE_ID, SPACE, OPCURLYBRACE, CLOSECURLYBRACE, COMMA, END
    }

    public class Error
    {
        public string InvalidFragment { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Expected { get; set; }
        public string Found { get; set; }

        public Error(string invalidFragment, string position, string description, string expected = null, string found = null)
        {
            InvalidFragment = invalidFragment;
            Position = position;
            Description = description;
            Expected = expected;
            Found = found;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Expected) && !string.IsNullOrEmpty(Found))
                return $"Ошибка: {Description} | Ожидалось: {Expected} | Найдено: '{Found}' | Позиция: {Position}";
            return $"Ошибка: {Description} | Фрагмент: '{InvalidFragment}' | Позиция: {Position}";
        }
    }

    public class Token
    {
        public CODE Code { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public Lexeme OriginalLexeme { get; set; }

        public Token(CODE code, string value, int line, int startColumn, int endColumn, Lexeme originalLexeme = null, bool isError = false, string errorMessage = null)
        {
            Code = code; Value = value; Line = line;
            StartColumn = startColumn; EndColumn = endColumn;
            OriginalLexeme = originalLexeme;
            IsError = isError;
            ErrorMessage = errorMessage;
        }

        public override string ToString() => $"Code: {Code,-12} | Value: {Value,-15} | Line: {Line,3} | Start: {StartColumn,3} | End: {EndColumn,3}";
    }

    public class Parser
    {
        private List<Token> _tokens;
        private List<Error> _errors;
        private string _text;
        private int _pos;
        private Token _current;
        private int _safetyCounter;
        private const int MAX_SAFETY = 10000;

        private enum ParserState
        {
            ExpectStruct,
            ExpectIdentifier,
            ExpectOpenCurlyBrace,
            ExpectPublic,
            ExpectType,
            ExpectSemicolonOrComma,
            ExpectFieldName,
            ExpectSemicolon,
            ExpectCloseCurlyBrace,
            ExpectSemicolonAfterClose,
            Accept
        }

        public Parser(string text)
        {
            _text = text;
            _errors = new List<Error>();
        }

        public List<Error> Parse()
        {
            var lexemes = Scanner.Analyze(_text);
            _tokens = ConvertLexemesToTokens(lexemes);
            _pos = 0;
            _safetyCounter = 0;
            GetNextToken();

            if (_tokens.Count == 0)
            {


                AddError("начало файла", "Строка 1, 0",
             "Строка должна начинаться с ключевого слова 'struct'",
             "'struct'", "начало файла");

                return _errors;
            }

            ParserState state = ParserState.ExpectStruct;

            while (state != ParserState.Accept && _safetyCounter < MAX_SAFETY)
            {
                _safetyCounter++;

                ConsumeLexicalErrors();

                if (MatchesState(state, _current))
                {
                    state = ConsumeExpectedToken(state);
                    continue;
                }

                AddStateError(state, _current);

                if (!Recover(ref state))
                {
                    state = ParserState.Accept;
                }
            }

            

            return _errors;
        }

        private string GetPosition(Token token)
        {
            if (token == null && _tokens.Count > 0)
            {
                var last = _tokens.Last();
                return $"Строка {last.Line}, {last.EndColumn + 1}";
            }
            if (token == null) return "Строка ?, ?";
            if (token.StartColumn == token.EndColumn)
                return $"Строка {token.Line}, {token.StartColumn}";
            else
                return $"Строка {token.Line}, {token.StartColumn}-{token.EndColumn}";
        }

        private void AddError(string invalidFragment, string position, string description, string expected = null, string found = null)
        {
            if (!_errors.Any(e => e.Position == position && e.Description == description))
            {
                
                _errors.Add(new Error(invalidFragment, position, description, expected, found));
            }
        }

        private string GetExpectedString(ParserState state)
        {
            switch (state)
            {
                case ParserState.ExpectStruct: return "'struct'";
                case ParserState.ExpectIdentifier: return "идентификатор";
                case ParserState.ExpectOpenCurlyBrace: return "'{'";
                case ParserState.ExpectPublic: return "'public',  или '}'";
                case ParserState.ExpectType: return "тип (int, float, string) с именем поля";
                case ParserState.ExpectSemicolonOrComma: return "';' или ','";
                case ParserState.ExpectFieldName: return "имя поля";
                case ParserState.ExpectSemicolon: return "';'";
                case ParserState.ExpectCloseCurlyBrace: return "'}'";
                case ParserState.ExpectSemicolonAfterClose: return "';'";
                default: return "допустимый токен";
            }
        }

        private void GetNextToken()
        {
            _current = _pos < _tokens.Count ? _tokens[_pos++] : null;
        }

        private void ConsumeLexicalErrors()
        {
            while (_current != null && _current.Code == CODE.ERROR)
            {
                AddError(_current.Value, GetPosition(_current), _current.ErrorMessage ?? "Неизвестный символ", "допустимый символ", _current.Value);
                GetNextToken();
            }
        }

        private bool MatchesState(ParserState state, Token token)
        {
            if (token == null) return state == ParserState.Accept;
            Debug.WriteLine(token); 
            switch (state)
            {
                case ParserState.ExpectStruct:
                    return token.Code == CODE.STRUCT;
                case ParserState.ExpectIdentifier:
                    return token.Code == CODE.IDENTIFIER;
                case ParserState.ExpectOpenCurlyBrace:
                    return token.Code == CODE.OPCURLYBRACE;
                case ParserState.ExpectPublic:
                    return token.Code == CODE.PUBLIC || token.Code == CODE.CLOSECURLYBRACE;
                case ParserState.ExpectType:
                    return token.Code == CODE.TYPE_ID;
                case ParserState.ExpectSemicolonOrComma:
                    return token.Code == CODE.END || token.Code == CODE.COMMA;
                case ParserState.ExpectFieldName:
                    return token.Code == CODE.IDENTIFIER;
                case ParserState.ExpectSemicolon:
                    return token.Code == CODE.END;
                case ParserState.ExpectCloseCurlyBrace:
                    return token.Code == CODE.CLOSECURLYBRACE;
                case ParserState.ExpectSemicolonAfterClose:
                    return token.Code == CODE.END;
                case ParserState.Accept:
                    return token == null;
                default:
                    return false;
            }
        }

        private ParserState ConsumeExpectedToken(ParserState state)
        {
            
            switch (state)
            {
                case ParserState.ExpectStruct:

                    if (_current != null && _current.Code == CODE.STRUCT)
                    {
                        GetNextToken();
                        return ParserState.ExpectIdentifier;
                    }
                    else if (_current != null && _current.Code == CODE.IDENTIFIER)
                    {
                        
                        AddError(_current.Value, GetPosition(_current), "Строка должна начинаться с ключевого слова 'struct'", "'struct'", _current.Value);
                 
                        return ParserState.ExpectIdentifier;
                    }
                    
                    else
                    {
                        AddError(_current?.Value ?? "конец файла", GetPosition(_current), "Строка должна начинаться с ключевого слова 'struct'", "'struct'", _current?.Value ?? "конец файла");
                        GetNextToken();
                        return ParserState.ExpectStruct; 
                    }

                case ParserState.ExpectIdentifier:
                    GetNextToken();
                    return ParserState.ExpectOpenCurlyBrace;

                case ParserState.ExpectOpenCurlyBrace:
                    GetNextToken();
                    return ParserState.ExpectPublic;

                case ParserState.ExpectPublic:
                    if (_current != null && _current.Code == CODE.PUBLIC)
                    {
                        GetNextToken();
                        return ParserState.ExpectType;
                    }
                    else if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                    {
                        GetNextToken();
                        return ParserState.ExpectSemicolonAfterClose;
                    }
                    else if (_current != null && _current.Code == CODE.TYPE_ID)
                    {
                        AddError(_current.Value, GetPosition(_current), "Поле должно начинаться с 'public'", "'public'", _current.Value);
                        GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }
                    else if (_current != null && _current.Code == CODE.IDENTIFIER)
                    {
                        AddError(_current.Value, GetPosition(_current), "Ожидается 'public' или тип с именем поля", "'public' или тип с именем поля", _current.Value);
                        GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }
                    else if (_current != null && _current.Code == CODE.END)
                    {
                        
                        GetNextToken();
                        return ParserState.ExpectPublic;
                    }
                    else
                    {
                        AddError(_current?.Value ?? "конец файла", GetPosition(_current), "Ожидается 'public' или '}'", "'public' или '}'", _current?.Value ?? "конец файла");
                        GetNextToken();
                        return ParserState.ExpectPublic;
                    }

                case ParserState.ExpectType:

                    if (_current != null && _current.Code == CODE.TYPE_ID)
                    {
                        GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }
                    else if (_current != null && _current.Code == CODE.IDENTIFIER)
                    {
                        
                        AddError(_current.Value, GetPosition(_current), "После 'public' ожидается тип с именем поля", "тип (int, float, string) с именем поля", _current.Value);
                        GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }
                    else
                    {
                        AddError(_current?.Value ?? "конец файла", GetPosition(_current), "Ожидается тип с именем поля", "тип (int, float, string) с именем поля", _current?.Value ?? "конец файла");
                        GetNextToken();
                        return ParserState.ExpectType;
                    }

                case ParserState.ExpectSemicolonOrComma:
                    while (_current != null && _current.Code == CODE.SPACE)
                        GetNextToken();

                    if (_current != null && _current.Code == CODE.END)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        return ParserState.ExpectPublic;
                    }
                    else if (_current != null && _current.Code == CODE.COMMA)
                    {
                        GetNextToken();
                        if (_current != null && _current.Code == CODE.IDENTIFIER)
                        {
                            return ParserState.ExpectFieldName;
                        }
                        else
                        {
                            AddError(_current?.Value ?? "конец файла", GetPosition(_current),
                                "После ',' ожидается имя поля",
                                "идентификатор", _current?.Value ?? "конец файла");
                            return ParserState.ExpectSemicolonOrComma;
                        }
                    }
                    else if (_current != null && _current.Code == CODE.IDENTIFIER)
                    {
                        AddError(_current.Value, GetPosition(_current), "Ожидается ';' или ','", "';' или ','", _current.Value);
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }
                    else if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                    {
                        return ParserState.ExpectCloseCurlyBrace;
                    }
                    else if (_current != null && _current.Code == CODE.PUBLIC)
                    {
                        AddError(_current.Value, GetPosition(_current),
                            "Ожидается ';' '",
                            "';'", _current.Value);
                        return ParserState.ExpectPublic;
                    }
                    else if (_current != null && _current.Code == CODE.TYPE_ID)
                    {
                        AddError(_current.Value, GetPosition(_current),
                            "Ожидается';' или ','",
                            "';' или ','", _current.Value);
                        return ParserState.ExpectType;
                    }
                    else
                    {
                        AddError(_current?.Value ?? "конец файла", GetPosition(_current), "Ожидается ';' или ','", "';' или ','", _current?.Value ?? "конец файла");
                        GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }

                case ParserState.ExpectFieldName:
                    if (_current != null && _current.Code == CODE.IDENTIFIER)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }
                    else if (_current != null && _current.Code == CODE.TYPE_ID)
                    {
                        
                        AddError(_current.Value, GetPosition(_current),
                            "Ожидается имя поля",
                            "идентификатор", _current.Value);
                       
                        return ParserState.ExpectType;
                    }
                    else
                    {
                        AddError(_current?.Value ?? "конец файла", GetPosition(_current),
                            "Ожидается имя поля",
                            "идентификатор", _current?.Value ?? "конец файла");
                        GetNextToken();
                        return ParserState.ExpectSemicolonOrComma;
                    }

                case ParserState.ExpectSemicolon:
                    if (_current != null && _current.Code == CODE.END)
                    {
                        GetNextToken();
                        return ParserState.ExpectPublic;
                    }
                    else if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                    {
                        GetNextToken();
                        return ParserState.ExpectSemicolonAfterClose;
                    }
                    return ParserState.ExpectSemicolon;

                case ParserState.ExpectCloseCurlyBrace:
                    if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                    {
                        GetNextToken();
                        return ParserState.ExpectSemicolonAfterClose;
                    }
                    return ParserState.ExpectCloseCurlyBrace;

                case ParserState.ExpectSemicolonAfterClose:
                    if (_current != null && _current.Code == CODE.END)
                    {
                        GetNextToken();
                        return ParserState.Accept;
                    }
                    return ParserState.ExpectSemicolonAfterClose;

                default:
                    return ParserState.Accept;
            }
        }

        private bool Recover(ref ParserState state)
        {
            int startPos = _pos;
            int lastFragments = 0;
            switch (state)
            {
                case ParserState.ExpectStruct:
                    bool hasStructNearby = false;
                    int checkPos = _pos - 1;



                    for (int i = 0; i < 10 && checkPos + i < _tokens.Count; i++)
                    {
                        Token t = _tokens[checkPos + i];

                        if (t.Code == CODE.STRUCT ||
                            (t.Code == CODE.IDENTIFIER && t.Value.ToLower() == "struct") ||
                            (t.Code == CODE.IDENTIFIER && (t.Value.ToLower() == "str" || t.Value.ToLower() == "stru" || t.Value.ToLower() == "st" || t.Value.ToLower() == "s")))
                        {
                            hasStructNearby = true;
                            break;
                        }
                    }


                    if (!hasStructNearby)
                    {
                        GetNextToken();
                        if (_current == null)
                        {   
                            state = ParserState.Accept;
                            break;
                        }

                        state = ParserState.ExpectOpenCurlyBrace;
                        break;
                       
                    }

                    bool foundStruct = false;
                    startPos = _pos - 1;


                    for (int i = 0; i < 5 && startPos + i < _tokens.Count; i++)
                    {
                        Token token = _tokens[startPos + i];
                        if (token.Code == CODE.STRUCT)
                        {


                            for (int j = 0; j <= i; j++)
                                GetNextToken();

                            if (_current == null)
                            {
                                state = ParserState.Accept;
                                break;
                            }
                            state = ParserState.ExpectIdentifier;
                            break;
                        }
                        else if (token.Code == CODE.IDENTIFIER || token.Code == CODE.ERROR)
                        {

                            string combined = "";
                            List<Token> fragments = new List<Token>();

                            for (int k = 0; k <= i && startPos + k < _tokens.Count; k++)
                            {
                                Token t = _tokens[startPos + k];
                                if (t.Code == CODE.IDENTIFIER || t.Code == CODE.ERROR)
                                {
                                    fragments.Add(t);
                                    combined += t.Value;
                                }
                                else if (t.Code == CODE.SPACE)
                                {
                                    continue;
                                }
                                
                                else
                                {
                                    break;
                                }
                            }
                            lastFragments = fragments.Count();
                            string cleaned = new string(combined.Where(c => char.IsLetter(c)).ToArray());
                            if (cleaned.ToLower() == "struct")
                            {
                                foundStruct = true;
                                

                                for (int j = 0; j < fragments.Count; j++)
                                    GetNextToken();


                                while (_current != null && _current.Code == CODE.SPACE)
                                    GetNextToken();

                                if (_current == null)
                                {
                                    state = ParserState.Accept;
                                    break;
                                }

                                state = ParserState.ExpectIdentifier;
                                break;
                            }
                        }
                    }

                    if (!foundStruct)
                    {
                       for (int j = 0; j < lastFragments; j++)
                            GetNextToken();
     

                        if (_current == null)
                        {
                            state = ParserState.Accept;
                            break;
                        }
                        state = ParserState.ExpectOpenCurlyBrace;
                    }

                    break;


                case ParserState.ExpectIdentifier:
                   
                    while (_current != null && _current.Code != CODE.IDENTIFIER && _current.Code != CODE.OPCURLYBRACE)
                    {
                        GetNextToken();
                       
                        while (_current != null && _current.Code == CODE.SPACE)
                        {
                            GetNextToken();
                        }
                    }

                    if (_current != null && _current.Code == CODE.IDENTIFIER)
                    {
                       
                        string firstIdentifier = _current.Value;
                        Token firstToken = _current;

                        
                        GetNextToken();

                       
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();

                        if (_current != null && _current.Code == CODE.IDENTIFIER)
                        {
                           
                            int lookaheadPos = _pos;
                            Token lookahead = null;

                            while (lookaheadPos < _tokens.Count && _tokens[lookaheadPos].Code == CODE.SPACE)
                                lookaheadPos++;

                            if (lookaheadPos < _tokens.Count)
                                lookahead = _tokens[lookaheadPos];

                          
                            if (lookahead != null && lookahead.Code == CODE.OPCURLYBRACE)
                            {
                                
                                GetNextToken(); 

                                while (_current != null && _current.Code == CODE.SPACE)
                                    GetNextToken();

                                state = ParserState.ExpectOpenCurlyBrace;
                                return true;
                            }
                            else
                            {
                              
                                state = ParserState.ExpectOpenCurlyBrace;
                                return true;
                            }
                        }
                        else
                        {
                            
                            state = ParserState.ExpectOpenCurlyBrace;
                            return true;
                        }
                    }
                    else if (_current != null && _current.Code == CODE.OPCURLYBRACE)
                    {
                       
                        state = ParserState.ExpectOpenCurlyBrace;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                   

                case ParserState.ExpectOpenCurlyBrace:
                    while (_current != null && _current.Code == CODE.SPACE)
                        GetNextToken();

                    if (_current == null)
                    {
                        state = ParserState.Accept;
                        return true;
                    }

                    if (_current.Code == CODE.OPCURLYBRACE)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectPublic;
                        return true;
                    }

                    
                    if (_current.Code == CODE.PUBLIC)
                    {
                        state = ParserState.ExpectPublic;
                        return true;
                    }

                 
                    if (_current.Code == CODE.IDENTIFIER)
                    {
                        
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();

                   
                        if (_current != null && _current.Code == CODE.OPCURLYBRACE)
                        {
                            GetNextToken();
                            while (_current != null && _current.Code == CODE.SPACE)
                                GetNextToken();
                            state = ParserState.ExpectPublic;
                            return true;
                        }

                        state = ParserState.ExpectPublic;
                        return true;
                    }

                    state = ParserState.ExpectPublic;
                    return true;

                case ParserState.ExpectPublic:
                    while (_current != null && _current.Code == CODE.SPACE)
                        GetNextToken();

                    if (_current == null)
                    {
                        state = ParserState.Accept;
                        return true;
                    }

                    if (_current.Code == CODE.OPCURLYBRACE)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectPublic;
                        return true;
                    }

                    if (_current.Code == CODE.CLOSECURLYBRACE)
                    {
                        state = ParserState.ExpectSemicolonAfterClose;
                        return true;
                    }

                    if (_current.Code == CODE.PUBLIC)
                    {
                        state = ParserState.ExpectType;
                        return true;
                    }

                    if (_current.Code == CODE.TYPE_ID)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectSemicolonOrComma;
                        return true;
                    }

                    if (_current.Code == CODE.IDENTIFIER)
                    {
                        
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();

                       
                        if (_current != null && _current.Code == CODE.TYPE_ID)
                        {
                            state = ParserState.ExpectType;
                            return true;
                        }

                        state = ParserState.ExpectPublic;
                        return true;
                    }

                    if (_current.Code == CODE.END)
                    {
                        GetNextToken();
                        state = ParserState.ExpectPublic;
                        return true;
                    }

                    
                    while (_current != null && _current.Code != CODE.PUBLIC && _current.Code != CODE.CLOSECURLYBRACE &&
                           _current.Code != CODE.TYPE_ID && _current.Code != CODE.IDENTIFIER && _current.Code != CODE.OPCURLYBRACE)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                    }

                    if (_current != null && _current.Code == CODE.PUBLIC)
                        state = ParserState.ExpectType;
                    else if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                        state = ParserState.ExpectSemicolonAfterClose;
                    else if (_current != null && _current.Code == CODE.TYPE_ID)
                    {
                        GetNextToken();
                        state = ParserState.ExpectFieldName;
                    }
                    else if (_current != null && _current.Code == CODE.IDENTIFIER)
                    {
                        GetNextToken();
                        state = ParserState.ExpectPublic;
                    }
                    else if (_current != null && _current.Code == CODE.OPCURLYBRACE)
                    {
                        GetNextToken();
                        state = ParserState.ExpectPublic;
                    }
                    else
                        return false;
                    break;

                case ParserState.ExpectType:
                    SkipUntil(CODE.TYPE_ID, CODE.END, CODE.CLOSECURLYBRACE, CODE.IDENTIFIER, CODE.ERROR, CODE.PUBLIC);
                    if (_current == null)
                    {
                        state = ParserState.Accept;
                        return true;
                    }
                    if (_current != null && _current.Code == CODE.TYPE_ID)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectFieldName;  
                        return true;
                    }
                    else if (_current != null && _current.Code == CODE.END)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectPublic;
                        return true;
                    }
                    else if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                    {
                        state = ParserState.ExpectCloseCurlyBrace;
                        return true;
                    }
                    else if (_current != null && _current.Code == CODE.PUBLIC)
                    {
                        state = ParserState.ExpectPublic;
                        return true;
                    }
                    else if (_current.Code == CODE.IDENTIFIER || _current.Code == CODE.ERROR)
                    {
                        string combined = "";
                        List<Token> fragments = new List<Token>();
                        int tempPos = _pos;
                        int maxFragments = 5;

                        for (int i = 0; i < maxFragments && tempPos + i < _tokens.Count; i++)
                        {
                            Token t = _tokens[tempPos + i];

                            
                            if (t.Code == CODE.SPACE)
                                break;

                           
                            if (t.Code != CODE.IDENTIFIER && t.Code != CODE.ERROR)
                                break;

                            fragments.Add(t);
                            combined += t.Value;

                            
                            string cleaned = new string(combined.Where(c => char.IsLetter(c)).ToArray());

                           
                            if (tempPos + i + 1 < _tokens.Count)
                            {
                                Token next = _tokens[tempPos + i + 1];

                               
                                if (next.Code == CODE.SPACE)
                                {
                                    break;
                                }

                                if (next.Code == CODE.END)
                                {
                                    break;
                                }

              
                                if (next.Code == CODE.COMMA)
                                {
                                    break;
                                }

                               
                                if (next.Code == CODE.PUBLIC)
                                {
                                    break;
                                }

                              
                                if (next.Code == CODE.CLOSECURLYBRACE)
                                {
                                    break;
                                }

                           
                                if (next.Code == CODE.TYPE_ID)
                                {
                                    break;
                                }

                        
                                if (cleaned.Length >= 3 && cleaned.Length <= 6)
                                {
                                    if (cleaned.ToLower() == "string" || cleaned.ToLower() == "int" || cleaned.ToLower() == "float")
                                    {
                                       
                                        if (next.Code == CODE.IDENTIFIER)
                                        {
                                
                                            break;
                                        }
                                    }
                                }

                         
                                if (cleaned.Length > 6 && next.Code == CODE.IDENTIFIER)
                                {
                                  
                                    break;
                                }
                            }
                        }

                        string finalCleaned = new string(combined.Where(c => char.IsLetter(c)).ToArray());

                       
                        if (fragments.Count == 0)
                        {
                            GetNextToken();
                            state = ParserState.ExpectSemicolonOrComma;
                            return true;
                        }

                   
                        if (finalCleaned.ToLower() == "string" || finalCleaned.ToLower() == "int" || finalCleaned.ToLower() == "float")
                        {
                           
                            for (int j = 0; j < fragments.Count; j++)
                                GetNextToken();

                           
                            while (_current != null && _current.Code == CODE.SPACE)
                                GetNextToken();

                            state = ParserState.ExpectFieldName;
                            return true;
                        }
                        else
                        {
                          
                            for (int j = 0; j < fragments.Count; j++)
                                GetNextToken();

                         
                            while (_current != null && _current.Code == CODE.SPACE)
                                GetNextToken();

                            state = ParserState.ExpectFieldName;
                            return true;
                        }
                    }
                    else
                    {
                        state = ParserState.ExpectPublic;
                        return true;
                    }
                    break;

                case ParserState.ExpectFieldName:
                    while (_current != null && _current.Code == CODE.SPACE)
                        GetNextToken();

                    if (_current == null)
                    {
                        state = ParserState.Accept;
                        return true;
                    }

                    if (_current.Code == CODE.IDENTIFIER)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectSemicolonOrComma;
                        return true;
                    }
                    else if (_current.Code == CODE.TYPE_ID)
                    {
                      
                        state = ParserState.ExpectType;
                        return true;
                    }
                    else if (_current.Code == CODE.END)
                    {
                       
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectPublic;
                        return true;
                    }
                    else if (_current.Code == CODE.CLOSECURLYBRACE)
                    {
                        state = ParserState.ExpectCloseCurlyBrace;
                        return true;
                    }
                    else if (_current.Code == CODE.PUBLIC)
                    {
                        state = ParserState.ExpectPublic;
                        return true;
                    }
                    else
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        return true;
                    }
                case ParserState.ExpectSemicolonOrComma:
                    while (_current != null && _current.Code != CODE.END && _current.Code != CODE.COMMA &&
                           _current.Code != CODE.CLOSECURLYBRACE && _current.Code != CODE.PUBLIC && _current.Code != CODE.TYPE_ID)
                    {
                        if (_current.Code == CODE.IDENTIFIER)
                        {
                            
                            GetNextToken();
                            while (_current != null && _current.Code == CODE.SPACE)
                                GetNextToken();
                            continue;
                        }
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                    }

                    if (_current != null && _current.Code == CODE.END)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();
                        state = ParserState.ExpectPublic;
                        return true;
                    }
                    else if (_current != null && _current.Code == CODE.COMMA)
                    {
                        GetNextToken();
                        while (_current != null && _current.Code == CODE.SPACE)
                            GetNextToken();

                        if (_current != null && (_current.Code == CODE.END || _current.Code == CODE.CLOSECURLYBRACE || _current.Code == CODE.PUBLIC))
                        {
                            
                            state = ParserState.ExpectPublic;
                            return true;
                        }
                        else
                        {
                            state = ParserState.ExpectFieldName;
                            return true;
                        }
                    }
                    else if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                    {
                        state = ParserState.ExpectCloseCurlyBrace;
                        return true;
                    }
                    else if (_current != null && _current.Code == CODE.PUBLIC)
                    {
                        
                        state = ParserState.ExpectPublic;
                        return true;
                    }
                  
                    else
                    {
                        state = ParserState.ExpectPublic;
                        return true;
                    }

                case ParserState.ExpectSemicolon:
                    SkipUntil(CODE.END, CODE.CLOSECURLYBRACE);
                    if (_current != null && _current.Code == CODE.END)
                        state = ParserState.ExpectPublic;
                    else if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                        state = ParserState.ExpectSemicolonAfterClose;
                    else
                        return false;
                    break;


                case ParserState.ExpectCloseCurlyBrace:
                    SkipUntil(CODE.CLOSECURLYBRACE);
                    if (_current != null && _current.Code == CODE.CLOSECURLYBRACE)
                        state = ParserState.ExpectSemicolonAfterClose;
                    else
                        return false;
                    break;

                case ParserState.ExpectSemicolonAfterClose:
                    SkipUntil(CODE.END);
                    if (_current != null && _current.Code == CODE.END)
                        state = ParserState.Accept;
                    else
                        return false;
                    break;

                default:
                    if (_current != null)
                        GetNextToken();
                    return true;
            }

            if (_pos == startPos && _current != null)
            {
                GetNextToken();
            }

            return true;
        }

        private void SkipUntil(params CODE[] codes)
        {
            while (_current != null)
            {
                if (_current.Code == CODE.ERROR)
                {
                    
                    GetNextToken();
                    continue;
                }

                if (codes.Contains(_current.Code))
                    return;

                GetNextToken();
            }
        }

        private void AddStateError(ParserState state, Token token)
        {
            string found = token?.Value ?? "конец строки";
            string position = GetPosition(token);
            string expected = GetExpectedString(state);

            switch (state)
            {
                case ParserState.ExpectStruct:
                    AddError(found, position, "Строка должна начинаться с ключевого слова 'struct'", expected, found);
                    break;
                case ParserState.ExpectIdentifier:
                    AddError(found, position, "Ожидается идентификатор (имя структуры)", expected, found);
                    break;
                case ParserState.ExpectOpenCurlyBrace:
                    AddError(found, position, "Ожидается '{' после имени структуры", expected, found);
                    break;
                case ParserState.ExpectPublic:
                    AddError(found, position, "Ожидается 'public', или '}'", expected, found);
                    break;
                case ParserState.ExpectType:
                    AddError(found, position, "Ожидается тип (int, float, string) с именем поля", expected, found);
                    break;
                case ParserState.ExpectSemicolonOrComma:
                    AddError(found, position, "Ожидается ';' или ','", expected, found);
                    break;
                case ParserState.ExpectFieldName:
                    AddError(found, position, "Ожидается имя поля", expected, found);
                    break;
                case ParserState.ExpectSemicolon:
                    AddError(found, position, "Ожидается ';'", expected, found);
                    break;
                case ParserState.ExpectCloseCurlyBrace:
                    AddError(found, position, "Ожидается '}'", expected, found);
                    break;
                case ParserState.ExpectSemicolonAfterClose:
                    AddError(found, position, "Ожидается ';' после '}'", expected, found);
                    break;
            }
        }

        private List<Token> ConvertLexemesToTokens(List<Lexeme> lexemes)
        {
            var tokens = new List<Token>();

            for (int i = 0; i < lexemes.Count; i++)
            {
                var lex = lexemes[i];
                CODE code = CODE.ERROR;
                bool isError = false;
                string errorMessage = null;

                switch (lex.Code)
                {
                    case "1": code = CODE.IDENTIFIER; break;
                    case "3": code = CODE.PUBLIC; break;
                    case "4": code = CODE.STRING; break;
                    case "5": code = CODE.INT; break;
                    case "6": code = CODE.FLOAT; break;
                    case "2": code = CODE.STRUCT; break;
                    case "7": code = CODE.OPCURLYBRACE; break;
                    case "8": code = CODE.CLOSECURLYBRACE; break;
                    case "9": code = CODE.END; break;
                    case "10": code = CODE.COMMA; break;
                    case "11": code = CODE.SPACE; break;
                    case "12":
                        code = CODE.ERROR;
                        isError = true;
                        break;
                    default:
                        code = CODE.ERROR;
                        isError = true;
                        break;
                }

                if (code == CODE.SPACE) continue;

                if ((code == CODE.INT || code == CODE.FLOAT || code == CODE.STRING) && !isError)
                {
                    int nextIdx = i + 1;
                    while (nextIdx < lexemes.Count && lexemes[nextIdx].Code == "11") nextIdx++;

                    if (nextIdx < lexemes.Count && lexemes[nextIdx].Code == "1")
                    {
                        int line = 1, start = 1, end = 1;
                        ParsePosition(lex.Position, out line, out start, out end);
                        tokens.Add(new Token(CODE.TYPE_ID, lex.Value + " " + lexemes[nextIdx].Value, line, start, end, lex));
                        i = nextIdx;
                        continue;
                    }
                }

                int lineNum, startCol, endCol;
                ParsePosition(lex.Position, out lineNum, out startCol, out endCol);
                tokens.Add(new Token(code, lex.Value, lineNum, startCol, endCol, lex, isError, errorMessage));
            }

            return tokens;
        }

        private void ParsePosition(string pos, out int line, out int start, out int end)
        {
            line = 1; start = 1; end = 1;
            if (string.IsNullOrEmpty(pos)) return;
            try
            {
                var nums = new List<int>();
                string cur = "";
                foreach (char c in pos)
                {
                    if (char.IsDigit(c)) cur += c;
                    else if (!string.IsNullOrEmpty(cur))
                    {
                        if (int.TryParse(cur, out int n)) nums.Add(n);
                        cur = "";
                    }
                }
                if (!string.IsNullOrEmpty(cur) && int.TryParse(cur, out int l)) nums.Add(l);
                if (nums.Count >= 1) line = nums[0];
                if (nums.Count >= 2) start = nums[1];
                if (nums.Count >= 3) end = nums[2];
                else end = start;
            }
            catch { }
        }
    }
}