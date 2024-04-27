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
using System.Text.RegularExpressions;

using AM.Collections;

using JetBrains.Annotations;

#endregion

namespace AM.IO;

/*
    Файлы с расширением .env используются для хранения конфигурационных
    данных среды выполнения программного обеспечения. Они обычно содержат
    переменные окружения, которые могут быть использованы программами
    или скриптами для настройки их поведения.

    Файлы .env часто используются в проектах, разрабатываемых с использованием
    систем управления версиями, таких как Git. Это позволяет разработчикам
    хранить чувствительные к безопасности данные (например, пароли,
    ключи API) отдельно от кода, который может быть публично доступен.

    Структура файла .env довольно проста. Каждая строка представляет собой
    пару ключ-значение, где ключ и значение разделены знаком равенства. Например:

    ```bash
    VARIABLE_NAME=value
    ```

    Здесь VARIABLE_NAME - это имя переменной, а value - её значение.
    Переменные в файле .env могут быть доступны через стандартные механизмы
    работы с переменными окружения в операционной системе.

    Проще говоря, любой .env-файл можно подать на вход интерпретатору
    командной строки:

    ```bash
    source .env
    ```

    Важно отметить, что файлы .env должны быть загружены в среду выполнения
    перед тем, как они будут использоваться. В некоторых языках программирования
    и фреймворках есть встроенная поддержка для загрузки файлов .env,
    например, в Python с помощью библиотеки dotenv.

    Важно отметить, что в файлах .env могут быть комментарии. Комментарии
    в файлах .env используются для документирования содержимого файла
    и облегчения понимания его содержимого другими разработчиками.

    Комментарии в файлах .env обычно начинаются с символа "#". Все, что
    следует после этого символа до конца строки, считается комментарием
    и игнорируется интерпретатором переменных окружения.

    Пример файла .env с комментариями:

    ```bash
    # This is a comment
    # It will be ignored by the environment parser

    SECRET_KEY=verysecretkey

    # This is another comment
    # Explaining what the variable below does

    DATABASE_URL=postgresql://user:password@host/dbname
    ```

    В этом примере первые две строки являются комментариями и не влияют
    на работу программы.

    Также стоит учитывать, что файлы .env являются обычными текстовыми файлами
    и могут быть открыты и отредактированы любым текстовым редактором.
    Однако, при работе с системами контроля версий, рекомендуется избегать
    добавления файлов .env в репозиторий, чтобы сохранить конфиденциальность
    хранящихся в них данных.

 */

/// <summary>
/// Файл <c>.env</c>.
/// </summary>
[PublicAPI]
public sealed partial class EnvFile
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

    // Resharper disable StringLiteralTypo
    [GeneratedRegex("[$]{?([A-Za-z0-9_]+)}?")]
    // ReSharper restore StringLiteralTypo
    private static partial Regex EnvironmentVariableRegex();

    private string Evaluate
        (
            Match match
        )
    {
        var name = match.Groups[1].Value;
        var value = _dictionary.GetValueOrDefault (name)
                ?? Environment.GetEnvironmentVariable (name);
        return value ?? string.Empty;
    }

    private string SubstituteEnvironmentVariables
        (
            string value
        )
    {
        var regex = EnvironmentVariableRegex();
        return regex.Replace (value, Evaluate);
    }

    private string FixValue
        (
            string value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return value;
        }

        value = value.Trim();
        if (string.IsNullOrEmpty (value))
        {
            return value;
        }

        if (value.StartsWith ('"'))
        {
            value = SubstituteEnvironmentVariables (value.Unquote ());
        }
        else if (value.StartsWith ('\''))
        {
            value = value.Unquote ('\'');
        }

        return value;
    }

    private void ReadFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var stream = File.OpenText (fileName);
        while (stream.ReadLine() is { } line)
        {
            ExtractOneVariable (line);
        }
    }

    private void ExtractOneVariable
        (
            string line
        )
    {
        line = line.Trim();
        if (!string.IsNullOrEmpty (line)
            && !line.StartsWith ('#')
            && line.Contains ('='))
        {
            var parts = line.Split ('=', 2);
            var key = parts[0].Trim();
            if (!string.IsNullOrEmpty (key))
            {
                var value = FixValue (parts[1]);
                if (!string.IsNullOrEmpty (value))
                {
                    _dictionary[key] = value;
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
