// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Stringifier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Formatting;

/// <summary>
///
/// </summary>
public class Stringifier
{
    #region Nested type

    /// <summary>
    ///
    /// </summary>
    public class PropertyOverrideStrategy
    {
        #region Peroperties

        /// <summary>
        ///
        /// </summary>
        public Func<PropertyInfo, bool>? Matches;

        /// <summary>
        ///
        /// </summary>
        public Func<GetStringRequest, string>? StringFunction;

        #endregion
    }

    #endregion

    #region Private members

    private readonly List<PropertyOverrideStrategy> _overrides = new ();
    private readonly List<StringifierStrategy> _strategies = new ();

    private Func<GetStringRequest, string> findConverter
        (
            GetStringRequest request
        )
    {
        if (request.PropertyType!.IsNullable())
        {
            if (request.RawValue == null)
            {
                return _ => string.Empty;
            }

            return findConverter (request.GetRequestForNullableType());
        }

        if (request.PropertyType!.IsArray)
        {
            if (request.RawValue == null)
            {
                return _ => string.Empty;
            }

            return r =>
            {
                if (r.RawValue == null)
                {
                    return string.Empty;
                }

                return r.RawValue.As<Array>().OfType<object>().Select (GetString).Join (", ");
            };
        }

        var strategy = _strategies.FirstOrDefault (x => x.Matches! (request));

        return strategy == null ? ToString : strategy.StringFunction!;
    }

    private static string ToString (GetStringRequest value) => value.RawValue?.ToString() ?? string.Empty;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetString
        (
            GetStringRequest request
        )
    {
        if (request.RawValue == null || request.RawValue as string == string.Empty)
        {
            return string.Empty;
        }

        var propertyOverride = _overrides.FirstOrDefault (o => o.Matches! (request.Property!));

        if (propertyOverride != null)
        {
            return propertyOverride.StringFunction! (request);
        }

        return findConverter (request) (request);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="rawValue"></param>
    /// <returns></returns>
    public string GetString
        (
            object? rawValue
        )
    {
        if (rawValue is null || rawValue as string == string.Empty)
        {
            return string.Empty;
        }

        return GetString
            (
                new GetStringRequest (null, rawValue, null, null, null)
            );
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="strategy"></param>
    public void AddStrategy
        (
            StringifierStrategy strategy
        )
    {
        Sure.NotNull (strategy);

        _strategies.Add (strategy);
    }

    #endregion
}
