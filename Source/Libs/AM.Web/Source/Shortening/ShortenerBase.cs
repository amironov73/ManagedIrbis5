// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ShortenerBase.cs -- абстрактный сокращатель ссылок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Web.Shortening;

/// <summary>
/// Абстрактный сокращатель ссылок.
/// </summary>
public abstract class ShortenerBase
    : ILinkShortener
{
    #region Public methods

    /// <summary>
    /// Построение короткой ссылки по ее полной версии.
    /// </summary>
    public virtual string CreateShortLink 
        (
            string fullLink
        )
    {
        Sure.NotNullNorEmpty (fullLink);

        // в данный момент генерируемая короткая ссылка
        // никак не зависит от полной версии
        var bytes = BitConverter.GetBytes (DateTime.Now.ToOADate());
        var result = Convert.ToBase64String (bytes);

        // Используем изменённый Base64 для URL, где не используется
        // заполнение знаком '=' и символы '+' и '/' соответственно
        // заменяются на '*' и '-'.
        result = result.Replace ("=", string.Empty);
        result = result.Replace ('+', '*');
        result = result.Replace ('/', '~');

        return result;
    }

    #endregion
    
    #region ILinkShortener members

    /// <inheritdoc cref="ILinkShortener.ShortenLink"/>
    public abstract string ShortenLink
        (
            string fullLink,
            string? description
        );

    /// <inheritdoc cref="ILinkShortener.GetFullLink"/>
    public abstract string? GetFullLink
        (
            string shortLink,
            bool count
        );

    /// <inheritdoc cref="ILinkShortener.GetLinkData"/>
    public abstract LinkData? GetLinkData
        (
            string shortLink
        );

    /// <inheritdoc cref="ILinkShortener.InsertLink"/>
    public abstract int InsertLink
        (
            LinkData linkData
        );

    /// <inheritdoc cref="ILinkShortener.DeleteLink"/>
    public abstract void DeleteLink
        (
            string shortLink
        );

    /// <inheritdoc cref="ILinkShortener.GetAllLinks"/>
    public abstract LinkData[] GetAllLinks();

    #endregion
}
