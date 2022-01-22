// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* UndoableCommand.cs -- команда с возможностью отмены
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// Команда с возможностью отмены.
/// </summary>
public abstract class UndoableCommand
    : Command
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="ts"></param>
    protected UndoableCommand
        (
            TextSource ts
        )
    {
        this.ts = ts;
        sel = new RangeInfo (ts.CurrentTB.Selection);
    }

    #endregion

    #region Private members

    internal RangeInfo sel;
    internal RangeInfo lastSel;
    internal bool autoUndo;

    #endregion

    #region Public methods

    /// <summary>
    /// Отмена команды.
    /// </summary>
    public virtual void Undo()
    {
        OnTextChanged (true);
    }

    /// <inheritdoc cref="Command.Execute"/>
    public override void Execute()
    {
        lastSel = new RangeInfo (ts.CurrentTB.Selection);
        OnTextChanged (false);
    }

    /// <summary>
    /// Реакция на изменение текста.
    /// </summary>
    /// <param name="invert">Отмена.</param>
    protected virtual void OnTextChanged
        (
            bool invert
        )
    {
        var b = sel.Start.Line < lastSel.Start.Line;
        if (invert)
        {
            ts.OnTextChanged (sel.Start.Line, b ? sel.Start.Line : lastSel.Start.Line);
        }
        else
        {
            ts.OnTextChanged (b ? sel.Start.Line : lastSel.Start.Line, lastSel.Start.Line);
        }
    }

    /// <summary>
    /// Клонирование команды.
    /// </summary>
    public abstract UndoableCommand Clone();

    #endregion
}
