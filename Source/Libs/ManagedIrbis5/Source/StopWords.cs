// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* StopWords.cs -- обертка над списком стоп-слов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis;

//
// STW file example:
//
// A
// ABOUT
// AFTER
// AGAINST
// ALL
// ALS
// AN
// AND
// AS
// AT
// AUF
// AUS
// AUX
// B
// BIJ
// BY
//

/// <summary>
/// Обертка над списком стоп-слов. STW-файлы.
/// </summary>
public sealed class StopWords
{
    #region Properties

    /// <summary>
    /// File name (for identification only).
    /// </summary>
    public string? FileName { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public StopWords()
    {
        _dictionary = new CaseInsensitiveDictionary<object?>();
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="StopWords"/> class.
    /// </summary>
    /// <param name="fileName">The name.</param>
    public StopWords
        (
            string? fileName
        )
        : this()
    {
        FileName = fileName;
    }

    #endregion

    #region Private members

    private readonly CaseInsensitiveDictionary<object?> _dictionary;

    private static readonly string[] _standardStopWords =
    {
        "a", "about", "after", "against", "all", "als", "an", "and",
        "as", "at", "auf", "aus", "aux", "b", "by", "c", "d", "da",
        "dans", "das", "de", "der", "des", "di", "die", "do", "du",
        "e", "ein", "eine", "einen", "eines", "einer", "el", "en",
        "et", "f", "for", "from", "fur", "g", "h", "i", "ihr", "ihre",
        "im", "in", "into", "its", "j", "k", "l", "la", "las", "le",
        "les", "los", "m", "mit", "mot", "n", "near", "non", "not",
        "o", "of", "on", "or", "over", "out", "p", "par", "para",
        "qui", "r", "s", "some", "sur", "t", "the", "their", "through",
        "till", "to", "u", "uber", "und", "under", "upon", "used",
        "using", "v", "van", "w", "when", "with", "x", "y", "your",
        "z", "а", "ая", "б", "без", "бы", "в", "вблизи", "вдоль",
        "во", "вокруг", "всех", "г", "го", "д", "для", "до", "е",
        "его", "ее", "ж", "же", "з", "за", "и", "из", "или", "им",
        "ими", "их", "к", "как", "ко", "кое", "л", "летию", "ли",
        "м", "между", "млн", "н", "на", "над", "не", "него", "ним",
        "них", "о", "об", "от", "п", "по", "под", "после", "при",
        "р", "с", "со", "т", "та", "так", "такой", "также", "то",
        "тоже", "у", "ф", "х", "ц", "ч", "ш", "щ", "ы", "ые", "ый",
        "э", "этих", "этой", "ю", "я",
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка стандартных стоп-слов.
    /// </summary>
    public static IReadOnlyList<string> GetStandardStopWords() => _standardStopWords;

    /// <summary>
    /// Является ли данный текст стандартным стоп-словом?
    /// </summary>
    public static bool IsStandardStopWord
        (
            string text
        )
    {
        if (string.IsNullOrWhiteSpace (text))
        {
            return true;
        }

        foreach (var word in _standardStopWords)
        {
            if (string.Compare (word, text, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
        }

        return false;
    }

//        /// <summary>
//        /// Load stopword list from server.
//        /// </summary>
//        public static IrbisStopWords FromServer
//            (
//                IrbisConnection connection
//            )
//        {
//            string database = connection.Database
//                .ThrowIfNull(Resources.DatabaseNotSet);
//            string fileName = database + ".stw";
//
//            return FromServer(connection, database, fileName);
//        }

//        /// <summary>
//        /// Load stopword list from server.
//        /// </summary>
//        public static IrbisStopWords FromServer
//            (
//                IrbisConnection connection,
//                string database,
//                string fileName
//            )
//        {
//            Sure.NotNullNorEmpty(database, nameof(database));
//            Sure.NotNullNorEmpty(fileName, nameof(fileName));
//
//            FileSpecification specification = new FileSpecification
//                (
//                    path: IrbisPath.MasterFile,
//                    database: database,
//                    fileName: fileName
//                );
//
//            string? text = connection
//                .ReadTextFileAsync(specification.ToString()).Result;
//            if (string.IsNullOrEmpty(text))
//            {
//                text = string.Empty;
//            }
//
//            IrbisStopWords result = ParseText(fileName, text);
//
//            return result;
//        }

    /// <summary>
    /// Is given word is stopword?
    /// </summary>
    public bool IsStopWord
        (
            string? word
        )
    {
        if (string.IsNullOrEmpty(word))
        {
            return true;
        }

        word = word.Trim();
        if (string.IsNullOrEmpty(word))
        {
            return true;
        }

        return _dictionary.ContainsKey(word);
    }

    /// <summary>
    /// Parse array of plain text lines.
    /// </summary>
    public static StopWords ParseLines
        (
            string? name,
            string[] lines
        )
    {
        var result = new StopWords(name);

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (!string.IsNullOrEmpty(trimmed))
            {
                result._dictionary[trimmed] = null;
            }
        }

        return result;
    }

//        /// <summary>
//        /// Parse plain text.
//        /// </summary>
//        public static IrbisStopWords ParseText
//            (
//                string? name,
//                string text
//            )
//        {
//            string[] lines = text.SplitLines();
//
//            return ParseLines(name, lines);
//        }

    /// <summary>
    /// Parse the text file.
    /// </summary>
    public static StopWords ParseFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty(fileName, nameof(fileName));

        var name = Path.GetFileNameWithoutExtension(fileName);
        var lines = File.ReadAllLines
            (
                path: fileName,
                encoding: IrbisEncoding.Ansi
            );

        return ParseLines(name, lines);
    }

    /// <summary>
    /// Convert <see cref="StopWords"/> to array
    /// of text lines.
    /// </summary>
    public string[] ToLines()
    {
        var result = _dictionary.Keys.ToArray();
        Array.Sort (result);

        return result;
    }

    /// <summary>
    /// Convert <see cref="StopWords"/> to plain text.
    /// </summary>
    public string ToText()
    {
        return string.Join
            (
                separator: Environment.NewLine,
                value: ToLines()
            );
    }

    #endregion
}
