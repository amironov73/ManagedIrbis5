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

public class EpubContainerException : EpubSchemaException
{
    public EpubContainerException()
        : base(EpubSchemaFileType.META_INF_CONTAINER)
    {
    }

    public EpubContainerException(string message)
        : base(message, EpubSchemaFileType.META_INF_CONTAINER)
    {
    }

    public EpubContainerException(string message, Exception innerException)
        : base(message, innerException, EpubSchemaFileType.META_INF_CONTAINER)
    {
    }
}