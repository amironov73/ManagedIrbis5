// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IAsyncResourceProvider.cs -- интерфейс асинхронного провайдера ресурсов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory;

/// <summary>
/// Интерфейс асинхронного провайдера ресурсов.
/// </summary>
public interface IAsyncResourceProvider
{
    /// <summary>
    /// Дамп.
    /// </summary>
    public Task DumpAsync
        (
            TextWriter output
        );

    /// <summary>
    /// Получение списка ресурсов по указанному пути.
    /// </summary>
    public Task<string[]> ListResourcesAsync
        (
            string path
        );

    /// <summary>
    /// Получение указанного ресурса.
    /// </summary>
    public Task<string?> ReadResourceAsync
        (
            string fileName
        );

    /// <summary>
    /// Ресурс с указанным именем существует?
    /// </summary>
    public Task<bool> ResourceExistsAsync
        (
            string fileName
        );

    /// <summary>
    /// Перезапись указанного ресурса.
    /// </summary>
    public Task<bool> WriteResourceAsync
        (
            string fileName,
            string? content
        );
}
