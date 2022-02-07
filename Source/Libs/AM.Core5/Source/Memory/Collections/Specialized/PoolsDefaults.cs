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

namespace AM.Memory.Collections.Specialized;

public static class PoolsDefaults
{
    public const int DefaultPoolBucketDegree = 7;
    public const int DefaultPoolBucketSize = 1 << DefaultPoolBucketDegree;
    public const int DefaultPoolBucketMask = DefaultPoolBucketSize - 1;
}