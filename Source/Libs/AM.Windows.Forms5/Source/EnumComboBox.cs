// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion

/* EnumComboBox.cs -- ComboBox, отображающий элементы заданного перечисления
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM.Reflection;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="ComboBox"/>, отображающий элементы заданного перечисления
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public class EnumComboBox
    : ComboBox
{
    #region Properties

    private Type? _enumType;

    /// <summary>
    /// Получение и установка типа перечисления.
    /// </summary>
    [System.ComponentModel.DefaultValue (null)]
    [System.ComponentModel.TypeConverter (typeof (EnumTypeConverter))]
    public Type? EnumType
    {
        get => _enumType;
        set => _SetEnumType (value);
    }

    /// <summary>
    /// Получение и установка текущего выбранного значения.
    /// </summary>
    public int? Value
    {
        get
        {
            int? result = null;
            var member = (EnumMemberInfo?) SelectedItem;
            if (member != null)
            {
                result = member.Value;
            }

            return result;
        }
        set
        {
            if (value == null)
            {
                SelectedItem = null;
            }
            else
            {
                foreach (EnumMemberInfo info in Items)
                {
                    if (info.Value == value)
                    {
                        SelectedItem = info;
                        break;
                    }
                }
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EnumComboBox()
    {
        _SetupControl();
    }

    #endregion

    #region Private members

    private void _SetupControl()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    private void _SetEnumType
        (
            Type? enumType
        )
    {
        _enumType = enumType;
        var members = enumType is null
            ? Array.Empty<EnumMemberInfo>()
            : EnumMemberInfo.Parse (enumType);
        Items.Clear();
        Items.AddRange (members);
    }

    #endregion
}
