// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AstVariableReference.cs -- ссылка на переменную по ее имени
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Ссылка на переменную по ее имени.
/// </summary>
public sealed class AstVariableReference
    : AstValue
{
    #region Properties

    /// <summary>
    /// Имя переменной.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AstVariableReference
        (
            Token token
        )
    {
        token.MustBe (TokenKind.Identifier);
        Name = token.Text.ThrowIfNullOrEmpty();
    }

    #endregion

    #region AstValue members

    /// <inheritdoc cref="AstValue.ComputeInt32"/>
    public override int ComputeInt32 (LanguageContext context) =>
        Convert.ToInt32 (context.RequireVariable (Name).Value);

    #endregion
}
