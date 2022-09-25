// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Unifor2Test.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace DirectTests;

public class Unifor2Test
{
    public static void Unifor2_GetMaxMfn_1()
    {
        // Нормальное выполнение
        Infrastructure.Execute ("2", "0000000333");
        Infrastructure.Execute ("21", "3");
        Infrastructure.Execute ("212", "000000000333");

        // Обработка ошибок
        Infrastructure.Execute ("2Q", "0000000333");
        Infrastructure.Execute ("20", "");
        Infrastructure.Execute ("2-1", "");
    }
}
