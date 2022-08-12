// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimplestMarcEditor.cs -- простейший редактор MARC-записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Editors;

/// <summary>
/// Простейший редактор MARC-записей.
/// </summary>
public sealed partial class SimplestMarcEditor
    : UserControl
{
    #region Properties

    /// <summary>
    /// Whether the editor is read-only.
    /// </summary>
    public bool ReadOnly
    {
        get => _gridView.ReadOnly;
        set
        {
            _bindingNavigator.Enabled = !value;
            _gridView.ReadOnly = value;
        }
    }

    #endregion

    #region Constructions

    /// <summary>
    ///
    /// </summary>
    public SimplestMarcEditor()
    {
        InitializeComponent();
    }

    #endregion

    #region Private members

    private List<Field>? _originalFields;

    private List<FieldItem>? _items;

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка.
    /// </summary>
    public void Clear()
    {
        _originalFields = new List<Field>();
        _items = new List<FieldItem>();
        _bindingSource.DataSource = _items;
    }

    /// <summary>
    /// Get the fields.
    /// </summary>
    public void GetFields
        (
            IEnumerable<Field> collection
        )
    {
        /*

        //collection.BeginUpdate();
        collection.Clear();
        //collection.EnsureCapacity(_items.Count);

        if (_items is not null)
        {
            foreach (var item in _items)
            {
                var tag = item.Tag;
                var text = item.Text;

                var field = new Field(tag);
                field.DecodeBody(text);
                if (field.Verify(false))
                {
                    collection.Add(field);
                }
            }
        }

        */

        //collection.EndUpdate();
    }

    /// <summary>
    /// Set the fields.
    /// </summary>
    public void SetFields
        (
            IEnumerable<Field> collection
        )
    {
        // _originalFields = collection;
        var list = new List<FieldItem>();
        foreach (var field in collection)
        {
            var item = new FieldItem
            {
                Tag = field.Tag,
                Text = field.ToText()
            };
            list.Add (item);
        }

        _items = list;
        _bindingSource.DataSource = _items;
    }

    #endregion
}
