// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RegexPart.cs -- регулярное выражение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;
using AM.Parameters;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Регулярное выражение.
/// </summary>
[PublicAPI]
public sealed class RegexPart
    : NamePart
{
    #region Properties

    /// <inheritdoc cref="NamePart.Designation"/>
    public override string Designation => "regex";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Регулярное выражение";
    
    /// <summary>
    /// Искомый шаблон.
    /// </summary>
    public Regex? Pattern { get; set; }

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        // var parameters = ParameterUtility.ParseString (text);
        var result = new RegexPart
        {
            Pattern = new Regex (text)
        };

        // foreach (var parameter in parameters)
        // {
        //     parameter.Verify (true);
        //     switch (parameter.Name)
        //     {
        //         default:
        //             throw new ApplicationException();
        //     }
        // }

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

        var match = Pattern.ThrowIfNull().Match (fileInfo.Name);
        if (!match.Success)
        {
            return string.Empty;
        }

        return match.Value;
    }

    #endregion
}
