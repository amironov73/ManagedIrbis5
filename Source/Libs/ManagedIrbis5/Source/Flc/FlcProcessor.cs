// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedType.Global

/* FlcProcessor.cs -- процессор FLC-скриптов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Flc
{
    //
    // Для формально-логического контроля данных (как на уровне поля,
    // так и документа в целом) применяются специальные форматы (Приложение 8).
    //
    // Форматы для ФЛК используются следующим образом:
    //
    // * форматированию подвергается контролируемый документ;
    //
    // * первый символ результата форматирования определяет результат ФЛК, а именно:
    //
    //   0 - означает положительный результат контроля;
    //
    //   1 - означает отрицательный результат и обнаруженные ошибки считаются
    //       непреодолимыми, т.е. подлежат обязательному устранению;
    //
    //   2 - означает отрицательный результат, но при этом ошибки считаются
    //       преодолимыми, т.е. их можно не исправлять.
    //
    // * остальной результат форматирования (начиная со второго символа)
    // в случае отрицательного контроля выдается пользователю в качестве сообщения.
    //
    // Собственно форматы ФЛК, как правило, содержат команды IF.
    //
    // Многочисленные примеры таких форматов находятся в директории БД IBIS.
    //

    /// <summary>
    /// Процессор FLC-скриптов.
    /// </summary>
    public sealed class FlcProcessor
    {
        #region Public methods

        /// <summary>
        /// Check the record.
        /// </summary>
        public FlcResult CheckRecord
            (
                ISyncIrbisProvider provider,
                Record record,
                string format
            )
        {
            /*

            string text = provider.FormatRecord
                (
                    record,
                    format
                );
            FlcResult result = FlcResult.Parse(text);

            return result;

            */

            throw new NotImplementedException();
        } // method CheckRecord

        #endregion

    } // class FlcProcessor

} // namespace ManagedIrbis.Flc
