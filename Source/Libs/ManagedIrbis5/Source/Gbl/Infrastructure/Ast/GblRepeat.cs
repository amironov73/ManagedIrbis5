﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblRepeat.cs -- пара операторов REPEAT-UNTIL
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

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{
    //
    // Official documentation
    //
    // Операторы REPEAT-UNTIL организуют цикл выполнения группы операторов.
    // Группа операторов между ними будет выполняться до тех пор,
    // пока формат в операторе UNTIL будет давать значение ‘1’.
    //
    // UNTIL
    // - второй строкой оператора должен быть формат, который позволяет
    // завершить цикл, если результат форматирования
    // на текущей записи отличен от ‘1’.
    //

    /// <summary>
    /// Операторы REPEAT-UNTIL организуют цикл выполнения группы операторов.
    /// </summary>
    public sealed class GblRepeat
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "REPEAT";

        #endregion

        #region Properties

        /// <summary>
        /// Children nodes.
        /// </summary>
        public GblNodeCollection Children { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblRepeat()
        {
            Children = new GblNodeCollection(this);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region GblNode members

        /// <summary>
        /// Execute the node.
        /// </summary>
        public override void Execute
            (
                GblContext context
            )
        {
            Sure.NotNull(context, nameof(context));

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);

        } // method Execute

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Mnemonic;

        #endregion

    } // class GblRepeat

} // ManagedIrbis.Gbl.Infrastructure.Ast
