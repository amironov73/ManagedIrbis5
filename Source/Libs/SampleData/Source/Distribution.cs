// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Distribution.cs -- тестовый класс для опытов: распределение товаров по магазинам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace SampleData;

/// <summary>
/// Тестовый класс для опытов: распределение товаров по магазинам.
/// </summary>
[UsedImplicitly]
public sealed class Distribution
{
    #region Properties

    /// <summary>
    /// Идентификатор.
    /// </summary>
    [UsedImplicitly]
    public int Id { get; init; }
    
    /// <summary>
    /// Артикул.
    /// </summary>
    [UsedImplicitly]
    public int ProductId { get; set; }

    /// <summary>
    /// Идентификатор магазина.
    /// </summary>
    [UsedImplicitly]
    public int MagazineId { get; set; }
    
    /// <summary>
    /// Количество.
    /// </summary>
    [UsedImplicitly]
    public int Amount { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Распределение товаров по магазинам.
    /// </summary>
    [UsedImplicitly]
    public static Distribution[] IrkutskDns() => new Distribution[]
    {
        new() { Id = 1, ProductId = 4793041, MagazineId = 5916, Amount = 1 },
        new() { Id = 2, ProductId = 4793041, MagazineId = 83476, Amount = 2 },
        new() { Id = 3, ProductId = 5013925, MagazineId = 1086, Amount = 1 },
        new() { Id = 4, ProductId = 5013925, MagazineId = 12377, Amount = 3 },
        new() { Id = 5, ProductId = 5013925, MagazineId = 2213, Amount = 1 },
        new() { Id = 6, ProductId = 4888314, MagazineId = 2213, Amount = 1 },
        new() { Id = 7, ProductId = 5088334, MagazineId = 4345, Amount = 2 },
        new() { Id = 8, ProductId = 5088334, MagazineId = 43, Amount = 2 },
        new() { Id = 9, ProductId = 5088334, MagazineId = 2126, Amount = 1 },
        new() { Id = 10, ProductId = 9908415, MagazineId = 2126, Amount = 1 },
        new() { Id = 11, ProductId = 9908415, MagazineId = 2005, Amount = 2 },
        new() { Id = 12, ProductId = 5063856, MagazineId = 5894, Amount = 3 },
        new() { Id = 13, ProductId = 5042201, MagazineId = 6175, Amount = 3 },
        new() { Id = 14, ProductId = 5042201, MagazineId = 5111, Amount = 1 },
        new() { Id = 15, ProductId = 1638645, MagazineId = 4381, Amount = 1 },
        new() { Id = 16, ProductId = 1638645, MagazineId = 22766, Amount = 5 },
        new() { Id = 17, ProductId = 1638645, MagazineId = 554401, Amount = 1 },
        new() { Id = 18, ProductId = 1331514, MagazineId = 5721, Amount = 1 },
        new() { Id = 19, ProductId = 5042232, MagazineId = 5875, Amount = 1 },
        new() { Id = 20, ProductId = 5042232, MagazineId = 301812, Amount = 1 },
        new() { Id = 21, ProductId = 5042232, MagazineId = 1675, Amount = 2 },
        new() { Id = 22, ProductId = 5354654, MagazineId = 5585, Amount = 1 },
        new() { Id = 23, ProductId = 5354654, MagazineId = 5916, Amount = 1 },
        new() { Id = 24, ProductId = 1671536, MagazineId = 83476, Amount = 2 },
        new() { Id = 25, ProductId = 1671536, MagazineId = 1086, Amount = 1 },
        new() { Id = 26, ProductId = 9905860, MagazineId = 12377, Amount = 1 },
        new() { Id = 27, ProductId = 5035540, MagazineId = 4345, Amount = 2 },
        new() { Id = 28, ProductId = 5035540, MagazineId = 43, Amount = 1 },
        new() { Id = 29, ProductId = 1607583, MagazineId = 43, Amount = 1 },
        new() { Id = 30, ProductId = 4863603, MagazineId = 43, Amount = 1 },
        new() { Id = 31, ProductId = 4863603, MagazineId = 2126, Amount = 1 },
        new() { Id = 32, ProductId = 4863603, MagazineId = 2005, Amount = 1 },
        new() { Id = 33, ProductId = 5056098, MagazineId = 5894, Amount = 2 },
        new() { Id = 34, ProductId = 5056098, MagazineId = 6175, Amount = 1 },
        new() { Id = 35, ProductId = 5074554, MagazineId = 5111, Amount = 3 },
        new() { Id = 36, ProductId = 9932396, MagazineId = 4381, Amount = 1 },
        new() { Id = 37, ProductId = 9907461, MagazineId = 22766, Amount = 1 },
        new() { Id = 38, ProductId = 9907461, MagazineId = 554401, Amount = 1 },
        new() { Id = 39, ProductId = 5094947, MagazineId = 952143, Amount = 1 },
        new() { Id = 40, ProductId = 4876024, MagazineId = 5721, Amount = 1 },
        new() { Id = 41, ProductId = 4876024, MagazineId = 301812, Amount = 1 },
        new() { Id = 42, ProductId = 5087848, MagazineId = 1675, Amount = 3 },
        new() { Id = 43, ProductId = 5087848, MagazineId = 5585, Amount = 1 },
        new() { Id = 44, ProductId = 5324829, MagazineId = 5916, Amount = 1 },
        new() { Id = 45, ProductId = 1617203, MagazineId = 83476, Amount = 2 },
        new() { Id = 46, ProductId = 1617203, MagazineId = 1086, Amount = 1 },
        new() { Id = 47, ProductId = 1639846, MagazineId = 1086, Amount = 1 },
        new() { Id = 48, ProductId = 1387421, MagazineId = 12377, Amount = 1 },
        new() { Id = 49, ProductId = 1387421, MagazineId = 2213, Amount = 5 },
        new() { Id = 50, ProductId = 1387421, MagazineId = 4345, Amount = 1 },
        new() { Id = 51, ProductId = 5007701, MagazineId = 43, Amount = 4 },
        new() { Id = 52, ProductId = 5066167, MagazineId = 43, Amount = 3 },
        new() { Id = 53, ProductId = 5066167, MagazineId = 2126, Amount = 1 },
        new() { Id = 54, ProductId = 5066167, MagazineId = 2005, Amount = 1 },
        new() { Id = 55, ProductId = 5046062, MagazineId = 5894, Amount = 1 },
        new() { Id = 56, ProductId = 5046062, MagazineId = 6175, Amount = 1 },
        new() { Id = 57, ProductId = 9939870, MagazineId = 5111, Amount = 1 },
        new() { Id = 58, ProductId = 9926393, MagazineId = 4381, Amount = 1 },
        new() { Id = 59, ProductId = 5046210, MagazineId = 22766, Amount = 1 },
        new() { Id = 60, ProductId = 5046210, MagazineId = 554401, Amount = 1 },
        new() { Id = 61, ProductId = 9913386, MagazineId = 952143, Amount = 1 },
        new() { Id = 62, ProductId = 4873023, MagazineId = 5721, Amount = 1 },
        new() { Id = 63, ProductId = 4873023, MagazineId = 5875, Amount = 2 },
        new() { Id = 64, ProductId = 5094465, MagazineId = 301812, Amount = 1 },
        new() { Id = 65, ProductId = 5094465, MagazineId = 1675, Amount = 1 },
        new() { Id = 66, ProductId = 4888821, MagazineId = 5585, Amount = 1 },
        new() { Id = 67, ProductId = 4888821, MagazineId = 5916, Amount = 1 },
        new() { Id = 68, ProductId = 4888821, MagazineId = 83476, Amount = 1 },
        new() { Id = 69, ProductId = 4888821, MagazineId = 4345, Amount = 2 },
        new() { Id = 70, ProductId = 1241341, MagazineId = 4345, Amount = 1 },
        new() { Id = 71, ProductId = 1241341, MagazineId = 43, Amount = 2 },
        new() { Id = 72, ProductId = 1023646, MagazineId = 43, Amount = 3 },
        new() { Id = 73, ProductId = 0197141, MagazineId = 43, Amount = 1 },
        new() { Id = 74, ProductId = 8189155, MagazineId = 2126, Amount = 2 },
        new() { Id = 75, ProductId = 1000720, MagazineId = 2005, Amount = 1 },
        new() { Id = 76, ProductId = 1000720, MagazineId = 5894, Amount = 1 },
        new() { Id = 77, ProductId = 5032352, MagazineId = 6175, Amount = 1 },
        new() { Id = 78, ProductId = 5032352, MagazineId = 5111, Amount = 1 },
        new() { Id = 79, ProductId = 5370641, MagazineId = 22766, Amount = 1 },
        new() { Id = 80, ProductId = 5030046, MagazineId = 952143, Amount = 1 },
        new() { Id = 81, ProductId = 5030046, MagazineId = 301812, Amount = 3 },
        new() { Id = 82, ProductId = 5030046, MagazineId = 1675, Amount = 1 },
        new() { Id = 83, ProductId = 4787881, MagazineId = 5585, Amount = 1 },
        new() { Id = 84, ProductId = 5010030, MagazineId = 5916, Amount = 1 },
        new() { Id = 85, ProductId = 5010030, MagazineId = 83476, Amount = 1 },
        new() { Id = 86, ProductId = 1627135, MagazineId = 1086, Amount = 1 },
        new() { Id = 87, ProductId = 1627135, MagazineId = 12377, Amount = 1 },
        new() { Id = 88, ProductId = 8105968, MagazineId = 12377, Amount = 1 },
        new() { Id = 89, ProductId = 1359448, MagazineId = 2213, Amount = 3 },
        new() { Id = 90, ProductId = 1399148, MagazineId = 4345, Amount = 1 },
        new() { Id = 91, ProductId = 1108603, MagazineId = 4345, Amount = 2 },
        new() { Id = 92, ProductId = 5354973, MagazineId = 2126, Amount = 1 },
        new() { Id = 93, ProductId = 5354973, MagazineId = 43, Amount = 1 },
        new() { Id = 94, ProductId = 1330834, MagazineId = 43, Amount = 2 },
        new() { Id = 95, ProductId = 9912783, MagazineId = 43, Amount = 4 },
        new() { Id = 96, ProductId = 9912783, MagazineId = 2126, Amount = 2 },
        new() { Id = 97, ProductId = 1365054, MagazineId = 2005, Amount = 1 },
        new() { Id = 98, ProductId = 4876362, MagazineId = 5894, Amount = 1 },
        new() { Id = 99, ProductId = 4876362, MagazineId = 6175, Amount = 3 },
        new() { Id = 100, ProductId = 9938785, MagazineId = 5111, Amount = 1 },
        new() { Id = 101, ProductId = 4887696, MagazineId = 4381, Amount = 1 },
        new() { Id = 102, ProductId = 1112951, MagazineId = 22766, Amount = 2 },
        new() { Id = 103, ProductId = 1040424, MagazineId = 554401, Amount = 1 },
        new() { Id = 104, ProductId = 4830071, MagazineId = 952143, Amount = 1 },
        new() { Id = 105, ProductId = 4844791, MagazineId = 5721, Amount = 1 },
        new() { Id = 106, ProductId = 4844791, MagazineId = 5875, Amount = 2 },
        new() { Id = 107, ProductId = 4899465, MagazineId = 301812, Amount = 1 },
        new() { Id = 108, ProductId = 1145121, MagazineId = 1675, Amount = 1 },
        new() { Id = 109, ProductId = 1145121, MagazineId = 5585, Amount = 1 },
        new() { Id = 110, ProductId = 1145122, MagazineId = 5585, Amount = 1 },
        new() { Id = 111, ProductId = 1145122, MagazineId = 5916, Amount = 1 },
        new() { Id = 112, ProductId = 4716232, MagazineId = 83476, Amount = 1 },
        new() { Id = 113, ProductId = 4716232, MagazineId = 1086, Amount = 1 },
        new() { Id = 114, ProductId = 4872432, MagazineId = 12377, Amount = 1 },
        new() { Id = 115, ProductId = 4872432, MagazineId = 2213, Amount = 1 },
        new() { Id = 116, ProductId = 4872432, MagazineId = 4345, Amount = 1 },
        new() { Id = 117, ProductId = 1279550, MagazineId = 4345, Amount = 1 },
        new() { Id = 118, ProductId = 1279550, MagazineId = 43, Amount = 2 },
        new() { Id = 119, ProductId = 1684185, MagazineId = 43, Amount = 1 },
        new() { Id = 120, ProductId = 1684185, MagazineId = 2126, Amount = 1 },
        new() { Id = 121, ProductId = 1684185, MagazineId = 2005, Amount = 1 },
    }; 

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj) => 
        obj is Distribution distribution && distribution.Id == Id;

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Id);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Id}: Product={ProductId}, Magazine={MagazineId}";

    #endregion
}
