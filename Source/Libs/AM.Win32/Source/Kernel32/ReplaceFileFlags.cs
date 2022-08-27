// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ReplaceFileFlags.cs -- варианты замены (перезаписи) файлов
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Варианты замены (перезаписи) файлов.
/// </summary>
[Flags]
public enum ReplaceFileFlags
{
    /// <summary>
    /// Значение не поддерживается.
    /// </summary>
    REPLACEFILE_WRITE_THROUGH = 0x00000001,

    /// <summary>
    /// Игнорирует ошибки, возникающие при объединении информации
    /// (например, атрибутов и списков ACL) из замененного файла
    /// в заменяющий файл. Таким образом, если вы укажете этот флаг
    /// и не имеете доступа WRITE_DAC, функция завершится успешно,
    /// но списки управления доступом не будут сохранены.
    /// </summary>
    REPLACEFILE_IGNORE_MERGE_ERRORS = 0x00000002,

    /// <summary>
    /// Игнорирует ошибки, возникающие при объединении информации
    /// ACL из замененного файла в файл замены. Таким образом,
    /// если вы укажете этот флаг и не имеете доступа WRITE_DAC,
    /// функция завершится успешно, но списки управления доступом
    /// не будут сохранены. Чтобы скомпилировать приложение,
    /// использующее это значение, определите макрос _WIN32_WINNT
    /// как 0x0600 или выше.
    /// Windows Server 2003 и Windows XP: это значение не поддерживается.
    /// </summary>
    REPLACEFILE_IGNORE_ACL_ERRORS = 0x00000004
}
