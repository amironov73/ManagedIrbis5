// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Grammar.cs -- облегченная PFT-грамматика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Облегченная PFT-грамматика.
/// </summary>
internal static class Grammar
{
    #region Private members

    /// <summary>
    /// Разбор PFT-скрипта.
    /// </summary>
    internal static IEnumerable<PftNode> Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var parsed = Program.ParseOrThrow (text);
        var result = BuildProgram (parsed);

        return result;
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
        .ThenReturn (PftNode.Comma);

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
    private static Parser<char, PftNode> NewLine => OneOf ('#', '/', '%')
        .Select<PftNode> (v => new NewLineNode (v));

    // начало группы
    private static Parser<char, PftNode> OpenGroup => Char ('(').ThenReturn (PftNode.OpenGroup);

    // конец группы
    private static Parser<char, PftNode> CloseGroup => Char (')').ThenReturn (PftNode.CloseGroup);

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

    private static readonly Parser<char, IEnumerable<PftNode>> Program =
        Expr.SeparatedAndOptionallyTerminated (SkipWhitespaces).Before (End);

    /// <summary>
    /// Пересобираем последовательность, привязывая литералы
    /// к командам вывода.
    /// </summary>
    private static List<PftNode> BuildProgram
        (
            IEnumerable<PftNode> nodes
        )
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

            if (ReferenceEquals (node, PftNode.Comma))
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

            if (ReferenceEquals (node, PftNode.OpenGroup))
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

            if (ReferenceEquals (node, PftNode.CloseGroup))
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

    #endregion
}
