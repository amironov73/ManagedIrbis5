// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubPackageException.cs --
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
public class EpubPackageException
    : EpubSchemaException
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    public EpubPackageException()
        : base (EpubSchemaFileType.OPF_PACKAGE)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    public EpubPackageException
        (
            string message
        )
        : base (message, EpubSchemaFileType.OPF_PACKAGE)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public EpubPackageException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException, EpubSchemaFileType.OPF_PACKAGE)
    {
        // пустое тело конструктора
    }

    #endregion
}
