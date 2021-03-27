// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ClientVersion.cs -- версия клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Версия клиента.
    /// </summary>
    public static class ClientVersion
    {
        #region Properties

        /// <summary>
        /// Собственно версия клиента.
        /// </summary>
        public static readonly Version Version = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version
            ?? throw new ApplicationException("ClientVersion not defined");

        #endregion

    } // class ClientVersion

} // namespace ManagedIrbis
