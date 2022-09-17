// ReSharper disable CheckNamespace
// ReSharper disable NonReadonlyMemberInGetHashCode

using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

/// <summary>
///     This attribute is a "query" attribute.  It is
///     an attribute that causes the type description provider
///     to narrow the scope of returned properties.  It differs
///     from normal attributes in that it cannot actually be
///     placed on a class as metadata and that the filter mechanism
///     is code rather than static metadata.
/// </summary>
[AttributeUsage (AttributeTargets.Property | AttributeTargets.Method)]
public sealed class PropertyFilterAttribute
    : Attribute
{
    //------------------------------------------------------
    //
    //  Constructors
    //
    //------------------------------------------------------

    #region Constructors

    /// <summary>
    ///     Creates a new attribute.
    /// </summary>
    public PropertyFilterAttribute (PropertyFilterOptions filter)
    {
        Filter = filter;
    }

    #endregion Constructors

    //------------------------------------------------------
    //
    //  Public Methods
    //
    //------------------------------------------------------

    #region Public Methods

    /// <inheritdoc cref="Attribute.Equals(object?)"/>
    public override bool Equals (object? value)
    {
        return value is PropertyFilterAttribute a && a.Filter.Equals (Filter);
    }

    /// <inheritdoc cref="Attribute.GetHashCode"/>
    public override int GetHashCode()
    {
        return Filter.GetHashCode();
    }

    /// <inheritdoc cref="Attribute.Match"/>
    public override bool Match (object? value)
    {
        if (value is not PropertyFilterAttribute a)
        {
            return false;
        }

        return ((Filter & a.Filter) == Filter);
    }

    #endregion Public Methods

    //------------------------------------------------------
    //
    //  Public Operators
    //
    //------------------------------------------------------

    //------------------------------------------------------
    //
    //  Public Properties
    //
    //------------------------------------------------------

    #region Public Properties

    /// <summary>
    ///     The filter value passed into the constructor.
    /// </summary>
    public PropertyFilterOptions Filter { get; }

    #endregion Public Properties

    //------------------------------------------------------
    //
    //  Public Events
    //
    //------------------------------------------------------

    //------------------------------------------------------
    //
    //  Public Fields
    //
    //------------------------------------------------------

    #region Public Fields

    /// <summary>
    ///     Attributes may declare a Default field that indicates
    ///     what should be done if the attribute is not defined.
    ///     Our default is to return all properties.
    /// </summary>
    [SuppressMessage ("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
    public static readonly PropertyFilterAttribute
        Default = new PropertyFilterAttribute (PropertyFilterOptions.All);

    #endregion Public Fields
}
