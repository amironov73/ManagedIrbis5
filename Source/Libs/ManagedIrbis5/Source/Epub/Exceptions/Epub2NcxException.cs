// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub2NcxException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public sealed class Epub2NcxException
    : EpubSchemaException
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Epub2NcxException()
        : base (EpubSchemaFileType.EPUB2_NCX)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Epub2NcxException
        (
            string message
        )
        : base (message, EpubSchemaFileType.EPUB2_NCX)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Epub2NcxException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException, EpubSchemaFileType.EPUB2_NCX)
    {
        // пустое тело конструктора
    }

    #endregion
}
