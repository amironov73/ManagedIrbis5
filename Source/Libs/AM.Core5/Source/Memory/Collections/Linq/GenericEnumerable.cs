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
	internal class GenericPoolingEnumerable<T> : IPoolingEnumerable<T>
	{
		private IEnumerable<T> _enumerable;
    
		public GenericPoolingEnumerable<T> Init(IEnumerable<T> enumerable)
		{
			_enumerable = enumerable;
			return this;
		}
    			
		public IPoolingEnumerator<T> GetEnumerator()
		{
			var enumerator = _enumerable.GetEnumerator();
			_enumerable = default;
			Pool<GenericPoolingEnumerable<T>>.Return(this);
			return Pool<GenericPoolingEnumerator<T>>.Get().Init(enumerator);
		}

		IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	internal class GenericEnumerable<T> : IEnumerable<T>
	{
		private IPoolingEnumerable<T> _enumerable;
    
		public GenericEnumerable<T> Init(IPoolingEnumerable<T> enumerable)
		{
			_enumerable = enumerable;
			return this;
		}
    			
		public IEnumerator<T> GetEnumerator()
		{
			var enumerator = _enumerable.GetEnumerator();
			_enumerable = default;
			Pool<GenericEnumerable<T>>.Return(this);
			return Pool<GenericEnumerator<T>>.Get().Init(enumerator);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}