// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LiteralPart.cs -- литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Литерал.
/// </summary>
[PublicAPI]
public sealed class LiteralPart
    : NamePart
{
    #region Propertie

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "literal";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Литерал";
    
    /// <summary>
    /// Значение литерала.
    /// </summary>
    public string? Value { get; set; }

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse 
        (
            string text
        )
    {
        var result = new LiteralPart
        {
            Value = text
        };

        return result;
    }

    /// <inheritdoc cref="NamePart.Render"/>
    public override string Render
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        return Value ?? string.Empty;
    }

    #endregion
}
