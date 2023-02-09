// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* PropertyControl.cs -- простой показ свойств объекта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Reflection;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Styling;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Простой показ свойств объекта.
/// </summary>
public class PropertyGrid
    : ReactiveUserControl<PropertyGrid.PropertyModel>
{
    #region Inner classes

    /// <summary>
    /// Модель данных свойства.
    /// </summary>
    public sealed class PropertyModel
        : ReactiveObject
    {
        /// <summary>
        /// Имя свойства.
        /// </summary>
        [Reactive]
        public string? Name { get; set; }

        /// <summary>
        /// Тип свойства.
        /// </summary>
        [Reactive]
        public string? Type { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        [Reactive]
        public object? Value { get; set; }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PropertyGrid()
    {
        var grayBackground = AvaloniaUtility.CreateControlTheme
            (
                typeof (DataGridCell),
                typeof (DataGridCell)
            );
        grayBackground?.Setters.Add (new Setter
            (
                BackgroundProperty,
                Brushes.LightGray
            ));

        _dataGrid = new DataGrid
        {
            IsReadOnly = true,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Columns =
            {
                new DataGridTextColumn
                {
                    Header = "Name",
                    IsReadOnly = true,
                    HeaderTemplate = BoldLabel(),
                    FontWeight = FontWeight.Bold,
                    Width = new DataGridLength (2, DataGridLengthUnitType.Star),
                    Binding = new Binding (nameof (PropertyModel.Name)),
                    CellTheme = grayBackground
                },

                new DataGridTextColumn
                {
                    Header = "Type",
                    IsReadOnly = true,
                    HeaderTemplate = BoldLabel(),
                    Width = new DataGridLength (2, DataGridLengthUnitType.Star),
                    Binding = new Binding (nameof (PropertyModel.Type)),
                    CellTheme = grayBackground
                },

                new DataGridTextColumn
                {
                    Header = "Value",
                    HeaderTemplate = BoldLabel(),
                    Width = new DataGridLength (3, DataGridLengthUnitType.Star),
                    Binding = new Binding (nameof (PropertyModel.Value))
                }
            }
        };

        FuncDataTemplate BoldLabel() =>
            new (typeof (object), (obj, _) => new Label
            {
                Content = obj,
                FontSize = 14,
                FontWeight = FontWeight.Bold
            });

        Content= _dataGrid;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Описание свойства "Выбранный объект".
    /// </summary>
    public static DirectProperty<PropertyGrid, object?> SelectedObjectProperty
        = AvaloniaProperty.RegisterDirect<PropertyGrid, object?>
            (
                nameof (SelectedObject),
                x => x._selectedObject,
                (x, v) => x._selectedObject = v
            );

    /// <summary>
    /// Выбранный объект.
    /// </summary>
    public object? SelectedObject
    {
        get => _selectedObject;
        set
        {
            SetAndRaise (SelectedObjectProperty, ref _selectedObject, value);
            _DiscoverProperties();
        }
    }

    #endregion

    #region Private members

    private object? _selectedObject;
    private readonly DataGrid _dataGrid;

    private void _DiscoverProperties()
    {
        _dataGrid.Items = null;
        if (_selectedObject is null)
        {
            return;
        }

        var list = new List<PropertyModel>();
        var propertyInfos = _selectedObject.GetType().GetProperties
            (
                BindingFlags.Instance | BindingFlags.Public
            );
        foreach (var info in propertyInfos)
        {
            var prop = new PropertyModel
            {
                Name = info.Name,
                Type = info.PropertyType.Name,
                Value = info.GetValue (_selectedObject)
            };
            list.Add (prop);
        }

        _dataGrid.Items = list.ToArray();
    }

    #endregion
}
