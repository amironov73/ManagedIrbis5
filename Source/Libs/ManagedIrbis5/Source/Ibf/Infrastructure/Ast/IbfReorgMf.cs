﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IbfReorgMf.cs --
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

namespace ManagedIrbis.Ibf.Infrastructure.Ast;

/// <summary>
/// Реорганизация файла документов.
/// </summary>
public sealed class IbfReorgMf
    : IbfNode
{
    #region Properties

    #endregion

    #region Construction

    #endregion

    #region Private members

    #endregion

    #region Public methods

    #endregion

    #region IbfNode members

    /// <inheritdoc cref="IbfNode.Execute" />
    public override void Execute
        (
            IbfContext context
        )
    {
        OnBeforeExecution(context);

        OnAfterExecution(context);
    }

    #endregion

    #region Object members

    #endregion
}