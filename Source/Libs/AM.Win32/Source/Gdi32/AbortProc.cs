// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* AbortProc.cs -- вызвается, когда задание печати должно быть отменено
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Функция AbortProc — это определяемая приложением функция
/// обратного вызова, используемая с функцией SetAbortProc.
/// Она вызывается, когда задание печати должно быть отменено
/// во время буферизации.
/// </summary>
public delegate bool AbortProc
    (
        IntPtr hdc,
        int iError
    );
