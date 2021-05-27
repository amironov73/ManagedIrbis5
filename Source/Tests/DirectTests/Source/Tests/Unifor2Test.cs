// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#nullable enable

namespace DirectTests
{
    public class Unifor2Test
    {
        public static void Unifor2_GetMaxMfn_1()
        {
            // Нормальное выполнение
            Infrastructure.Execute("2", "0000000333");
            Infrastructure.Execute("21", "3");
            Infrastructure.Execute("212", "000000000333");

            // Обработка ошибок
            Infrastructure.Execute("2Q", "0000000333");
            Infrastructure.Execute("20", "");
            Infrastructure.Execute("2-1", "");
        }
    }
}
