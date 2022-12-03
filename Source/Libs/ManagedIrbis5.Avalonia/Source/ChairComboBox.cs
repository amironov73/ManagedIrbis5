// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ChairComboBox.cs -- выпадающий список кафедр обслуживания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Avalonia.Controls;
using Avalonia.Styling;

using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Выпадающий список кафедр обслуживания.
/// </summary>
public sealed class ChairComboBox
    : ComboBox, IStyleable
{
    #region Properties

    /// <summary>
    /// Выбранная кафедра.
    /// </summary>
    public ChairInfo? SelectedChair => SelectedItem as ChairInfo;

    #endregion

    #region Public methods

    /// <summary>
    /// Заполнение контрола списком кафедр.
    /// </summary>
    public void FillWithChairs
        (
            ISyncProvider connection,
            bool addAllItem = false
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        Items = ChairInfo.Read
            (
                connection,
                ChairInfo.ChairMenu,
                addAllItem
            );
    }

    /// <summary>
    /// Заполнение контрола списком мест обслуживания.
    /// </summary>
    public void FillWithPlaces
        (
            ISyncProvider connection,
            bool addAllItem = false
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        Items = ChairInfo.Read
            (
                connection,
                ChairInfo.PlacesMenu,
                addAllItem
            );
    }

    #endregion

    #region Private members

    /// <inheritdoc cref="IStyleable.StyleKey"/>
    Type IStyleable.StyleKey => typeof (ComboBox);

    #endregion
}
