// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* SemesterConverter.cs -- конвертер типа для перечисления семестров
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

using AM;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Конвертер типа для перечисления семестров.
    /// </summary>
    public class SemesterConverter
        : TypeConverter
    {
        #region TypeConverter members

        /// <inheritdoc cref="TypeConverter.CanConvertFrom(ITypeDescriptorContext,Type)"/>
        public override bool CanConvertFrom
            (
                ITypeDescriptorContext context,
                Type sourceType
            )
            => sourceType == typeof (string) || base.CanConvertFrom (context, sourceType);


        /// <inheritdoc cref="TypeConverter.CanConvertTo(ITypeDescriptorContext,Type)"/>
        public override bool CanConvertTo
            (
                ITypeDescriptorContext context,
                Type destinationType
            )
            => destinationType == typeof (string) || base.CanConvertTo (context, destinationType);

        /// <inheritdoc cref="TypeConverter.ConvertFrom(ITypeDescriptorContext,CultureInfo,object)"/>
        public override object ConvertFrom
            (
                ITypeDescriptorContext context,
                CultureInfo culture,
                object? value
            )
            => value switch
            {
                string text => SemesterUtility.Parse (text),

                _ => base.ConvertFrom (context, culture, value).ThrowIfNull()
            };

        /// <inheritdoc cref="TypeConverter.ConvertTo(ITypeDescriptorContext,CultureInfo,object,Type)"/>
        public override object ConvertTo
            (
                ITypeDescriptorContext context,
                CultureInfo culture,
                object? value,
                Type destinationType
            )
            => value is Semester semester
                ? SemesterUtility.ToString (semester)
                : base.ConvertTo (context, culture, value, destinationType)
                    .ThrowIfNull();

        #endregion

    } // class SemesterConverter

} // namespace Istu.BookSupply
