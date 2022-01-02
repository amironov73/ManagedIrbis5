// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* LvalueNode.cs -- lvalue
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;

using static AM.Scripting.Barsik.Grammar;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Lvalue, т. е. то, что может появиться слева от знака присваивания.
/// </summary>
internal sealed class Lvalue
    : Rvalue
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="topNode">Топовый, т. е. самый правый узел. </param>
    public Lvalue
        (
            AtomNode topNode
        )
    {
        _topNode = topNode;
    }

    #endregion

    #region Private members

    private readonly AtomNode _topNode;

    // цель присваивания
    internal static readonly Parser<char, AtomNode> Target = ExpressionParser.Build
        (
            Variable,
            new []
            {
                Operator.PostfixChainable
                    (
                        Try (MethodCall (_MethodCall)),
                        Try (Property (_Property)),
                        Try (Index (_Index))
                    )
            }
        );

    /// <summary>
    /// Присвоение значения.
    /// </summary>
    internal void Execute
        (
            Context context,
            string operation,
            dynamic? value
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (operation);

        _topNode.Assign (context, operation, value);
    }

    #endregion
}
