﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BY_HANDLE_FILE_INFORMATION.cs -- информация, полученная от функции GetFileInformationByHandle
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Структура содержит информацию, полученную от функции
/// <c>GetFileInformationByHandle</c>.
/// </summary>
[StructLayout (LayoutKind.Sequential, Size = 52)]
public struct BY_HANDLE_FILE_INFORMATION
{
    /// <summary>
    /// Атрибуты файла.
    /// </summary>
    public FileAttributes dwFileAttributes;

    /// <summary>
    /// A FILETIME structure that specifies when the file or directory
    /// was created. If the underlying file system does not support
    /// creation time, this member is zero.
    /// </summary>
    public FILETIME ftCreationTime;

    /// <summary>
    /// A FILETIME structure. For a file, the structure specifies when
    /// the file was last read from or written to. For a directory,
    /// the structure specifies when the directory was created.
    /// For both files and directories, the specified date will be correct,
    /// but the time of day will always be set to midnight. If the underlying
    /// file system does not support last access time, this member is zero.
    /// </summary>
    public FILETIME ftLastAccessTime;

    /// <summary>
    /// A FILETIME structure. For a file, the structure specifies when
    /// the file was last written to. For a directory, the structure
    /// specifies when the directory was created. If the underlying
    /// file system does not support last write time, this member is zero.
    /// </summary>
    public FILETIME ftLastWriteTime;

    /// <summary>
    /// Serial number of the volume that contains the file.
    /// </summary>
    public uint dwVolumeSerialNumber;

    /// <summary>
    /// High-order part of the file size.
    /// </summary>
    public int nFileSizeHigh;

    /// <summary>
    /// Low-order part of the file size.
    /// </summary>
    public uint nFileSizeLow;

    /// <summary>
    /// Number of links to this file. For the FAT file system this
    /// member is always 1. For NTFS, it may be more than 1.
    /// </summary>
    public int nNumberOfLinks;

    /// <summary>
    /// High-order part of a unique identifier associated with
    /// the file. For more information, see nFileIndexLow.
    /// </summary>
    public int nFileIndexHigh;

    /// <summary>
    /// <para>Low-order part of a unique identifier associated
    /// with the file.</para>
    /// <para>Note that this value is useful only while the file
    /// is open by at least one process. If no processes have it
    /// open, the index may change the next time the file is opened.
    /// </para>
    /// <para>The identifier (low and high parts) and the volume
    /// serial number uniquely identify a file on a single computer.
    /// To determine whether two open handles represent the same file,
    /// combine this identifier and the volume serial number for each
    /// file and compare them.</para>
    /// </summary>
    public uint nFileIndexLow;
}
