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

/* TemplateBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace MiniRazor;

/// <summary>
/// Base implementation of a Razor template.
/// </summary>
public abstract class TemplateBase<TModel> : ITemplate
{
    private string? _lastAttributeSuffix;

    /// <inheritdoc />
    public TextWriter? Output { get; set; }

    /// <inheritdoc cref="ITemplate.Model" />
    public TModel Model { get; set; } = default!;

    object? ITemplate.Model
    {
        get => Model;
        set => Model = (TModel) value!;
    }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// Wraps a string in a container that instructs the renderer to avoid encoding.
    /// </summary>
    protected RawString Raw(string value) => new(value);

    /// <summary>
    /// Writes a template literal.
    /// </summary>
    protected void WriteLiteral(string? literal) => Output?.Write(literal);

    /// <summary>
    /// Writes a string with encoding.
    /// </summary>
    protected void Write(string? str)
    {
        if (str is not null)
        {
            WriteLiteral(WebUtility.HtmlEncode(str));
        }
    }

    /// <summary>
    /// Writes a string without encoding.
    /// </summary>
    protected void Write(RawString str) => WriteLiteral(str.Value);

    /// <summary>
    /// Writes an object.
    /// </summary>
    protected void Write(object? obj)
    {
        if (obj is string str)
        {
            Write(str);
        }
        else if (obj is RawString raw)
        {
            Write(raw);
        }
        else
        {
            Write(obj?.ToString());
        }
    }

    /// <summary>
    /// Begins writing attribute.
    /// </summary>
    protected void BeginWriteAttribute(
        string? name,
        string? prefix,
        int prefixOffset,
        string? suffix,
        int suffixOffset,
        int attributeValuesCount)
    {
        _lastAttributeSuffix = suffix;
        WriteLiteral(prefix);
    }

    /// <summary>
    /// Writes attribute value.
    /// </summary>
    protected void WriteAttributeValue(
        string? prefix,
        int prefixOffset,
        object? value,
        int valueOffset,
        int valueLength,
        bool isLiteral)
    {
        WriteLiteral(prefix);
        Write(value);
    }

    /// <summary>
    /// Ends writing attribute.
    /// </summary>
    protected void EndWriteAttribute()
    {
        WriteLiteral(_lastAttributeSuffix);
        _lastAttributeSuffix = null;
    }

    /// <inheritdoc />
    public abstract Task ExecuteAsync();
}
