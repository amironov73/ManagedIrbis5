// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DOCINFO.cs -- содержит имена входных и выходных файлов и другую информацию, используемую функцией StartDoc
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Содержит имена входных и выходных файлов и другую информацию,
/// используемую функцией StartDoc.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct DOCINFO
{
    /// <summary>
    /// Размер структуры в 32-битной среде (используется в <see cref="cbSize"/>).
    /// </summary>
    public const int Size32 = 20;

    /// <summary>
    /// Размер структуры в 64-битной среде (используется в <see cref="cbSize"/>).
    /// </summary>
    public const int Size64 = 32;

    /// <summary>
    /// Указывает размер структуры в байтах.
    /// </summary>
    public int cbSize;

    /// <summary>
    /// Указатель на строку с завершающим нулем, которая определяет
    /// имя документа.
    /// </summary>
    [MarshalAs (UnmanagedType.LPTStr)]
    public string? lpszDocName;

    /// <summary>
    /// Указатель на строку с завершающим нулем, которая определяет
    /// имя выходного файла. Если этот указатель имеет значение NULL,
    /// выходные данные будут отправлены на устройство,
    /// идентифицированное дескриптором контекста устройства,
    /// который был передан функции StartDoc.
    /// </summary>
    [MarshalAs (UnmanagedType.LPTStr)]
    public string? lpszOutput;

    /// <summary>
    /// Указатель на заканчивающуюся нулем строку, указывающую тип данных,
    /// например "raw" или "emf", используемый для записи задания на печать.
    /// Этот элемент может быть NULL. Обратите внимание, что запрошенный
    /// тип данных может быть проигнорирован.
    /// </summary>
    [MarshalAs (UnmanagedType.LPTStr)]
    public string? lpszDatatype;

    /// <summary>
    /// Указывает дополнительную информацию о задании на печать.
    /// </summary>
    public DocInfoFlags fwType;
}
