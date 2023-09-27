// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

using SharpCompress.Archives.Zip;
using SharpCompress.Readers.Zip;

#endregion

namespace Epuber;

internal sealed class DublinCore
{
    #region Properties

    /// <summary>
    /// Идентификатор, например, ISBN.
    /// </summary>
    [JsonPropertyName ("identifiers")]
    public Identifier[]? Identifiers { get; set; }

    /// <summary>
    /// Заглавие.
    /// </summary>
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Автор.
    /// </summary>
    [JsonPropertyName ("creators")]
    public string[]? Creators { get; set; }

    /// <summary>
    /// Издатель.
    /// </summary>
    [JsonPropertyName ("publisher")]
    public string? Publisher { get; set; }

    /// <summary>
    /// Дата издания.
    /// </summary>
    [JsonPropertyName ("date")]
    public string? Date { get; set; }

    /// <summary>
    /// Язык.
    /// </summary>
    [JsonPropertyName ("language")]
    public string? Language { get; set; }

    /// <summary>
    /// Формат, например, объем.
    /// </summary>
    [JsonPropertyName ("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Аннотация.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Рубрики.
    /// </summary>
    [JsonPropertyName ("subjects")]
    public string[]? Subjects { get; set; }

    /// <summary>
    /// Источник.
    /// </summary>
    [JsonPropertyName ("source")]
    public string? Source { get; set; }

    #endregion
}
