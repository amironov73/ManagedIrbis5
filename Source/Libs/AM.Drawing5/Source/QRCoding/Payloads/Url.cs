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

/* Url.cs -- URL
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// URL.
/// </summary>
public sealed class Url
    : Payload
{
    #region Construction

    /// <summary>
    /// Generates a link. If not given, http/https protocol will be added.
    /// </summary>
    /// <param name="url">Link url target</param>
    public Url
        (
            string url
        )
    {
        Sure.NotNullNorEmpty (url);

        _url = url;
    }

    #endregion

    #region Private members

    private readonly string _url;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return (!_url.StartsWith ("http") ? "http://" + _url : _url);
    }

    #endregion
}
