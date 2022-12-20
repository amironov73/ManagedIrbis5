// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IDisplayFormatter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions.Formatting;

#region Using directives

using Reflection;

#endregion

#nullable enable

/// <summary>
///
/// </summary>
public interface IDisplayFormatter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    string GetDisplay
        (
            GetStringRequest request
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    string GetDisplay
        (
            IAccessor accessor,
            object target
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="rawValue"></param>
    /// <returns></returns>
    string GetDisplayForValue
        (
            IAccessor accessor,
            object rawValue
        );
}
