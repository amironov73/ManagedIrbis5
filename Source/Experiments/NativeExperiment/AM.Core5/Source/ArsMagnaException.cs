// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ArsMagnaException.cs -- базовый класс для наших исключений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Базовый класс для исключений,
/// специфичных для проекта Ars Magna.
/// </summary>
public class ArsMagnaException
    : ApplicationException
{
    #region Properties

    /// <summary>
    /// Аттачменты.
    /// </summary>
    public List<BinaryAttachment> Attachments { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ArsMagnaException()
    {
        Attachments = new List<BinaryAttachment>();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ArsMagnaException
        (
            string message
        )
        : base (message)
    {
        Attachments = new List<BinaryAttachment>();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ArsMagnaException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
        Attachments = new List<BinaryAttachment>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Прикрепление контента к исключению.
    /// </summary>
    public ArsMagnaException Attach
        (
            string name,
            byte[] content
        )
    {
        Attachments.Add (new BinaryAttachment { Name = name, Content = content });

        return this;
    }

    #endregion
}
