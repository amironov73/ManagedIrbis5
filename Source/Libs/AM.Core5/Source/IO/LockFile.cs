// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LockFile.cs -- файл блокировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Файл блокировки - создается программой, чтобы "застолбить место"
/// (также, возможно, для хранения сессионных данных).
/// </summary>
/// <remarks>
/// Заимствован из проекта: https://github.com/Tyrrrz/LockFile
/// </remarks>
/// <example>
/// <para>Однократная попытка захвата</para>
/// <code>
/// using (var lockFile = LockFile.TryAcquire("some.lock"))
/// {
///   if (lockFile != null)
///   {
///     // лок-файл захвачен
///   }
///   else
///   {
///     // что-то нам мешает
///   }
/// }
/// </code>
/// <para>Попытка захвата в течение двух секунд.</para>
/// <code>
/// using (var cts = new CancellationTokenSource (TimeSpan.FromSeconds (2)))
/// using (var lockFile = LockFile.WaitAcquire ("some.lock", cts.Token))
/// {
///    // Если файл блокировки не получен в течение 2 секунд, генерируется исключение.
/// }
/// </code>
/// </example>
public sealed class LockFile
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Поток.
    /// </summary>
    public Stream Stream { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LockFile (FileStream stream)
    {
        Stream = stream;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Пытается получить файл блокировки с заданным путем.
    /// Возвращает null, если файл уже используется.
    /// </summary>
    public static LockFile? TryAcquire
        (
            string filePath
        )
    {
        try
        {
            var fileStream = File.Open
                (
                    filePath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None
                );

            return new LockFile (fileStream);
        }

        // Когда доступ к файлу запрещен, генерируем исключение
        catch (IOException ex) when (ex.GetType() == typeof (IOException))
        {
            return null;
        }
    }

    /// <summary>
    /// Пытается получить файл блокировки,
    /// пока операция не завершится успешно или не будет отменена.
    /// </summary>
    public static LockFile WaitAcquire
        (
            string filePath,
            CancellationToken cancellationToken = default
        )
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lockFile = TryAcquire (filePath);
            if (lockFile != null)
            {
                return lockFile;
            }
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Stream.Dispose();
    }

    #endregion
}
