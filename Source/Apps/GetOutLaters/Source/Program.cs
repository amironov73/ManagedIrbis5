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
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.AppServices;
using ManagedIrbis.Batch;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

using Microsoft.Extensions.Logging;

#endregion

namespace GetOutLaters;

/// <summary>
/// Наше приложение.
/// </summary>
internal sealed class Program
    : IrbisApplication
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public Program
        (
            string[] args
        )
        : base (args)
    {
        // пустое тело конструктора
    }

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
            Record record,
            BatchRecordWriter writer
        )
    {
        var reader = ReaderInfo.Parse (record);
        var visits = reader.Visits;
        if (visits is null)
        {
            return;
        }

        // отбираем посещения со временем входа в библиотеку,
        // но без времени выхода из нее
        visits = visits
            .Where (static visit => visit.IsVisit
                    && !string.IsNullOrEmpty (visit.TimeIn)
                    && string.IsNullOrEmpty (visit.TimeOut))
            .ToArray();


        // если нечего делать, досрочно выходим из записи.
        if (visits.Length == 0 && !record.HaveField (999))
        {
            return;
        }

        var builder = StringBuilderPool.Shared.Get();
        var modified = false;
        try
        {
            // формируем запись в лог-файл
            builder.AppendLine ($"{reader.FullName}: {visits.Length}");

            var first = true;
            foreach (var visit in visits)
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                builder.Append ($"{visit.DateGiven:d} {visit.TimeIn}");
                first = false;

                var field = visit.Field;
                if (field is null)
                {
                    continue;
                }

                // устанавливаем дату возврата равной дате выдачи
                field.SetSubFieldValue ('f', field.GetFirstSubFieldValue ('d'));

                // устанавливаем время возврата времени входа
                field.SetSubFieldValue ('2', field.GetFirstSubFieldValue ('1'));
                modified = true;
            }

            // убираем признак "читатель в библиотеке"
            if (record.HaveField (999))
            {
                modified = true;
                record.RemoveField (999);
            }

            Logger.LogInformation ("Прогнали: {Reader}", builder.ToString());
        }
        finally
        {
            builder.DismissShared();
        }

        if (modified)
        {
            // отправляем запись обратно на сервер
            writer.Append (record);
        }
    }

    private int DoTheWork()
    {
        Logger.LogInformation ("Прогоняем ночующих");
        var database = Connection.EnsureDatabase();

        var searchExpression = Configuration["search"].ThrowIfNull();
        Logger.LogInformation ("Поисковое выражение: {Expression}", searchExpression);

        var maxMfn = Connection.GetMaxMfn();
        Logger.LogInformation ("Максимальный MFN: {MaxMfn}", maxMfn);

        var found = Connection.Search (searchExpression);
        Logger.LogInformation ("Найдено ночующих: {Count}", found.Length);

        if (found.Length != 0)
        {
            using var writer = new BatchRecordWriter (Connection, database, 500);
            writer.BatchWrite += /* capturing */ (_, _) => Logger.LogInformation ("Сохранение записей");

            var reader = new BatchRecordReader (Connection, found);

            foreach (var record in reader)
            {
                ProcessRecord (record, writer);
            }
        }

        return 0;
    }

    /// <summary>
    /// Формальная точка входа в программу
    /// </summary>
    public static int Main
        (
            string[] args
        )
    {
        var program = new Program (args);
        program.ConfigureCancelKey();
        program.Run();

        var result = program.DoTheWork();
        program.Shutdown();

        return result;
    }
}
