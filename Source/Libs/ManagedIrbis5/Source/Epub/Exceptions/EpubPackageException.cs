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

public class EpubPackageException : EpubSchemaException
{
    public EpubPackageException()
        : base(EpubSchemaFileType.OPF_PACKAGE)
    {
    }

    public EpubPackageException(string message)
        : base(message, EpubSchemaFileType.OPF_PACKAGE)
    {
    }

    public EpubPackageException(string message, Exception innerException)
        : base(message, innerException, EpubSchemaFileType.OPF_PACKAGE)
    {
    }
}