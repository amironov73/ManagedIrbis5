// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* PlainFormatDriver.cs -- тривиальный драйвер для украшения расформатированного текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Formatting;

/// <summary>
/// Тривиальный драйвер для украшения расформатированного текста.
/// </summary>
public sealed class PlainFormatDriver
    : IFormatDriver
{
    #region IFormatDriver members

    /// <inheritdoc cref="IFormatDriver.Bold"/>
    public void Bold
        (
            StringBuilder builder,
            string? text
        )
    {
        Sure.NotNull (builder);

        builder.Append (text);
    }

    /// <inheritdoc cref="IFormatDriver.Italic"/>
    public void Italic
        (
            StringBuilder builder,
            string? text
        )
    {
        Sure.NotNull (builder);

        builder.Append (text);
    }

    /// <inheritdoc cref="IFormatDriver.Underline"/>
    public void Underline
        (
            StringBuilder builder,
            string? text
        )
    {
        Sure.NotNull (builder);

        builder.Append (text);
    }

    /// <inheritdoc cref="IFormatDriver.Link"/>
    public void Link
        (
            StringBuilder builder,
            string? link,
            string? text
        )
    {
        Sure.NotNull (builder);

        builder.AppendWithSeparator (" ", text, link);
    }

    #endregion
}
