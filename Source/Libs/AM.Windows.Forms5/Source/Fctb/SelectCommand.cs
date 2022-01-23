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
    /// <param name="textSource"></param>
    public SelectCommand
        (
            TextSource textSource
        )
        : base (textSource)
    {
    }

    #endregion

    #region Command members

    /// <inheritdoc cref="UndoableCommand.Execute"/>
    public override void Execute()
    {
        //remember selection
        lastSel = new RangeInfo (textSource.CurrentTextBox.Selection);
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
        textSource.CurrentTextBox.Selection = new TextRange (textSource.CurrentTextBox, lastSel.Start, lastSel.End);
    }

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        var result = new SelectCommand (textSource);
        if (lastSel != null)
        {
            result.lastSel = new RangeInfo (new TextRange (textSource.CurrentTextBox, lastSel.Start, lastSel.End));
        }

        return result;
    }

    #endregion
}
