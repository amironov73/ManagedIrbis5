// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SelectCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Remembers current selection and restore it after Undo
/// </summary>
public class SelectCommand
    : UndoableCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="ts"></param>
    public SelectCommand
        (
            TextSource ts
        )
        : base (ts)
    {
    }

    #endregion

    #region Command members

    /// <inheritdoc cref="UndoableCommand.Execute"/>
    public override void Execute()
    {
        //remember selection
        lastSel = new RangeInfo (ts.CurrentTB.Selection);
    }

    /// <inheritdoc cref="UndoableCommand.OnTextChanged"/>
    protected override void OnTextChanged
        (
            bool invert
        )
    {
    }

    /// <inheritdoc cref="UndoableCommand.Undo"/>
    public override void Undo()
    {
        //restore selection
        ts.CurrentTB.Selection = new TextRange (ts.CurrentTB, lastSel.Start, lastSel.End);
    }

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        var result = new SelectCommand (ts);
        if (lastSel != null)
        {
            result.lastSel = new RangeInfo (new TextRange (ts.CurrentTB, lastSel.Start, lastSel.End));
        }

        return result;
    }

    #endregion
}
