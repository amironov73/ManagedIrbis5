// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsnToken.cs -- единичный токен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Asn1;

/// <summary>
/// Единичный токен.
/// </summary>
public sealed class AsnToken
{
    #region Properties

    /// <summary>
    /// Column.
    /// </summary>
    public int Column;

    /// <summary>
    /// Token kind.
    /// </summary>
    public AsnTokenKind Kind;

    /// <summary>
    /// Line number.
    /// </summary>
    public int Line;

    /// <summary>
    /// Token text.
    /// </summary>
    public string? Text;

    /// <summary>
    /// Arbitrary user data.
    /// </summary>
    public object? UserData;

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnToken()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnToken
        (
            AsnTokenKind kind,
            int line,
            int column,
            string text
        )
    {
        Kind = kind;
        Column = column;
        Line = line;
        Text = text;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Требует, чтобы токен был только указанного вида.
    /// </summary>
    public AsnToken MustBe
        (
            AsnTokenKind kind
        )
    {
        Sure.Defined (kind);

        if (Kind != kind)
        {
            Magna.Logger.LogError
                (
                    nameof (AsnToken) + "::" + nameof (MustBe)
                    + ": expecting {Expected}, got {Actual}",
                    kind,
                    Kind
                );

            throw new AsnSyntaxException();
        }

        return this;
    }

    #endregion
}
