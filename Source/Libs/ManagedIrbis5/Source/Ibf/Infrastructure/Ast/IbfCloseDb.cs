﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfCloseDb.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Ibf.Infrastructure.Ast
{
    /// <summary>
    /// Закрытие базы данных.
    /// </summary>
    /// <remarks>
    /// В клиентском "Администраторе" команда не поддерживается.
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfCloseDb
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
}
