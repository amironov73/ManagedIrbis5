// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblUndel.cs -- восстановление записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast;

//
// Восстанавливает записи в диапазоне MFN,
// который задан в форме ГЛОБАЛЬНОЙ.
// Не требует никаких дополнительных данных.
// Операторы, следующие за данным, выполняются
// на восстановленных записях.
//

/// <summary>
/// Восстановление записи.
/// </summary>
public sealed class GblUndel
    : GblNode
{
    #region Constants

    /// <summary>
    /// Мнемоническое обозначение команды.
    /// </summary>
    public const string Mnemonic = "UNDEL";

    #endregion

    #region GblNode members

    /// <inheritdoc cref="GblNode.Execute"/>
    public override void Execute
        (
            GblContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        var record = context.CurrentRecord;
        if (record is null)
        {
            return;
        }

        var provider = context.SyncProvider;
        if (provider is null)
        {
            return;
        }

        var mfn = record.Mfn;
        if (mfn <= 0)
        {
            return;
        }

        if ((record.Status & RecordStatus.LogicallyDeleted) == 0)
        {
            return;
        }

        record.Status &= ~RecordStatus.LogicallyDeleted;

        var parameters = new WriteRecordParameters()
        {
            Record = record,
            Actualize = true,
            DontParse = false,
            Lock = false
        };

        if (!provider.WriteRecord (parameters))
        {
            return;
        }

        OnAfterExecution (context);
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
