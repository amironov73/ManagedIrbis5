// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Barsor.cs -- аналог Blazor, только для Barsik
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Аналог Blazor, только для Barsik.
/// </summary>
public sealed class BarsorParser
{
    #region Properties

    /// <summary>
    /// Признак начала директивы.
    /// </summary>
    public char DirectiveStart { get; set; } = '@';

    #endregion

    #region Private members

    /// <summary>
    /// Вызов <c>print</c> с указанным текстом.
    /// </summary>
    private StatementNode Print
        (
            string text
        )
    {
        var constant = new ConstantNode (text);

        return Print (constant);
    }

    /// <summary>
    /// Вызов <c>print</c> с указанным текстом.
    /// </summary>
    private StatementNode Print
        (
            AtomNode node
        )
    {
        var printCall = new FreeCallNode ("print", new [] { node });
        var expression = new ExpressionNode (printCall);

        return expression;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор шаблона на стейтменты.
    /// </summary>
    public ProgramNode ParseTemplate
        (
            string templateText
        )
    {
        Sure.NotNull (templateText);

        var statements = new List<StatementNode>();
        var navigator = new ValueTextNavigator (templateText);
        while (!navigator.IsEOF)
        {
            if (navigator.PeekChar() == DirectiveStart)
            {
                navigator.ReadChar();
                if (navigator.PeekChar() == '{')
                {
                    navigator.ReadChar();
                    var source = navigator.ReadUntil
                        (
                            "{",
                            "}",
                            "}"
                        );
                    navigator.ReadChar(); // доедаем закрывающий символ '}'
                    if (!source.IsEmpty)
                    {
                        var subProgram = Grammar.ParseProgram (source.ToString());
                        statements.AddRange (subProgram.Statements);
                    }
                }
                else if (navigator.PeekChar() == '@')
                {
                    var statement = Print ("@");
                    statements.Add (statement);
                }
                else
                {
                    var source = navigator.ReadUntil (' ', '\t', '\r', '\n', '<');
                    if (!source.IsEmpty)
                    {
                        var expression = Grammar.ParseExpression (source.ToString());
                        var statement = Print (expression);
                        statements.Add (statement);
                    }
                }
            }
            else
            {
                var line = navigator.ReadUntil (DirectiveStart);
                if (!line.IsEmpty)
                {
                    var statement = Print (line.ToString());
                    statements.Add (statement);
                }
            }
        }

        return new ProgramNode (statements);
    }

    #endregion
}
