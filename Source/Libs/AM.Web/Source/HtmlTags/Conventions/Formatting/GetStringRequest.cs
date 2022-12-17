// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* GetStringRequest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Formatting;

#region Using directives

using Reflection;

#endregion

/// <summary>
///
/// </summary>
public class GetStringRequest
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public Type? OwnerType { get; }

    /// <summary>
    ///
    /// </summary>
    public Type? PropertyType
    {
        get
        {
            if (_propertyType == null && Property != null)
            {
                return Property.PropertyType;
            }

            return _propertyType;
        }
        set => _propertyType = value;
    }

    /// <summary>
    ///
    /// </summary>
    public PropertyInfo? Property { get; }

    /// <summary>
    ///
    /// </summary>
    public object? RawValue { get; }

    /// <summary>
    ///
    /// </summary>
    public string? Format { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="property"></param>
    /// <param name="rawValue"></param>
    public GetStringRequest
        (
            PropertyInfo property,
            object? rawValue
        )
        : this (new SingleProperty (property), rawValue, null, null, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="rawValue"></param>
    /// <param name="locator"></param>
    /// <param name="format"></param>
    /// <param name="ownerType"></param>
    public GetStringRequest
        (
            Accessor? accessor,
            object? rawValue,
            Func<Type, object>? locator,
            string? format,
            Type? ownerType
        )
    {
        Locator = locator;
        if (accessor != null)
        {
            Property = accessor.InnerProperty;
        }

        RawValue = rawValue;

        if (Property != null)
        {
            PropertyType = Property.PropertyType;
        }
        else if (RawValue != null)
        {
            PropertyType = RawValue.GetType();
        }

        if (ownerType == null)
        {
            if (accessor != null)
            {
                OwnerType = accessor.OwnerType;
            }
            else if (Property != null)
            {
                OwnerType = Property.DeclaringType;
            }
        }
        else
        {
            OwnerType = ownerType;
        }

        Format = format;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ownerType"></param>
    /// <param name="property"></param>
    /// <param name="rawValue"></param>
    /// <param name="format"></param>
    /// <param name="propertyType"></param>
    public GetStringRequest
        (
            Type? ownerType,
            PropertyInfo? property,
            object? rawValue,
            string? format,
            Type propertyType
        )
    {
        OwnerType = ownerType;
        Property = property;
        RawValue = rawValue;
        Format = format;
        PropertyType = propertyType;
    }

    #endregion

    #region Private members

    private Type? _propertyType;

    // Yes, I made this internal.  Don't necessarily want it in the public interface,
    // but needs to be "settable"
    internal Func<Type, object>? Locator { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public string WithFormat (string format) => string.Format (format, RawValue);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public GetStringRequest GetRequestForNullableType()
    {
        return new (OwnerType, Property, RawValue, Format, PropertyType!.GetInnerTypeFromNullable())
        {
            Locator = Locator
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public GetStringRequest GetRequestForElementType()
    {
        return new (OwnerType, Property, RawValue, Format, PropertyType!.GetElementType()!)
        {
            Locator = Locator,
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>() => (T)Locator! (typeof (T));

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals
        (
            GetStringRequest other
        )
    {
        if (ReferenceEquals (null, other))
        {
            return false;
        }

        if (ReferenceEquals (this, other))
        {
            return true;
        }

        return Equals (other.OwnerType, OwnerType) && Equals (other.Property, Property) &&
               Equals (other.RawValue, RawValue) && Equals (other.Format, Format);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        if (ReferenceEquals (null, obj))
        {
            return false;
        }

        if (ReferenceEquals (this, obj))
        {
            return true;
        }

        if (obj.GetType() != typeof (GetStringRequest))
        {
            return false;
        }

        return Equals ((GetStringRequest)obj);
    }
    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        unchecked
        {
            int result = OwnerType?.GetHashCode() ?? 0;
            result = (result * 397) ^ (Property?.GetHashCode() ?? 0);
            result = (result * 397) ^ (RawValue?.GetHashCode() ?? 0);
            result = (result * 397) ^ (Format?.GetHashCode() ?? 0);
            return result;
        }
    }

    #endregion
}
