// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* NullMonitoringSink.cs -- sink take no action
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
/// Sink take no action.
/// </summary>
public sealed class NullMonitoringSink
    : MonitoringSink
{
    #region MonitoringSink members

    /// <inheritdoc cref="MonitoringSink.WriteData" />
    public override bool WriteData
        (
            MonitoringData data
        )
    {
        // Nothing to do here.

        return true;
    }

    #endregion
}
