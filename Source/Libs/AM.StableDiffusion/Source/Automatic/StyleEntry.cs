// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StyleEntry.cs -- запись в файле styles.txt
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Запись в файле <c>styles.txt</c>.
/// </summary>
[PublicAPI]
public sealed class StyleEntry
{
    #region Properties

    /// <summary>
    /// Наименование стиля.
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Позитивная подсказка.
    /// </summary>
    [JsonPropertyName ("positive")]
    public string? Positive { get; set; }

    /// <summary>
    /// Негативная подсказка.
    /// </summary>
    [JsonPropertyName ("negative")]
    public string? Negative { get; set; }

    #endregion

    #region Private members

    private string ReadOne
        (
            ref ValueTextNavigator navigator
        )
    {
        if (!navigator.SkipWhitespace())
        {
            return string.Empty;
        }

        string result;
        if (navigator.PeekChar() == '"')
        {
            navigator.ReadChar(); // считываем кавычку
            result = navigator.ReadUntil ('"').Trim().ToString();
            navigator.ReadChar(); // считываем кавычку
            navigator.ReadTo (',');
        }
        else
        {
            result = navigator.ReadUntil (',').Trim().ToString();
        }

        // считываем запятую
        navigator.ReadChar();

        return result;
    }

    /// <summary>
    /// Вывод в поток одного фрагмента.
    /// При необходимости он включается в кавычки.
    /// </summary>
    private static void WriteOne
        (
            TextWriter textWriter,
            string text
        )
    {
        if (text.Contains (','))
        {
            textWriter.Write ('"');
            textWriter.Write (text);
            textWriter.Write ('"');
        }
        else
        {
            textWriter.Write (text);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор строки.
    /// </summary>
    public bool ParseLine
        (
            string line
        )
    {
        Sure.NotNull (line);

        if (string.IsNullOrWhiteSpace (line))
        {
            return false;
        }

        var navigator = new ValueTextNavigator (line);
        Name = ReadOne (ref navigator);
        Positive = ReadOne (ref navigator);
        Negative = ReadOne (ref navigator);

        return true;
    }

    /// <summary>
    /// Разбор текстового файла.
    /// </summary>
    public static List<StyleEntry> ParseFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var streamReader = File.OpenText (fileName);
        return ParseStream (streamReader);
    }

    /// <summary>
    /// Разбор текстового потока.
    /// </summary>
    public static List<StyleEntry> ParseStream
        (
            TextReader textReader
        )
    {
        Sure.NotNull (textReader);

        var result = new List<StyleEntry>();
        while (textReader.ReadLine() is { } line)
        {
            var entry = new StyleEntry();
            if (entry.ParseLine (line))
            {
                result.Add (entry);
            }
        }

        return result;
    }

    /// <summary>
    /// Вывод элемента в текстовый поток.
    /// </summary>
    public void WriteTo
        (
            TextWriter textWriter
        )
    {
        Sure.NotNull (textWriter);

        WriteOne (textWriter, Name.ThrowIfNullOrEmpty());
        WriteOne (textWriter, Positive ?? string.Empty);
        WriteOne (textWriter, Negative ?? string.Empty);
        textWriter.WriteLine();
    }

    /// <summary>
    /// Сохранение стилей в текстовом файле.
    /// </summary>
    public static void WriteTo
        (
            string fileName,
            IEnumerable<StyleEntry> entries
        )
    {
        Sure.NotNullNorEmpty (fileName);
        Sure.NotNull (entries);

        using var textWriter = File.CreateText (fileName);
        foreach (var entry in entries)
        {
            entry.WriteTo (textWriter);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Name.ToVisibleString();

    #endregion
}
