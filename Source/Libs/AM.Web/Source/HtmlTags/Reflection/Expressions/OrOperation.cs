// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* OrOperation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public class ComposableOrOperation
{
    #region Private members

    private readonly List<Tuple<IPropertyOperation, MemberExpression, object>> _listOfOperations = new ();

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public void Set<T>
        (
            Expression<Func<T, object>> path,
            object value
        )
    {
        Sure.NotNull (path);

        //why am I falling into here
        var memberExpression = path.GetMemberExpression (true);
        var operation = new EqualsPropertyOperation();
        _listOfOperations.Add
            (
                new Tuple<IPropertyOperation, MemberExpression, object> (operation, memberExpression, value)
            );
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public void Set<T>
        (
            Expression<Func<T, object>> path,
            IEnumerable<object> value
        )
    {
        Sure.NotNull (path);
        Sure.NotNull (value);

        var memberExpression = path.GetMemberExpression (true);
        var operation = new CollectionContainsPropertyOperation();
        _listOfOperations.Add
            (
                new Tuple<IPropertyOperation, MemberExpression, object> (operation, memberExpression, value)
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Expression<Func<T, bool>> GetPredicateBuilder<T>()
    {
        if (!_listOfOperations.Any())
        {
            throw new Exception ($"You must have at least one operation registered for an 'or' operation (you have {new[] { _listOfOperations.Count }})");
        }

        //the parameter to use
        var lambdaParameter = Expression.Parameter (typeof (T));

        var initialPredicate = Expression.Constant (false);
        Expression builtUpPredicate = initialPredicate;

        foreach (var operation in _listOfOperations)
        {
            var predBuilder = operation.Item1.GetPredicateBuilder<T> (operation.Item2);
            var predicate = predBuilder (operation.Item3);
            var expPredicate = Rebuild (predicate, lambdaParameter);
            builtUpPredicate = Expression.MakeBinary (ExpressionType.OrElse, builtUpPredicate, expPredicate);
        }

        return Expression.Lambda<Func<T, bool>> (builtUpPredicate, lambdaParameter);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    Expression Rebuild
        (
            Expression expression,
            ParameterExpression parameter
        )
    {
        Sure.NotNull (expression);
        Sure.NotNull (parameter);

        var lb = (LambdaExpression) expression;
        var targetBody = lb.Body;

        return new RewriteToLambda (parameter).Visit (targetBody);
    }

    #endregion
}

/// <summary>
///
/// </summary>
public class OrOperation
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="leftPath"></param>
    /// <param name="leftValue"></param>
    /// <param name="rightPath"></param>
    /// <param name="rightValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Expression<Func<T, bool>> GetPredicateBuilder<T>
        (
            Expression<Func<T, object>> leftPath,
            object leftValue,
            Expression<Func<T, object>> rightPath,
            object rightValue
        )
    {
        Sure.NotNull (leftPath);
        Sure.NotNull (rightPath);

        var comp = new ComposableOrOperation();
        comp.Set (leftPath, leftValue);
        comp.Set (rightPath, rightValue);

        return comp.GetPredicateBuilder<T>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="leftPath"></param>
    /// <param name="leftValue"></param>
    /// <param name="rightPath"></param>
    /// <param name="rightValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Expression<Func<T, bool>> GetPredicateBuilder<T>
        (
            Expression<Func<T, object>> leftPath,
            IEnumerable<object> leftValue,
            Expression<Func<T, object>> rightPath,
            object rightValue
        )
    {
        Sure.NotNull (leftPath);
        Sure.NotNull (leftValue);
        Sure.NotNull (rightPath);

        var comp = new ComposableOrOperation();
        comp.Set (leftPath, leftValue);
        comp.Set (rightPath, rightValue);

        return comp.GetPredicateBuilder<T>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="leftPath"></param>
    /// <param name="leftValue"></param>
    /// <param name="rightPath"></param>
    /// <param name="rightValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Expression<Func<T, bool>> GetPredicateBuilder<T>
        (
            Expression<Func<T, object>> leftPath,
            object leftValue,
            Expression<Func<T, object>> rightPath,
            IEnumerable<object> rightValue
        )
    {
        Sure.NotNull (leftPath);
        Sure.NotNull (rightPath);
        Sure.NotNull (rightValue);

        var comp = new ComposableOrOperation();
        comp.Set (leftPath, leftValue);
        comp.Set (rightPath, rightValue);

        return comp.GetPredicateBuilder<T>();
    }
}

/// <summary>
///
/// </summary>
public class RewriteToLambda
    : ExpressionVisitor
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    public RewriteToLambda
        (
            ParameterExpression parameter
        )
    {
        Sure.NotNull (parameter);

        _parameter = parameter;
    }

    #endregion

    #region Private members

    private readonly ParameterExpression _parameter;

    #endregion

    #region Protected members

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    protected override Expression VisitBinary
        (
            BinaryExpression expression
        )
    {
        Sure.NotNull (expression);

        var a = VisitMember ((MemberExpression) expression.Left);
        return Expression.Equal (a, expression.Right);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    protected override Expression VisitMember
        (
            MemberExpression member
        )
    {
        Sure.NotNull (member);

        Expression? expression = null;
        if (member.Expression!.NodeType == ExpressionType.Parameter)
        {
            //c.IsThere
            expression = Expression.MakeMemberAccess (_parameter, member.Member);
        }

        if (member.Expression.NodeType == ExpressionType.MemberAccess)
        {
            //c.Thing.IsThere

            //rewrite c.Thing
            var intermediate = VisitMember ((MemberExpression)member.Expression);

            //now combine 'c.Thing' with '.IsThere'
            expression = Expression.MakeMemberAccess (intermediate, member.Member);
        }

        return expression!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    protected override Expression VisitMethodCall
        (
            MethodCallExpression expression
        )
    {
        Sure.NotNull (expression);

        if (expression.Method.IsStatic)
        {
            var aa = expression.Arguments.Skip (1).First();
            if (aa.NodeType != ExpressionType.Constant)
            {
                aa = VisitMember ((MemberExpression)expression.Arguments.Skip (1).First());
            }

            //if second arg is a constant of our type swap other wise continue down the rabbit hole
            var args = new[] { expression.Arguments.First(), aa };
            return Expression.Call (expression.Method, args);
        }

        return Expression.Call (_parameter, expression.Method, expression.Arguments.First());
    }

    #endregion
}
