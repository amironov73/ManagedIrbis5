// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DisplayFormatterExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Formatting;

#region Using directives

using Reflection;

#endregion

/// <summary>
///
/// </summary>
[PublicAPI]
public static class DisplayFormatterExtensions
{
    #region Public methods

    /// <summary>
    /// Formats the provided value using the accessor accessor metadata and a custom format
    /// </summary>
    /// <param name="formatter">The formatter</param>
    /// <param name="modelType">The type of the model to which the accessor belongs (i.e. Case where the accessor might be on its base class WorkflowItem)</param>
    /// <param name="accessor">The property that holds the given value</param>
    /// <param name="value">The data to format</param>
    /// <param name="format">The custom format specifier</param>
    public static string FormatValue
        (
            this IDisplayFormatter formatter,
            Type modelType,
            IAccessor accessor,
            object value,
            string format
        )
    {
        Sure.NotNull (formatter);
        Sure.NotNull (accessor);

        var request = new GetStringRequest (accessor, value, null, format, null);

        return formatter.GetDisplay (request);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="formatter"></param>
    /// <param name="accessor"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string GetDisplayForProperty
        (
            this IDisplayFormatter formatter,
            IAccessor accessor,
            object target
        )
    {
        Sure.NotNull (formatter);
        Sure.NotNull (accessor);

        return formatter.GetDisplay (accessor, accessor.GetValue (target));
    }

    /// <summary>
    /// Formats the provided value using the property accessor metadata
    /// </summary>
    /// <param name="modelType">The type of the model to which the property belongs (i.e. Case where the property might be on its base class WorkflowItem)</param>
    /// <param name="formatter">The formatter</param>
    /// <param name="property">The property that holds the given value</param>
    /// <param name="value">The data to format</param>
    public static string FormatValue
        (
            this IDisplayFormatter formatter,
            Type modelType,
            IAccessor property,
            object value
        )
    {
        Sure.NotNull (formatter);

        return formatter.GetDisplay
            (
                new GetStringRequest (property, value, null, null, modelType)
            );
    }

    /// <summary>
    /// Retrieves the formatted value of a property from an instance
    /// </summary>
    /// <param name="formatter">The formatter</param>
    /// <param name="modelType">The type of the model to which the property belongs (i.e. Case where the property might be on its base class WorkflowItem)</param>
    /// <param name="property">The property of <paramref name="entity"/> whose value should be formatted</param>
    /// <param name="entity">The instance containing the data to format</param>
    public static string FormatProperty
        (
            this IDisplayFormatter formatter,
            Type modelType,
            IAccessor property,
            object entity
        )
    {
        Sure.NotNull (formatter);

        var raw = property.GetValue (entity);
        return formatter.FormatValue (modelType, property, raw);
    }

    #endregion
}
