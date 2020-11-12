// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ArsMagnaException.cs -- базовый класс для наших исключений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Базовый класс для исключений,
    /// специфичных для проекта Ars Magna.
    /// </summary>
    public class ArsMagnaException
    {
        #region Properties

        /// <summary>
        /// Аттачменты.
        /// </summary>
        public List<BinaryAttachment> Attachments { get; } = new ();

        #endregion

        #region Public methods

        public ArsMagnaException Attach
            (
                string name,
                byte[] content
            )
        {
            Attachments.Add(new BinaryAttachment { Name = name, Content = content });

            return this;
        }

        #endregion
    }
}