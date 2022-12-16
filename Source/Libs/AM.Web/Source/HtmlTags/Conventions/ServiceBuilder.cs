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

public class ServiceBuilder
{
    private readonly Cache<Type, object> _services = new ();

    public bool Has (Type type) => _services.Has (type);

    public T Build<T>() => ((Func<T>)_services[typeof (T)])();

    public void Add<T> (Func<T> func) => _services[typeof (T)] = func;

    // START HERE!
    public void FillInto (ServiceBuilder serviceBuilder) =>
        _services.Each ((type, o) => serviceBuilder._services.Fill (type, o));
}
