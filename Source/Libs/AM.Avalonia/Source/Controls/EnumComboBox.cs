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

using Avalonia;
using Avalonia.Controls;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Комбобокс, позволяющий выбрать элемент перечисления.
/// </summary>
[PublicAPI]
public sealed class EnumComboBox
    : ComboBox
{
    #region Properties

    /// <summary>
    /// Описание свойства "тип перечисления".
    /// </summary>
    public static readonly StyledProperty<Type?> EnumTypeProperty
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
            // TODO implement
            // SetAndRaise (EnumTypeProperty, ref _enumType, value);
            _enumType = value;
        }
    }

    #endregion

    #region Private members

    private Type? _enumType;

    private void FillItems
        (
            Type? enumType
        )
    {
        if (enumType is null)
        {
            ItemsSource = null;
            return;
        }

        ItemsSource = Enum.GetValues (enumType);
    }

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (ComboBox);

    #endregion
}
