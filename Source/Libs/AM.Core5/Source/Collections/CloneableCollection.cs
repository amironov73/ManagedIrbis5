// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Cloneable.cs -- клонируемая коллекция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Клонируемая коллекция.
/// </summary>
[DebuggerDisplay ("Count={" + nameof (Count) + "}")]
public class CloneableCollection<T>
    : Collection<T>,
        ICloneable
{
    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public object Clone()
    {
        var result = new CloneableCollection<T>();

        foreach (var item in this)
        {
            result.Add (item);
        }

        return result;
    }

    #endregion
}
