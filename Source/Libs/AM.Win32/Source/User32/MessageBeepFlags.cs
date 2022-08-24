// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MessageBeepFlags.cs -- тип звука для функции MessageBeep
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Тип звука, определяемый записью в реестре.
/// </summary>
public enum MessageBeepFlags
{
    /// <summary>
    /// Простой звуковой сигнал. Если звуковая карта недоступна,
    /// звук генерируется с помощью динамика.
    /// </summary>
    SimpleBeep = unchecked ((int) 0xFFFFFFFF),

    /// <summary>
    /// Звук "звездочка".
    /// </summary>
    MB_ICONASTERISK = 0x40,

    /// <summary>
    /// Звук "восклицательный знак".
    /// </summary>
    MB_ICONEXCLAMATION = 0x30,

    /// <summary>
    /// Звук "рука".
    /// </summary>
    MB_ICONHAND = 0x10,

    /// <summary>
    /// Звук "вопросительный знак".
    /// </summary>
    MB_ICONQUESTION = 0x20,

    /// <summary>
    /// Звук по умолчанию.
    /// </summary>
    MB_OK = 0
}
