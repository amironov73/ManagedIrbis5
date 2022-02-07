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

using System;

#endregion

#nullable enable

namespace AM.Memory.Collections.Specialized
{
	/// <summary>
	/// Represents ideal dictionary with extra fast access to its items. Items should inherit IdealHashObjectBase to be
	/// able to set hashcode.
	/// </summary>
	/// <typeparam name="TK">Key of dictionary</typeparam>
	/// <typeparam name="TV">Corresponding Value</typeparam>
	public class IdealHashDictionary<TK, TV> : 
		IDisposable
		where TK : IdealHashObjectBase
		where TV : class
	{
		readonly PoolingListCanon<TV> _list = Pool<PoolingListCanon<TV>>.Get().Init();
		readonly PoolingQueue<int> _freeNodes = new PoolingQueueVal<int>();
  
		public TV this[TK key]
		{
			get
			{
				var hc = key.IdealHashCode;
				if(hc >= _list.Count)
					throw new ArgumentOutOfRangeException(nameof(key));
				return _list[hc];
			}
		}
  
		public void Add(TK key, TV value)
		{
			var index = AcquireHashCode(value);
			key.IdealHashCode = index;
		}
  
		public bool Remove(TK key)
		{
			var index = key.IdealHashCode;
			_freeNodes.Enqueue(index);
			_list[index] = default;
			return true;
		}

		private int AcquireHashCode(TV value)
		{
			if(_freeNodes.Count > 0)
			{
				return _freeNodes.Dequeue();
			}
			_list.Add(value);
			return _list.Count - 1;
		}

		public void Dispose()
		{
			_list.Clear();
			_freeNodes.Clear();
		}
	}
}