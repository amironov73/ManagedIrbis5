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

#nullable enable

namespace AM.Memory.Collections.Specialized
{
	public sealed class PoolingList<T> : PoolingListBase<T>
	{
		public PoolingList() => Init();

		public PoolingList<T> Init()
		{
			_root = Pool.GetBuffer<IPoolingNode<T>>(PoolsDefaults.DefaultPoolBucketSize);
			_ver = 0;
			return this;
		}
		
		protected override IPoolingNode<T> CreateNodeHolder()
		{
			return Pool<PoolingNode<T>>.Get().Init(PoolsDefaults.DefaultPoolBucketSize);
		}
	}
}