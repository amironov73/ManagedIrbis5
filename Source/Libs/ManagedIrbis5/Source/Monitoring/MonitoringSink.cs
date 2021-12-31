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

/* MonitoringSink.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
/// Абстрактный приемник для данных мониторинга.
/// </summary>
public abstract class MonitoringSink
{
    #region Public methods

    /// <summary>
    /// Write monitoring data.
    /// </summary>
    public abstract bool WriteData
        (
            MonitoringData data
        );

    #endregion
}
