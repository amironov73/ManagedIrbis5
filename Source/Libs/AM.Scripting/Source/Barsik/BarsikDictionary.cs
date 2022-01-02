// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BarsikDictionary.cs -- словарь для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Словарь для Барсика.
/// </summary>
public sealed class BarsikDictionary
    : Dictionary<object, object?>
{
    #region Object directives

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var output = new StringWriter();
        BarsikUtility.PrintDictionary (output, this);

        return output.ToString();
    }

    #endregion
}
