// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* CssClassNameValidator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.HtmlTags;

/// <summary>
///
/// </summary>
public static class CssClassNameValidator
{
    #region Constants

    /// <summary>
    ///
    /// </summary>
    public const string DefaultClass = "cssclassnamevalidator-santized";

    /// <summary>
    ///
    /// </summary>
    public const string ValidStartRegex = @"^-?[_a-zA-Z]+";

    /// <summary>
    ///
    /// </summary>
    public const string InvalidStartRegex = @"^-?[^_a-zA-Z]+";

    /// <summary>
    ///
    /// </summary>
    public const string ValidClassChars = @"_a-zA-Z0-9-";

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public static bool AllowInvalidCssClassNames { get; set; }

    #endregion

    #region Private members

    private static readonly Regex RxValidClassName = new ($@"{ValidStartRegex}[{ValidClassChars}]*$");
    private static readonly Regex RxReplaceInvalidChars = new ($"[^{ValidClassChars}]");
    private static readonly Regex RxReplaceLeadingChars = new ($"{InvalidStartRegex}(?<rest>.*)$");

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static bool IsJsonClassName
        (
            string className
        )
    {
        return className.StartsWith ("{") && className.EndsWith ("}")
               || className.StartsWith ("[") && className.EndsWith ("]");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static bool IsValidClassName
        (
            string className
        )
    {
        return AllowInvalidCssClassNames || IsJsonClassName (className) || RxValidClassName.IsMatch (className);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static string SanitizeClassName
        (
            string className
        )
    {
        if (string.IsNullOrEmpty (className))
        {
            return DefaultClass;
        }

        if (IsValidClassName (className))
        {
            return className;
        }

        // it can't have anything other than _,-,a-z,A-Z, or 0-9
        className = RxReplaceInvalidChars.Replace (className, "");

        // Strip invalid leading combinations (i.e. '-9test' -> 'test')
        // if it starts with '-', it must be followed by _, a-z, A-Z
        className = RxReplaceLeadingChars.Replace (className, @"${rest}");

        // if the whole thing was invalid, we'll end up with an empty string. That's not valid either, so return the default
        return string.IsNullOrEmpty (className) ? DefaultClass : className;
    }

    #endregion
}
