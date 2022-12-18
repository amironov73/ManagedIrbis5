// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TagGenerator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public class ActiveProfile
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string Name => _profiles.Peek();

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public ActiveProfile()
    {
        _profiles.Push (TagConstants.Default);
    }

    #endregion

    #region Private members

    private readonly Stack<string> _profiles = new ();

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    public void Push
        (
            string profile
        )
    {
        Sure.NotNullNorEmpty (profile);

        _profiles.Push (profile);
    }

    /// <summary>
    ///
    /// </summary>
    public void Pop() => _profiles.Pop();

    #endregion
}

/// <summary>
///
/// </summary>
public class TagGenerator
    : ITagGenerator
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string ActiveProfile => _profile.Name;

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="library"></param>
    /// <param name="profile"></param>
    /// <param name="serviceLocator"></param>
    public TagGenerator
        (
            ITagLibrary library,
            ActiveProfile profile,
            Func<Type, object> serviceLocator
        )
    {
        Sure.NotNull (library);
        Sure.NotNull (profile);
        Sure.NotNull (serviceLocator);

        _library = library;
        _profile = profile;
        _serviceLocator = serviceLocator;
    }

    #endregion

    #region Private members

    private readonly ITagLibrary _library;
    private readonly ActiveProfile _profile;
    private readonly Func<Type, object> _serviceLocator;

    #endregion

    #region ITagGenerator methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    public HtmlTag Build
        (
            ElementRequest request,
            string? category = null,
            string? profile = null
        )
    {
        Sure.NotNull (request);

        profile ??= _profile.Name ?? TagConstants.Default;
        category ??= TagConstants.Default;

        var token = request.ToToken();

        token.Attach (_serviceLocator);

        var plan = _library.PlanFor (token, profile, category);

        request.Attach (_serviceLocator);

        return plan.Build (request);
    }

    #endregion
}
