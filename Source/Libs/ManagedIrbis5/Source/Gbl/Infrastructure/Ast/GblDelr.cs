// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* DelrNode.cs -- удаление записей, поданных на глобальную корректировку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{

    /// <summary>
    /// Удаление записей, поданных на глобальную корректировку.
    /// Дополнительных данных не требуется.
    /// </summary>
    public sealed class GblDelr
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "DELR";

        #endregion

        #region GblNode members

        /// <inheritdoc cref="GblNode.Execute"/>
        public override void Execute
            (
                GblContext context
            )
        {
            Sure.NotNull (context, nameof (context));

            OnBeforeExecution (context);

            if (context.CurrentRecord is { } record)
            {
                var newStatus = record.Status |= RecordStatus.LogicallyDeleted;
                if (record.Status != newStatus)
                {
                    record.Status = newStatus;
                    context.RecordSink?.PostRecord
                        (
                            record,
                            "Record was deleted"
                        );

                } // if

            } // if

            OnAfterExecution (context);

        } // method Execute

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Mnemonic;

        #endregion

    } // class GblDelr

} // namespace ManagedIrbis.Gbl.Infrastructure.Ast
