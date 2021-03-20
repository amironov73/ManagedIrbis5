// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxContext.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Infrastructrure
{
    /// <summary>
    ///
    /// </summary>
    public sealed class MxContext
    {
        #region Properties

        /// <summary>
        /// Executive.
        /// </summary>
        public MxExecutive Executive { get; private set; }

        /// <summary>
        /// Handlers.
        /// </summary>
        public List<HandlerInstance> Handlers { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxContext
            (
                MxExecutive executive
            )
        {
            Handlers = new List<HandlerInstance>();
            Executive = executive;
        }

        #endregion

    }
}
