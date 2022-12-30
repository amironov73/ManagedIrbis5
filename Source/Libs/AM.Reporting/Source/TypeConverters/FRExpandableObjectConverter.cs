// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Reporting.TypeConverters
{
    /// <summary>
    /// Provides a type converter for a property representing an expandable object.
    /// </summary>
    public class FRExpandableObjectConverter : TypeConverter
    {
        #region Public Methods

        /// <inheritdoc/>
        public override bool CanConvertTo (ITypeDescriptorContext context,
            Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return true;
            }

            return base.CanConvertTo (context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType)
        {
            if (value != null && destinationType == typeof (string))
            {
                return "(" + value.GetType().Name + ")";
            }

            return null;
        }

        /// <inheritdoc/>
        public override PropertyDescriptorCollection GetProperties (ITypeDescriptorContext context,
            object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties (value, attributes);
        }

        /// <inheritdoc/>
        public override bool GetPropertiesSupported (ITypeDescriptorContext context)
        {
            return true;
        }

        #endregion Public Methods
    }
}
