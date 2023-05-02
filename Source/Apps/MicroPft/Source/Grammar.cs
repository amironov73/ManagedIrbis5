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
using System.Globalization;

using AM;
using AM.Kotik;
using AM.Kotik.Parsers;
using AM.Kotik.Tokenizers;

using MicroPft.Ast;
using MicroPft.Tokenizers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace MicroPft;

/// <summary>
/// Облегченная PFT-грамматика.
/// </summary>
internal static class Grammar
{
    #region Private members

    private static Parser<string> Term (params string[] expected) => new TermParser (expected);

    // запятая
    private static readonly Parser<PftNode> Comma = new TermParser (new[] { "," })
        .Map (_ => (PftNode) new CommaNode())
        .Labeled ("Comma");

    // команда вывода поля
    private static readonly Parser<PftNode> FieldCmd =
        new SimplestParser<PftNode> ("field")
            .Labeled ("Field");

    // безусловный литерал
    private static readonly Parser<PftNode> Unconditional =
        new SimplestParser<PftNode> ("unconditional")
        .Labeled ("Unconditional");

    // условный литерал
    private static readonly Parser<PftNode> Conditional =
        new SimplestParser<PftNode> ("conditional")
        .Labeled ("Conditional");

    // повторяющийся литерал
    private static readonly Parser<PftNode> Repeating =
        new SimplestParser<PftNode> ("repeating")
        .Labeled ("Repeating");

    // начало группы
    private static readonly Parser<PftNode> OpenGroup = Term ("(").Map (_ => PftNode.OpenGroup)
        .Labeled ("Open");

    // конец группы
    private static readonly Parser<PftNode> CloseGroup = Term (")").Map (_ => PftNode.CloseGroup)
        .Labeled ("Close");

    // перевод строки
    private static readonly Parser<PftNode> NewLine = Term ("#", "/", "%")
        .Map (x => (PftNode)new NewLineNode (x[0]))
        .Labeled ("NewLine");

    // команда смены режима `mpl`
    private static readonly Parser<PftNode> ModeCommand = new SimplestParser<string> ("mode")
        .Map (x => (PftNode)new ModeNode
            (
                char.ToLowerInvariant (x[1]),
                x[2].SameChar ('u')
            ))
        .Labeled ("Mode");

    // команда позиционирования
    private static readonly Parser<PftNode> CommandC = new SimplestParser<string> ("c")
        .Map (x => (PftNode)new CNode (int.Parse (x[1..], CultureInfo.InvariantCulture)))
        .Labeled ("Position");

    // команда вывода пробелов
    private static readonly Parser<PftNode> CommandX = new SimplestParser<string> ("x")
        .Map (x => (PftNode)new XNode (int.Parse (x[1..], CultureInfo.InvariantCulture)))
        .Labeled ("Indent");

    private static readonly Parser<PftNode> Expr = Parser.OneOf
        (
            FieldCmd,
            Comma,
            Unconditional,
            Conditional,
            Repeating,
            CommandC,
            CommandX,
            ModeCommand,
            NewLine,
            OpenGroup,
            CloseGroup
        );

    // скрипт в целом
    private static readonly Parser<IList<PftNode>> Program =
        Expr.Repeated().End();

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

        var logger = Magna.Logger;

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

            if (node is ConditionalNode conditional)
            {
                if (currentField is null)
                {
                    // разбираем левую часть
                    conditional.LeftHand = true;
                    stack.Add (conditional);
                }
                else
                {
                    // разбираем правую часть
                    currentField.RightHand.Add (node);
                }
                continue;
            }

            if (node is RepeatingNode repeating)
            {
                if (currentField is null)
                {
                    // разбираем левую часть
                    repeating.LeftHand = true;
                    stack.Add (node);
                }
                else
                {
                    // разбираем правую часть
                    currentField.RightHand.Add (node);
                }
                continue;
            }

            if (node is CNode or XNode)
            {
                // может быть только слева
                stack.Add (node);
                continue;
            }

            if (node is ModeNode or NewLineNode)
            {
                if (group is not null)
                {
                    group.Items.Add (node);
                }
                else
                {
                    result.Add (node);
                }
            }

            if (ReferenceEquals (node, PftNode.OpenGroup))
            {
                if (group is not null)
                {
                    // вложенная группа
                    logger.LogError ("Nested group detected");
                    throw new ApplicationException ("Nested group detected");
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
                    logger.LogError ("Closing an unopened group");
                    throw new ApplicationException ("Closing an unopened group");
                }

                group = null;
            }
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор PFT-скрипта.
    /// </summary>
    public static IEnumerable<PftNode> Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var settings = new TokenizerSettings
        {
            KnownTerms = new [] { "(", ")", ",", "#", "/", "%" }
        };
        var tokeninzer = new Tokenizer (settings)
        {
            Tokenizers =
            {
                new WhitespaceTokenizer(),
                new FieldTokenizer(),
                new RegexTokenizer ("c", "^[Cc]\\d+$"),
                new RegexTokenizer ("conditional", "$^[|][^|]*?[|]$"),
                new RegexTokenizer ("mode", "^[Mm][DdHhPp][UuLl]$"),
                new RegexTokenizer ("x", "^[Xx]\\d+$"),
                new RepeatingNodeTokenizer(),
                new UnconditionalTokenizer(),
                new ConditionalTokenizer(),
                new TermTokenizer()
            }
        };
        var tokens = tokeninzer.Tokenize (text);
        var state = new ParseState (tokens);
        var parsed = Program.ParseOrThrow (state);
        var result = BuildProgram (parsed);

        return result;
    }

    #endregion
}
