// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

using Elements;

public class HtmlConventionLibrary
{
    private readonly Cache<string, ServiceBuilder> _services = new (key => new ServiceBuilder());
    private readonly ServiceBuilder _defaultBuilder;

    public HtmlConventionLibrary()
    {
        TagLibrary = new TagLibrary();

        _defaultBuilder = _services[TagConstants.Default];
    }

    public TagLibrary TagLibrary { get; }

    public T Get<T> (string? profile = null)
    {
        profile = profile ?? TagConstants.Default;
        var builder = _services[profile];
        if (builder.Has (typeof (T)))
        {
            return builder.Build<T>();
        }

        if (profile != TagConstants.Default && _defaultBuilder.Has (typeof (T)))
        {
            return _defaultBuilder.Build<T>();
        }

        throw new ArgumentOutOfRangeException ("T",
            "No service implementation is registered for type " + typeof (T).FullName);
    }

    public void RegisterService<T, TImplementation> (string? profile = null) where TImplementation : T, new()
        => RegisterService<T> (() => new TImplementation(), profile);

    public void RegisterService<T> (Func<T> builder, string? profile = null)
    {
        profile = profile ?? TagConstants.Default;
        _services[profile].Add (builder);
    }

    public void Import (HtmlConventionLibrary library)
    {
        TagLibrary.Import (library.TagLibrary);
        library._services.Each ((key, builder) => builder.FillInto (_services[key]));
    }

    public IElementGenerator<T> GeneratorFor<T>() where T : class => ElementGenerator<T>.For (this);
}
