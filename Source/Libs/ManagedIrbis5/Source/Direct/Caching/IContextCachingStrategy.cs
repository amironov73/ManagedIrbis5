// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* IContextCachingStrategy.cs -- стратегия кеширования текстовых файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Стратегия кеширования текстовых файлов.
/// </summary>
public interface IContextCachingStrategy
{
    /// <summary>
    /// Полная очистка кеша.
    /// </summary>
    void ClearFileCache();

    /// <summary>
    /// Получение из кеша файла с указанным именем.
    /// </summary>
    string? GetCachedFile (ISyncProvider provider, string fileName);

    /// <summary>
    /// Помещение файла в кеш.
    /// </summary>
    void StoreFile (string fileName, string content);

    /// <summary>
    /// Удаление файла из кеша.
    /// </summary>
    void ForgetFile (string fileName);
}
