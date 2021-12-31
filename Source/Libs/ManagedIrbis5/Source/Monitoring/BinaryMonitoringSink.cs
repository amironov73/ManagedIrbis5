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

/* BinaryMonitoringSink.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
///
/// </summary>
public class BinaryMonitoringSink
    : MonitoringSink
{
    #region Properties

    /// <summary>
    /// Writer.
    /// </summary>
    public BinaryWriter Writer { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public BinaryMonitoringSink
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        Writer = writer;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Open file for appending monitoring data.
    /// </summary>
    public static BinaryMonitoringSink AppendFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var stream = new FileStream (fileName, FileMode.Append, FileAccess.Write);
        var writer = new BinaryWriter (stream);
        var result = new BinaryMonitoringSink (writer);

        return result;
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

        data.SaveToStream (Writer);

        return true;
    }

    #endregion
}
