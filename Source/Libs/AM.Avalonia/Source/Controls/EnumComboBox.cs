// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EnumComboBox.cs -- комбобокс, позволяющий выбрать элемент перечисления
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Controls;
using Avalonia.Styling;

#endregion

#nullable enable

namespace Avalonia.NETCoreApp5;

/// <summary>
/// Комбобокс, позволяющий выбрать элемент перечисления.
/// </summary>
public sealed class EnumComboBox
    : ComboBox, IStyleable
{
    #region Properties

    /// <summary>
    /// Описание свойства "тип перечисления".
    /// </summary>
    public static StyledProperty<Type?> EnumTypeProperty
        = AvaloniaProperty.Register<EnumComboBox, Type?> (nameof (EnumType));

    /// <summary>
    /// Тип перечисления.
    /// </summary>
    public Type? EnumType
    {
        get => _enumType;
        set
        {
            if (value is not null && !value.IsEnum)
            {
                throw new ArgumentException ("Type is not enum");
            }

            FillItems (value);
            SetAndRaise (EnumTypeProperty, ref _enumType, value);
        }
    }

    #endregion

    private Type? _enumType;

    private void FillItems (Type? enumType)
    {
        if (enumType is null)
        {
            Items = null;
            return;
        }

        Items = Enum.GetValues (enumType);
    }

    /// <inheritdoc cref="IStyleable.StyleKey"/>
    Type IStyleable.StyleKey => typeof (ComboBox);
}
