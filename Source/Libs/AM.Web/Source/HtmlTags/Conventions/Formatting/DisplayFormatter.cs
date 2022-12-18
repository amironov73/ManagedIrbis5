// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DisplayFormatter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Formatting;

#region Using directives

using Reflection;

#endregion

/// <summary>
///
/// </summary>
public class DisplayFormatter
    : IDisplayFormatter
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="locator"></param>
    public DisplayFormatter
        (
            Func<Type, object> locator
        )
    {
        Sure.NotNull (locator);

        _locator = locator;
        _stringifier = new Stringifier();
    }

    #endregion

    #region Private members

    private readonly Func<Type, object> _locator;
    private readonly Stringifier _stringifier;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetDisplay
        (
            GetStringRequest request
        )
    {
        Sure.NotNull (request);

        request.Locator = _locator;
        return _stringifier.GetString (request);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public string GetDisplay
        (
            Accessor accessor,
            object? target
        )
    {
        Sure.NotNull (accessor);

        var request = new GetStringRequest
            (
                accessor,
                target,
                _locator,
                format: null,
                ownerType: null
            );

        return _stringifier.GetString (request);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="rawValue"></param>
    /// <returns></returns>
    public string GetDisplayForValue
        (
            Accessor accessor,
            object? rawValue
        )
    {
        Sure.NotNull (accessor);

        var request = new GetStringRequest
            (
                accessor,
                rawValue,
                _locator,
                format: null,
                ownerType: null
            );

        return _stringifier.GetString (request);
    }

    #endregion
}
