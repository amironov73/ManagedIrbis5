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

/* AssemblyNameEqualityComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

#nullable enable

namespace MiniRazor.Utils;

internal class AssemblyNameEqualityComparer : IEqualityComparer<AssemblyName>
{
    public static AssemblyNameEqualityComparer Instance { get; } = new();

    public bool Equals(AssemblyName? x, AssemblyName? y) =>
        StringComparer.OrdinalIgnoreCase.Equals(x?.FullName, y?.FullName);

    public int GetHashCode(AssemblyName obj) =>
        StringComparer.OrdinalIgnoreCase.GetHashCode(obj.FullName);
}