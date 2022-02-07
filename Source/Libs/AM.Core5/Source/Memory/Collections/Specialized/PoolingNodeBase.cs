// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Buffers;

#endregion

#nullable enable

namespace AM.Memory.Collections.Specialized
{
	internal abstract class PoolingNodeBase<T> : IPoolingNode<T>
	{
		protected IMemoryOwner<T> _buf;

		public int Length => _buf.Memory.Length;

		public virtual T this[int index]
		{
			get => _buf.Memory.Span[index];
			set => _buf.Memory.Span[index] = value;
		}

		public virtual void Dispose()
		{
			_buf.Dispose();
			_buf = null;
			Next = null;
		}

		public IPoolingNode<T> Next { get; set; }

		public abstract IPoolingNode<T> Init(int capacity);

		public void Clear()
		{
			_buf.Memory.Span.Clear();
		}
	}
}