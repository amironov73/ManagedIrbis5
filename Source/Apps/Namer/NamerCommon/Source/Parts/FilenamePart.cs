// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FilenamePart.cs -- собственно имя файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Собственно имя файла.
/// </summary>
[PublicAPI]
public sealed class FilenamePart
    : NamePart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "name";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Имя";

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse
        (
            string text
        )
    {
        return new FilenamePart();
    }

    /// <inheritdoc cref="NamePart.Render"/>
    public override string Render
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        return fileInfo.Name;
    }

    #endregion
}
