// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StopWords.cs -- обертка над списком стоп-слов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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

    #endregion

    #region Public methods

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
