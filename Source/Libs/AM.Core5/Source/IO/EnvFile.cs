// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EnvFile.cs -- файл .env
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Collections;

using JetBrains.Annotations;

#endregion

namespace AM.IO;

/// <summary>
/// Файл <c>.env</c>.
/// </summary>
[PublicAPI]
public sealed class EnvFile
{
    #region Properties

    /// <summary>
    /// Доступ к строкам по именам.
    /// </summary>
    public string? this [string key] => _dictionary.GetValueOrDefault (key);

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    private EnvFile()
    {
        _dictionary = new CaseInsensitiveDictionary<string>();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EnvFile
        (
            string fileName
        )
        : this()
    {
        Sure.NotNullNorEmpty (fileName);

        ReadFile (fileName);
    }

    #endregion

    #region Private members

    private readonly Dictionary<string, string> _dictionary;

    private void ReadFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var stream = File.OpenText (fileName);
        while (stream.ReadLine() is { } line)
        {
            line = line.Trim();
            if (!string.IsNullOrEmpty (line)
                && !line.StartsWith ('#')
                && line.Contains ('='))
            {
                // TODO разобраться с кавычками?
                var parts = line.Split ('=', 2);
                var key = parts[0].Trim();
                if (!string.IsNullOrEmpty (key))
                {
                    _dictionary[key] = parts[1].Trim();
                }
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Применение переменных, найденных в файле.
    /// </summary>
    public void Apply()
    {
        foreach (var pair in _dictionary)
        {
            Environment.SetEnvironmentVariable (pair.Key, pair.Value);
        }
    }

    /// <summary>
    /// Файл <c>.env</c>, лежащий рядом с программой.
    /// Если такого файла нет, возвращается пустышка,
    /// не содержащая ни одной переменной.
    /// </summary>
    public static EnvFile GetDefaultEnvFile()
    {
        var path = Path.Combine
            (
                AppContext.BaseDirectory,
                ".env"
            );

        return File.Exists (path)
            ? new EnvFile ()
            : new EnvFile (path);
    }

    #endregion
}
