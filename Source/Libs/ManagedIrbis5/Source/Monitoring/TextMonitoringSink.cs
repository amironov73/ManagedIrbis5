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

/* TextMonitoringSink.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
///
/// </summary>
public sealed class TextMonitoringSink
    : MonitoringSink
{
    #region Public members

    /// <summary>
    /// Text writer.
    /// </summary>
    public TextWriter Writer { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="writer"></param>
    public TextMonitoringSink
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer);

        Writer = writer;
    }

    #endregion

    #region MonitoringSink members

    /// <inheritdoc cref="MonitoringSink.WriteData" />
    public override bool WriteData
        (
            MonitoringData data
        )
    {
        Sure.NotNull (data);

        try
        {
            var text = JsonUtility.SerializeShort (data);
            Writer.WriteLine (text);
        }
        catch (Exception exception)
        {
            Magna.TraceException
                (
                    nameof (TeeMonitoringSink) + "::" + nameof (WriteData),
                    exception
                );

            return false;
        }

        return true;
    }

    #endregion
}
