// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* JetBrainsAnnotations.cs -- аннотации JetBrains
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace JetBrains.Annotations;

/// <summary>
/// Indicates that IEnumarable, passed as parameter, is not enumerated.
/// </summary>
[AttributeUsage (AttributeTargets.Parameter)]
public sealed class NoEnumerationAttribute
    : Attribute
{
    // пустое тело класса
}

/// <summary>
/// Indicates that the integral value falls into the specified interval.
/// It's allowed to specify multiple non-intersecting intervals.
/// Values of interval boundaries are inclusive.
/// </summary>
/// <example><code>
/// void Foo([ValueRange(0, 100)] int value) {
///   if (value == -1) { // Warning: Expression is always 'false'
///     ...
///   }
/// }
/// </code></example>
[AttributeUsage (AttributeTargets.Parameter | AttributeTargets.Field
                                            | AttributeTargets.Property | AttributeTargets.Method |
                                            AttributeTargets.Delegate,
    AllowMultiple = true)]
public sealed class ValueRangeAttribute
    : Attribute
{
    /// <summary>
    ///
    /// </summary>
    public object From { get; }

    /// <summary>
    ///
    /// </summary>
    public object To { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public ValueRangeAttribute (long from, long to)
    {
        From = from;
        To = to;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public ValueRangeAttribute (ulong from, ulong to)
    {
        From = from;
        To = to;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public ValueRangeAttribute (long value)
    {
        From = To = value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public ValueRangeAttribute (ulong value)
    {
        From = To = value;
    }
}

/// <summary>
/// Indicates that the integral value never falls below zero.
/// </summary>
/// <example><code>
/// void Foo([NonNegativeValue] int value) {
///   if (value == -1) { // Warning: Expression is always 'false'
///     ...
///   }
/// }
/// </code></example>
[AttributeUsage (AttributeTargets.Parameter | AttributeTargets.Field
                                            | AttributeTargets.Property | AttributeTargets.Method
                                            | AttributeTargets.Delegate)]
public sealed class NonNegativeValueAttribute
    : Attribute
{
    // пустое тело класса
}

/// <summary>
/// Indicates whether the marked element should be localized.
/// </summary>
/// <example><code>
/// [LocalizationRequiredAttribute(true)]
/// class Foo {
///   string str = "my string"; // Warning: Localizable string
/// }
/// </code></example>
[AttributeUsage (AttributeTargets.All)]
public sealed class LocalizationRequiredAttribute
    : Attribute
{
    /// <summary>
    ///
    /// </summary>
    public LocalizationRequiredAttribute() : this (true)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="required"></param>
    public LocalizationRequiredAttribute (bool required)
    {
        Required = required;
    }

    /// <summary>
    ///
    /// </summary>
    public bool Required { get; }
}

/// <summary>
/// Indicates that the value of the marked type (or its derivatives)
/// cannot be compared using '==' or '!=' operators and <c>Equals()</c>
/// should be used instead. However, using '==' or '!=' for comparison
/// with <c>null</c> is always permitted.
/// </summary>
/// <example><code>
/// [CannotApplyEqualityOperator]
/// class NoEquality { }
///
/// class UsesNoEquality {
///   void Test() {
///     var ca1 = new NoEquality();
///     var ca2 = new NoEquality();
///     if (ca1 != null) { // OK
///       bool condition = ca1 == ca2; // Warning
///     }
///   }
/// }
/// </code></example>
[AttributeUsage (AttributeTargets.Interface | AttributeTargets.Class
                                            | AttributeTargets.Struct)]
public sealed class CannotApplyEqualityOperatorAttribute
    : Attribute
{
    // пустое тело класса
}

/// <summary>
/// Specifies the details of implicitly used symbol when it is marked
/// with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>.
/// </summary>
[Flags]
public enum ImplicitUseKindFlags
{
    /// <summary>
    ///
    /// </summary>
    Default = Access | Assign | InstantiatedWithFixedConstructorSignature,

    /// <summary>Only entity marked with attribute considered used.</summary>
    Access = 1,

    /// <summary>Indicates implicit assignment to a member.</summary>
    Assign = 2,

    /// <summary>
    /// Indicates implicit instantiation of a type with fixed constructor signature.
    /// That means any unused constructor parameters won't be reported as such.
    /// </summary>
    InstantiatedWithFixedConstructorSignature = 4,

    /// <summary>Indicates implicit instantiation of a type.</summary>
    InstantiatedNoFixedConstructorSignature = 8,
}

/// <summary>
/// Specifies what is considered to be used implicitly when marked
/// with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>.
/// </summary>
[Flags]
public enum ImplicitUseTargetFlags
{
    /// <summary>
    ///
    /// </summary>
    Default = Itself,

    /// <summary>
    ///
    /// </summary>
    Itself = 1,

    /// <summary>Members of the type marked with the attribute are considered used.</summary>
    Members = 2,

    /// <summary> Inherited entities are considered used. </summary>
    WithInheritors = 4,

    /// <summary>Entity marked with the attribute and all its members considered used.</summary>
    WithMembers = Itself | Members
}

/// <summary>
/// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
/// so this symbol will be ignored by usage-checking inspections. <br/>
/// You can use <see cref="ImplicitUseKindFlags"/> and <see cref="ImplicitUseTargetFlags"/>
/// to configure how this attribute is applied.
/// </summary>
/// <example><code>
/// [UsedImplicitly]
/// public class TypeConverter {}
///
/// public class SummaryData
/// {
///   [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
///   public SummaryData() {}
/// }
///
/// [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Default)]
/// public interface IService {}
/// </code></example>
[AttributeUsage (AttributeTargets.All)]
public sealed class UsedImplicitlyAttribute
    : Attribute
{
    /// <summary>
    ///
    /// </summary>
    public UsedImplicitlyAttribute()
        : this (ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="useKindFlags"></param>
    public UsedImplicitlyAttribute (ImplicitUseKindFlags useKindFlags)
        : this (useKindFlags, ImplicitUseTargetFlags.Default)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="targetFlags"></param>
    public UsedImplicitlyAttribute (ImplicitUseTargetFlags targetFlags)
        : this (ImplicitUseKindFlags.Default, targetFlags)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="useKindFlags"></param>
    /// <param name="targetFlags"></param>
    public UsedImplicitlyAttribute (ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
    {
        UseKindFlags = useKindFlags;
        TargetFlags = targetFlags;
    }

    /// <summary>
    ///
    /// </summary>
    public ImplicitUseKindFlags UseKindFlags { get; }

    /// <summary>
    ///
    /// </summary>
    public ImplicitUseTargetFlags TargetFlags { get; }
}

/// <summary>
/// Can be applied to attributes, type parameters, and parameters of a type assignable from <see cref="System.Type"/> .
/// When applied to an attribute, the decorated attribute behaves the same as <see cref="UsedImplicitlyAttribute"/>.
/// When applied to a type parameter or to a parameter of type <see cref="System.Type"/>,
/// indicates that the corresponding type is used implicitly.
/// </summary>
[AttributeUsage (AttributeTargets.Class | AttributeTargets.GenericParameter | AttributeTargets.Parameter)]
public sealed class MeansImplicitUseAttribute : Attribute
{
    /// <summary>
    ///
    /// </summary>
    public MeansImplicitUseAttribute()
        : this (ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="useKindFlags"></param>
    public MeansImplicitUseAttribute (ImplicitUseKindFlags useKindFlags)
        : this (useKindFlags, ImplicitUseTargetFlags.Default)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="targetFlags"></param>
    public MeansImplicitUseAttribute (ImplicitUseTargetFlags targetFlags)
        : this (ImplicitUseKindFlags.Default, targetFlags)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="useKindFlags"></param>
    /// <param name="targetFlags"></param>
    public MeansImplicitUseAttribute (ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
    {
        UseKindFlags = useKindFlags;
        TargetFlags = targetFlags;
    }

    /// <summary>
    ///
    /// </summary>
    [UsedImplicitly]
    public ImplicitUseKindFlags UseKindFlags { get; }

    /// <summary>
    ///
    /// </summary>
    [UsedImplicitly]
    public ImplicitUseTargetFlags TargetFlags { get; }
}

/// <summary>
/// This attribute is intended to mark publicly available API,
/// which should not be removed and so is treated as used.
/// </summary>
[MeansImplicitUse (ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage (AttributeTargets.All, Inherited = false)]
public sealed class PublicAPIAttribute
    : Attribute
{
    /// <summary>
    ///
    /// </summary>
    public PublicAPIAttribute()
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    public PublicAPIAttribute (string comment)
    {
        Comment = comment;
    }

    /// <summary>
    ///
    /// </summary>
    public string? Comment { get; }
}

/// <summary>
/// Indicates how method, constructor invocation, or property access
/// over collection type affects the contents of the collection.
/// When applied to a return value of a method indicates if the returned collection
/// is created exclusively for the caller (CollectionAccessType.UpdatedContent) or
/// can be read/updated from outside (CollectionAccessType.Read | CollectionAccessType.UpdatedContent)
/// Use <see cref="CollectionAccessType"/> to specify the access type.
/// </summary>
/// <remarks>
/// Using this attribute only makes sense if all collection methods are marked with this attribute.
/// </remarks>
/// <example><code>
/// public class MyStringCollection : List&lt;string&gt;
/// {
///   [CollectionAccess(CollectionAccessType.Read)]
///   public string GetFirstString()
///   {
///     return this.ElementAt(0);
///   }
/// }
/// class Test
/// {
///   public void Foo()
///   {
///     // Warning: Contents of the collection is never updated
///     var col = new MyStringCollection();
///     string x = col.GetFirstString();
///   }
/// }
/// </code></example>
[AttributeUsage (AttributeTargets.Method | AttributeTargets.Constructor
                                         | AttributeTargets.Property | AttributeTargets.ReturnValue)]
public sealed class CollectionAccessAttribute
    : Attribute
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="collectionAccessType"></param>
    public CollectionAccessAttribute (CollectionAccessType collectionAccessType)
    {
        CollectionAccessType = collectionAccessType;
    }

    /// <summary>
    ///
    /// </summary>
    public CollectionAccessType CollectionAccessType { get; }
}

/// <summary>
/// Provides a value for the <see cref="CollectionAccessAttribute"/> to define
/// how the collection method invocation affects the contents of the collection.
/// </summary>
[Flags]
public enum CollectionAccessType
{
    /// <summary>Method does not use or modify content of the collection.</summary>
    None = 0,

    /// <summary>Method only reads content of the collection but does not modify it.</summary>
    Read = 1,

    /// <summary>Method can change content of the collection but does not add new elements.</summary>
    ModifyExistingContent = 2,

    /// <summary>Method can add new elements to the collection.</summary>
    UpdatedContent = ModifyExistingContent | 4
}

/// <summary>
/// Indicates that the marked method is assertion method, i.e. it halts the control flow if
/// one of the conditions is satisfied. To set the condition, mark one of the parameters with
/// <see cref="AssertionConditionAttribute"/> attribute.
/// </summary>
[AttributeUsage (AttributeTargets.Method)]
public sealed class AssertionMethodAttribute
    : Attribute
{
    // пустое тело класса
}

/// <summary>
/// Indicates the condition parameter of the assertion method. The method itself should be
/// marked by <see cref="AssertionMethodAttribute"/> attribute. The mandatory argument of
/// the attribute is the assertion type.
/// </summary>
[AttributeUsage (AttributeTargets.Parameter)]
public sealed class AssertionConditionAttribute
    : Attribute
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="conditionType"></param>
    public AssertionConditionAttribute (AssertionConditionType conditionType)
    {
        ConditionType = conditionType;
    }

    /// <summary>
    ///
    /// </summary>
    public AssertionConditionType ConditionType { get; }
}

/// <summary>
/// Specifies assertion type. If the assertion method argument satisfies the condition,
/// then the execution continues. Otherwise, execution is assumed to be halted.
/// </summary>
public enum AssertionConditionType
{
    /// <summary>Marked parameter should be evaluated to true.</summary>
    IS_TRUE = 0,

    /// <summary>Marked parameter should be evaluated to false.</summary>
    IS_FALSE = 1,

    /// <summary>Marked parameter should be evaluated to null value.</summary>
    IS_NULL = 2,

    /// <summary>Marked parameter should be evaluated to not null value.</summary>
    IS_NOT_NULL = 3,
}
