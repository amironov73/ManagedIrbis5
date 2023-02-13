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

/* InMemoryShortener.cs -- сокращатель ссылок, работающий в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM.Json;

#endregion

#nullable enable

namespace AM.Web.Shortening;

/// <summary>
/// Сокращатель ссылок, работающий в оперативной памяти.
/// Предназначен для тестирования, не для продакшена.
/// </summary>
public sealed class InMemoryShortener
    : ShortenerBase
{
    #region Private members

    /// <summary>
    /// Да, простой список недостаточно эффективен,
    /// но для целей "замокать реальную реализацию"
    /// его вполне достаточно.
    /// </summary>
    private readonly List<LinkData> _linkData = new ();

    /// <summary>
    /// Следующий доступный идентификатор.
    /// </summary>
    private int _nextId;

    /// <summary>
    /// Глобальный экземпляр.
    /// </summary>
    private static InMemoryShortener? _instance;
    
    private LinkData? _FindShort (string shortLink) =>
        _linkData.FirstOrDefault (row => row.ShortLink == shortLink);
    
    private LinkData? _FindFull (string fullLink) =>
        _linkData.FirstOrDefault (row => row.FullLink == fullLink);

    #endregion

    #region Public methods

    /// <summary>
    /// Получение глобального экземпляра, возможно,
    /// с загрузкой состояния из указанного файла.
    /// </summary>
    public static InMemoryShortener GetInstance
        (
            string? fileName
        )
    {
        if (_instance is not null)
        {
            return _instance;
        }

        _instance = new InMemoryShortener();
        if (!string.IsNullOrEmpty (fileName))
        {
            var data = JsonUtility.ReadObjectFromFile<LinkData[]> (fileName);
            _instance._linkData.AddRange (data);
            if (data.Length != 0)
            {
                _instance._nextId = data.Max (row => row.Id) + 1;
            }
        }

        return _instance;
    }

    /// <summary>
    /// Сохранение состояния в указанный файл.
    /// </summary>
    public void SaveState
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);
        
        JsonUtility.SaveObjectToFile (_linkData.ToArray(), fileName);
    }

    #endregion
    
    #region ILinkShortener members

    /// <inheritdoc cref="ShortenerBase.ShortenLink"/>
    public override string ShortenLink
        (
            string fullLink,
            string? description
        )
    {
        Sure.NotNullNorEmpty (fullLink);

        var linkData = _FindFull (fullLink);
        if (linkData is null)
        {
            linkData = new LinkData
            {
                Id = _nextId++,
                Moment = DateTime.Now,
                FullLink = fullLink,
                ShortLink = CreateShortLink (fullLink),
                Description = description,
                Counter = 0
            };

            _linkData.Add (linkData);
        }

        return linkData.ShortLink.ThrowIfNullOrEmpty();
    }

    /// <inheritdoc cref="GetFullLink"/>
    public override string? GetFullLink
        (
            string shortLink, 
            bool count
        )
    {
        Sure.NotNullNorEmpty (shortLink);
        
        var linkData = _FindShort (shortLink);
        if (linkData is null)
        {
            return null;
        }

        if (count)
        {
            linkData.Counter++;
        }

        return linkData.FullLink.ThrowIfNullOrEmpty();
    }

    /// <inheritdoc cref="ShortenerBase.GetLinkData"/>
    public override LinkData? GetLinkData
        (
            string shortLink
        )
    {
        Sure.NotNullNorEmpty (shortLink);

        return _FindShort (shortLink);
    }

    /// <inheritdoc cref="ShortenerBase.InsertLink"/>
    public override int InsertLink
        (
            LinkData linkData
        )
    {
        Sure.NotNull (linkData);

        linkData.Id = _nextId++;
        _linkData.Add (linkData);

        return linkData.Id;
    }

    /// <inheritdoc cref="ShortenerBase.DeleteLink"/>
    public override void DeleteLink
        (
            string shortLink
        )
    {
        Sure.NotNullNorEmpty (shortLink);

        var found = _FindShort (shortLink);
        if (found is not null)
        {
            _linkData.Remove (found);
        }
    }

    /// <inheritdoc cref="ShortenerBase.GetAllLinks"/>
    public override LinkData[] GetAllLinks()
    {
        return _linkData.ToArray();
    }

    #endregion
}
