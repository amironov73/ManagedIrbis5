// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* WIN32_FIND_DATA.cs -- описывает файл, найденный функциями FindFirstFile, FindFirstFileEx или FindNextFile
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Структура описывает файл, найденный функциями
/// <c>FindFirstFile</c>, <c>FindFirstFileEx</c>
/// или <c>FindNextFile</c>.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct WIN32_FIND_DATA
{
    /// <summary>
    /// Максимальная длина пути в Win32.
    /// </summary>
    public const int MAX_PATH = 260;

    /// <summary>
    /// Атрибуты найденного файла.
    /// </summary>
    public FileAttributes dwFileAttributes;

    /// <summary>
    /// Структура <see cref="FILETIME" />, указывающая, когда
    /// был создан файл или каталог. Если базовая файловая система
    /// не поддерживает  время создания, этот элемент равен нулю.
    /// </summary>
    public FILETIME ftCreationTime;

    /// <summary>
    /// Структура <see cref="FILETIME" />. Для файла структура
    /// указывает, когда в последний раз файл считывался
    /// или записывался. Для каталога структура указывает,
    /// когда каталог был создан. И для файлов, и для каталогов
    /// указанная дата будет правильной, но время суток всегда
    /// будет установлено на полночь. Если базовая файловая
    /// система не поддерживает время последнего доступа,
    /// этот элемент равен нулю.
    /// </summary>
    public FILETIME ftLastAccessTime;

    /// <summary>
    /// Структура <see cref="FILETIME" />. Для файла структура
    /// указывает, когда файл был в последний раз записан.
    /// Для каталога структура указывает, когда каталог был создан.
    /// Если базовая файловая система не поддерживает время
    /// последней записи, этот элемент равен нулю.
    /// </summary>
    public FILETIME ftLastWriteTime;

    /// <summary>
    /// Старшее значение <c>DWORD</c> размера файла в байтах.
    /// Это значение равно нулю, если размер файла не превышает
    /// <c>MAXDWORD</c>. Размер файла равен
    /// <c>(nFileSizeHigh * (MAXDWORD+1)) + nFileSizeLow</c>.
    /// </summary>
    public int nFileSizeHigh;

    /// <summary>
    /// Младшее значение <c>DWORD</c> размера файла в байтах.
    /// </summary>
    public uint nFileSizeLow;

    /// <summary>
    /// Если элемент dwFileAttributes включает в себя атрибут
    /// <c>FILE_ATTRIBUTE_REPARSE_POINT</c>, этот элемент определяет
    /// тег повторной обработки. В противном случае это значение
    /// не определено и не должно использоваться.
    /// </summary>
    public int dwReserved0;

    /// <summary>
    /// Зарезервировано для будущего использования.
    /// </summary>
    public int dwReserved1;

    /// <summary>
    /// Строка с завершающим нулем, содержащая имя файла.
    /// </summary>
    [MarshalAs (UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
    public string? cFileName;

    /// <summary>
    /// Строка с завершающим нулем, содержащая альтернативное
    /// имя файла. Это имя имеет классический формат имени
    /// файла 8.3 (<c>filename.ext</c>).
    /// </summary>
    [MarshalAs (UnmanagedType.ByValTStr, SizeConst = 14)]
    public string? cAlternateFileName;
}
