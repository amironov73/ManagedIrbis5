// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable StringLiteralTypo

/* DatabaseComboBox.cs -- выпадающий список баз данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Avalonia.Controls;
using Avalonia.Styling;

using JetBrains.Annotations;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Выпадающий список баз данных.
/// </summary>
[PublicAPI]
public sealed class DatabaseComboBox
    : ComboBox, IStyleable
{
    #region Properties

    /// <summary>
    /// Выбранная пользователем база данных.
    /// </summary>
    public DatabaseInfo? SelectedDatabase => SelectedItem as DatabaseInfo;

    #endregion

    #region Public methods

    /// <summary>
    /// Заполнение контрола списком баз данных.
    /// </summary>
    public void FillWithDatabases
        (
            ISyncProvider connection,
            string listFile
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (listFile);
        connection.EnsureConnected();

        ItemsSource = connection.ListDatabases (listFile);
    }

    #endregion

    #region Private members

    /// <inheritdoc cref="IStyleable.StyleKey"/>
    Type IStyleable.StyleKey => typeof (ComboBox);

    #endregion
}
