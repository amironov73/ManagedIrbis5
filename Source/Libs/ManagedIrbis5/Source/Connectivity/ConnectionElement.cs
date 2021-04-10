// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConnectionElement.cs -- элемент строки подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Элемент строки подключения.
    /// </summary>
    [Flags]
    public enum ConnectionElement
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Host name or address.
        /// </summary>
        Host = 1,

        /// <summary>
        /// Port number.
        /// </summary>
        Port = 2,

        /// <summary>
        /// User name.
        /// </summary>
        Username = 4,

        /// <summary>
        /// Password.
        /// </summary>
        Password = 8,

        /// <summary>
        /// Workstation.
        /// </summary>
        Workstation = 16,

        /// <summary>
        /// All of above.
        /// </summary>
        All = 31
    } // enum ConnectionElement

} // namespace ManagedIrbis
