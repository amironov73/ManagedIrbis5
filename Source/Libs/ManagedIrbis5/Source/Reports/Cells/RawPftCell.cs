// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RawPftCell.cs -- ячейка, выводящая отформатированный PFT в "сыром" виде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Reports;

/// <summary>
/// Ячейка, выводящая отформатированный PFT в "сыром" виде.
/// </summary>
public sealed class RawPftCell
    : PftCell
{
    #region PftCell members

    /// <inheritdoc cref="PftCell.Render" />
    public override void Render
        (
            ReportContext context
        )
    {
        Sure.NotNull (context);

        Magna.Logger.LogTrace (nameof (RawPftCell) + "::" + nameof (Render));

        var text = Text;

        if (string.IsNullOrEmpty (text))
        {
            // TODO: Skip or not on empty format?

            return;
        }

        var driver = context.Driver;
        var formatted = Compute (context);
        driver.BeginCell (context, this);
        if (!string.IsNullOrEmpty (formatted))
        {
            context.Output.Write (formatted);
        }

        driver.EndCell (context, this);
    }

    #endregion
}
