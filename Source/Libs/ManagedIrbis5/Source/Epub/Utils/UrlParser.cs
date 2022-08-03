// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UrlParser.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Internal;

/// <summary>
///
/// </summary>
internal class UrlParser
{
    /// <summary>
    ///
    /// </summary>
    public UrlParser
        (
            string? url
        )
    {
        if (url == null)
        {
            Path = null;
            Anchor = null;
        }
        else
        {
            var anchorCharIndex = url.IndexOf ('#');
            if (anchorCharIndex == -1)
            {
                Path = url;
                Anchor = null;
            }
            else
            {
                Path = url.Substring (0, anchorCharIndex);
                Anchor = url.Substring (anchorCharIndex + 1);
            }
        }
    }

    public string? Path { get; }
    public string? Anchor { get; }
}
