// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* StringEqualsPropertyOperation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public class StringEqualsPropertyOperation
    : CaseInsensitiveStringMethodPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="CaseInsensitiveStringMethodPropertyOperation.Text"/>
    public override string Text => "is";

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public StringEqualsPropertyOperation()
        : base (_method)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private static readonly MethodInfo _method =
        ReflectionHelper.GetMethod<string>
            (
                s => s.Equals ("", StringComparison.CurrentCulture)
            );

    #endregion
}
