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

/* TemplateDescriptor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MiniRazor.Utils.Extensions;

#endregion

#nullable enable

namespace MiniRazor;

/// <summary>
/// Represents a compiled Razor template.
/// </summary>
public class TemplateDescriptor
{
    private readonly Type _templateType;

    /// <summary>
    /// Initializes an instance of <see cref="TemplateDescriptor"/>.
    /// </summary>
    public TemplateDescriptor(Type templateType) => _templateType = templateType;

    private ITemplate CreateTemplateInstance() => (ITemplate) (
        Activator.CreateInstance(_templateType) ??
        throw new InvalidOperationException($"Could not instantiate compiled template of type '{_templateType}'.")
    );

    /// <summary>
    /// Renders the template using the specified writer.
    /// </summary>
    public async Task RenderAsync(TextWriter output, object? model, CancellationToken cancellationToken = default)
    {
        var template = CreateTemplateInstance();

        template.Output = output;

        template.Model = model?.GetType().IsAnonymousType() == true
            ? model.ToExpando()
            : model;

        template.CancellationToken = cancellationToken;

        await template.ExecuteAsync();
    }

    /// <summary>
    /// Renders the template to a string.
    /// </summary>
    public async Task<string> RenderAsync(object? model, CancellationToken cancellationToken = default)
    {
        using var output = new StringWriter();
        await RenderAsync(output, model, cancellationToken);

        return output.ToString();
    }
}