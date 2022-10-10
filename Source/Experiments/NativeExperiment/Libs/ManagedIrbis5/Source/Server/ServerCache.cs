// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ServerCache.cs -- простой серверный кеш
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.IO;

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server;

/// <summary>
/// Простой серверный кеш.
/// </summary>
public sealed class ServerCache
{
    #region Constants

    /// <summary>
    /// Предел использования оперативной памяти по умолчанию, байты.
    /// </summary>
    public const int DefaultLimit = 100 * 1024 * 1024;

    #endregion

    #region Nested classes

    /// <summary>
    /// Запись о кешированном файле.
    /// </summary>
    class Entry
    {
        public byte[]? Content;

        public DateTime ModificationTime;

        public DateTime AccessTime;

        public int AccessCount;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Предел использования оперативной памяти, байты.
    /// </summary>
    public int MemoryLimit { get; private set; }

    /// <summary>
    /// Текущее использование оперативной памяти, байты.
    /// </summary>
    public int MemoryUsage { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ServerCache()
    {
        _sync = new object();
        _dictionary = new ConcurrentDictionary<string, Entry> (StringComparer.InvariantCultureIgnoreCase);
        MemoryLimit = DefaultLimit;
        MemoryUsage = 0;
    } // constructor

    #endregion

    #region Private members

    private readonly object _sync;
    private readonly ConcurrentDictionary<string, Entry> _dictionary;

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка всего кеша.
    /// </summary>
    public void Clear()
    {
        _dictionary.Clear();
        MemoryUsage = 0;
    }

    /// <summary>
    /// Get file content.
    /// </summary>
    public byte[]? GetFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        if (!_dictionary.TryGetValue (fileName, out var entry))
        {
            byte[]? content = null;

            try
            {
                var unixed = Unix.FindFileOrThrow (fileName);

                content = File.ReadAllBytes (unixed);
                entry = new Entry
                {
                    Content = content,
                    ModificationTime = File.GetLastWriteTime (unixed)
                };
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (ServerCache) + "::" + nameof (GetFile)
                    );

                entry = new Entry
                {
                    Content = null,
                    ModificationTime = DateTime.MinValue
                };
            }

            entry.AccessCount = 1;
            entry.AccessTime = DateTime.Now;

            var newUsage = MemoryUsage + content?.Length ?? 0;
            if (newUsage >= MemoryLimit)
            {
                // TODO: как-то очистить кеш
            }

            _dictionary[fileName] = entry;
            MemoryUsage = newUsage;

            return content;
        }

        // TODO: проверять последнюю модификацию файла
        // var modified = File.GetLastWriteTime(fileName);
        //    if (entry.ModificationTime < modified)
        //    {
        // MemoryUsage -= entry.Content?.Length ?? 0;
        // TODO: учесть Unix
        // entry.Content = File.ReadAllBytes(fileName);
        // MemoryUsage += entry.Content.Length;
        // entry.ModificationTime = modified;
        //    }

        entry.AccessTime = DateTime.Now;
        entry.AccessCount++;

        return entry.Content;
    }

    /// <summary>
    /// Получение содержимого текстового файла в кодировке ANSI.
    /// </summary>
    public string? GetAnsiFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var bytes = GetFile (fileName);
        if (bytes is null)
        {
            return null;
        }

        var result = IrbisEncoding.Ansi.GetString (bytes);

        return result;
    }

    #endregion
}
