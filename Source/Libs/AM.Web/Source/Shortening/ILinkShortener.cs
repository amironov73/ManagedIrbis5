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

/* ILinkShortener.cs -- интерфейс сокращателя ссылок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Web.Shortening;

/// <summary>
/// Интерфейс сокращателя ссылок.
/// </summary>
public interface ILinkShortener
{
    /// <summary>
    /// Сокращение указанной ссылки.
    /// </summary>
    string ShortenLink (string fullLink, string? description);

    /// <summary>
    /// Получение полной ссылки по указанному короткому варианту.
    /// </summary>
    string? GetFullLink (string shortLink, bool count);

    /// <summary>
    /// Получение полной информации о ссылке
    /// по указанному короткому варианту.
    /// </summary>
    LinkData? GetLinkData (string shortLink);

    /// <summary>
    /// Добавление ссылки в базу.
    /// </summary>
    int InsertLink (LinkData linkData);
    
    /// <summary>
    /// Удаление указанной ссылки.
    /// </summary>
    void DeleteLink (string shortLink);

    /// <summary>
    /// Получение сведений обо всех ссылках.
    /// </summary>
    LinkData[] GetAllLinks();
}
