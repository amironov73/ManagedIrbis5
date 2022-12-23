// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberCallInConstructor

/* ReflectionHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

internal static class ReflectionHelper
{
    #region Public methods

    public static bool MeetsSpecialGenericConstraints
        (
            Type genericArgType,
            Type proposedSpecificType
        )
    {
        Sure.NotNull (genericArgType);
        Sure.NotNull (proposedSpecificType);

        var gpa = genericArgType.GetTypeInfo().GenericParameterAttributes;
        var constraints = gpa & GenericParameterAttributes.SpecialConstraintMask;

        // No constraints, away we go!
        if (constraints == GenericParameterAttributes.None)
        {
            return true;
        }

        // "class" constraint and this is a value type
        if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0
            && proposedSpecificType.GetTypeInfo().IsValueType)
        {
            return false;
        }

        // "struct" constraint and this is not a value type
        if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0
            && !proposedSpecificType.GetTypeInfo().IsValueType)
        {
            return false;
        }

        // "new()" constraint and this type has no default constructor
        if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0
            && proposedSpecificType.GetConstructor (Type.EmptyTypes) == null)
        {
            return false;
        }

        return true;
    }

    public static PropertyInfo GetProperty<TModel>
        (
            Expression<Func<TModel, object>> expression
        )
    {
        Sure.NotNull (expression);

        var memberExpression = GetMemberExpression (expression);
        return (PropertyInfo)memberExpression.Member;
    }

    public static PropertyInfo GetProperty<TModel, T>
        (
            Expression<Func<TModel, T>> expression
        )
    {
        Sure.NotNull (expression);

        var memberExpression = GetMemberExpression (expression);
        return (PropertyInfo)memberExpression.Member;
    }

    public static PropertyInfo GetProperty
        (
            LambdaExpression expression
        )
    {
        Sure.NotNull (expression);

        var memberExpression = GetMemberExpression (expression, true);
        return (PropertyInfo) memberExpression!.Member;
    }

    public static IAccessor GetAccessor
        (
            LambdaExpression expression
        )
    {
        Sure.NotNull (expression);

        var memberExpression = GetMemberExpression (expression, true);

        return GetAccessor (memberExpression!);
    }

    public static MemberExpression? GetMemberExpression
        (
            this LambdaExpression expression,
            bool enforceMemberExpression
        )
    {
        Sure.NotNull (expression);

        MemberExpression? memberExpression = null;
        if (expression.Body.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression.Body as MemberExpression;
        }


        if (enforceMemberExpression && memberExpression == null)
        {
            throw new ArgumentException ("Not a member access", nameof (expression));
        }

        return memberExpression;
    }

    public static bool IsMemberExpression<T>
        (
            Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        return IsMemberExpression<T, object> (expression);
    }

    public static bool IsMemberExpression<T1, T2>
        (
            Expression<Func<T1, T2>> expression
        )
    {
        Sure.NotNull (expression);

        return GetMemberExpression (expression, false) is not null;
    }

    public static IAccessor GetAccessor<TModel>
        (
            Expression<Func<TModel, object>> expression
        )
    {
        Sure.NotNull (expression);

        if (expression.Body is MethodCallExpression || expression.Body.NodeType == ExpressionType.ArrayIndex)
        {
            return GetAccessor (expression.Body);
        }

        var memberExpression = GetMemberExpression (expression);

        return GetAccessor (memberExpression);
    }

    public static IAccessor GetAccessor
        (
            Expression memberExpression
        )
    {
        Sure.NotNull (memberExpression);

        var list = new List<IValueGetter>();
        BuildValueGetters (memberExpression, list);
        switch (list)
        {
            case [PropertyValueGetter pvg]:
                return new SingleProperty (pvg.PropertyInfo);

            case [MethodValueGetter mvg]:
                return new SingleMethod (mvg);

            case [IndexerValueGetter ivg]:
                return new ArrayIndexer (ivg);

            default:
                list.Reverse();
                return new PropertyChain (list.ToArray());
        }
    }

    private static void BuildValueGetters
        (
            Expression expression,
            ICollection<IValueGetter> list
        )
    {
        Sure.NotNull (expression);
        Sure.NotNull (list);

        if (expression is MemberExpression memberExpression)
        {
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            list.Add (new PropertyValueGetter (propertyInfo));
            if (memberExpression.Expression != null)
            {
                BuildValueGetters (memberExpression.Expression, list);
            }
        }

        //deals with collection indexers, an indexer [0] will look like a get(0) method call expression
        if (expression is MethodCallExpression methodCallExpression)
        {
            var methodInfo = methodCallExpression.Method;
            var argument = methodCallExpression.Arguments.FirstOrDefault();

            if (argument == null)
            {
                var methodValueGetter = new MethodValueGetter (methodInfo, Array.Empty<object>());
                list.Add (methodValueGetter);
            }
            else
            {
                if (TryEvaluateExpression (argument, out var value))
                {
                    var methodValueGetter = new MethodValueGetter (methodInfo, new[] { value });
                    list.Add (methodValueGetter);
                }
            }


            if (methodCallExpression.Object is not null)
            {
                BuildValueGetters (methodCallExpression.Object, list);
            }
        }

        if (expression.NodeType == ExpressionType.ArrayIndex)
        {
            var binaryExpression = (BinaryExpression)expression;

            var indexExpression = binaryExpression.Right;

            if (TryEvaluateExpression (indexExpression, out var index))
            {
                var indexValueGetter = new IndexerValueGetter (binaryExpression.Left.Type, (int) index!);

                list.Add (indexValueGetter);
            }

            BuildValueGetters (binaryExpression.Left, list);
        }
    }

    public static IAccessor GetAccessor<TModel, T>
        (
            Expression<Func<TModel, T>> expression
        )
    {
        Sure.NotNull (expression);

        var memberExpression = GetMemberExpression (expression);

        return GetAccessor (memberExpression);
    }

    public static MethodInfo GetMethod<T>
        (
            Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        return new FindMethodVisitor (expression).Method;
    }

    public static MethodInfo GetMethod
        (
            Expression<Func<object>> expression
        )
    {
        Sure.NotNull (expression);

        return GetMethod<Func<object>> (expression);
    }

    public static MethodInfo GetMethod
        (
            Expression expression
        )
    {
        Sure.NotNull (expression);

        return new FindMethodVisitor (expression).Method;
    }

    public static MethodInfo GetMethod<TDelegate>
        (
            Expression<TDelegate> expression
        )
    {
        Sure.NotNull (expression);

        return new FindMethodVisitor (expression).Method;
    }

    public static MethodInfo GetMethod<T1, T2>
        (
            Expression<Func<T1, T2>> expression
        )
    {
        Sure.NotNull (expression);

        return new FindMethodVisitor (expression).Method;
    }

    public static MethodInfo GetMethod<T1, T2, T3>
        (
            Expression<Func<T1, T2, T3>> expression
        )
    {
        Sure.NotNull (expression);

        return new FindMethodVisitor (expression).Method;
    }

    public static MethodInfo GetMethod<T>
        (
            Expression<Action<T>> expression
        )
    {
        Sure.NotNull (expression);

        return new FindMethodVisitor (expression).Method;
    }

    #endregion

    #region Private members

    private static MemberExpression GetMemberExpression<TModel, T>
        (
            Expression<Func<TModel, T>> expression
        )
    {
        Sure.NotNull (expression);

        MemberExpression? memberExpression = null;
        if (expression.Body.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression.Body as MemberExpression;
        }


        if (memberExpression is null)
        {
            throw new ArgumentException ("Not a member access", nameof (expression));
        }

        return memberExpression;
    }

    private static bool TryEvaluateExpression
        (
            Expression? operation,
            out object? value
        )
    {
        Sure.NotNull (operation);

        if (operation is null)
        {
            // used for static fields, etc
            value = null;
            return true;
        }

        switch (operation.NodeType)
        {
            case ExpressionType.Constant:
                value = ((ConstantExpression)operation).Value;
                return true;

            case ExpressionType.MemberAccess:
                var me = (MemberExpression)operation;
                if (TryEvaluateExpression (me.Expression, out var target))
                {
                    // instance target
                    if (me.Member is FieldInfo fieldInfo)
                    {
                        value = fieldInfo.GetValue (target);
                        return true;
                    }

                    if (me.Member is PropertyInfo propertyInfo)
                    {
                        value = propertyInfo.GetValue (target, null);
                        return true;
                    }
                }

                break;
        }

        value = null;
        return false;
    }

    #endregion
}

/// <summary>
///
/// </summary>
public class FindMethodVisitor
    : ExpressionVisitor
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public MethodInfo Method { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    public FindMethodVisitor
        (
            Expression expression
        )
    {
        Sure.NotNull (expression);

        Method = null!;
        Visit (expression);
    }

    #endregion

    #region ExpressionVisitor members

    /// <inheritdoc cref="ExpressionVisitor.VisitMethodCall"/>
    protected override Expression VisitMethodCall
        (
            MethodCallExpression expression
        )
    {
        Sure.NotNull (expression);

        Method = expression.Method;
        return expression;
    }

    #endregion
}
