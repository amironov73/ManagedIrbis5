// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ServiceBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public class ServiceBuilder
{
    #region Private members

    private readonly Cache<Type, object> _services = new ();

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool Has
        (
            Type type
        )
    {
        Sure.NotNull (type);

        return _services.Has (type);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Build<T>() => ((Func<T>) _services[typeof (T)])();

    /// <summary>
    ///
    /// </summary>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    public void Add<T>
        (
            Func<T> func
        )
    {
        Sure.NotNull (func);

        _services[typeof (T)] = func;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="serviceBuilder"></param>
    // START HERE!
    public void FillInto
        (
            ServiceBuilder serviceBuilder
        )
    {
        Sure.NotNull (serviceBuilder);

        _services.Each ((type, o) => serviceBuilder._services.Fill (type, o));
    }

    #endregion
}
