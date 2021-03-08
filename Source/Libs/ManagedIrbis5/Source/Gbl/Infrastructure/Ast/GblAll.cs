// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblAll.cs --
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
    // Official documentation:
    //
    // Оператор можно использовать в группе операторов после
    // операторов NEWMFN или CORREC. Он дополняет записи всеми
    // полями текущей записи. Т.е. это способ, например,
    // создать новую запись и наполнить ее содержимым текущей записи.
    // Или можно вызвать на корректировку другую запись (CORREC),
    // очистить ее (EMPTY) и наполнить содержимым текущей записи.
    //

    /// <summary>
    ///
    /// </summary>
    public sealed class GblAll
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "ALL";

        #endregion

        #region Properties

        #endregion

        #region Construction

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
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Mnemonic;
        }

        #endregion
    }
}
