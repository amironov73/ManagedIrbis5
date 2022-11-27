// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontResourceFlags.cs -- характеристики шрифта, который будет добавлен в систему
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Задает характеристики шрифта, который будет добавлен в систему.
/// </summary>
[Flags]
public enum FontResourceFlags
{
    /// <summary>
    /// Указывает, что только процесс, вызвавший функцию AddFontResourceEx,
    /// может использовать этот шрифт. Когда имя шрифта совпадает
    /// с общедоступным шрифтом, будет выбран частный шрифт.
    /// Когда процесс завершится, система удалит все шрифты,
    /// установленные процессом с помощью функции AddFontResourceEx.
    /// </summary>
    FR_PRIVATE = 0x10,

    /// <summary>
    /// Указывает, что ни один процесс, включая процесс,
    /// вызвавший функцию AddFontResourceEx, не может перечислить этот шрифт.
    /// </summary>
    FR_NOT_ENUM = 0x20
}
