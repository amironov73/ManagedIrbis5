// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DataGridViewExtensions.cs -- методы расширения для DataGridView
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="DataGridView"/>.
/// </summary>
public static class DataGridViewExtensions
{
    #region Public methods

    /// <summary>
    /// Пользователь может добавлять строки в грид?
    /// </summary>
    public static TDataGridView AllowUserToAddRows<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool enable
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.AllowUserToAddRows = enable;

        return dataGrid;
    }

    /// <summary>
    /// Пользователь может удалять строки из грида?
    /// </summary>
    public static TDataGridView AllowUserToDeleteRows<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool enable
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.AllowUserToDeleteRows = enable;

        return dataGrid;
    }

    /// <summary>
    /// Пользователь может переупорядочивать столбцы в гриде?
    /// </summary>
    public static TDataGridView AllowUserToOrderColumns<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool enable
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.AllowUserToOrderColumns = enable;

        return dataGrid;
    }

    /// <summary>
    /// Пользователь может менять размер столбцов в гриде?
    /// </summary>
    public static TDataGridView AllowUserToResizeColumns<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool enable
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.AllowUserToResizeColumns = enable;

        return dataGrid;
    }

    /// <summary>
    /// Пользователь может менять размер строк в гриде?
    /// </summary>
    public static TDataGridView AllowUserToResizeRows<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool enable
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.AllowUserToResizeRows = enable;

        return dataGrid;
    }

    /// <summary>
    /// Столбцы создаются автоматически?
    /// </summary>
    public static TDataGridView AutoGenerateColumns<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool enable
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.AutoGenerateColumns = enable;

        return dataGrid;
    }

    /// <summary>
    /// Режим автоматического изменения размеров столбцов.
    /// </summary>
    public static TDataGridView AutoSizeColumnsMode<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewAutoSizeColumnsMode mode
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.Defined (mode);

        dataGrid.AutoSizeColumnsMode = mode;

        return dataGrid;
    }

    /// <summary>
    /// Режим автоматического изменения размеров строк.
    /// </summary>
    public static TDataGridView AutoSizeRowsMode<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewAutoSizeRowsMode mode
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.Defined (mode);

        dataGrid.AutoSizeRowsMode = mode;

        return dataGrid;
    }

    /// <summary>
    /// Стиль границ ячеек.
    /// </summary>
    public static TDataGridView CellBorderStyle<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellBorderStyle borderStyle
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.Defined (borderStyle);

        dataGrid.CellBorderStyle = borderStyle;

        return dataGrid;
    }

    /// <summary>
    /// Добавление колонок.
    /// </summary>
    public static TDataGridView Columns<TDataGridView>
        (
            this TDataGridView dataGrid,
            params DataGridViewColumn[] columns
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (columns);

        dataGrid.AutoGenerateColumns = false;
        dataGrid.Columns.AddRange (columns);

        return dataGrid;
    }

    /// <summary>
    /// Комфортабельные (для меня!) настройки.
    /// </summary>
    public static TDataGridView ComfortableMode<TDataGridView>
        (
            this TDataGridView dataGrid
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.AllowUserToOrderColumns = false;
        dataGrid.AllowUserToResizeColumns = false;
        dataGrid.AllowUserToResizeRows = false;
        dataGrid.AutoGenerateColumns = false;
        dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dataGrid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        dataGrid.EnableHeadersVisualStyles = true;
        dataGrid.MultiSelect = false;
        dataGrid.RowHeadersVisible = false;
        dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGrid.ShowCellErrors = true;
        dataGrid.ShowCellToolTips = true;
        dataGrid.ShowEditingIcon = true;
        dataGrid.ShowRowErrors = true;

        return dataGrid;
    }

    /// <summary>
    /// Имя свойства, поставляющего данные.
    /// </summary>
    public static TDataGridView DataMember<TDataGridView>
        (
            this TDataGridView dataGrid,
            string dataMember
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNullNorEmpty (dataMember);

        dataGrid.DataMember = dataMember;

        return dataGrid;
    }

    /// <summary>
    /// Объект-поставщик данных.
    /// </summary>
    public static TDataGridView DataSource<TDataGridView>
        (
            this TDataGridView dataGrid,
            object dataSource
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (dataSource);

        dataGrid.DataSource = dataSource;

        return dataGrid;
    }

    /// <summary>
    /// Как происходит переход в режим редактирования
    /// содержимого ячейки.
    /// </summary>
    public static TDataGridView EditMode<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewEditMode editMode
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.EditMode = editMode;

        return dataGrid;
    }

    /// <summary>
    /// Как происходит переход в режим редактирования
    /// содержимого ячейки -- по началу набора.
    /// </summary>
    public static TDataGridView EditModeKeystroke<TDataGridView>
        (
            this TDataGridView dataGrid
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.EditMode = DataGridViewEditMode.EditOnKeystroke;

        return dataGrid;
    }

    /// <summary>
    /// Задание цвета сетки.
    /// </summary>
    public static TDataGridView GridColor<TDataGridView>
        (
            this TDataGridView dataGrid,
            Color gridColor
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.GridColor = gridColor;

        return dataGrid;
    }

    /// <summary>
    /// Включение/отключение режима множественного выбора.
    /// </summary>
    public static TDataGridView MultiSelect<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool multiSelect
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.MultiSelect = multiSelect;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellClick"/>.
    /// </summary>
    public static TDataGridView OnCellClick<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellClick += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellDoubleClick"/>.
    /// </summary>
    public static TDataGridView OnCellDoubleClick<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellDoubleClick += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellEnter"/>.
    /// </summary>
    public static TDataGridView OnCellEnter<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellEnter += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellLeave"/>.
    /// </summary>
    public static TDataGridView OnCellLeave<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellLeave += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellMouseClick"/>.
    /// </summary>
    public static TDataGridView OnCellMouseClick<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellMouseEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellMouseClick += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellMouseDoubleClick"/>.
    /// </summary>
    public static TDataGridView OnCellMouseDoubleClick<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellMouseEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellMouseDoubleClick += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellPainting"/>.
    /// </summary>
    public static TDataGridView OnCellPainting<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellPaintingEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellPainting += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellParsing"/>.
    /// </summary>
    public static TDataGridView OnCellParsing<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellParsingEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellParsing += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellValueChanged"/>.
    /// </summary>
    public static TDataGridView OnCellValueChanged<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellValueChanged += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CellValueNeeded"/>.
    /// </summary>
    public static TDataGridView OnCellValueNeeded<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellValueEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CellValueNeeded += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.CurrentCellChanged"/>.
    /// </summary>
    public static TDataGridView OnCurrentCellChanged<TDataGridView>
        (
            this TDataGridView dataGrid,
            EventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.CurrentCellChanged += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.NewRowNeeded"/>.
    /// </summary>
    public static TDataGridView OnNewRowNeeded<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewRowEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.NewRowNeeded += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.RowEnter"/>.
    /// </summary>
    public static TDataGridView OnRowEnter<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.RowEnter += handler;

        return dataGrid;
    }

    /// <summary>
    /// Подписка на событие <see cref="DataGridView.RowLeave"/>.
    /// </summary>
    public static TDataGridView OnRowLeave<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewCellEventHandler handler
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (handler);

        dataGrid.RowLeave += handler;

        return dataGrid;
    }

    /// <summary>
    /// Включение/отключение режима только для чтения.
    /// </summary>
    public static TDataGridView ReadOnly<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool readOnly = true
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.ReadOnly = readOnly;

        return dataGrid;
    }

    /// <summary>
    /// Включение/отключение заголовков строк.
    /// </summary>
    public static TDataGridView RowHeadersVisible<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool visible
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.RowHeadersVisible = visible;

        return dataGrid;
    }

    /// <summary>
    /// Задание шаблона строки.
    /// </summary>
    public static TDataGridView RowTemplate<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewRow template
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.NotNull (template);

        dataGrid.RowTemplate = template;

        return dataGrid;
    }

    /// <summary>
    /// Задание режима выбора ячеек/строк/столбцов.
    /// </summary>
    public static TDataGridView SelectionMode<TDataGridView>
        (
            this TDataGridView dataGrid,
            DataGridViewSelectionMode selectionMode
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);
        Sure.Defined (selectionMode);

        dataGrid.SelectionMode = selectionMode;

        return dataGrid;
    }

    /// <summary>
    /// Задание режима выбора всей строки сразу.
    /// </summary>
    public static TDataGridView SelectionModeFullRow<TDataGridView>
        (
            this TDataGridView dataGrid
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        return dataGrid;
    }

    /// <summary>
    /// Включение/выключение режима отображения ошибок для ячеек.
    /// </summary>
    public static TDataGridView ShowCellErrors<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool show = true
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.ShowCellErrors = show;

        return dataGrid;
    }

    /// <summary>
    /// Включение/выключение режима отображения тултипов для ячеек.
    /// </summary>
    public static TDataGridView ShowCellToolTips<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool show = true
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.ShowCellToolTips = show;

        return dataGrid;
    }

    /// <summary>
    /// Включение/выключение режима отображения иконки, означающей
    /// редактирование данных в строке.
    /// </summary>
    public static TDataGridView ShowEditingIcon<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool show = true
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.ShowEditingIcon = show;

        return dataGrid;
    }

    /// <summary>
    /// Включение/выключение режима отображения ошибок в строках
    /// таблицы.
    /// </summary>
    public static TDataGridView ShowRowErrors<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool show = true
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.ShowRowErrors = show;

        return dataGrid;
    }

    /// <summary>
    /// Включение/выключение стандартной отработки клавиши <c>TAB</c>
    /// (переход к следующему контролу, а не ячейке).
    /// </summary>
    public static TDataGridView StandardTab<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool standardTab = true
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.StandardTab = standardTab;

        return dataGrid;
    }

    /// <summary>
    /// Включение/выключение виртуального режима.
    /// </summary>
    public static TDataGridView VirtualMode<TDataGridView>
        (
            this TDataGridView dataGrid,
            bool virtualMode = true
        )
        where TDataGridView: DataGridView
    {
        Sure.NotNull (dataGrid);

        dataGrid.VirtualMode = virtualMode;

        return dataGrid;
    }

    #endregion
}
