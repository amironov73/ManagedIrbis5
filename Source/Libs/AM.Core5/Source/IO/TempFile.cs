// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TempFile.cs -- временный файл
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Временный файл, удаляемый сразу после того, как стал ненужным.
/// </summary>
public sealed class TempFile
    : IDisposable
{
    #region Events

    /// <summary>
    /// Событие, возникающее при закрытии файла.
    /// </summary>
    public event EventHandler? Disposing;

    #endregion

    #region Properties

    /// <summary>
    /// Поток.
    /// </summary>
    public Stream Stream { get; }

    /// <summary>
    /// Полный путь ко временному файлу.
    /// </summary>
    public string FullPath { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TempFile()
    {
        FullPath = Path.GetTempFileName();
        Stream = File.Create (FullPath);
    }

    /// <summary>
    /// Конструктор с конкретным (несуществующим) файлом.
    /// </summary>
    public TempFile
        (
            string fullPath
        )
    {
        Sure.NotNullNorEmpty (fullPath);

        FullPath = fullPath;
        Stream = File.Create (FullPath);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Stream.Dispose();
        File.Delete (FullPath);
        Disposing?.Invoke (this, EventArgs.Empty);
    }

    #endregion

    #region Objecte members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return FullPath;
    }

    #endregion
}
