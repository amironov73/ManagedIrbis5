// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement

/* HtmlConventionLibrary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

#region Using directives

using Elements;

#endregion

/// <summary>
///
/// </summary>
[PublicAPI]
public class HtmlConventionLibrary
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public TagLibrary TagLibrary { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public HtmlConventionLibrary()
    {
        TagLibrary = new TagLibrary();

        _defaultBuilder = _services[TagConstants.Default];
    }

    #endregion

    #region Private members

    private readonly Cache<string, ServiceBuilder> _services = new (_ => new ServiceBuilder());
    private readonly ServiceBuilder _defaultBuilder;

    #endregion

    #region Public methods


    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public T Get<T>
        (
            string? profile = null
        )
    {
        profile ??= TagConstants.Default;
        var builder = _services[profile];
        if (builder.Has (typeof (T)))
        {
            return builder.Build<T>();
        }

        if (profile != TagConstants.Default && _defaultBuilder.Has (typeof (T)))
        {
            return _defaultBuilder.Build<T>();
        }

        throw new ArgumentOutOfRangeException
            (
                nameof (T),
                $"No service implementation is registered for type {typeof (T).FullName}"
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    public void RegisterService<T, TImplementation>
        (
            string? profile = null
        )
        where TImplementation : T, new()
    {
        RegisterService<T> (() => new TImplementation(), profile);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="profile"></param>
    /// <typeparam name="T"></typeparam>
    public void RegisterService<T>
        (
            Func<T> builder,
            string? profile = null
        )
    {
        Sure.NotNull (builder);

        profile ??= TagConstants.Default;
        _services[profile].Add (builder);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="library"></param>
    public void Import
        (
            HtmlConventionLibrary library
        )
    {
        Sure.NotNull (library);

        TagLibrary.Import (library.TagLibrary);
        library._services.Each ((key, builder) => builder.FillInto (_services[key]));
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IElementGenerator<T> GeneratorFor<T>()
        where T : class
    {
        return ElementGenerator<T>.For (this);
    }

    #endregion
}
