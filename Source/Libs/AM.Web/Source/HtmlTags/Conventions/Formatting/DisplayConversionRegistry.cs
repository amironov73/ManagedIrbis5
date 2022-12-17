// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DisplayConversionRegistry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Formatting;

/// <summary>
///
/// </summary>
public class DisplayConversionRegistry
{
    #region Nested types

    /// <summary>
    ///
    /// </summary>
    public class MakeDisplayExpression
        : MakeDisplayExpressionBase
    {
        #region Construction

        /// <summary>
        ///
        /// </summary>
        /// <param name="callback"></param>
        public MakeDisplayExpression
            (
                Action<Func<GetStringRequest, string>> callback
            )
            : base (callback)
        {
            // пустое тело конструктора
        }

        #endregion

        #region Public methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        public void ConvertBy
            (
                Func<GetStringRequest, string> display
            )
        {
            Sure.NotNull (display);

            _callback (display);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        /// <typeparam name="TService"></typeparam>
        public void ConvertWith<TService>
            (
                Func<TService, GetStringRequest, string> display
            )
        {
            Sure.NotNull (display);

            Apply (o => display (o.Get<TService>(), o));
        }

        #endregion
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MakeDisplayExpression<T>
        : MakeDisplayExpressionBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="callback"></param>
        public MakeDisplayExpression
            (
                Action<Func<GetStringRequest, string>> callback
            )
            : base (callback)
        {
            // пустое тело конструктора
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        public void ConvertBy
            (
                Func<T, string> display
            )
        {
            Sure.NotNull (display);

            Apply (o => display ((T)o.RawValue));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        public void ConvertBy
            (
                Func<GetStringRequest, T, string> display
            )
        {
            Sure.NotNull (display);

            Apply (o => display (o, (T)o.RawValue));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        /// <typeparam name="TService"></typeparam>
        public void ConvertWith<TService>
            (
                Func<TService, T, string> display
            )
        {
            Sure.NotNull (display);

            Apply (o => display (o.Get<TService>(), (T)o.RawValue));
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class MakeDisplayExpressionBase
    {
        #region Construction

        /// <summary>
        ///
        /// </summary>
        /// <param name="callback"></param>
        protected MakeDisplayExpressionBase
            (
                Action<Func<GetStringRequest, string>> callback
            )
        {
            _callback = callback;
        }

        #endregion

        #region Protected members

        /// <summary>
        ///
        /// </summary>
        protected Action<Func<GetStringRequest, string>> _callback;

        /// <summary>
        ///
        /// </summary>
        /// <param name="func"></param>
        protected void Apply
            (
                Func<GetStringRequest, string> func
            )
        {
            Sure.NotNull (func);

            _callback (func);
        }

        #endregion
    }

    #endregion

    #region Private members

    private readonly IList<StringifierStrategy> _strategies = new List<StringifierStrategy>();

    private MakeDisplayExpression MakeDisplay
        (
            Func<GetStringRequest, bool> filter
        )
    {
        return new (func =>
        {
            _strategies.Add (new StringifierStrategy
            {
                Matches = filter,
                StringFunction = func
            });
        });
    }

    private MakeDisplayExpression<T> MakeDisplay<T>
        (
            Func<GetStringRequest, bool> filter
        )
    {
        return new (func =>
        {
            _strategies.Add (new StringifierStrategy
            {
                Matches = filter,
                StringFunction = func
            });
        });
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Stringifier BuildStringifier()
    {
        var stringifier = new Stringifier();
        Configure (stringifier);
        return stringifier;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stringifier"></param>
    public void Configure
        (
            Stringifier stringifier
        )
    {
        Sure.NotNull (stringifier);

        _strategies.Each (stringifier.AddStrategy);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public MakeDisplayExpression IfTypeMatches
        (
            Func<Type, bool> filter
        )
    {
        Sure.NotNull (filter);

        return MakeDisplay (request => filter (request.PropertyType));
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public MakeDisplayExpression<T> IfIsType<T>()
    {
        return MakeDisplay<T>
            (
                request => request.PropertyType == typeof (T)
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public MakeDisplayExpression<T> IfCanBeCastToType<T>() =>
        MakeDisplay<T> (t => t.PropertyType.CanBeCastTo<T>());

    /// <summary>
    ///
    /// </summary>
    /// <param name="matches"></param>
    /// <returns></returns>
    public MakeDisplayExpression IfPropertyMatches
        (
            Func<PropertyInfo, bool> matches
        )
    {
        Sure.NotNull (matches);

        return MakeDisplay
            (
                request => request.Property != null
                           && matches (request.Property)
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matches"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public MakeDisplayExpression<T> IfPropertyMatches<T>
        (
            Func<PropertyInfo, bool> matches
        )
    {
        Sure.NotNull (matches);

        return MakeDisplay<T>
            (
                request => request.Property != null
                           && request.PropertyType == typeof (T)
                           && matches (request.Property)
            );
    }

    #endregion

}
