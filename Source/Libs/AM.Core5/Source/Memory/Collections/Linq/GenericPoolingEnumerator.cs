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

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq
{
	internal sealed class GenericPoolingEnumerator<T> : IPoolingEnumerator<T>
	{
		private IEnumerator<T> _source;
    
		public GenericPoolingEnumerator<T> Init(IEnumerator<T> source)
		{
			_source = source;
			return this;
		}

		public bool MoveNext() => _source.MoveNext();

		public void Reset() => _source.Reset();
		
		object IPoolingEnumerator.Current => Current;

		public T Current => _source.Current;
    
		public void Dispose()
		{
			_source.Dispose();
			_source = default;
			Pool<GenericPoolingEnumerator<T>>.Return(this);
		}
	}

	internal sealed class GenericEnumerator<T> : IEnumerator<T>
	{
		private IPoolingEnumerator<T> _source;
    
		public GenericEnumerator<T> Init(IPoolingEnumerator<T> source)
		{
			_source = source;
			return this;
		}

		public bool MoveNext() => _source.MoveNext();

		public void Reset() => _source.Reset();
		
		object IEnumerator.Current => Current;

		public T Current => _source.Current;
    
		public void Dispose()
		{
			_source.Dispose();
			_source = default;
			Pool<GenericEnumerator<T>>.Return(this);
		}
	}
}