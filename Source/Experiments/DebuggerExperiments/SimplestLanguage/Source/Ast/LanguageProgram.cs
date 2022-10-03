// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* LanguageProgram.cs -- программа на языке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AM;

#endregion

#nullable enable

/// <summary>
/// Программа на языке.
/// </summary>
public sealed class LanguageProgram
    : AstNode
{
    #region Properties

    /// <summary>
    /// Последовательность операторов, которые необходимо выполнить.
    /// </summary>
    public List<AstNode> Statements { get; } = new ();

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.Execute"/>
    public override void Execute
        (
            LanguageContext context
        )
    {
        Sure.NotNull (context);

        foreach (var statement in Statements)
        {
            statement.Execute (context);
        }
    }

    #endregion
}
