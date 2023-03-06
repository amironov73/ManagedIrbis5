// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DirPart.cs -- родительская директория файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.IO;
using AM.Parameters;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Родительская директория файла.
/// </summary>
[PublicAPI]
public sealed class DirPart
    : SystemPart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "dir";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Директория";

    /// <summary>
    /// Уровень директории.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Опциональный текст после.
    /// </summary>
    public string? After { get; set; }

    /// <summary>
    /// Опциональный текст перед.
    /// </summary>
    public string? Before { get; set; }

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var result = new DirPart();
        if (!Parse (result, text))
        {
            var parameters = ParameterUtility.SimpleParseString (text);
            foreach (var parameter in parameters)
            {
                switch (parameter.Name)
                {
                    case "after":
                        result.After = parameter.Value;
                        break;

                    case "before":
                        result.Before = parameter.Value;
                        break;

                    case "level":
                        result.Level = parameter.Value!.ParseInt32();
                        break;
                }
            }
        }

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

        var result = fileInfo.DirectoryName;
        if (!string.IsNullOrEmpty (result))
        {
            result = PathUtility.StripTrailingBackslash (result);
            var level = Level;
            while (level > 1)
            {
                result = Path.GetDirectoryName (result);
                if (string.IsNullOrEmpty (result))
                {
                    return string.Empty;
                }

                level--;
            }
        }

        result = Path.GetFileName (result);
        if (string.IsNullOrEmpty (result))
        {
            return string.Empty;
        }

        return Before + Render (result) + After;
    }

    #endregion
}
