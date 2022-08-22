// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* AnimateWindowFlags.cs -- тип анимации окна
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Specifies the type of window animation.
/// </summary>
[Flags]
public enum AnimateWindowFlags
{
    /// <summary>
    /// Анимирует окно слева направо. Этот флаг можно использовать
    /// с анимацией прокрутки или слайда. Игнорируется
    /// при использовании с AW_CENTER или AW_BLEND.
    /// </summary>
    AW_HOR_POSITIVE = 0x00000001,

    /// <summary>
    /// Анимирует окно справа налево. Этот флаг можно использовать
    /// с анимацией прокрутки или слайда. Игнорируется
    /// при использовании с AW_CENTER или AW_BLEND.
    /// </summary>
    AW_HOR_NEGATIVE = 0x00000002,

    /// <summary>
    /// Анимирует окно сверху вниз. Этот флаг можно использовать
    /// с анимацией прокрутки или слайда. Игнорируется
    /// при использовании с AW_CENTER или AW_BLEND.
    /// </summary>
    AW_VER_POSITIVE = 0x00000004,

    /// <summary>
    /// Анимирует окно снизу вверх. Этот флаг можно использовать
    /// с анимацией прокрутки или слайда. Игнорируется
    /// при использовании с AW_CENTER или AW_BLEND.
    /// </summary>
    AW_VER_NEGATIVE = 0x00000008,

    /// <summary>
    /// Заставляет окно сворачиваться внутрь, если используется
    /// AW_HIDE, или расширяться наружу, если AW_HIDE
    /// не используется. Различные флаги направления не действуют.
    /// </summary>
    AW_CENTER = 0x00000010,

    /// <summary>
    /// Скрывает окно. По умолчанию окно отображается.
    /// </summary>
    AW_HIDE = 0x00010000,

    /// <summary>
    /// Активирует окно. Не используйте это значение с AW_HIDE.
    /// </summary>
    AW_ACTIVATE = 0x00020000,

    /// <summary>
    /// Использует слайд-анимацию. По умолчанию используется
    /// анимация вращения. Этот флаг игнорируется при использовании
    /// с AW_CENTER.
    /// </summary>
    AW_SLIDE = 0x00040000,

    /// <summary>
    /// Использует эффект затухания. Этот флаг можно использовать,
    /// только если hwnd является окном верхнего уровня.
    /// </summary>
    AW_BLEND = 0x00080000
}
