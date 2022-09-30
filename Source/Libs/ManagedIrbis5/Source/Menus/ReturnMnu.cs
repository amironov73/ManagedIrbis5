// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ReturnMnu.cs -- обертка над файлом RETURN.MNU
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Menus;

/// <summary>
/// Обертка над файлом RETURN.MNU.
/// </summary>
public sealed class ReturnMnu
{
    #region Constants

    /// <summary>
    /// Имя файла меню по умолчанию.
    /// </summary>
    public const string DefaultFileName = "return.mnu";

    #endregion

    #region Nested classes

    /// <summary>
    /// Отдельный элемент меню. Содержит дату и комментарий.
    /// </summary>
    public sealed class Item
    {
        #region Properties

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Комментарий в произвольной форме. Чаще всего пустой.
        /// </summary>
        public string? Comment { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Item()
        {
            // пустое тело конструктора
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Item
            (
                MenuEntry entry
            )
        {
            Sure.NotNull (entry);

            var code = entry.Code.ThrowIfNull();
            Comment = entry.Comment;
            if (code.StartsWith ("@"))
            {
                Date = DateTime.ParseExact
                    (
                        code [1..],
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture
                    );
            }
            else
            {
                Date = DateTime.Today.AddDays (int.Parse (code));
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            string.Create
                (
                    null,
                    $"{Date.ToString ("dd.MM.yyyy", CultureInfo.InvariantCulture)} {Comment}"
                );

        #endregion
    }

    #endregion

    #region Properties

    /// <summary>
    /// Элементы меню.
    /// </summary>
    public List<Item> Items { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReturnMnu
        (
            MenuFile menu
        )
    {
        Sure.NotNull (menu);

        Items = new List<Item>();
        foreach (var entry in menu.Entries)
        {
            Items.Add (new Item (entry));
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Чтение RETURN.MNU с сервера.
    /// </summary>
    public static ReturnMnu FromConnection
        (
            ISyncProvider connection,
            string fileName = DefaultFileName
        )
    {
        Sure.NotNull (connection);
        connection.CheckProviderState();
        Sure.NotNullNorEmpty (fileName);

        var specification = new FileSpecification
        {
            Path = IrbisPath.MasterFile,
            Database = StandardDatabases.Readers,
            FileName = DefaultFileName
        };
        var menu = MenuFile.ReadFromServer (connection, specification);
        menu = menu.ThrowIfNull();
        var result = new ReturnMnu (menu);

        return result;
    }

    /// <summary>
    /// Чтение RETURN.MNU из локального файла.
    /// </summary>
    public static ReturnMnu FromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var menu = MenuFile.ParseLocalFile (fileName);
        var result = new ReturnMnu (menu);

        return result;
    }

    #endregion
}
