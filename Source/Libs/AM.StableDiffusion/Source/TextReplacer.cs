// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* TextReplacer.cs -- заменяет текст в указанных файлах
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Заменяет текст в указанных файлах.
/// </summary>
[PublicAPI]
public sealed class TextReplacer
{
    #region Properties

    /// <summary>
    /// Используемая кодировка.
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.ASCII;

    /// <summary>
    /// Обязательно ли наличие исходного фрагмента?
    /// </summary>
    public bool IsMandatory { get; set; } = true;

    /// <summary>
    /// Рекурсивно?
    /// </summary>
    public bool Recursive { get; set; }

    /// <summary>
    /// Нечувствительно к регистру?
    /// </summary>
    public bool CaseInsensitive { get; set; }

    /// <summary>
    /// Использовать регулярные выражения?
    /// </summary>
    public bool UseRegex { get; set; }

    /// <summary>
    /// Вывод прогресса.
    /// </summary>
    public TextWriter Output { get; set; } = TextWriter.Null;

    #endregion

    #region Private members

    private void ProcessSingleFile
        (
            string fileName,
            string oldValue,
            string newValue
        )
    {
        // TODO реализовать нечувствительность к регистру
        // регулярные выражения
        // и вывод прогресса

        var sourceText = File.ReadAllText (fileName, Encoding);
        if (IsMandatory && !sourceText.Contains (oldValue))
        {
            throw new Exception();
        }

        var processedText = sourceText.Replace
            (
                oldValue,
                newValue
            );
        File.WriteAllText
            (
                fileName,
                processedText,
                Encoding
            );
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор аргументов командной строки.
    /// </summary>
    public string[] ParseCommandLine
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        var result = new List<string>();
        foreach (var arg in args)
        {
            result.Add (arg);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Замена указанного текста.
    /// </summary>
    /// <param name="path">Путь, где хранятся файлы.</param>
    /// <param name="pattern">Шаблон имени файла.</param>
    /// <param name="oldValue">Исходный фрагмент.</param>
    /// <param name="newValue">Заменяющий фрагмент.</param>
    public void Replace
        (
            string path,
            string pattern,
            string oldValue,
            string newValue
        )
    {
        Sure.NotNullNorEmpty (oldValue);
        Sure.NotNull (newValue);

        var options = Recursive ?
            SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;
        var foundFiles = Directory.GetFiles (path, pattern, options);
        foreach (var fileName in foundFiles)
        {
            ProcessSingleFile (fileName, oldValue, newValue);
        }
    }

    #endregion
}
