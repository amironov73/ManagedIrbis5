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

public class Epub3NavException : EpubSchemaException
{
    public Epub3NavException()
        : base(EpubSchemaFileType.EPUB3_NAV_DOCUMENT)
    {
    }

    public Epub3NavException(string message)
        : base(message, EpubSchemaFileType.EPUB3_NAV_DOCUMENT)
    {
    }

    public Epub3NavException(string message, Exception innerException)
        : base(message, innerException, EpubSchemaFileType.EPUB3_NAV_DOCUMENT)
    {
    }
}