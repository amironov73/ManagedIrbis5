// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeNotNullable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Local

using System;
using System.Collections.Generic;
using System.Text;

using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#nullable enable

namespace ParsingExperiments;

static class PftExperimentOne
{
    abstract class PftNode
    {
    }

    sealed class OpenGroupNode
        : PftNode
    {
        public override string ToString()
        {
            return "open group";
        }
    }

    sealed class CloseGroupNode
        : PftNode
    {
        public override string ToString()
        {
            return "close group";
        }
    }

    // просто запятая
    sealed class CommaNode
        : PftNode
    {
        public override string ToString()
        {
            return "comma";
        }
    }

    // перевод строки
    sealed class NewLineNode
        : PftNode
    {
        private readonly char _command;

        public NewLineNode
            (
                char command
            )
        {
            _command = command;
        }

        public override string ToString()
        {
            return $"new line {_command}";
        }
    }

    sealed class GroupNode
        : PftNode
    {
        public List<PftNode> Items { get; } = new ();

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append ("<group<");
            var first = true;
            foreach (var item in Items)
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                builder.Append (item);
                first = false;
            }
            builder.Append (">>");

            return builder.ToString();
        }
    }

    // команда вывода поля
    sealed class FieldNode
        : PftNode
    {
        public List<PftNode> LeftHand { get; }
        public List<PftNode> RightHand { get; }

        public char Command { get; }

        public int Tag { get; }

        public char Code { get; }

        public int Offset { get; }

        public int Length { get; }

        public FieldNode
            (
                char command,
                int tag,
                char code = '\0',
                int offset = 0,
                int length = 0
            )
        {
            LeftHand = new ();
            RightHand = new ();
            Command = command;
            Tag = tag;
            Code = code;
            Offset = offset;
            Length = length;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append ("field: ");
            result.Append (Command);
            result.Append ('_');
            result.Append (Tag);

            if (Code != '\0')
            {
                result.Append ('_');
                result.Append ('^');
                result.Append (Code);
            }

            if (Offset > 0)
            {
                result.Append ('_');
                result.Append ('*');
                result.Append (Offset);
            }

            if (Length > 0)
            {
                result.Append ('_');
                result.Append ('.');
                result.Append (Length);
            }

            if (LeftHand.Count != 0)
            {
                result.Append (" [left:");
                foreach (var node in LeftHand)
                {
                    result.Append (' ');
                    result.Append (node);
                }
                result.Append (']');
            }

            if (RightHand.Count != 0)
            {
                result.Append (" [right:");
                foreach (var node in RightHand)
                {
                    result.Append (' ');
                    result.Append (node);
                }
                result.Append (']');
            }

            return result.ToString();
        }
    }

    // безусловный литерал
    sealed class UnconditionalNode
        : PftNode
    {
        private readonly string _value;

        public UnconditionalNode
            (
                string value
            )
        {
            _value = value;
        }

        public override string ToString()
        {
            return $"unconditional: \"{_value}\"";
        }
    }

    // условный литерал
    sealed class ConditionalNode
        : PftNode
    {
        private readonly string _value;

        public ConditionalNode
            (
                string value
            )
        {
            _value = value;
        }

        public override string ToString()
        {
            return $"conditional: \"{_value}\"";
        }
    }

    // повторяющийся литерал
    sealed class RepeatingNode
        : PftNode
    {
        private readonly string _value;
        private readonly bool _plus;

        public RepeatingNode
            (
                string value,
                bool plus
            )
        {
            _value = value;
            _plus = plus;
        }

        public override string ToString()
        {
            return $"repeating: \"{_value}\" {_plus}";
        }
    }

    sealed class CNode
        : PftNode
    {
        private readonly int _position;

        public CNode
            (
                int position
            )
        {
            _position = position;
        }

        public override string ToString()
        {
            return $"C_{_position}";
        }
    }

    sealed class XNode
        : PftNode
    {
        private readonly int _length;

        public XNode
            (
                int length
            )
        {
            _length = length;
        }

        public override string ToString()
        {
            return $"X_{_length}";
        }
    }

    sealed class ModeNode
        : PftNode
    {
        private readonly char _mode;
        private readonly bool _upper;

        public ModeNode
            (
                char mode,
                bool upper
            )
        {
            _mode = mode;
            _upper = upper;
        }

        public override string ToString()
        {
            return $"mode_{_mode}_{_upper}";
        }
    }

    /// <summary>
    /// Разбирает конструкцию <c>"v123^4*5.6"</c>
    /// </summary>
    sealed class FieldParser
        : Parser<char, FieldNode>
    {
        #region Parser members

        public override bool TryParse
            (
                ref ParseState<char> state,
                ref PooledList<Expected<char>> expecteds,
                out FieldNode result
            )
        {
            result = null!;

            if (!state.HasCurrent)
            {
                return false;
            }

            var chr = char.ToLowerInvariant (state.ReadChar());
            if (chr != 'v' && chr != 'd' && chr != 'n')
            {
                return false;
            }

            var command = chr;
            var tag = 0;
            while (state.HasCurrent)
            {
                chr = state.Current;
                if (!char.IsDigit (chr))
                {
                    break;
                }

                tag = tag * 10 + chr - '0';
                state.Advance();
            }

            if (tag == 0)
            {
                return false;
            }

            var code = '\0';
            if (chr == '^')
            {
                state.Advance();
                if (!state.HasCurrent)
                {
                    return false;
                }

                code = state.ReadChar();
                chr = state.ReadChar();
            }

            var offset = 0;
            if (chr == '*')
            {
                if (!state.HasCurrent)
                {
                    return false;
                }

                state.Advance();
                while (state.HasCurrent)
                {
                    chr = state.Current;
                    if (!char.IsDigit (chr))
                    {
                        break;
                    }

                    offset = offset * 10 + chr - '0';
                    state.Advance();
                }

                if (offset == 0)
                {
                    return false;
                }
            }

            var length = 0;
            if (chr == '.')
            {
                if (!state.HasCurrent)
                {
                    return false;
                }

                state.Advance();
                while (state.HasCurrent)
                {
                    chr = state.Current;
                    if (!char.IsDigit (chr))
                    {
                        break;
                    }

                    length = length * 10 + chr - '0';
                    state.Advance();
                }

                if (length == 0)
                {
                    return false;
                }
            }

            if (chr == '*')
            {
                if (offset != 0 || !state.HasCurrent)
                {
                    return false;
                }

                state.Advance();
                while (state.HasCurrent)
                {
                    chr = state.Current;
                    if (!char.IsDigit (chr))
                    {
                        break;
                    }

                    offset = offset * 10 + chr - '0';
                    state.Advance();
                }

                if (offset == 0)
                {
                    return false;
                }
            }

            result = new FieldNode (command, tag, code, offset, length);

            return true;
        }

        #endregion
    }

    sealed class RepeatingParser :
        Parser<char, RepeatingNode>
    {
        public override bool TryParse
            (
                ref ParseState<char> state,
                ref PooledList<Expected<char>> expecteds,
                out RepeatingNode result
            )
        {
            result = null!;

            if (!state.HasCurrent)
            {
                return false;
            }

            while (state.HasCurrent)
            {
                if (!char.IsWhiteSpace (state.Current))
                {
                    break;
                }

                state.Advance();
            }

            if (!state.HasCurrent)
            {
                return false;
            }

            var plus = false;
            if (state.Current == '+')
            {
                plus = true;
                state.Advance();
            }

            if (!state.HasCurrent)
            {
                return false;
            }

            while (state.HasCurrent)
            {
                if (!char.IsWhiteSpace (state.Current))
                {
                    break;
                }

                state.Advance();
            }

            if (state.Current != '|')
            {
                return false;
            }

            state.Advance();

            var value = new StringBuilder();
            while (state.HasCurrent)
            {
                if (state.Current == '|')
                {
                    break;
                }

                value.Append (state.Current);
                state.Advance();
            }

            if (!state.HasCurrent)
            {
                return false;
            }

            if (state.Current != '|')
            {
                return false;
            }

            state.Advance();

            while (state.HasCurrent)
            {
                if (!char.IsWhiteSpace (state.Current))
                {
                    break;
                }

                state.Advance();
            }

            if (state.HasCurrent)
            {
                if (state.Current == '+')
                {
                    state.Advance();
                    plus = true;
                }
            }

            result = new RepeatingNode (value.ToString(), plus);

            return true;
        }
    }

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (SkipWhitespaces);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    // команда вывода поля
    private static readonly Parser<char, PftNode> FieldCmd = Tok (new FieldParser())
        .Select<PftNode> (v => v);

    // запятая
    private static readonly Parser<char, PftNode> Comma = Tok (',')
        .ThenReturn ((PftNode) new CommaNode());

    // безусловный литерал
    private static Parser<char, PftNode> Unconditional =>
        Tok (AnyCharExcept ('\'').ManyString()).Between (Char ('\''))
            .Select<PftNode> (v => new UnconditionalNode (v));

    // условный литерал
    private static Parser<char, PftNode> Conditional =>
        Tok (AnyCharExcept ('"').ManyString()).Between (Char ('"'))
            .Select<PftNode> (v => new ConditionalNode (v));

    // повторяющийся литерал
    private static Parser<char, PftNode> Repeating => new RepeatingParser()
        .Select<PftNode> (v => v);

    // команда позиционирования
    private static Parser<char, PftNode> CmdC =>
        CIChar ('c').Then (Num).Select<PftNode> (v => new CNode (v));

    // команда вывода пробелов
    private static Parser<char, PftNode> CmdX =>
        CIChar ('x').Then (Num).Select<PftNode> (v => new XNode (v));

    // команда смена режимов
    private static Parser<char, PftNode> ModeCmd => Tok (Map
        (
            (_, mode, upper) => (PftNode) new ModeNode (char.ToLowerInvariant (mode),
                char.ToLowerInvariant (upper) == 'u'),
            CIChar ('m'),
            CIOneOf ('p', 'h', 'd'),
            CIOneOf ('l', 'u')
        ));

    // перевод строки
    private static Parser<char, PftNode> NewLine => CIOneOf ('#', '/', '%')
        .Select<PftNode> (v => new NewLineNode (v));

    // начало группы
    private static Parser<char, PftNode> OpenGroup => Char ('(')
        .Select<PftNode> (_ => new OpenGroupNode());

    // конец группы
    private static Parser<char, PftNode> CloseGroup => Char (')')
        .Select<PftNode> (_ => new CloseGroupNode());

    private static readonly Parser<char, PftNode> Expr = OneOf
        (
            Try (FieldCmd),
            Try (Comma),
            Try (Unconditional),
            Try (Conditional),
            Try (Repeating),
            Try (CmdC),
            Try (CmdX),
            Try (ModeCmd),
            Try (NewLine),
            Try (OpenGroup),
            Try (CloseGroup)
        );

    private static readonly Parser<char, IEnumerable<PftNode>> ManyExpr =
        Expr.SeparatedAndOptionallyTerminated (SkipWhitespaces);

    /// <summary>
    /// Пересобираем последовательность, привязывая литералы
    /// к командам вывода.
    /// </summary>
    private static List<PftNode> Rebuild (IEnumerable<PftNode> nodes)
    {
        // алгоритм таков: в результирующем списке могут быть лишь:
        // группа, команда вывода поля и безусловный литерал

        var result = new List<PftNode>();
        GroupNode? group = null;
        var stack = new List<PftNode>(); // сюда помещаем левую часть
        FieldNode? currentField = null;
        foreach (var node in nodes)
        {
            if (node is FieldNode field)
            {
                if (group is not null)
                {
                    group.Items.Add (node);
                }
                else
                {
                    result.Add (node);
                }

                field.LeftHand.AddRange (stack);
                stack.Clear();
                currentField = field.Command == 'v'
                    ? field // команда вывода реального поля
                    : null; // фиктивное поле
                continue;
            }

            if (node is CommaNode)
            {
                stack.Clear();
                currentField = null;
                continue;
            }

            if (node is UnconditionalNode)
            {
                if (group is not null)
                {
                    group.Items.Add (node);
                }
                else
                {
                    result.Add (node);
                }
                continue;
            }

            if (node is ConditionalNode)
            {
                if (currentField is null)
                {
                    // разбираем левую часть
                    stack.Add (node);
                }
                else
                {
                    // разбираем правую часть
                    currentField.RightHand.Add (node);
                }
                continue;
            }

            if (node is RepeatingNode)
            {
                if (currentField is null)
                {
                    // разбираем левую часть
                    stack.Add (node);
                }
                else
                {
                    // разбираем правую часть
                    currentField.RightHand.Add (node);
                }
                continue;
            }

            if (node is CNode or XNode or ModeNode or NewLineNode)
            {
                // может быть только слева
                stack.Add (node);
                continue;
            }

            if (node is OpenGroupNode)
            {
                if (group is not null)
                {
                    // вложенная группа
                    throw new Exception();
                }

                group = new GroupNode();
                result.Add (group);
                continue;
            }

            if (node is CloseGroupNode)
            {
                if (group is null)
                {
                    // группа еще не открыта
                    throw new Exception();
                }

                group = null;
            }
        }

        return result;
    }

    private static void ParsePft
        (
            string text
        )
    {
        Console.WriteLine (text);
        Console.WriteLine (new string ('.', 60));

        try
        {
            var parser = ManyExpr.Before (End);
            var result = Rebuild (parser.ParseOrThrow (text));
            foreach (var node in result)
            {
                Console.WriteLine (node);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }

        Console.WriteLine (new string ('=', 60));
    }

    public static void DoExperiments()
    {
        ParsePft ("v123");
        ParsePft ("v123^a");
        ParsePft ("v123*45");
        ParsePft ("v123.67");
        ParsePft ("v123*45.67");
        ParsePft ("v123.45*67");
        ParsePft ("v123^x.45*67");

        ParsePft ("'literal'");
        ParsePft ("'l1', 'l2',");
        ParsePft ("'l1' 'l2'");

        ParsePft ("'l1' v1 v2");

        ParsePft ("|r1| v1 |r2| v2");
        ParsePft ("|r1| v1, |r2| v2");
        ParsePft ("|r1|v1 |r2|v2|r3|");
        ParsePft ("\"c1\" v1");
        ParsePft ("\"c1\" |r1| v1");

        ParsePft ("\"c1\" d1 \"c2\" d2 \"c3\"");

        ParsePft ("c4 v1");
        ParsePft ("x4 v1");

        ParsePft ("mpl");
        ParsePft ("MHU");

        ParsePft ("(v1 v2 v3) v4");
    }
}
