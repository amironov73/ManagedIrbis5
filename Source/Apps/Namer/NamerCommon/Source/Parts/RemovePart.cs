// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RemovePart.cs -- удаление части имени файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Удаление части имени файла.
/// </summary>
[PublicAPI]
public sealed class RemovePart
    : NamePart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "remove";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Удаление";

    /// <summary>
    /// Удаляемое значение.
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
        Sure.NotNull (text);

        var result = new RemovePart
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
        Sure.NotNull (context);
        Sure.NotNull (fileInfo);

        var result = fileInfo.Name;
        if (!string.IsNullOrEmpty (Value))
        {
            result = result.Replace (Value, string.Empty);
        }

        return result;
    }

    #endregion
}
