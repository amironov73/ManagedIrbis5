// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global

/* TreeGridColumn.cs -- колонка грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Колонка грида.
/// </summary>
[XmlRoot ("column")]
[System.ComponentModel.DesignerCategory ("Code")]
public abstract class TreeGridColumn
    : Component,
    IXmlSerializable
{
    #region Events

    /// <summary>
    /// Событие отрисовки ячейки.
    /// </summary>
    public event EventHandler<TreeGridDrawCellEventArgs>? DrawCell;

    #endregion

    #region Properties

    private const int DefaultWidth = 100;

    /// <summary>
    /// Заголовок колонки.
    /// </summary>
    [DefaultValue (null)]
    public string? Title
    {
        get => _title;
        set
        {
            _title = value;
            Update();
        }
    }

    /// <summary>
    /// Фактор заполнения.
    /// </summary>
    [DefaultValue (0)]
    public int FillFactor
    {
        get => _fillFactor;
        set
        {
            _fillFactor = value;
            Update();
        }
    }

    /// <summary>
    /// Иконка для колонки.
    /// </summary>
    [DefaultValue (null)]
    public Icon? Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            Update();
        }
    }

    /// <summary>
    /// Выравнивание данных в колонке.
    /// </summary>
    [DefaultValue (TreeGridAlignment.Near)]
    public TreeGridAlignment Alignment
    {
        get => _alignment;
        set
        {
            _alignment = value;
            Update();
        }
    }

    /// <summary>
    /// Колонка только для чтения
    /// (пользователь не может редактировать данные)?.
    /// </summary>
    [DefaultValue (false)]
    public bool ReadOnly
    {
        get => _readOnly;
        set
        {
            _readOnly = value;
            Update();
        }
    }

    /// <summary>
    /// Пользователь может менять ширину колонки?
    /// </summary>
    [DefaultValue (false)]
    public bool Resizeable
    {
        get => _resizeable;
        set
        {
            _resizeable = value;
            Update();
        }
    }

    /// <summary>
    /// Ширина колонки.
    /// </summary>
    [DefaultValue (DefaultWidth)]
    public int Width
    {
        get => _width;
        set
        {
            Sure.NonNegative (value, nameof (value));

            _width = value;
            Update();
        }
    }

    /// <summary>
    /// Редактируемая колонка?
    /// </summary>
    [Browsable (false)]
    [DefaultValue (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public virtual bool Editable => false;

    /// <summary>
    /// Тип редактора для ячеек колонки.
    /// По умолчанию не задан.
    /// </summary>
    [Browsable (false)]
    [DefaultValue (null)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public virtual Type? EditorType => null;

    /// <summary>
    /// Левая граница колонки.
    /// </summary>
    [Browsable (false)]
    [DefaultValue (0)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public int Left => _left;

    /// <summary>
    /// Правая граница колонки.
    /// </summary>
    [Browsable (false)]
    [DefaultValue (0)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public int Right => _right;

    /// <summary>
    /// Индекс колонки.
    /// </summary>
    [Browsable (false)]
    [DefaultValue (0)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public int Index => _index;

    /// <summary>
    /// Значение индекса в массиве данных по умолчанию.
    /// </summary>
    public const int DefaultDataIndex = -1;

    /// <summary>
    /// Индекс в массиве данных строки грида.
    /// </summary>
    [DefaultValue (DefaultDataIndex)]
    public int DataIndex { get; set; } = DefaultDataIndex;

    /// <summary>
    /// Ссылка на грид-владелец.
    /// </summary>
    [Browsable (false)]
    [DefaultValue (null)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TreeGrid? Grid => _grid;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию для внутреннего применения.
    /// </summary>
    protected TreeGridColumn()
        : this (null)
    {
        // пустое тело метода
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="grid">Грид, которому принадлежит колонка.</param>
    protected TreeGridColumn
        (
            TreeGrid? grid
        )
    {
        _width = DefaultWidth;
        _grid = grid;
    }

    #endregion

    #region Private members

    private TreeGridAlignment _alignment = TreeGridAlignment.Near;
    private Icon? _icon;
    private string? _title;
    private int _fillFactor;
    internal int _left;
    internal int _width;
    private bool _readOnly;
    private bool _resizeable;
    internal int _right;
    internal int _index;
    internal TreeGrid? _grid;

    /// <summary>
    /// Отрисовка заголовка колонки.
    /// </summary>
    protected internal virtual void OnDrawHeader
        (
            TreeGridDrawColumnHeaderEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        eventArgs.DrawBackground();
        eventArgs.DrawText();
    }

    /// <summary>
    /// Отрисовка ячейки колонки.
    /// </summary>
    protected internal virtual void OnDrawCell
        (
            TreeGridDrawCellEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        eventArgs.Node.ThrowIfNull().OnDrawCell (eventArgs);
        DrawCell?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Инвалидация грида для отрисовки внесенных изменений.
    /// </summary>
    protected internal void Update()
    {
        Grid?.Update();
    }

    /// <summary>
    /// Обработка клика мышью.
    /// </summary>
    protected internal virtual void OnMouseClick
        (
            TreeGridMouseEventArgs eventArgs
        )
    {
        eventArgs.NotUsed();

        // пустое тело метода
    }

    /// <summary>
    /// Обработка двойного клика мышью.
    /// </summary>
    protected internal virtual void OnMouseDoubleClick
        (
            TreeGridMouseEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        if (eventArgs.Node is { Enabled: true } node)
        {
            if (Editable && !ReadOnly)
            {
                var initialValue = Grid!.GetInitialValue (node, this);
                if (!Grid.BeginEdit (Index, initialValue))
                {
                    node.Expanded = !node.Expanded;
                }
            }
            else
            {
                node.Expanded = !node.Expanded;
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Начинает редактирование ячейки.
    /// </summary>
    /// <param name="initialValue">Начальное значение ячейки.</param>
    public virtual void BeginEdit
        (
            string? initialValue
        )
    {
        Grid?.BeginEdit (Index, initialValue);
    }

    /// <summary>
    /// Завершает редактирование ячейки.
    /// </summary>
    /// <param name="accept">Пользователь принял результат редактирования?</param>
    public virtual void EndEdit
        (
            bool accept
        )
    {
        Grid?.EndEdit (accept);
    }

    #endregion

    #region Implementation of IXmlSerializable

    /// <inheritdoc cref="IXmlSerializable.GetSchema"/>
    public XmlSchema? GetSchema() => null;

    /// <inheritdoc cref="IXmlSerializable.ReadXml"/>
    public void ReadXml
        (
            XmlReader reader
        )
    {
        throw new NotImplementedException();

        /*

        Title = reader.GetAttribute("title");
        FillFactor = reader.GetInt32("fill-factor");
        ReadOnly = reader.GetBoolean("read-only");
        Resizeable = reader.GetBoolean("resizeable");
        Alignment = reader
            .GetEnum<TreeGridAlignment>("alignment");
        Width = reader.GetInt32("width");
        reader.Read();
        if (reader.LocalName == "icon")
        {
            string base64 = reader.ReadString();
            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                Icon = new Icon(stream);
            }
            reader.Read();
            reader.Read();
        }

        */
    }

    /// <inheritdoc cref="IXmlSerializable.WriteXml"/>
    public void WriteXml
        (
            XmlWriter writer
        )
    {
        writer.WriteAttributeString ("title", Title);
        writer.WriteAttributeString
            (
                "fill-factor",
                FillFactor.ToString()
            );

        writer.WriteAttributeString
            (
                "read-only",
                ReadOnly.ToString()
            );

        writer.WriteAttributeString
            (
                "resizeable",
                Resizeable.ToString()
            );

        writer.WriteAttributeString
            (
                "alignment",
                Alignment.ToString()
            );

        writer.WriteAttributeString
            (
                "width",
                Width.ToString()
            );

        if (Icon is not null)
        {
            using var stream = new MemoryStream();
            Icon.Save (stream);
            writer.WriteStartElement ("icon");
            var bytes = stream.ToArray();
            writer.WriteBase64 (bytes, 0, bytes.Length);
            writer.WriteEndElement();
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{_title} ({_width})";
    }

    #endregion
}
