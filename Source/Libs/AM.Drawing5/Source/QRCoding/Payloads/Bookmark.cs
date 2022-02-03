// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Bookmark.cs -- закладка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using static AM.Drawing.QRCoding.PayloadGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Закладка.
/// </summary>
public sealed class Bookmark
    : Payload
{
    #region Construction

    /// <summary>
    /// Generates a bookmark payload. Scanned by an QR Code reader, this one creates a browser bookmark.
    /// </summary>
    /// <param name="url">Url of the bookmark</param>
    /// <param name="title">Title of the bookmark</param>
    public Bookmark
        (
            string url,
            string title
        )
    {
        _url = EscapeInput (url);
        _title = EscapeInput (title);
    }

    #endregion

    #region Private members

    private readonly string _url, _title;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"MEBKM:TITLE:{_title};URL:{_url};;";
    }

    #endregion
}
