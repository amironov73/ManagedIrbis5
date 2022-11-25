// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubSchemaException.cs --
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
public abstract class EpubSchemaException
    : EpubReaderException
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public EpubSchemaFileType SchemaFileType { get; }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="schemaFileType"></param>
    protected EpubSchemaException
        (
            EpubSchemaFileType schemaFileType
        )
    {
        SchemaFileType = schemaFileType;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="schemaFileType"></param>
    protected EpubSchemaException
        (
            string message,
            EpubSchemaFileType schemaFileType
        )
        : base (message)
    {
        SchemaFileType = schemaFileType;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    /// <param name="schemaFileType"></param>
    protected EpubSchemaException
        (
            string message,
            Exception innerException,
            EpubSchemaFileType schemaFileType
        )
        : base (message, innerException)
    {
        SchemaFileType = schemaFileType;
    }
}
