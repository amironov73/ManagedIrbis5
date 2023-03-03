// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FilenamePart.cs -- собственно имя файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Parameters;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Собственно имя файла.
/// </summary>
[PublicAPI]
public sealed class FilenamePart
    : SystemPart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "name";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Имя";
    
    #endregion

    #region NamePart members

    public override NamePart Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var result = new FilenamePart();
        Parse (result, text);

        return result;
    }

    /// <inheritdoc cref="NamePart.Render"/>
    public override string Render
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        return Render (NameWithoutExtension (fileInfo.Name));
    }

    #endregion
}
