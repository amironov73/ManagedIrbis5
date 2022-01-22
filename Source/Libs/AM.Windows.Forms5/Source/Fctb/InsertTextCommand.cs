// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InsertTextCommand.cs -- вставка текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Вставка текста.
/// </summary>
public sealed class InsertTextCommand
    : UndoableCommand
{
    #region Properties

    /// <summary>
    /// Вставляемый текст
    /// </summary>
    public string InsertedText;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="ts">Underlaying textbox</param>
    /// <param name="insertedText">Text for inserting</param>
    public InsertTextCommand
        (
            TextSource ts,
            string insertedText
        )
        : base (ts)
    {
        InsertedText = insertedText;
    }

    #endregion

    #region Private members

    internal static void InsertText
        (
            string insertedText,
            TextSource ts
        )
    {
        var tb = ts.CurrentTB;
        try
        {
            tb.Selection.BeginUpdate();
            var cc = '\x0';

            if (ts.Count == 0)
            {
                InsertCharCommand.InsertLine (ts);
                tb.Selection.Start = Place.Empty;
            }

            tb.ExpandBlock (tb.Selection.Start.Line);
            var len = insertedText.Length;
            for (var i = 0; i < len; i++)
            {
                var c = insertedText[i];
                if (c == '\r' && (i >= len - 1 || insertedText[i + 1] != '\n'))
                    InsertCharCommand.InsertChar ('\n', ref cc, ts);
                else
                    InsertCharCommand.InsertChar (c, ref cc, ts);
            }

            ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
        }
        finally
        {
            tb.Selection.EndUpdate();
        }
    }

    #endregion

    #region Command members

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        ts.CurrentTB.Selection.Start = sel.Start;
        ts.CurrentTB.Selection.End = lastSel.Start;
        ts.OnTextChanging();
        ClearSelectedCommand.ClearSelected (ts);
        base.Undo();
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        ts.OnTextChanging (ref InsertedText);
        InsertText (InsertedText, ts);
        base.Execute();
    }

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        return new InsertTextCommand (ts, InsertedText);
    }

    #endregion
}
