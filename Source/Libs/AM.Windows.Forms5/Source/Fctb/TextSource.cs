// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* TextSource.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.IO;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// This class contains the source text (chars and styles).
/// It stores a text lines, the manager of commands, undo/redo stack, styles.
/// </summary>
public class TextSource
    : IList<Line>, IDisposable
{
    #region Events

    /// <summary>
    /// Occurs when line was inserted/added
    /// </summary>
    public event EventHandler<LineInsertedEventArgs>? LineInserted;

    /// <summary>
    /// Occurs when line was removed
    /// </summary>
    public event EventHandler<LineRemovedEventArgs>? LineRemoved;

    /// <summary>
    /// Occurs when text was changed
    /// </summary>
    public event EventHandler<TextChangedEventArgs>? TextChanged;

    /// <summary>
    /// Occurs when recalc is needed
    /// </summary>
    public event EventHandler<TextChangedEventArgs>? RecalcNeeded;

    /// <summary>
    /// Occurs when recalc wordwrap is needed
    /// </summary>
    public event EventHandler<TextChangedEventArgs>? RecalcWordWrap;

    /// <summary>
    /// Occurs before text changing
    /// </summary>
    public event EventHandler<TextChangingEventArgs>? TextChanging;

    /// <summary>
    /// Occurs after CurrentTB was changed
    /// </summary>
    public event EventHandler? CurrentTBChanged;

    #endregion

    readonly protected List<Line?> lines = new();

    protected LinesAccessor linesAccessor;

    int lastLineUniqueId;

    public CommandManager Manager { get; set; }

    SyntaxTextBox _currentTextBox;

    /// <summary>
    /// Styles
    /// </summary>
    public readonly Style[] Styles;

    /// <summary>
    /// Current focused FastColoredTextBox
    /// </summary>
    public SyntaxTextBox CurrentTextBox
    {
        get { return _currentTextBox; }
        set
        {
            if (_currentTextBox == value)
            {
                return;
            }

            _currentTextBox = value;
            OnCurrentTBChanged();
        }
    }

    public virtual void ClearIsChanged()
    {
        foreach (var line in lines)
        {
            line.IsChanged = false;
        }
    }

    public virtual Line CreateLine()
    {
        return new Line (GenerateUniqueLineId());
    }

    private void OnCurrentTBChanged()
    {
        CurrentTBChanged?.Invoke (this, EventArgs.Empty);
    }

    /// <summary>
    /// Default text style
    /// This style is using when no one other TextStyle is not defined in Char.style
    /// </summary>
    public TextStyle DefaultStyle { get; set; }

    #region Construction

    public TextSource (SyntaxTextBox currentTextBox)
    {
        this.CurrentTextBox = currentTextBox;
        linesAccessor = new LinesAccessor (this);
        Manager = new CommandManager (this);

        if (Enum.GetUnderlyingType (typeof (StyleIndex)) == typeof (UInt32))
            Styles = new Style[32];
        else
            Styles = new Style[16];

        InitDefaultStyle();
    }

    #endregion

    public virtual void InitDefaultStyle()
    {
        DefaultStyle = new TextStyle (null, null, FontStyle.Regular);
    }

    public virtual bool IsLineLoaded (int iLine)
    {
        return lines[iLine] != null;
    }

    /// <summary>
    /// Text lines
    /// </summary>
    public virtual IList<string> GetLines()
    {
        return linesAccessor;
    }

    public IEnumerator<Line> GetEnumerator()
    {
        return lines.GetEnumerator();
    }

    IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return (lines as IEnumerator);
    }

    public virtual int BinarySearch (Line item, IComparer<Line> comparer)
    {
        return lines.BinarySearch (item, comparer);
    }

    /// <summary>
    /// Генерация уникального идентификатора строки.
    /// </summary>
    /// <returns></returns>
    public virtual int GenerateUniqueLineId()
    {
        return lastLineUniqueId++;
    }

    /// <summary>
    /// Вставка строки.
    /// </summary>
    public virtual void InsertLine
        (
            int index,
            Line line
        )
    {
        lines.Insert (index, line);
        OnLineInserted (index);
    }

    /// <summary>
    /// Реакция на вставленную строку.
    /// </summary>
    public virtual void OnLineInserted
        (
            int index
        )
    {
        OnLineInserted (index, 1);
    }

    /// <summary>
    /// Реакция на вставленные строки.
    /// </summary>
    public virtual void OnLineInserted
        (
            int index,
            int count
        )
    {
        LineInserted?.Invoke (this, new LineInsertedEventArgs (index, count));
    }

    /// <summary>
    /// Удаление строки по указанному индексу.
    /// </summary>
    public virtual void RemoveLine
        (
            int index
        )
    {
        RemoveLine (index, 1);
    }

    public virtual bool IsNeedBuildRemovedLineIds => LineRemoved != null;

    /// <summary>
    /// Удаление строк по указанному индексу.
    /// </summary>
    public virtual void RemoveLine
        (
            int index,
            int count
        )
    {
        var removedLineIds = new List<int>();

        //
        if (count > 0)
            if (IsNeedBuildRemovedLineIds)
                for (var i = 0; i < count; i++)
                    removedLineIds.Add (this[index + i].UniqueId);

        //
        lines.RemoveRange (index, count);

        OnLineRemoved (index, count, removedLineIds);
    }

    /// <summary>
    /// Реакция на удаление строк.
    /// </summary>
    public virtual void OnLineRemoved
        (
            int index,
            int count,
            List<int> removedLineIds
        )
    {
        if (count > 0)
        {
            LineRemoved?.Invoke (this, new LineRemovedEventArgs (index, count, removedLineIds));
        }
    }

    /// <summary>
    /// Реакция на изменение текста.
    /// </summary>
    public virtual void OnTextChanged
        (
            int fromLine,
            int toLine
        )
    {
        TextChanged?.Invoke (this, new TextChangedEventArgs (Math.Min (fromLine, toLine), Math.Max (fromLine, toLine)));
    }

    public class TextChangedEventArgs : EventArgs
    {
        public int iFromLine;
        public int iToLine;

        public TextChangedEventArgs (int iFromLine, int iToLine)
        {
            this.iFromLine = iFromLine;
            this.iToLine = iToLine;
        }
    }

    public virtual void NeedRecalc (TextChangedEventArgs args)
    {
        RecalcNeeded?.Invoke (this, args);
    }

    public virtual void OnRecalcWordWrap (TextChangedEventArgs args)
    {
        RecalcWordWrap?.Invoke (this, args);
    }

    /// <summary>
    /// Реакция на изменение текста.
    /// </summary>
    public virtual void OnTextChanging()
    {
        string? temp = null;
        OnTextChanging (ref temp);
    }

    /// <summary>
    /// Реакция на изменение текста.
    /// </summary>
    public virtual void OnTextChanging
        (
            ref string? text
        )
    {
        if (TextChanging is not null)
        {
            var args = new TextChangingEventArgs() { InsertingText = text };
            TextChanging (this, args);
            text = args.InsertingText;
            if (args.Cancel)
                text = string.Empty;
        }

        ;
    }

    /// <summary>
    /// Получение длины строки с указанным индексом.
    /// </summary>
    public virtual int GetLineLength (int index)
    {
        return lines[index].Count;
    }

    /// <summary>
    /// Строка имеет маркер начала свертывания?
    /// </summary>
    public virtual bool LineHasFoldingStartMarker
        (
            int index
        )
    {
        return !string.IsNullOrEmpty (lines[index].FoldingStartMarker);
    }

    /// <summary>
    /// Строка имеет маркер конца свертывания?
    /// </summary>
    public virtual bool LineHasFoldingEndMarker
        (
            int index
        )
    {
        return !string.IsNullOrEmpty (lines[index].FoldingEndMarker);
    }


    /// <summary>
    /// Сохранение в файл.
    /// </summary>
    public virtual void SaveToFile
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.NotNullNorEmpty (fileName);
        Sure.NotNull (encoding);

        using var writer = new StreamWriter (fileName, false, encoding);
        for (var i = 0; i < Count - 1; i++)
        {
            writer.WriteLine (lines[i].Text);
        }

        writer.Write (lines[Count - 1].Text);
    }

    #region IList<T> members

    /// <inheritdoc cref="IList{T}.this"/>
    public virtual Line this [int i]
    {
        get => lines[i];
        set => throw new NotSupportedException();
    }

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    public virtual int IndexOf
        (
            Line item
        )
    {
        Sure.NotNull (item);

        return lines.IndexOf (item);
    }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public virtual void Insert
        (
            int index,
            Line item
        )
    {
        Sure.NotNull (item);

        InsertLine (index, item);
    }

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    public virtual void RemoveAt
        (
            int index
        )
    {
        RemoveLine (index);
    }

    #endregion

    #region ICollection members

    /// <inheritdoc cref="ICollection{T}.Add"/>
    public virtual void Add
        (
            Line item
        )
    {
        Sure.NotNull (item);

        InsertLine (Count, item);
    }

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public virtual void Clear()
    {
        RemoveLine (0, Count);
    }

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public virtual bool Contains (Line item)
    {
        return lines.Contains (item);
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public virtual void CopyTo (Line[] array, int arrayIndex)
    {
        lines.CopyTo (array, arrayIndex);
    }

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public virtual int Count => lines.Count;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public virtual bool IsReadOnly => false;

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public virtual bool Remove
        (
            Line item
        )
    {
        Sure.NotNull (item);

        var i = IndexOf (item);
        if (i >= 0)
        {
            RemoveLine (i);
            return true;
        }

        return false;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public virtual void Dispose()
    {
        // пустое тело метода
    }

    #endregion
}
