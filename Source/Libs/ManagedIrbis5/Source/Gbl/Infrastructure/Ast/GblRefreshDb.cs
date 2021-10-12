// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblRefreshDb.cs -- обновление контекста работы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{
    //
    // Official documentation
    //
    // Начиная с 2016.1
    //
    // Добавлена новая команда для пакетных заданий
    // (только для АРМа Каталогизатор):
    // REFRESHDB - обновить контекст работы (команда не имеет операндов).
    //
    //

    /// <summary>
    /// Обновление контекста работы.
    /// </summary>
    public sealed class GblRefreshDb
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "REFRESHDB";

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
            Sure.NotNull (context, nameof (context));

            OnBeforeExecution (context);

            // Nothing to do here

            OnAfterExecution (context);

        } // method Execute

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Mnemonic;

        #endregion

    } // class GblRefreshDb

} // ManagedIrbis.Gbl.Infrastructure.Ast
