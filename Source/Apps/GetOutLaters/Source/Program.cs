// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- утилита для "разгона ночующих читателей"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;
using AM.AppServices;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.AppServices;
using ManagedIrbis.Batch;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace GetOutLaters
{
    /// <summary>
    /// Наше приложение.
    /// </summary>
    internal class Program
        : IrbisApplication
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        private Program(string[] args)
            : base(args)
        {
        }

        private BatchRecordWriter? _writer;

        /// <summary>
        /// <para>Обрабатывает одну запись. Что делается:</para>
        /// <list type="number">
        /// <item><description>Отбираются поля с меткой <c>40</c>, в которых присутствует
        /// подполе <c>1</c>, но отсутствует подполе <c>2</c>.</description></item>
        /// <item><description>В этих полях подполю <c>2</c> присваивается значение
        /// <c>23:59:59</c>, а значение из подполя <c>d</c> копируется
        /// в подполе <c>f</c>. Таким образом закрывается посещение.</description></item>
        /// <item><description>Из записи удаляется подполе <c>999</c>,
        /// служащее признаком присутствия читателя в библиотеке.</description></item>
        /// <item><description>Модифицированная запись отправляется обратно на сервер.
        /// </description></item>
        /// </list>
        /// </summary>
        private void ProcessRecord
            (
                Record record
            )
        {
            var reader = ReaderInfo.Parse(record);
            var visits = reader.Visits;
            if (visits is null)
            {
                return;
            }

            // отбираем посещения со временем входа в библиотеку,
            // но без времени выхода из нее
            visits = visits
                .Where(visit => visit.IsVisit
                    && !string.IsNullOrEmpty(visit.TimeIn)
                    && string.IsNullOrEmpty(visit.TimeOut))
                .ToArray();


            // если нечего делать, досрочно выходим из записи.
            if (visits.Length == 0 && !record.HaveField(999))
            {
                return;
            }

            var builder = StringBuilderPool.Shared.Get();
            var modified = false;
            try
            {
                // формируем запись в лог-файл
                builder.AppendLine($"{reader.FullName}: {visits.Length}");

                var first = true;
                foreach (var visit in visits)
                {
                    if (!first)
                    {
                        builder.Append(", ");
                    }

                    builder.Append($"{visit.DateGiven:d} {visit.TimeIn}");
                    first = false;

                    var field = visit.Field;
                    if (field is null)
                    {
                        continue;
                    }

                    // устанавливаем дату возврата равной дате выдачи
                    field.SetSubFieldValue('f', field.GetFirstSubFieldValue('d'));

                    // устанавливаем время возврата в полночь
                    field.SetSubFieldValue('2', "23:59:59");
                    modified = true;
                }

                // убираем признак "читатель в библиотеке"
                if (record.HaveField(999))
                {
                    modified = true;
                    record.RemoveField(999);
                }

                Logger.LogInformation(builder.ToString());
            }
            finally
            {
                StringBuilderPool.Shared.Return(builder);
            }

            if (modified)
            {
                // отправляем запись обратно на сервер
                _writer?.Append(record);
            }

        } // method ProcessRecord

        /// <inheritdoc cref="MagnaApplication.ActualRun"/>
        protected override int ActualRun()
        {
            Logger.LogInformation("Прогоняем ночующих");

            var connection = Connection.ThrowIfNull();
            var database = connection.EnsureDatabase();

            var searchExpression = Configuration["search"];
            Logger.LogInformation($"Поисковое выражение: {searchExpression}");

            var maxMfn = connection.GetMaxMfn();
            Logger.LogInformation($"Максимальный MFN: {maxMfn}");

            var found = connection.Search(searchExpression);
            Logger.LogInformation($"Найдено ночующих: {found.Length}");

            if (found.Length != 0)
            {
                using (_writer = new BatchRecordWriter(connection, database, 500))
                {
                    _writer.BatchWrite += (_, _) =>
                        Logger.LogInformation("Сохранение записей");

                    var reader = new BatchRecordReader(connection, found);

                    foreach (var record in reader)
                    {
                        ProcessRecord(record);
                    }
                }
            }

            return 0;

        } // method ActualRun

        /// <summary>
        /// Формальная точка входа в программу
        /// </summary>
        static int Main(string[] args) => new Program(args).Run();

    } // class Program

} // namespace GetOutLaters
