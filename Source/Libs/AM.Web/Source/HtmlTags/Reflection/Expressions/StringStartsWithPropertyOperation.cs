// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* StringStartsWithPropertyOperation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public class StringStartsWithPropertyOperation
    : CaseInsensitiveStringMethodPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="CaseInsensitiveStringMethodPropertyOperation.Text"/>
    public override string Text => "starts with";

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public StringStartsWithPropertyOperation()
        : base (_method)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private static readonly MethodInfo _method = ReflectionHelper.GetMethod<string>
            (
                s => s.StartsWith ("", StringComparison.CurrentCulture)
            );

    #endregion
}

/// <summary>
///
/// </summary>
public class CollectionContainsPropertyOperation
    : IPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="IPropertyOperation.OperationName"/>
    public string OperationName => _operationName;

    /// <inheritdoc cref="Text"/>
    public string Text => _description;

    #endregion

    #region Private members

    private const string _operationName = "Contains";
    private const string _description = "contains";


    private readonly MethodInfo _method =
        typeof (Enumerable).GetMethods (BindingFlags.Static | BindingFlags.Public)
            .First (m => m.Name.SameString ("Contains"));

    #endregion

    #region Public methods

    /// <inheritdoc cref="IPropertyOperation.GetPredicateBuilder{T}"/>
    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>
        (
            MemberExpression propertyPath
        )
    {
        return valuesToCheck =>
        {
            var enumerationOfObjects = (IEnumerable<object>?) valuesToCheck;
            if (enumerationOfObjects == null)
            {
                return c => false;
            }

            //what's the type of the collection?
            var valuesToCheckType = valuesToCheck.GetType();
            var collectionOf = valuesToCheckType.IsAnEnumerationOf();


            //capture and close the Enumerbable.Contains _method
            var closedMethod = _method.MakeGenericMethod (collectionOf!);

            //the list that we need to call contains on
            var list = Expression.Constant (enumerationOfObjects);


            //lambda parameter
            var param = Expression.Parameter (typeof (T));

            //this should be a property call
            var memberAccess = Expression.MakeMemberAccess (param, propertyPath.Member);


            //call 'Contains' with the desired 'value' to check on the 'list'
            var call = Expression.Call (closedMethod, list, memberAccess);


            var lambda = Expression.Lambda<Func<T, bool>> (call, param);

            //return enumerationOfObjects.Contains(((PropertyInfo) propertyPath.Member).GetValue(c, null));
            return lambda;
        };

        #endregion
    }
}

/// <summary>
///
/// </summary>
public class StringContainsPropertyOperation
    : StringContainsPropertyOperationBase
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    public StringContainsPropertyOperation()
        : base ("Contains", "contains", false)
    {
        // пустое тело конструктора
    }

    #endregion
}

/// <summary>
///
/// </summary>
public class StringDoesNotContainPropertyOperation
    : StringContainsPropertyOperationBase
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    public StringDoesNotContainPropertyOperation()
        : base ("DoesNotContain", "does not contain", true)
    {
        // пустое тело конструктора
    }

    #endregion
}

/// <summary>
///
/// </summary>
public abstract class StringContainsPropertyOperationBase
    : IPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="IPropertyOperation.OperationName"/>
    public string OperationName { get; }

    /// <inheritdoc cref="IPropertyOperation.Text"/>
    public string Text { get; }

    #endregion

    #region Construction

    static StringContainsPropertyOperationBase()
    {
        _indexOfMethod = ReflectionHelper.GetMethod<string>
            (
                s => s.IndexOf ("", StringComparison.OrdinalIgnoreCase)
            );
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="description"></param>
    /// <param name="negate"></param>
    protected StringContainsPropertyOperationBase
        (
            string operation,
            string description,
            bool negate
        )
    {
        Sure.NotNullNorEmpty (operation);

        OperationName = operation;
        Text = description;
        _negate = negate;
    }

    #endregion

    #region Private members

    private static readonly MethodInfo _indexOfMethod;
    private readonly bool _negate;

    #endregion

    #region Public methods

    /// <inheritdoc cref="IPropertyOperation.GetPredicateBuilder{T}"/>
    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>
        (
            MemberExpression propertyPath
        )
    {
        Sure.NotNull (propertyPath);

        return valueToCheck =>
        {
            var valueToCheckConstant = Expression.Constant (valueToCheck);
            var indexOfCall = Expression.Call
                (
                    Expression.Coalesce (propertyPath, Expression.Constant (String.Empty)),
                    _indexOfMethod,
                    valueToCheckConstant,
                    Expression.Constant (StringComparison.OrdinalIgnoreCase)
                );
            var operation = _negate ? ExpressionType.LessThan : ExpressionType.GreaterThanOrEqual;
            var comparison = Expression.MakeBinary (operation, indexOfCall,
                Expression.Constant (0));
            var lambdaParameter = propertyPath.GetParameter<T>();

            return Expression.Lambda<Func<T, bool>> (comparison, lambdaParameter);
        };
    }

    #endregion
}
