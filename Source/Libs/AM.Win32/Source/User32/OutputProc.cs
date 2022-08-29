// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* OutputProc.cs -- определяемая приложением функция обратного вызова, используемая с функцией GrayString
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Функция <c>OutputProc</c> - это определяемая приложением функция
/// обратного вызова, используемая с функцией <c>GrayString</c>.
/// Она используется для рисования строки.
/// Тип <c>GRAYSTRINGPROC</c> определяет указатель на эту функцию
/// обратного вызова.
/// </summary>
/// <param name="hdc">
///Дескриптор контекста устройства с растровым изображением
/// по крайней мере шириной и высотой, указанными параметрами
/// <c>nWidth</c> и <c>nHeight</c>, переданными в <c>GrayString</c>.
/// </param>
/// <param name="lpData">
/// Указатель на строку, которую нужно нарисовать.
/// </param>
/// <param name="cchData">
/// Задает длину строки в символах.
/// </param>
/// <returns>
/// В случае успеха функция обратного вызова должна вернуть TRUE.
/// </returns>
public delegate bool OutputProc
    (
        IntPtr hdc,
        IntPtr lpData,
        int cchData
    );
