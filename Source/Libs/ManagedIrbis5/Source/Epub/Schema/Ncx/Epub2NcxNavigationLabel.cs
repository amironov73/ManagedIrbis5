// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubNcxNavigationLabel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class Epub2NcxNavigationLabel
{

    /// <summary>
    ///
    /// </summary>
    public string? Text { get; set; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Text.ToVisibleString();
    }
}
