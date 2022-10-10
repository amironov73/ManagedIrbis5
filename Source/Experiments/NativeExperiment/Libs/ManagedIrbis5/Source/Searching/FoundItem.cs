// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FoundLine.cs -- одна строка в ответе на поисковый запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Одна строка в ответе сервера на поисковый запрос.
/// </summary>
[XmlRoot ("item")]
[DebuggerDisplay ("{Mfn} {Text}")]
public sealed class FoundItem
    : IEquatable<FoundItem>,
    IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Текстовая часть (может отсутствовать,
    /// если не запрашивалось форматирование).
    /// </summary>
    [XmlAttribute ("text")]
    [JsonPropertyName ("text")]
    public string? Text { get; set; }

    /// <summary>
    /// MFN найденной записи.
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    public int Mfn { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Выбирает только MFN из найденных записей.
    /// </summary>
    public static int[] ToMfn
        (
            FoundItem[]? found
        )
    {
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<int>();
        }

        var result = new int[found.Length];
        for (var i = 0; i < found.Length; i++)
        {
            result[i] = found[i].Mfn;
        }

        return result;
    }

    /// <summary>
    /// Выбирает только текст из найденных записей.
    /// </summary>
    public static string?[] ToText
        (
            FoundItem[]? found
        )
    {
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<string>();
        }

        var result = new string?[found.Length];
        for (var i = 0; i < found.Length; i++)
        {
            result[i] = found[i].Text;
        }

        return result;
    }

    /// <summary>
    /// Разбор ответа сервера.
    /// </summary>
    public static FoundItem[] Parse
        (
            Response response
        )
    {
        var expected = response.ReadInteger();
        var result = new List<FoundItem> (expected);
        while (!response.EOT)
        {
            var line = response.ReadUtf();
            if (string.IsNullOrEmpty (line))
            {
                break;
            }

            var parts = line.Split ('#', 2);
            var item = new FoundItem
            {
                Mfn = parts[0].ParseInt32(),
                Text = parts.Length == 2 ? parts[1] : string.Empty
            };
            result.Add (item);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Разбор ответа сервера.
    /// </summary>
    public static int[] ParseMfn
        (
            Response response
        )
    {
        var expected = response.ReadInteger();
        var result = new List<int> (expected);
        while (!response.EOT)
        {
            var line = response.ReadAnsi();
            if (string.IsNullOrEmpty (line))
            {
                break;
            }

            var parts = line.Split ('#', 2);
            var mfn = int.Parse (parts[0]);
            result.Add (mfn);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Загружаем найденные записи с сервера.
    /// </summary>
    public static FoundItem[] Read
        (
            ISyncProvider connection,
            string format,
            IEnumerable<int> found
        )
    {
        var array = found.ToArray();
        var length = array.Length;
        var result = new FoundItem[length];
        var formatted = connection.FormatRecords (array, format);
        if (formatted is null)
        {
            return Array.Empty<FoundItem>();
        }

        for (var i = 0; i < length; i++)
        {
            var item = new FoundItem
            {
                Mfn = array[i],
                Text = formatted[i]
            };
            result[i] = item;
        }

        return result;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Mfn = reader.ReadPackedInt32();
        Text = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WritePackedInt32 (Mfn)
            .WriteNullable (Text);
    }

    #endregion

    #region IEquatable<T> members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
    public bool Equals
        (
            FoundItem? other
        )
    {
        return Mfn == other?.Mfn;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<FoundItem> (this, throwOnError);

        verifier
            .Assert (Mfn > 0)
            .NotNullNorEmpty (Text);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"[{Mfn}] {Text.ToVisibleString()}";
    }

    #endregion
}
