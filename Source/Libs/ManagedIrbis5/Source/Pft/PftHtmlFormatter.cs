// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftHtmlFormatter.cs -- умеет форматировать PFT в HTML
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft;

/// <summary>
/// Умеет форматировать PFT в HTML.
/// </summary>
public class PftHtmlFormatter
    : PftFormatter
{
    #region Properties

    /// <summary>
    /// Разделитель текста.
    /// </summary>
    public PftTextSeparator Separator { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftHtmlFormatter()
    {
        Separator = new PftTextSeparator();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftHtmlFormatter
        (
            PftContext context
        )
        : base (context)
    {
        Separator = new PftTextSeparator();
    }

    #endregion

    #region PftFormatter members

    /// <inheritdoc cref="PftFormatter.ParseProgram" />
    public override void ParseProgram
        (
            string source
        )
    {
        if (Separator.SeparateText (source))
        {
            Magna.Logger.LogError
                (
                    nameof (PftHtmlFormatter) + "::" + nameof (ParseProgram)
                    + ": can't separate text"
                );

            throw new PftSyntaxException();
        }

        var prepared = Separator.Accumulator;

        base.ParseProgram (prepared);
    }

    #endregion
}
