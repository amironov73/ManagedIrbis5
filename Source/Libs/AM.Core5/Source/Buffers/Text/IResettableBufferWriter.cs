// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* IResettableBufferWriter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Buffers;

#endregion

namespace AM.Buffers.Text;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResettableBufferWriter<T> 
    : IBufferWriter<T>
{
    /// <summary>
    /// 
    /// </summary>
    void Reset();
}
