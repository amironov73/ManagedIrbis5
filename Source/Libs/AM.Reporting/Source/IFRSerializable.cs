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
using System.Collections.Generic;
using System.Text;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Provides the serialize/deserialize functionality.
    /// </summary>
    public interface IFRSerializable
    {
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        void Serialize (ReportWriter writer);

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="reader">Reader object.</param>
        void Deserialize (FRReader reader);
    }
}
