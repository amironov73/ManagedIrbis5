// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* BooleanTypeConverter.cs -- конвертер для булевых значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Конвертер для булевых значений с русскими словами "Да" и "Нет".
    /// </summary>
    public sealed class BooleanTypeConverter
        : BooleanConverter
    {
        #region TypeConverter members

        /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,object,System.Type)"/>
        public override object ConvertTo
            (
                ITypeDescriptorContext? context,
                CultureInfo? culture,
                object? value,
                Type destType
            )
        {
            return (bool) (value ?? false) ? "Да" : "Нет";
        }

        /// <inheritdoc cref="TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,object)"/>
        public override object ConvertFrom
            (
                ITypeDescriptorContext? context,
                CultureInfo? culture,
                object value
            )
        {
            return string.Compare((string) value, "Да", StringComparison.OrdinalIgnoreCase) == 0;
        }

        #endregion

    }
}
