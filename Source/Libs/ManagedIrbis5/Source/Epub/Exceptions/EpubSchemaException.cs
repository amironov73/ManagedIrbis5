// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

public abstract class EpubSchemaException : EpubReaderException
{
    protected EpubSchemaException(EpubSchemaFileType schemaFileType)
    {
        SchemaFileType = schemaFileType;
    }

    protected EpubSchemaException(string message, EpubSchemaFileType schemaFileType)
        : base(message)
    {
        SchemaFileType = schemaFileType;
    }

    protected EpubSchemaException(string message, Exception innerException, EpubSchemaFileType schemaFileType)
        : base(message, innerException)
    {
        SchemaFileType = schemaFileType;
    }

    public EpubSchemaFileType SchemaFileType { get; }
}