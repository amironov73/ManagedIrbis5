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
    internal class FillConverter : TypeConverter
    {
        #region Public Methods

        public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }

            return base.CanConvertFrom (context, sourceType);
        }

        public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return true;
            }

            return base.CanConvertTo (context, destinationType);
        }

        public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string className)
            {
                FillBase result = null;
                switch (className)
                {
                    case "Solid":
                        result = new SolidFill();
                        break;

                    case "LinearGradient":
                        result = new LinearGradientFill();
                        break;

                    case "PathGradient":
                        result = new PathGradientFill();
                        break;

                    case "Hatch":
                        result = new HatchFill();
                        break;

                    case "Glass":
                        result = new GlassFill();
                        break;
                    case "Texture":
                        result = new TextureFill();
                        break;
                }

                return result;
            }

            return base.ConvertFrom (context, culture, value);
        }

        public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                if (value is not FillBase fill)
                {
                    return "";
                }

                return fill.Name;
            }

            return base.ConvertTo (context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties (ITypeDescriptorContext context,
            object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties (value, attributes);
        }

        public override bool GetPropertiesSupported (ITypeDescriptorContext context)
        {
            return true;
        }

        #endregion Public Methods
    }
}
