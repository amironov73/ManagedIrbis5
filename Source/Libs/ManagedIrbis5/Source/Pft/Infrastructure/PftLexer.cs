// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftLexer.cs -- лексический анализатор для PFT
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Лексический анализатор для PFT.
/// </summary>
public class PftLexer
{
    #region Private members

    private TextNavigator? _navigator;

    private static readonly char[] Identifier =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
        'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
        'y', 'z',

        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
        'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
        'Y', 'Z',

        'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к',
        'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц',
        'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я',

        'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К',
        'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц',
        'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я',

        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_'
    };

    private int Column => _navigator?.Column ?? 0;

    private bool IsEOF => _navigator?.IsEOF ?? true;

    private int Line => _navigator?.Line ?? 0;

    private char PeekChar() => _navigator?.PeekCharNoCrLf() ?? '\0';

    private char ReadChar() => _navigator?.ReadCharNoCrLf() ?? '\0';

    private FieldSpecification? ReadField()
    {
        var result = new FieldSpecification();
        var position = _navigator!.SavePosition();
        _navigator.Move (-1);

        if (!result.Parse (_navigator))
        {
            _navigator.RestorePosition (position);
            return null;
        }

        return result;
    }

    private string? ReadIdentifier()
    {
        if (IsEOF)
        {
            return null;
        }

        var result = new StringBuilder();
        var reserved = PftUtility.GetReservedWords();

        while (true)
        {
            var c = PeekChar();
            if (c == TextNavigator.EOF
                || Array.IndexOf (Identifier, c) < 0)
            {
                break;
            }

            result.Append (c);
            ReadChar();
            c = _navigator!.PeekChar();
            if ((c == '\r' || c == '\n')
                && Array.IndexOf (reserved, result.ToString()) >= 0)
            {
                break;
            }
        }

        return result.ToString();
    }

    private string ReadIdentifier
        (
            char initialLetter
        )
    {
        if (IsEOF)
        {
            return initialLetter.ToString();
        }

        var result = new StringBuilder();
        result.Append (initialLetter);
        var reserved = PftUtility.GetReservedWords();

        while (true)
        {
            var c = PeekChar();
            if (c == TextNavigator.EOF
                || Array.IndexOf (Identifier, c) < 0)
            {
                break;
            }

            result.Append (c);
            ReadChar();
            c = _navigator!.PeekChar();
            if ((c == '\r' || c == '\n')
                && Array.IndexOf (reserved, result.ToString()) >= 0)
            {
                break;
            }
        }

        return result.ToString();
    }

    private string? ReadInteger()
    {
        var result = new StringBuilder();

        var c = PeekChar();
        if (!c.IsArabicDigit())
        {
            return null;
        }

        result.Append (c);
        ReadChar();

        while (true)
        {
            c = PeekChar();
            if (!c.IsArabicDigit())
            {
                break;
            }

            result.Append (c);
            ReadChar();
        }

        return result.ToString();
    }

    private string? ReadFloat()
    {
        var result = new StringBuilder();

        var dotFound = false;
        var digitFound = false;

        var c = PeekChar();

        //if (c != '+'
        //    && c != '-'
        //    && c != '.'
        //    && !c.IsArabicDigit())
        //{
        //    return null;
        //}
        if (c == '.')
        {
            dotFound = true;
        }

        if (c.IsArabicDigit())
        {
            digitFound = true;
        }

        result.Append (c);
        ReadChar();

        while (true)
        {
            c = PeekChar();
            if (!c.IsArabicDigit())
            {
                break;
            }

            digitFound = true;
            result.Append (c);
            ReadChar();
        }

        if (!dotFound && c == '.')
        {
            result.Append (c);
            ReadChar();

            while (true)
            {
                c = PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                digitFound = true;
                result.Append (c);
                ReadChar();
            }
        }

        if (!digitFound)
        {
            ThrowSyntax();
        }

        if (c == 'E' || c == 'e')
        {
            result.Append (c);
            ReadChar();
            digitFound = false;
            c = PeekChar();

            if (c == '+' || c == '-')
            {
                result.Append (c);
                ReadChar();
                c = PeekChar();
            }

            while (true)
            {
                c = PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                digitFound = true;
                result.Append (c);
                ReadChar();
            }

            if (!digitFound)
            {
                ThrowSyntax();
            }
        }

        return result.ToString();
    }

    private string ReadTo
        (
            char stop
        )
    {
        var result = _navigator?.ReadUntilNoCrLf (stop);
        if (ReferenceEquals (result, null))
        {
            ThrowSyntax();
        }

        var c = ReadChar();
        if (c != stop)
        {
            ThrowSyntax();
        }

        return result!;
    }

    private void SkipWhitespace() => _navigator?.SkipWhitespace();

    private void ThrowSyntax()
    {
        var message = $"Syntax error at line {Line}, column {Column}";
        Magna.Logger.LogError
            (
                nameof (PftLexer) + "::" + nameof (ThrowSyntax)
                + ": {Message}",
                message
            );

        throw new PftSyntaxException (message);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Tokenize the text.
    /// </summary>
    public PftTokenList Tokenize
        (
            string text
        )
    {
        var result = new List<PftToken>();
        _navigator = new TextNavigator (text);

        while (!IsEOF)
        {
            SkipWhitespace();
            if (IsEOF)
            {
                break;
            }

            var line = Line;
            var column = Column;
            var c = ReadChar();
            char c2, c3;
            string? value = null;
            FieldSpecification? field = null;
            PftTokenKind kind;
            switch (c)
            {
                case '\'':
                    value = ReadTo ('\'');
                    kind = PftTokenKind.UnconditionalLiteral;
                    break;

                case '"':
                    value = ReadTo ('"');
                    kind = PftTokenKind.ConditionalLiteral;
                    break;

                case '|':
                    value = ReadTo ('|');
                    kind = PftTokenKind.RepeatableLiteral;
                    break;

                case '!':
                    c2 = PeekChar();
                    if (c2 == '=')
                    {
                        kind = PftTokenKind.NotEqual2;
                        value = "!=";
                        ReadChar();
                        if (PeekChar() == '=')
                        {
                            ReadChar();
                            value = "!==";
                        }
                    }
                    else if (c2 == '~')
                    {
                        kind = PftTokenKind.NotEqual2;
                        value = "!~";
                        ReadChar();
                        if (PeekChar() == '~')
                        {
                            ReadChar();
                            value = "!~~";
                        }
                    }
                    else
                    {
                        kind = PftTokenKind.Bang;
                        value = c.ToString();
                    }

                    break;

                case ':':
                    kind = PftTokenKind.Colon;
                    value = c.ToString();
                    if (PeekChar() == ':')
                    {
                        ReadChar();
                        value = "::";
                    }

                    break;

                case ';':
                    kind = PftTokenKind.Semicolon;
                    value = c.ToString();
                    break;

                case ',':
                    kind = PftTokenKind.Comma;
                    value = c.ToString();
                    break;

                case '\\':
                    kind = PftTokenKind.Backslash;
                    value = c.ToString();
                    break;

                case '=':
                    kind = PftTokenKind.Equals;
                    value = c.ToString();
                    if (PeekChar() == '=')
                    {
                        ReadChar();
                        value = "==";
                    }

                    break;

                case '#':
                    kind = PftTokenKind.Hash;
                    value = c.ToString();
                    break;

                case '%':
                    kind = PftTokenKind.Percent;
                    value = c.ToString();
                    break;

                case '{':
                    c2 = PeekChar();
                    if (c2 == '{')
                    {
                        c3 = _navigator.LookAhead (1);
                        if (c3 == '{')
                        {
                            ReadChar();
                            ReadChar();
                            kind = PftTokenKind.TripleCurly;
                            value = _navigator.ReadTo ("}}}").ToString();
                            if (string.IsNullOrEmpty (value))
                            {
                                ThrowSyntax();
                            }
                        }
                        else
                        {
                            kind = PftTokenKind.LeftCurly;
                            value = c.ToString();
                        }
                    }
                    else
                    {
                        kind = PftTokenKind.LeftCurly;
                        value = c.ToString();
                    }

                    break;

                case '[':
                    c2 = PeekChar();
                    c3 = _navigator.LookAhead (1);
                    if (c2 == '[' && c3 == '[')
                    {
                        ReadChar();
                        ReadChar();
                        kind = PftTokenKind.EatOpen;
                        value = "[[[";
                    }
                    else
                    {
                        kind = PftTokenKind.LeftSquare;
                        value = c.ToString();
                    }

                    break;

                case '(':
                    kind = PftTokenKind.LeftParenthesis;
                    value = c.ToString();
                    break;

                case '}':
                    kind = PftTokenKind.RightCurly;
                    value = c.ToString();
                    break;

                case ']':
                    c2 = PeekChar();
                    c3 = _navigator.LookAhead (1);
                    if (c2 == ']' && c3 == ']')
                    {
                        ReadChar();
                        ReadChar();
                        kind = PftTokenKind.EatClose;
                        value = "]]]";
                    }
                    else
                    {
                        kind = PftTokenKind.RightSquare;
                        value = c.ToString();
                    }

                    break;

                case ')':
                    kind = PftTokenKind.RightParenthesis;
                    value = c.ToString();
                    break;

                case '+':
                    kind = PftTokenKind.Plus;
                    value = c.ToString();
                    break;

                case '-':
                    kind = PftTokenKind.Minus;
                    value = c.ToString();
                    break;

                case '*':
                    kind = PftTokenKind.Star;
                    value = c.ToString();
                    break;

                case '~':
                    kind = PftTokenKind.Tilda;
                    value = c.ToString();
                    if (PeekChar() == '~')
                    {
                        ReadChar();
                        value = "~~";
                    }

                    break;

                case '_':
                    kind = PftTokenKind.Underscore;
                    value = c.ToString();
                    break;

                case '`':
                    kind = PftTokenKind.GraveAccent;
                    value = ReadTo ('`');
                    break;

                case '«':
                case '»':
                case '‘':
                case '’':
                case '“':
                case '”':
                case '‹':
                case '›':
                case '–':
                case '—':
                case '‰':
                case '¦':
                case '§':
                case '±':
                    throw new NotImplementedException();

                case '?':
                    kind = PftTokenKind.Question;
                    value = c.ToString();
                    break;

                case '/':
                    c2 = PeekChar();
                    if (c2 == '*')
                    {
                        ReadChar();
                        value = _navigator.ReadUntil ('\r', '\n').ToString();
                        kind = PftTokenKind.Comment;
                    }
                    else
                    {
                        kind = PftTokenKind.Slash;
                        value = c.ToString();
                    }

                    break;

                case '<':
                    c2 = PeekChar();
                    if (c2 == '=')
                    {
                        kind = PftTokenKind.LessEqual;
                        value = "<=";
                        ReadChar();
                    }
                    else if (c2 == '<')
                    {
                        c3 = _navigator.LookAhead (1);
                        if (c3 == '<')
                        {
                            ReadChar();
                            ReadChar();
                            kind = PftTokenKind.TripleLess;
                            value = _navigator.ReadTo (">>>").ToString();
                            if (string.IsNullOrEmpty (value))
                            {
                                ThrowSyntax();
                            }
                        }
                        else
                        {
                            kind = PftTokenKind.Less;
                            value = c.ToString();
                        }
                    }
                    else if (c2 == '>')
                    {
                        kind = PftTokenKind.NotEqual1;
                        value = "<>";
                        ReadChar();
                    }
                    else
                    {
                        kind = PftTokenKind.Less;
                        value = c.ToString();
                    }

                    break;

                case '>':
                    c2 = PeekChar();
                    if (c2 == '=')
                    {
                        kind = PftTokenKind.MoreEqual;
                        value = ">=";
                        ReadChar();
                    }
                    else
                    {
                        kind = PftTokenKind.More;
                        value = c.ToString();
                    }

                    break;

                case '&':
                    value = ReadIdentifier();
                    if (string.IsNullOrEmpty (value))
                    {
                        ThrowSyntax();
                    }

                    kind = PftTokenKind.Unifor;
                    break;

                case '$':
                    value = ReadIdentifier();
                    if (string.IsNullOrEmpty (value))
                    {
                        ThrowSyntax();
                    }

                    kind = PftTokenKind.Variable;
                    break;

                case '@':
                    value = ReadIdentifier();
                    if (string.IsNullOrEmpty (value))
                    {
                        ThrowSyntax();
                    }

                    kind = PftTokenKind.At;
                    break;

                case '^':
                    kind = PftTokenKind.Hat;
                    value = "^";
                    break;

                case '\x1C':
                case '\u221F':
                    value = _navigator.ReadUntil ('\x1D', '\u2194').ToString();
                    if (string.IsNullOrEmpty (value)
                        || !ReadChar().IsOneOf ('\x1D', '\u2194'))
                    {
                        ThrowSyntax();
                    }

                    kind = PftTokenKind.At;
                    break;

                case 'a':
                case 'A':
                    value = ReadIdentifier (c);
                    if (value.Length != 1)
                    {
                        goto default;
                    }

                    kind = PftTokenKind.A;
                    value = "a";
                    break;

                case 'c':
                case 'C':
                    value = ReadInteger();
                    if (!string.IsNullOrEmpty (value))
                    {
                        kind = PftTokenKind.C;
                        break;
                    }

                    value = ReadIdentifier (c);
                    goto default;

                case 'd':
                case 'D':
                    field = ReadField();
                    if (ReferenceEquals (field, null))
                    {
                        goto default;
                    }

                    value = field.RawText;
                    kind = PftTokenKind.V;
                    break;

                case 'f':
                case 'F':
                    value = ReadIdentifier (c);
                    if (value.Length != 1)
                    {
                        goto default;
                    }

                    kind = PftTokenKind.F;
                    value = "f";
                    break;

                case 'g':
                case 'G':
                    field = ReadField();
                    if (ReferenceEquals (field, null))
                    {
                        goto default;
                    }

                    value = field.RawText;
                    kind = PftTokenKind.V;
                    break;

                case 'l':
                case 'L':
                    value = ReadIdentifier (c);
                    if (value.Length != 1)
                    {
                        goto default;
                    }

                    kind = PftTokenKind.L;
                    value = "l";
                    break;

                case 'm':
                case 'M':
                    value = ReadIdentifier (c);
                    var value2 = value.ToLower();
                    if (value2.Length == 3)
                    {
                        if (value2 == "mfn")
                        {
                            var builder = new StringBuilder();

                            if (PeekChar() == '(')
                            {
                                builder.Append ('(');
                                ReadChar();

                                var ok = false;
                                while (!IsEOF)
                                {
                                    c3 = PeekChar();
                                    builder.Append (c3);
                                    ReadChar();
                                    if (c3 == ')')
                                    {
                                        ok = true;
                                        break;
                                    }
                                }

                                if (!ok)
                                {
                                    ThrowSyntax();
                                }
                            }

                            kind = PftTokenKind.Mfn;
                            value = "mfn" + builder;
                            break;
                        }

                        if ((value2[1] == 'h'
                             || value2[1] == 'd'
                             || value2[1] == 'p')
                            && (value2[2] == 'l'
                                || value2[2] == 'u'))
                        {
                            kind = PftTokenKind.Mpl;
                            break;
                        }
                    }

                    goto default;

                case 'n':
                case 'N':
                    field = ReadField();
                    if (ReferenceEquals (field, null))
                    {
                        goto default;
                    }

                    value = field.RawText;
                    kind = PftTokenKind.V;
                    break;

                case 'p':
                case 'P':
                    value = ReadIdentifier (c);
                    if (value.Length != 1)
                    {
                        goto default;
                    }

                    kind = PftTokenKind.P;
                    value = "p";
                    break;

                case 's':
                case 'S':
                    value = ReadIdentifier (c);
                    if (value.Length != 1)
                    {
                        goto default;
                    }

                    kind = PftTokenKind.S;
                    value = "s";
                    break;

                case 'v':
                case 'V':
                    field = ReadField();
                    if (ReferenceEquals (field, null))
                    {
                        goto default;
                    }

                    value = field.RawText;
                    kind = PftTokenKind.V;
                    break;

                case 'x':
                case 'X':
                    value = ReadInteger();
                    if (!string.IsNullOrEmpty (value))
                    {
                        kind = PftTokenKind.X;
                        break;
                    }

                    value = ReadIdentifier (c);
                    goto default;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '.':
                    _navigator.Move (-1);
                    value = ReadFloat();
                    kind = PftTokenKind.Number;
                    break;

                default:
                    if (string.IsNullOrEmpty (value))
                    {
                        _navigator.Move (-1);
                        value = ReadIdentifier();
                    }

                    if (string.IsNullOrEmpty (value))
                    {
                        ThrowSyntax();
                    }

                    switch (value.ThrowIfNull().ToLower())
                    {
                        case "abs":
                            kind = PftTokenKind.Abs;
                            value = "abs";
                            break;

                        case "all":
                            kind = PftTokenKind.All;
                            value = "all";
                            break;

                        case "and":
                        case "и":
                            kind = PftTokenKind.And;
                            value = "and";
                            break;

                        case "any":
                            kind = PftTokenKind.Any;
                            value = "any";
                            break;

                        case "blank":
                            kind = PftTokenKind.Blank;
                            value = "blank";
                            break;

                        case "break":
                            kind = PftTokenKind.Break;
                            value = "break";
                            break;

                        case "ceil":
                            kind = PftTokenKind.Ceil;
                            value = "ceil";
                            break;

                        case "cseval":
                            kind = PftTokenKind.CsEval;
                            value = "cseval";
                            break;

                        case "div":
                            kind = PftTokenKind.Div;
                            value = "div";
                            break;

                        case "do":
                            kind = PftTokenKind.Do;
                            value = "do";
                            break;

                        case "else":
                        case "иначе":
                            kind = PftTokenKind.Else;
                            value = "else";
                            break;

                        case "empty":
                            kind = PftTokenKind.Empty;
                            value = "empty";
                            break;

                        case "end":
                        case "конец":
                            kind = PftTokenKind.End;
                            value = "end";
                            break;

                        case "eval":
                            kind = PftTokenKind.Eval;
                            value = "eval";
                            break;

                        case "fmt":
                            kind = PftTokenKind.Fmt;
                            value = "fmt";
                            break;

                        case "false":
                        case "ложь":
                            kind = PftTokenKind.False;
                            value = "false";
                            break;

                        case "fi":
                        case "все":
                        case "всё":
                        case "илсе":
                        case "фи":
                            kind = PftTokenKind.Fi;
                            value = "fi";
                            break;

                        case "first":
                            kind = PftTokenKind.First;
                            value = "first";
                            break;

                        case "floor":
                            kind = PftTokenKind.Floor;
                            value = "floor";
                            break;

                        case "for":
                        case "для":
                            kind = PftTokenKind.For;
                            value = "for";
                            break;

                        case "foreach":
                            kind = PftTokenKind.ForEach;
                            value = "foreach";
                            break;

                        case "frac":
                            kind = PftTokenKind.Frac;
                            value = "frac";
                            break;

                        case "from":
                            kind = PftTokenKind.From;
                            value = "from";
                            break;

                        case "global":
                            kind = PftTokenKind.Global;
                            value = "global";
                            break;

                        case "have":
                            kind = PftTokenKind.Have;
                            value = "have";
                            break;

                        case "if":
                        case "если":
                            kind = PftTokenKind.If;
                            value = "if";
                            break;

                        case "in":
                            kind = PftTokenKind.In;
                            value = "in";
                            break;

                        case "last":
                            kind = PftTokenKind.Last;
                            value = "last";
                            break;

                        case "local":
                            kind = PftTokenKind.Local;
                            value = "local";
                            break;

                        case "nl":
                            kind = PftTokenKind.Nl;
                            value = "nl";
                            break;

                        case "not":
                        case "не":
                            kind = PftTokenKind.Not;
                            value = "not";
                            break;

                        case "or":
                        case "или":
                            kind = PftTokenKind.Or;
                            value = "or";
                            break;

                        case "order":
                            kind = PftTokenKind.Order;
                            value = "order";
                            break;

                        case "parallel":
                        case "параллельно":
                            kind = PftTokenKind.Parallel;
                            value = "parallel";
                            break;

                        case "pow":
                            kind = PftTokenKind.Pow;
                            value = "pow";
                            break;

                        case "proc":
                            kind = PftTokenKind.Proc;
                            value = "proc";
                            break;

                        case "ravr":
                            kind = PftTokenKind.Ravr;
                            value = "ravr";
                            break;

                        case "ref":
                            kind = PftTokenKind.Ref;
                            value = "ref";
                            break;

                        case "rmax":
                            kind = PftTokenKind.Rmax;
                            value = "rmax";
                            break;

                        case "rmin":
                            kind = PftTokenKind.Rmin;
                            value = "rmin";
                            break;

                        case "round":
                            kind = PftTokenKind.Round;
                            value = "round";
                            break;

                        case "rsum":
                            kind = PftTokenKind.Rsum;
                            value = "rsum";
                            break;

                        case "select":
                        case "выбор":
                            kind = PftTokenKind.Select;
                            value = "select";
                            break;

                        case "sign":
                            kind = PftTokenKind.Sign;
                            value = "sign";
                            break;

                        case "then":
                        case "то":
                        case "тогда":
                            kind = PftTokenKind.Then;
                            value = "then";
                            break;

                        case "true":
                        case "истина":
                            kind = PftTokenKind.True;
                            value = "true";
                            break;

                        case "trunc":
                            kind = PftTokenKind.Trunc;
                            value = "trunc";
                            break;

                        case "val":
                            kind = PftTokenKind.Val;
                            value = "val";
                            break;

                        case "where":
                            kind = PftTokenKind.Where;
                            value = "where";
                            break;

                        case "while":
                        case "пока":
                            kind = PftTokenKind.While;
                            value = "while";
                            break;

                        case "with":
                            kind = PftTokenKind.With;
                            value = "with";
                            break;

                        default:
                            kind = PftTokenKind.Identifier;
                            break;
                    }

                    break;
            }

            if (kind == PftTokenKind.None) //-V3022
            {
                ThrowSyntax();
            }

            var token = new PftToken (kind, line, column, value);
            if (kind == PftTokenKind.V)
            {
                token.UserData = field;
            }

            result.Add (token);
        }

        return new PftTokenList (result.ToArray());
    }

    #endregion
}
