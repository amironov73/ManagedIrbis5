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

/* RawString.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace MiniRazor;

/// <summary>
/// Contains a string value that is not meant to be encoded by the template renderer.
/// </summary>
public readonly struct RawString
{
    /// <summary>
    /// Underlying string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes an instance of <see cref="RawString"/>.
    /// </summary>
    public RawString(string value) => Value = value;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => Value;
}
