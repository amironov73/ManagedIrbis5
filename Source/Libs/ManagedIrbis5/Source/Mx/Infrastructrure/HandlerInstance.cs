// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* HandlerInstance.cs --
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

namespace ManagedIrbis.Mx.Infrastructrure;

/// <summary>
///
/// </summary>
public class HandlerInstance
{
    #region Properties

    /// <summary>
    /// Handler.
    /// </summary>
    public MxHandler Handler { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public HandlerInstance
        (
            MxHandler handler
        )
    {
        Handler = handler;
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    #endregion

    #region Object members

    #endregion
}
