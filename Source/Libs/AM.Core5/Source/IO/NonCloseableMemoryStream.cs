// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* NonCloseableMemoryStream.cs -- незакрываемый MemoryStream
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Незакрываемая версия <see cref="MemoryStream"/>.
/// </summary>
public sealed class NonCloseableMemoryStream
    : MemoryStream
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NonCloseableMemoryStream()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NonCloseableMemoryStream
        (
            byte[] buffer
        )
        : base (buffer)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NonCloseableMemoryStream
        (
            byte[] buffer,
            bool writable
        )
        : base (buffer, writable)
    {
    }

    /// <inheritdoc />
    public NonCloseableMemoryStream (byte[] buffer, int index, int count) : base (buffer, index, count)
    {
    }

    /// <inheritdoc />
    public NonCloseableMemoryStream (byte[] buffer, int index, int count, bool writable) : base (buffer, index, count,
        writable)
    {
    }

    /// <inheritdoc />
    public NonCloseableMemoryStream (byte[] buffer, int index, int count, bool writable, bool publiclyVisible) : base (
        buffer, index, count, writable, publiclyVisible)
    {
    }

    /// <inheritdoc />
    public NonCloseableMemoryStream (int capacity) : base (capacity)
    {
    }

    #endregion

    #region Stream members

    /// <inheritdoc cref="MemoryStream.Dispose(bool)" />
    protected override void Dispose (bool disposing)
    {
        // Nothing to do here
    }

    /// <inheritdoc cref="Stream.Close" />
    public override void Close()
    {
        // Nothing to do here
    }

    #endregion
}
