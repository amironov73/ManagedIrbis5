// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* IHttpClientPool.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net.Http;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// A pool implementation for <see cref="HttpClient"/> pooling.
/// </summary>
public interface IHttpClientPool
{
    /// <summary>
    /// Gets a client for the specified <see cref="IRequestBuilder"/>.
    /// </summary>
    /// <param name="builder">The builder to get a client for.</param>
    /// <returns>A <see cref="HttpClient"/> from the pool.</returns>
    HttpClient Get(IRequestBuilder builder);

    /// <summary>
    /// Clears the pool, in case you need to.
    /// </summary>
    void Clear();
}
