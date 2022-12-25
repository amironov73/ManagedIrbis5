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

/* ITemplate.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace MiniRazor;

/// <summary>
/// Non-generic handle for <see cref="TemplateBase{TModel}"/> to simplify usage from reflection.
/// </summary>
public interface ITemplate
{
    /// <summary>
    /// Output associated with this template.
    /// </summary>
    TextWriter? Output { get; set; }

    /// <summary>
    /// Model associated with this template.
    /// </summary>
    object? Model { get; set; }

    /// <summary>
    /// Cancellation token associated with this template.
    /// </summary>
    CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// Executes the template.
    /// </summary>
    Task ExecuteAsync();
}
