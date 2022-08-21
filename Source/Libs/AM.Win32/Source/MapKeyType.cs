// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MapKeyType.cs -- тип трансляции для функции MapVirtualKey.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Указывает тип трансляции для функции MapVirtualKey.
/// </summary>
[Serializable]
public enum MapKeyType
{
    /// <summary>
    /// uCode - это код виртуального ключа, который транслируется
    /// в скан-код. Если это код виртуальной клавиши, который
    /// не различает клавиши для левой и правой руки, возвращается
    /// скан-код для левой руки. Если перевода нет, функция возвращает 0.
    /// </summary>
    VirtualKeyToScanCode = 0,

    /// <summary>
    /// uCode - это скан-код, который транслируется в код виртуальной
    /// клавиши, не делающей различий между левой и правой клавишами.
    /// Если перевода нет, функция возвращает 0.
    /// </summary>
    ScanCodeToVirtualKey = 1,

    /// <summary>
    /// uCode - это код виртуального ключа, который преобразуется
    /// в несдвинутое символьное значение в младшем слове возвращаемого
    /// значения. Мертвые клавиши (диакритические знаки) обозначаются
    /// установкой верхнего бита возвращаемого значения.
    /// Если перевода нет, функция возвращает 0.
    /// </summary>
    VirtualKeyToUnshiftedKey = 2,

    /// <summary>
    /// Windows NT/2000/XP: uCode - это скан-код, транслируемый
    /// в код виртуальной клавиши, который различает клавиши
    /// для левой и правой руки. Если перевода нет, функция возвращает 0.
    /// </summary>
    VirtualKeyToScanCodeRL = 3
}
