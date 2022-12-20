// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* StringNotEqualsPropertyOperation.cs --
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
public class StringNotEqualPropertyOperation
    : CaseInsensitiveStringMethodPropertyOperation
{
    #region Properties

    /// <inheritdoc cref="CaseInsensitiveStringMethodPropertyOperation.OperationName"/>
    public override string OperationName => "DoesNotEqual";

    /// <inheritdoc cref="CaseInsensitiveStringMethodPropertyOperation.Text"/>
    public override string Text => "is not";

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public StringNotEqualPropertyOperation()
        : base (_method, true)
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
