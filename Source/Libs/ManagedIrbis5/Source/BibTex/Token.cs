// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Token.cs -- BibTex-токен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

namespace ManagedIrbis.BibTex;

/// <summary>
/// BibTex-токен.
/// </summary>
public sealed class Token
{
    #region Properties

    /// <summary>
    /// Тип токена.
    /// </summary>
    public TokenKind Kind { get; }

    /// <summary>
    /// Значение токена.
    /// </summary>
    public string? Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Token
        (
            TokenKind kind,
            string? value = null
        )
    {
        Kind = kind;
        Value = value;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Требование, чтобы токен был определенного типа.
    /// </summary>
    public void MustBe
        (
            TokenKind kind
        )
    {
        if (Kind != kind)
        {
            throw new IrbisException();
        }
    }

    /// <summary>
    /// Требование, чтобы токен был определенного типа.
    /// </summary>
    public void MustBe
        (
            TokenKind firstKind,
            TokenKind secondKind
        )
    {
        if (Kind != firstKind && Kind != secondKind)
        {
            throw new IrbisException();
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Kind}: {Value.ToVisibleString()}";
    }

    #endregion
}
