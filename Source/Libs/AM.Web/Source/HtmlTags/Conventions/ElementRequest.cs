// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ElementRequest.cs --
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
using Formatting;
using Reflection;

#endregion

/// <summary>
///
/// </summary>
[PublicAPI]
public class ElementRequest
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public object? RawValue
    {
        get
        {
            if (!_hasFetched)
            {
                _rawValue = Model == null ? null : Accessor.GetValue (Model);
                _hasFetched = true;
            }

            return _rawValue;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public string ElementId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public object? Model { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IAccessor Accessor { get; }

    /// <summary>
    ///
    /// </summary>
    public HtmlTag? OriginalTag { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public HtmlTag CurrentTag { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    public ElementRequest
        (
            IAccessor accessor
        )
    {
        Sure.NotNull (accessor);

        Accessor = accessor;
    }

    #endregion

    #region Private members

    private bool _hasFetched;
    private object? _rawValue;
    private Func<Type, object>? _services;

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    public void WrapWith
        (
            HtmlTag tag
        )
    {
        CurrentTag.WrapWith (tag);
        ReplaceTag (tag);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    public void ReplaceTag
        (
            HtmlTag tag
        )
    {
        if (OriginalTag == null)
        {
            OriginalTag = tag;
        }

        CurrentTag = tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public AccessorDef ToAccessorDef() => new (Accessor, HolderType());


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Type HolderType() => Model == null ? Accessor.DeclaringType : Model?.GetType();

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>() => (T)_services! (typeof (T));

    /// <summary>
    ///
    /// </summary>
    /// <param name="service"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryGet<T> (out T service) => (service = (T)_services (typeof (T))) != null;

    /// <summary>
    ///
    /// </summary>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    // virtual for mocking
    public virtual HtmlTag BuildForCategory (string category, string? profile = null) =>
        Get<ITagGenerator>().Build (this, category, profile);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Value<T>() => (T)RawValue;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string StringValue() =>
        new DisplayFormatter (_services).GetDisplay (new GetStringRequest (Accessor, RawValue, _services, null,
            null));

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool ValueIsEmpty() => RawValue == null || string.Empty.Equals (RawValue);

    /// <summary>
    ///
    /// </summary>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public void ForValue<T>
        (
            Action<T?> action
        )
    {
        if (ValueIsEmpty())
        {
            return;
        }

        action ((T?) RawValue);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="locator"></param>
    public void Attach (Func<Type, object> locator) => _services = locator;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ElementRequest ToToken() => new (Accessor);
}
