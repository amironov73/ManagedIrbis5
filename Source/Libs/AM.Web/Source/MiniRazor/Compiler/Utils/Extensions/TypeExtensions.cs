// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* TypeExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace MiniRazor.Utils.Extensions;

internal static class TypeExtensions
{
    public static bool Implements(this Type type, Type interfaceType) =>
        type.GetInterfaces().Contains(interfaceType);

    public static bool IsAnonymousType(this Type type) =>
        type.IsDefined(typeof(CompilerGeneratedAttribute)) &&
        type.Name.Contains("AnonymousType", StringComparison.Ordinal);

    public static ExpandoObject ToExpando(this object anonymousObject)
    {
        var expando = new ExpandoObject();
        var expandoMap = (IDictionary<string, object?>) expando;

        foreach (var property in anonymousObject.GetType().GetTypeInfo().GetProperties())
        {
            var obj = property.GetValue(anonymousObject);
            if (obj is not null && obj.GetType().IsAnonymousType())
                obj = obj.ToExpando();

            expandoMap[property.Name] = obj;
        }

        return expando;
    }
}