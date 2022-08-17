// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable RedundantNameQualifier

/* TreeGridNode.cs -- узел иерархического грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Узел иерархического грида.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[DesignTimeVisible (false)]
public class TreeGridNode
    : Component
{
    #region Events

    /// <summary>
    /// Событие, возникающее непосредственно перед началом редактирования данных.
    /// </summary>
    public event EventHandler? BeforeEdit;

    /// <summary>
    /// Событие, возникающее непосредственно после окончания редактирования данных.
    /// </summary>
    public event EventHandler? AfterEdit;

    #endregion

    #region Properties

    /// <summary>
    /// Цвет фона.
    /// </summary>
    [DefaultValue (typeof (Color), "Empty")]
    public virtual Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Цвет текста.
    /// </summary>
    [DefaultValue (typeof (Color), "Empty")]
    public virtual Color ForegroundColor
    {
        get => _foregroundColor;
        set
        {
            _foregroundColor = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Шрифт (опционально).
    /// </summary>
    /// <value>The font.</value>
    [DefaultValue (null)]
    public Font Font
    {
        get => _font ?? Grid?.Font ?? throw new ApplicationException();
        set => _font = value;
    }

    /// <summary>
    /// Данный узел может быть отмечен?
    /// </summary>
    [DefaultValue (false)]
    public bool Checkable { get; set; }

    /// <summary>
    /// Данный узел отмечен?
    /// </summary>
    [DefaultValue (false)]
    public bool Checked
    {
        get => _checked;
        set
        {
            _checked = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Данный узел разрешен?
    /// </summary>
    [DefaultValue (false)]
    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Иконка для данного узла (опционально).
    /// </summary>
    public Icon? Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Данный узел развернут?
    /// </summary>
    /// <remarks>
    /// Дочерние узлы могут быть при этом свернуты.
    /// </remarks>
    [DefaultValue (false)]
    public bool Expanded
    {
        get => _expanded;
        set
        {
            _expanded = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Узел только для чтения?
    /// </summary>
    [DefaultValue (false)]
    public virtual bool ReadOnly
    {
        get => _readOnly;
        set
        {
            _readOnly = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Высота строки.
    /// </summary>
    public virtual int Height
    {
        get
        {
            var result = _height;
            if (result <= 0)
            {
                result = Grid?.LineHeight ?? Font.Height + 4;
            }

            return result;
        }
        set
        {
            _height = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Заголовок узла.
    /// </summary>
    public virtual string? Title
    {
        get => _title;
        set
        {
            _title = value;
            _UpdateGrid();
        }
    }

    /// <summary>
    /// Координата верхней части узла.
    /// </summary>
    public int Top { get; internal set; }

    /// <summary>
    /// Координата нижней части узла.
    /// </summary>
    public int Bottom => Top + Height;

    /// <summary>
    /// Тип редактора, связанного с данным узлом.
    /// </summary>
    public virtual Type EditorType
    {
        get => _editorType;
        set
        {
            if (!value.IsSubclassOf (typeof (TreeGridEditor)))
            {
                throw new ArgumentException ("value");
            }

            _editorType = value;
        }
    }

    /// <summary>
    /// Хранящиеся в узле данные.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TreeGridDataCollection Data { get; private set; }

    /// <summary>
    /// Ссылка на грид, которому принадлежит данный узел.
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TreeGrid? Grid { get; internal set; }

    /// <summary>
    /// Родительский узел (если есть).
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TreeGridNode? Parent { get; internal set; }

    /// <summary>
    /// У данного узла имеются дочерние?
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public bool HasChildren => Nodes.Count != 0;

    /// <summary>
    /// Уровень узла (отсчет от 0).
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public int Level
    {
        get
        {
            var result = 0;
            for (var node = this; node.Parent is not null; node = node.Parent)
            {
                result++;
            }

            return result;
        }
    }

    /// <summary>
    /// Коллекция потомков.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TreeGridNodeCollection Nodes { get; }

    /// <summary>
    /// Границы данного узла.
    /// </summary>
    public Rectangle Bounds
    {
        get
        {
            var result = new Rectangle
                (
                    0,
                    Top,
                    Grid?.ClientSize.Width ?? 0,
                    Height
                );

            return result;
        }
    }

    /// <summary>
    /// Получение плоского индекса узла.
    /// </summary>
    public int FlatIndex { get; internal set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [DefaultValue (null)]
    public object? UserData { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridNode()
    {
        Enabled = true;

        Nodes = new TreeGridNodeCollection (null, this);
        Data = new TreeGridDataCollection (this);

        _backgroundColor = Color.White;
        _foregroundColor = Color.Black;
    }

    /// <summary>
    /// Конструктор с заголовком.
    /// </summary>
    /// <param name="title">Заголовок узла.</param>
    public TreeGridNode
        (
            string? title
        )
        : this()
    {
        _title = title;
    }

    /// <summary>
    /// Конструктор с заголовком и данными.
    /// </summary>
    /// <param name="title">Заголовок узла.</param>
    /// <param name="data">Данные, хранящиеся в узле.</param>
    public TreeGridNode
        (
            string title,
            params object?[] data
        )
        : this (title)
    {
        Data.AddRange (data);
    }

    #endregion

    #region Private members

    private Color _backgroundColor;
    private bool _checked;
    private bool _enabled;
    private bool _expanded;
    private Font? _font;
    private Color _foregroundColor;
    private Icon? _icon;
    private int _height;

    private bool _readOnly;

    private string? _title;

    private Type _editorType = typeof (TreeGridTextBox);

    /// <summary>
    /// Вызов события "перед началом редактирования".
    /// </summary>
    protected virtual void OnBeforeEdit()
    {
        BeforeEdit?.Invoke (this, EventArgs.Empty);
    }

    /// <summary>
    /// Вызов события "после окончания редактирования".
    /// </summary>
    protected virtual void OnAfterEdit() =>
        AfterEdit?.Invoke (this, EventArgs.Empty);

    /// <summary>
    /// Отрисовка строки.
    /// </summary>
    protected internal virtual void OnDrawRow
        (
            TreeGridDrawNodeEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        var grid = eventArgs.Grid;
        if (grid != null)
        {
            foreach (var column in grid.VisibleColumns)
            {
                var bounds = eventArgs.Bounds;
                bounds.X = column.Left;
                bounds.Width = column.Width;

                var cellEventArgs = new TreeGridDrawCellEventArgs
                {
                    Graphics = eventArgs.Graphics,
                    Grid = grid,
                    Node = eventArgs.Node,
                    Column = column,
                    Bounds = bounds,
                    State = eventArgs.State
                };

                column.OnDrawCell (cellEventArgs);
            }
        }
    }

    /// <summary>
    /// Отрисовка ячейки.
    /// </summary>
    protected internal virtual void OnDrawCell
        (
            TreeGridDrawCellEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        eventArgs.DrawBackground();
        eventArgs.DrawText();
        eventArgs.DrawSelection();
    }

    internal void _SetTreeGrid
        (
            TreeGrid? value
        )
    {
        Grid = value;
        Nodes._grid = value;
        foreach (var child in Nodes)
        {
            child._SetTreeGrid (value);
        }

        _UpdateGrid();
    }

    /// <summary>
    /// Отработка нажатия на клавишу.
    /// </summary>
    protected internal virtual void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        eventArgs.NotUsed();

        // TODO: нужно ли тут что-нибудь делать?
    }

    /// <summary>
    /// Отработка щелчка мышью по узлу.
    /// </summary>
    protected internal virtual void OnMouseClick
        (
            TreeGridMouseEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        eventArgs.Column?.OnMouseClick (eventArgs);
    }

    /// <summary>
    /// Отработка двойного щелчка мышью.
    /// </summary>
    protected internal virtual void OnMouseDoubleClick
        (
            TreeGridMouseEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        eventArgs.Column?.OnMouseDoubleClick (eventArgs);
    }

    /// <summary>
    /// Создание редактора в указанных координатах.
    /// </summary>
    protected internal virtual TreeGridEditor? CreateEditor
        (
            TreeGridColumn column,
            Rectangle bounds,
            string? initialValue
        )
    {
        Sure.NotNull (column);

        if (ReadOnly || !Enabled)
        {
            return null;
        }

        var result = (TreeGridEditor?) Activator.CreateInstance (EditorType);
        var control = result?.Control;
        if (control is not null)
        {
            control.Bounds = bounds;
            result!.SetValue (initialValue ?? (string?)Data.SafeGet (0));
            if (!string.IsNullOrEmpty (initialValue))
            {
                result.SelectText (initialValue.Length, 0);
            }

            control.PreviewKeyDown += _editor_PreviewKeyDown;
        }

        return result;
    }

    /// <summary>
    /// Принятие (части) данных по окончании редактирования.
    /// </summary>
    protected internal virtual void AcceptData
        (
            TreeGridEditor? editor,
            int index
        )
    {
        Sure.InRange (index, Data);

        if (editor is not null)
        {
            Data.SafeSet (index, editor.GetValue());
            _UpdateGrid();
        }
    }

    private void _editor_PreviewKeyDown
        (
            object? sender,
            PreviewKeyDownEventArgs eventArgs
        )
    {
        switch (eventArgs.KeyData)
        {
            case Keys.Enter:
                Grid?.EndEdit (true);
                Grid?.GotoLine (Grid.CurrentLine + 1);
                eventArgs.IsInputKey = false;
                break;

            case Keys.Escape:
                Grid?.EndEdit (false);
                eventArgs.IsInputKey = false;
                break;

            case Keys.Down:
                Grid?.EndEdit (true);
                Grid?.GotoLine (Grid.CurrentLine + 1);
                eventArgs.IsInputKey = false;
                break;

            case Keys.Up:
                Grid?.EndEdit (true);
                Grid?.GotoLine (Grid.CurrentLine - 1);
                eventArgs.IsInputKey = false;
                break;
        }
    }

    internal void _UpdateGrid()
    {
        Grid?.Invalidate();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение плоского списка, включающего данный узел
    /// со всеми его подчиненными.
    /// </summary>
    public List<TreeGridNode> Flatten()
    {
        var result = new List<TreeGridNode> { this };
        if (Expanded)
        {
            foreach (var child in Nodes)
            {
                result.AddRange (child.Flatten());
            }
        }

        return result;
    }

    /// <summary>
    /// Разворачивание/сворачивание данного узла и всех его подчиненных.
    /// </summary>
    public void ExpandAll
        (
            bool expand
        )
    {
        Expanded = expand;
        foreach (var node in Nodes)
        {
            node.ExpandAll (expand);
        }
    }

    /// <summary>
    /// Создание клона данного узла (с хранящимися в нем данными
    /// или без них).
    /// </summary>
    /// <remarks>
    /// Все подчиненные узлы, если таковые есть, также клонируются.
    /// </remarks>
    public TreeGridNode Clone
        (
            bool withData
        )
    {
        var result = (TreeGridNode) MemberwiseClone();

        result.Data = new TreeGridDataCollection (result);
        if (withData)
        {
            for (var i = 0; i < Data.Count; i++)
            {
                result.Data.SafeSet (i, Data.SafeGet (i));
            }
        }

        for (var i = 0; i < Nodes.Count; i++)
        {
            var node = Nodes[i];
            var clone = node.Clone (withData);
            result.Nodes.Add (clone);
        }

        result._SetTreeGrid (Grid);
        return result;
    }

    /// <summary>
    /// Выбираем данный узел.
    /// </summary>
    public void Select()
    {
        Grid?.GotoLine (FlatIndex);
    }

    /// <summary>
    /// Выброр первого редактируемого узла (если возможно).
    /// </summary>
    /// <remarks>
    /// Если не получилось выбрать редактируемый узел,
    /// может оказаться, что всё равно какой-нибудь узел выбран,
    /// но он не редактируемый.
    /// </remarks>
    public void SelectFirstEditable()
    {
        if (!HasChildren)
        {
            Select();
        }
        else
        {
            if (!ReadOnly)
            {
                Select();
            }
            else
            {
                foreach (var child in Nodes)
                {
                    if (!child.ReadOnly)
                    {
                        child.Select();
                        return;
                    }
                }

                Select();
            }
        }
    }

    /// <summary>
    /// Список всех подчиненных узлов.
    /// </summary>
    public List<TreeGridNode> GetAllSubNodes()
    {
        var result = new List<TreeGridNode> (Nodes);
        result.AddRange (Nodes.SelectMany (_ => _.GetAllSubNodes()));

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Title.ToVisibleString();
    }

    #endregion
}
