// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CopyFileFlags.cs -- флаги, указывающие, как файл должен быть скопирован функцией CopyFileEx
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги, указывающие, как файл должен быть скопирован функцией CopyFileEx.
/// </summary>
[Flags]
public enum CopyFileFlags
{
    /// <summary>
    /// Операция копирования немедленно завершится ошибкой,
    /// если целевой файл уже существует.
    /// </summary>
    COPY_FILE_FAIL_IF_EXISTS = 0x00000001,

    /// <summary>
    /// Ход копирования отслеживается в целевом файле на случай сбоя
    /// копирования. Неудавшуюся копию можно перезапустить позже,
    /// указав те же значения для lpExistingFileName и lpNewFileName,
    /// которые использовались в неудачном вызове.
    /// </summary>
    COPY_FILE_RESTARTABLE = 0x00000002,

    /// <summary>
    /// Разрешеется копировать файл, открытый для записи.
    /// </summary>
    COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,

    /// <summary>
    /// <para>Попытка скопировать зашифрованный файл будет успешной,
    /// даже если целевая копия не может быть зашифрована.</para>
    /// <para>Windows 2000/NT и Windows Me/98/95: это значение
    /// не поддерживается.</para>
    /// </summary>
    COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
}
