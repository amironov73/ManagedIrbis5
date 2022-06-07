// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ISyncResourceProvider.cs -- интерфейс синхронного провайдера ресурсов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory;

/// <summary>
/// Интерфейс синхронного провайдера ресурсов.
/// </summary>
public interface ISyncResourceProvider
{
    /// <summary>
    /// Дамп.
    /// </summary>
    public void Dump
        (
            TextWriter output
        );

    /// <summary>
    /// Получение списка ресурсов по указанному пути.
    /// </summary>
    public string[] ListResources
        (
            string path
        );

    /// <summary>
    /// Получение указанного ресурса.
    /// </summary>
    public string? ReadResource
        (
            string fileName
        );

    /// <summary>
    /// Ресурс с указанным именем существует?
    /// </summary>
    public bool ResourceExists
        (
            string fileName
        );

    /// <summary>
    /// Перезапись указанного ресурса.
    /// </summary>
    public bool WriteResource
        (
            string fileName,
            string? content
        );
}
