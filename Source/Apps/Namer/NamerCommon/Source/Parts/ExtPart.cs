// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExtPart.cs -- расширение имени файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Расширение имени файла.
/// </summary>
[PublicAPI]
public sealed class ExtPart
    : SystemPart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "ext";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Расширение";

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse
        (
            string text
        )
    {
        Sure.NotNull (text);
        
        var result = new ExtPart();
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
        Sure.NotNull (context);
        Sure.NotNull (fileInfo);
        
        return Render (fileInfo.Extension);
    }

    #endregion
}
