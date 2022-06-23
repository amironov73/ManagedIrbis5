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

    #endregion
}
