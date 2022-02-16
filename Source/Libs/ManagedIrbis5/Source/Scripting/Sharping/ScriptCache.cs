// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ScriptCache.cs -- кэш скомпилированных сборок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting.Sharping;

/// <summary>
/// Кэш скомпилированных сборок.
/// </summary>
public sealed class ScriptCache
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Путь к папке, в которой хранятся сборки.
    /// </summary>
    public string? CachePath { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ScriptCache
        (
            string? cachePath = null,
            MemoryCacheOptions? options = null
        )
    {
        CachePath = cachePath;
        options ??= new MemoryCacheOptions();
        _cache = new MemoryCache (options);
    }

    #endregion

    #region Private members

    private readonly IMemoryCache _cache;

    #endregion

    #region Public methods

    /// <summary>
    /// Вычисление хеша для указанной строки.
    /// </summary>
    public static string ComputeHash
        (
            string input
        )
    {
        Sure.NotNull (input);

        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes (input);
        var hashBytes = md5.ComputeHash (inputBytes);

        return Convert.ToBase64String (hashBytes);
    }

    /// <summary>
    /// Получение кэшированной сборки для указанного исходного кода.
    /// </summary>
    public byte[]? GetAssembly
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        var key = ComputeHash (sourceCode);
        _cache.TryGetValue (key, out byte[]? result);

        if (result is null && CachePath is not null)
        {
            var fullPath = Path.Combine (CachePath, key);
            if (File.Exists (fullPath))
            {
                result = File.ReadAllBytes (fullPath);
                _cache.Set (key, result);
            }
        }

        return result;
    }

    /// <summary>
    /// Сохранение сборки в кэше.
    /// </summary>
    public void StoreAssembly
        (
            string sourceCode,
            byte[] assembly
        )
    {
        var key = ComputeHash (sourceCode);

        if (CachePath is not null)
        {
            var fullPath = Path.Combine (CachePath, key);
            File.WriteAllBytes (fullPath, assembly);
        }

        _cache.Set (key, assembly);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _cache.Dispose();
    }

    #endregion
}
