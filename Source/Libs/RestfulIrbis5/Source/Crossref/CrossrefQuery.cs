// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CrossrefQuery.cs -- запрос Crossref.org
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using RestSharp;

#endregion

namespace RestfulIrbis.Crossref;

/// <summary>
/// Запрос Crossref.org.
/// </summary>
[PublicAPI]
public sealed class CrossrefQuery
{
    #region Properties

    /// <summary>
    /// Автор.
    /// </summary>
    public string? Author { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Кодирование запроса.
    /// </summary>
    public void Encode
        (
            RestRequest request
        )
    {
        Sure.NotNull (request);

        if (!string.IsNullOrEmpty (Author))
        {
            request.AddParameter ("query.author", Author);
        }

        // TODO implement
    }

    #endregion
}
