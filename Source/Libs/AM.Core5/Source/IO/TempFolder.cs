// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* TempFolder.cs -- временная папка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.IO;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Временная папка.
/// </summary>
public sealed class TempFolder
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Полный путь ко временной папке.
    /// </summary>
    public string FullPath { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TempFolder()
    {
        FullPath = Path.Combine (Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory (FullPath);
        _files = new ();
    }

    #endregion

    #region Private members

    private readonly ConcurrentDictionary<TempFile, object?> _files;

    private void DisposingHandler
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        var file = (TempFile) sender!;
        _files.TryRemove (file, out _);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание временного файла.
    /// </summary>
    public TempFile CreateFile()
    {
        var fileName = Path.Combine (FullPath, Guid.NewGuid().ToString());
        var result = new TempFile (fileName);
        result.Disposing += DisposingHandler;
        _files.TryAdd (result, null);

        return result;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        foreach (var tempFile in _files)
        {
            tempFile.Key.Dispose();
        }

        try
        {
            Directory.Delete (FullPath, true);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (TempFolder) + "::" + nameof (Dispose)
                );
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return FullPath;
    }

    #endregion
}
