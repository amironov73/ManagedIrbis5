// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* HookProcedure.cs -- функция обратного вызова, используемая с функцией SetWindowsHookEx
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Процедура подключения HookProcedure — это определяемая
/// приложением или библиотекой функция обратного вызова,
/// используемая с функцией SetWindowsHookEx.
/// </summary>
public delegate int HookProcedure
    (
        int code,
        int wParam,
        int lParam
    );
