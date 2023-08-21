// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* RequestUtility.cs -- полезные методы расширения для RestRequest
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.CompilerServices;

using RestSharp;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Полезные методы расширения для <see cref="RestRequest"/>.
/// </summary>
public static class RequestUtility
{
    #region Public methods

    /// <summary>
    /// Добавление параметра в URL при условии, что переданное значение
    /// не дефолтное.
    /// </summary>
    public static RestRequest AddNonDefaultQueryParameter<TValue>
        (
            this RestRequest request,
            TValue value,
            [CallerArgumentExpression ("value")]
            string? name = default
        )
    {
        Sure.NotNull (request);

        if (!Equals (value, default (TValue)))
        {
            request.AddQueryParameter (name!, value.ToInvariantString());
        }

        return request;
    }

    #endregion
}
