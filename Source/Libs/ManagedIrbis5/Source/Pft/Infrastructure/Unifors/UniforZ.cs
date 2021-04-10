// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforZ.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Размножение экземпляров – &uf('Z')
    // Вид функции: Z.
    // Назначение: Размножение экземпляров.
    // Функция ничего не возвращает.
    // Можно применять только в глобальной корректировке.
    // Формат (передаваемая строка):
    // Z
    //
    // Пример:
    // &unifor('Z')
    //
    // Код статуса «R» временный(до сохранения документа),
    // технологический, используется для облегчения ввода группы
    // экземпляров, описываемых индивидуально и различающихся лишь
    // последовательными инвентарными номерами (и/или штрих кодами)
    // и местом хранения (остальные ЭД – номер партии (КСУ, акт),
    // канал поступления, цена и дата ввода – у них одинаковы);
    // число экземпляров и начальный номер номер задаются
    // в ЭД «Инвентарный номер экземпляра» через разделительный
    // знак «/» (например, 5/12567 – 5 экземпляров, начиная
    // с номера 12567; начальный штрих-код вводится
    // в соответствующем подполе).
    //
    // Поля экземпляров со статусом «R» при сохранении документа
    // автоматически «размножаются» соответственно указанному
    // ЧИСЛУ таким образом, что в каждое из них вводятся:
    // * Статус 0;
    // * ЭД «Инвентарный номер» и/или Штрих-код,
    // значение которого отличается от предыдущего на 1.
    // При этом ПЕРВОМУ экземпляру присваивается инвентарный номер,
    // значение которого либо равно заданному, либо равно 1
    // (при отсутствии заданного значения в поле со статусом «R»
    // и отсутствии штрих-кода); начальное значение штрих-кода
    // указывается в подполе штрих-кода.
    // * В новые повторения вводятся инв. номера и/или ШК,
    // увеличивающиеся на 1 в последнем(правом) разряде.
    // При этом:
    // * если введены начальные значения  инв.номера и штрих-кода,
    // размножаются оба;
    // * если введен только штрих-код, в размноженные поля
    // вводятся только ШК
    // если ШК не введен, то в размноженные поля вводятся
    // только инв.номера
    // * если начальный номер не указан, то размножение инв. номеров
    // начинается с 1
    // при одновременном вводе нескольких повторений поля 910 со
    // статусом R, инв. номера и/или ШК присваиваются с продолжением
    // Все остальные данные переносятся без изменения из поля,
    // введенного со статусом «R».
    //
    // Размножение экземпляров
    //
    // Возможно применение технологии процесса «Размножение
    // экземпляров», позволяющей вводить последовательные инвентарные
    // * номера экземпляров для разных мест их хранения:
    // * за одно обращение к справочнику путем группового ввода
    // в повторяющиеся поля отбираются и вводятся в документ направления
    // (места хранения) всех экземпляров; при этом статус может либо
    // отсутствовать, либо быть равным «2» (заказан);
    // * вводится одно поле со статусом «R» (с указанием числа
    // экземпляров и инвентарного номера первого из них)
    // и всеми остальными ЭД, общими для всех размножаемых экземпляров;
    // * при сохранении документа размноженные по инвентарным
    // номерам экземпляры объединяются с направлениями;
    // «лишние» направления формируются как заказанные со статусом «2».
    //

    static class UniforZ
    {
        #region Private members

        private static void _ProcessField
            (
                Record record,
                Field field
            )
        {
            field.SetSubFieldValue('a', "0".AsMemory());
            var specification = field.GetFirstSubFieldValue('B');
            if (specification.IsEmpty)
            {
                return;
            }
            var navigator = new TextNavigator(specification);
            var countText = navigator.ReadUntil('/').ToString();
            if (string.IsNullOrEmpty(countText))
            {
                navigator.ReadChar();
                field.SetSubFieldValue('b', navigator.GetRemainingText());
                return;
            }
            var count = countText.SafeToInt32();

            if (count <= 0)
            {
                record.Fields.Remove(field);
                return;
            }

            if (navigator.ReadChar() != '/')
            {
                return;
            }

            var inventoryText = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(inventoryText))
            {
                inventoryText = "1";
            }
            NumberText inventory = inventoryText;

            var barcodeText = field.GetFirstSubFieldValue('H');
            var barcode = barcodeText.IsEmpty
                ? null
                : new NumberText(barcodeText.ToString());

            var first = true;
            while (count > 0)
            {
                var current = field;
                if (!first)
                {
                    current = field.Clone();
                    record.Fields.Add(current);
                }

                current.SetSubFieldValue('b', inventory.ToString().AsMemory());
                if (!ReferenceEquals(barcode, null))
                {
                    current.SetSubFieldValue('h', barcode.ToString().AsMemory());
                    barcode = barcode.Increment();
                }

                count--;
                inventory = inventory.Increment();
                first = false;
            }
        }

        #endregion

        #region Public methods

        public static void GenerateExemplars
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                var fields = record.Fields
                    .GetField(910)
                    .GetField('A', "R")
                    .ToArray();
                foreach (var field in fields)
                {
                    _ProcessField(record, field);
                }
            }
        }

        #endregion
    }
}
