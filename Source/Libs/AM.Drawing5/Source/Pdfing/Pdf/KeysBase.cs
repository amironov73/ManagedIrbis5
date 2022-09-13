// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Base class for all dictionary Keys classes.
/// </summary>
public class KeysBase
{
    #region Internal methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static DictionaryMeta CreateMeta
        (
            Type type
        )
    {
        Sure.NotNull (type);

        return new DictionaryMeta (type);
    }

    #endregion
}
