// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global


/* EnumChildProc.cs -- функция обратного вызова, используемая с функцией EnumChildWindows
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Функция EnumChildProc — это определяемая приложением функция
/// обратного вызова, используемая с функцией EnumChildWindows.
/// Он получает дескрипторы дочернего окна.
/// </summary>
///
/// <param name="hwnd">Дескриптор дочернего окна родительского окна,
/// указанного в EnumChildWindows.</param>
///
/// <param name="lParam">Указывает определяемое приложением значение,
/// заданное в EnumChildWindows.</param>
///
/// <returns>Чтобы продолжить перечисление, функция обратного вызова
/// должна вернуть TRUE; чтобы остановить перечисление,
/// он должен вернуть FALSE.</returns>
public delegate bool EnumChildProc
    (
        IntPtr hwnd,
        IntPtr lParam
    );
