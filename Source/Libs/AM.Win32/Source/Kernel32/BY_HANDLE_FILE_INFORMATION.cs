// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

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
    /// Структура FILETIME, указывающая, когда был создан файл
    /// или каталог. Если базовая файловая система не поддерживает
    /// время создания, этот элемент равен нулю.
    /// </summary>
    public FILETIME ftCreationTime;

    /// <summary>
    /// Структура FILETIME. Для файла структура указывает, когда
    /// в последний раз файл считывался или записывался. Для каталога
    /// структура указывает, когда каталог был создан. И для файлов,
    /// и для каталогов указанная дата будет правильной, но время
    /// суток всегда будет установлено на полночь. Если базовая
    /// файловая система не поддерживает время последнего доступа,
    /// этот элемент равен нулю.
    /// </summary>
    public FILETIME ftLastAccessTime;

    /// <summary>
    /// Структура FILETIME. Для файла структура указывает, когда
    /// файл был в последний раз записан. Для каталога структура
    /// указывает, когда каталог был создан. Если базовая файловая
    /// система не поддерживает время последней записи, этот
    /// элемент равен нулю.
    /// </summary>
    public FILETIME ftLastWriteTime;

    /// <summary>
    /// Серийный номер тома, содержащего файл.
    /// </summary>
    public uint dwVolumeSerialNumber;

    /// <summary>
    /// Старшая часть размера файла.
    /// </summary>
    public int nFileSizeHigh;

    /// <summary>
    /// Младшая часть размера файла.
    /// </summary>
    public uint nFileSizeLow;

    /// <summary>
    /// Количество ссылок на этот файл. Для файловой системы FAT
    /// этот член всегда равен 1. Для NTFS может быть больше 1.
    /// </summary>
    public int nNumberOfLinks;

    /// <summary>
    /// Старшая часть уникального идентификатора, связанного с файлом.
    /// Дополнительные сведения см. nFileIndexLow.
    /// </summary>
    public int nFileIndexHigh;

    /// <summary>
    /// <para>Младшая часть уникального идентификатора, связанного
    /// с файлом.</para>
    /// <para>Обратите внимание, что это значение полезно только тогда,
    /// когда файл открыт хотя бы одним процессом. Если ни один процесс
    /// не открыл его, индекс может измениться при следующем
    /// открытии файла.</para>
    /// <para>Идентификатор (младшая и старшая части) и серийный номер
    /// тома однозначно идентифицируют файл на одном компьютере.
    /// Чтобы определить, представляют ли два открытых дескриптора
    /// один и тот же файл, объедините этот идентификатор и серийный
    /// номер тома для каждого файла и сравните их.</para>
    /// </summary>
    public uint nFileIndexLow;
}
