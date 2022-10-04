// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AstNumber.cs -- числовой литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Числовой литерал.
/// </summary>
public sealed class AstNumber
    : AstValue
{
    #region Properties

    /// <summary>
    /// Числовое значение.
    /// </summary>
    public int Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AstNumber
        (
            Token token
        )
    {
        token.MustBe (TokenKind.NumericLiteral);
        Value = token.Text.ThrowIfNullOrEmpty().ParseInt32();
    }

    #endregion

    #region AstValue members

    /// <inheritdoc cref="AstValue.ComputeInt32"/>
    public override int ComputeInt32 (LanguageContext context) => Value;

    #endregion
}
