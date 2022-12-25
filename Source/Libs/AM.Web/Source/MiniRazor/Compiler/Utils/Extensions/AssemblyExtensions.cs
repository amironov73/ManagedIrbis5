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

/* AssemblyExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Reflection;
using System.Runtime.Loader;

using AM;

using Microsoft.CodeAnalysis;

#endregion

#nullable enable

namespace MiniRazor.Utils.Extensions;

internal static class AssemblyExtensions
{
    public static MetadataReference ToMetadataReference
        (
            this Assembly assembly
        )
    {
        Sure.NotNull (assembly);

        return MetadataReference.CreateFromFile (assembly.Location);
    }

    public static Assembly? TryLoadFromAssemblyName
        (
            this AssemblyLoadContext loadContext,
            AssemblyName name
        )
    {
        Sure.NotNull (loadContext);
        Sure.NotNull (name);

        try
        {
            return loadContext.LoadFromAssemblyName (name);
        }
        catch
        {
            return null;
        }
    }
}
