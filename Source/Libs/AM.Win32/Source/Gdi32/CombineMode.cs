// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CombineMode.cs -- определяет режим, указывающий, как будут объединены две области.
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Определяет режим, указывающий, как будут объединены две области.
/// </summary>
public enum CombineMode
    : uint
{
    /// <summary>
    /// Создает пересечение двух объединяемых регионов.
    /// </summary>
    RGN_AND = 1,

    /// <summary>
    /// Создает объединение двух объединяемых регионов.
    /// </summary>
    RGN_OR = 2,

    /// <summary>
    /// Создает объединение двух объединяемых областей,
    /// за исключением любых перекрывающихся областей.
    /// </summary>
    RGN_XOR = 3,

    /// <summary>
    /// Объединяет части hrgnSrc1, которые не являются частью hrgnSrc2.
    /// </summary>
    RGN_DIFF = 4,

    /// <summary>
    /// Создает копию региона, указанного hrgnSrc1.
    /// </summary>
    RGN_COPY = 5
}
