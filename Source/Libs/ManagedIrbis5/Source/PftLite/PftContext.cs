// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PftContext.cs -- контекст исполнения PFT-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Контекст исполнения PFT-скрипта.
/// </summary>
sealed class PftContext
{
    #region Properties

    /// <summary>
    /// Запись, подлежащая расформатированию.
    /// </summary>
    public Record Record { get; set; } = null!;

    /// <summary>
    /// Выходной поток.
    /// </summary>
    public PftOutput Output { get; } = new ();

    /// <summary>
    /// Режим вывода полей/подполей.
    /// </summary>
    public char Mode { get; set; }

    /// <summary>
    /// Текущая группа.
    /// </summary>
    public GroupNode? CurrentGroup { get; set; }

    /// <summary>
    /// Номер текущего повторения (отсчет от нуля).
    /// </summary>
    public int CurrentRepeat { get; set; }

    /// <summary>
    /// Предполагаемое количество повторений.
    /// </summary>
    public int RepeatCount { get; set; }

    /// <summary>
    /// Съесть следующий перевод строки?
    /// </summary>
    public bool EatNextNewLine { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Сброс состояния перед новым форматированием.
    /// </summary>
    public void Reset()
    {
        Mode = 'p';
        Output.Reset();
        CurrentGroup = null;
        CurrentRepeat = 0;
    }

    /// <summary>
    /// Вывод повторяющегося символа.
    /// Флаг не устанавливается.
    /// </summary>
    public void Write
        (
            char chr,
            int count = 1
        )
    {
        Output.Write (chr, chr);
    }

    /// <summary>
    /// Вывод текста. Флаг не устанавливается.
    /// </summary>
    public void Write
        (
            ReadOnlySpan<char> text
        )
    {
        Output.Write (text);
    }

    #endregion
}
