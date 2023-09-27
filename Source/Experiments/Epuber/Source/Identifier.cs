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

/// <summary>
/// Идентификатор.
/// </summary>
public sealed class Identifier
{
    #region Properties

    /// <summary>
    /// Идентификатор идентификатора.
    /// </summary>
    [JsonPropertyName ("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Схема.
    /// </summary>
    [JsonPropertyName ("scheme")]
    public string? Scheme { get; set; }

    /// <summary>
    /// Значение.
    /// </summary>
    [JsonPropertyName ("value")]
    public string? Value { get; set; }

    #endregion
}
