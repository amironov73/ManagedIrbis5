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

using System.IO;
using System.Text;

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
    public StringWriter Output { get; }

    /// <summary>
    /// Режим вывода полей/подполей.
    /// </summary>
    public char Mode { get; set; }

    /// <summary>
    /// Преобразование в верхний регистр.
    /// </summary>
    public bool UpperMode { get; set; }

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

    /// <summary>
    /// Был вывод?
    /// </summary>
    public bool OutputFlag { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftContext()
    {
        _builder = new StringBuilder();
        Output = new StringWriter (_builder);
    }

    #endregion

    #region Private members

    private readonly StringBuilder _builder;

    #endregion

    #region Public methods

    /// <summary>
    /// Сброс состояния перед новым форматированием.
    /// </summary>
    public void Reset()
    {
        Mode = 'p';
        UpperMode = false;
        _builder.Clear();
        OutputFlag = false;
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
            int length
        )
    {
        while (--length >= 0)
        {
            Output.Write (chr);
        }
    }

    /// <summary>
    /// Вывод текста. Флаг не устанавливается.
    /// </summary>
    public void Write
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            Output.Write
                (
                    UpperMode
                        ? text.ToUpperInvariant()
                        : text
                );
        }
    }

    /// <summary>
    /// Вывод текста с установкой флага (если текст не пустой).
    /// </summary>
    public void WriteAndSetFlag
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            Output.Write (text);
            OutputFlag = true;
        }
    }

    #endregion
}
