// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* BindingManager.cs -- менеджер подшивок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Collections;
using AM.Text.Ranges;

using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/*
 * NJ   - Описание отдельного номера журнала
 * NJP  - Описание отдельного номера газеты/журнала, входящего в подшивку
 * NJK  - Описание подшивки (в формате отдельного номера журнала/газеты)
 * SPEC - Номер журнала оформлен как том многотомника
 */

/// <summary>
/// Менеджер подшивок.
/// </summary>
public sealed class BindingManager
    : IBindingManager
{
    #region Properties

    /// <summary>
    /// Синхронный провайдер.
    /// </summary>
    public ISyncProvider Provider { get; }

    /// <summary>
    /// Конфигурация библиографических записей.
    /// </summary>
    public RecordConfiguration RecordConfiguration { get; }

    /// <summary>
    /// Конфигурация подшивателя.
    /// </summary>
    public BindingConfiguration BindingConfiguration { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BindingManager
        (
            IHost host,
            ISyncProvider provider,
            BindingConfiguration? bindingConfiguration = null,
            RecordConfiguration? recordConfiguration = null
        )
    {
        Sure.NotNull (host);
        Sure.NotNull (provider);

        _logger = LoggingUtility.GetLogger (host.Services, typeof (BindingManager));
        Provider = provider;
        BindingConfiguration = bindingConfiguration ?? BindingConfiguration.GetDefault();
        RecordConfiguration = recordConfiguration ?? RecordConfiguration.GetDefault();
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    #endregion

    #region Public methods


    /// <summary>
    /// Добавление номера (например, из другого года) в подшивку.
    /// </summary>
    /// <param name="bindingRecord">Запись подшивки.</param>
    /// <param name="issueRecord">Запись добавляемого номера журнала.</param>
    /// <param name="exemplarField">Поле с подшиваемым экземпляром.</param>
    public void BindIssue
        (
            Record bindingRecord,
            Record issueRecord,
            Field exemplarField
        )
    {
        Sure.NotNull (bindingRecord);
        Sure.NotNull (issueRecord);
        Sure.NotNull (exemplarField);

        // TODO: добавление информации о приплетенном номере?

        var bindingIndex = RecordConfiguration.GetIndex (bindingRecord).ThrowIfNullOrEmpty ();
        var bindingNumber = RecordConfiguration.GetExemplars (bindingRecord).First().Number;
        exemplarField.SetSubFieldValue ('a', ExemplarStatus.Bound) // подполе A: статус экземпляра
            .SetSubFieldValue ('p', bindingIndex) // подполе P: шифр подшивки
            .SetSubFieldValue ('i', bindingNumber); // подполе I: инвентарный номер подшивки
        // если нет поля 463, добавляем его
        if (issueRecord.GetField (463, 'w', bindingIndex, 0) is null)
        {
            issueRecord.Add
                (
                    463, // поле 463: издание, в котором опубликована статья
                    'w', // подполе W: шифр документа в базе
                    bindingIndex
                );
        }

        // меняем рабочий лист на подходящий
        issueRecord.SetValue
            (
                RecordConfiguration.WorksheetTag,
                Constants.Njp
            );

        Provider.WriteRecord (issueRecord);
    }

    /// <inheritdoc cref="IBindingManager.BindMagazines"/>
    public void BindMagazines
        (
            BindingSpecification specification
        )
    {
        // TODO: реализовать добавление номеров из другого года

        Sure.NotNull (specification);
        if (!specification.Verify (false))
        {
            // пустая спецификация подшивки
            _logger.LogError ("Empty binding specification");
            throw new IrbisException ("Empty binding specification");
        }

        var issueNumbers = specification.IssueNumbers.ThrowIfNullOrEmpty();
        var numbers = NumberRangeCollection.Parse (issueNumbers);
        if (numbers.Count == 0)
        {
            // список номеров пустой
            _logger.LogError ("Empty list of magazine issues");
            throw new IrbisException ("Empty list of magazine issues");
        }

        foreach (var number in numbers)
        {
            var numberText = number.ToString();
            var issueIndex = specification.BuildIndex (numberText);
            var issueRecord = Provider.ByIndex (issueIndex);
            if (issueRecord is null)
            {
                if (specification.IssueMustExist)
                {
                    _logger.LogError ("Can't find issue {IssueNumber}", numberText);
                    throw new IrbisException ($"Can't find issue {numberText}");
                }
            }
            else
            {
                if (!CheckIssue (specification, issueRecord))
                {
                    throw new IrbisException();
                }
            }
        }

        var bindingDescription = specification.Description.ThrowIfNullOrEmpty ();
        var bindingIndex = specification.BuildIndex (bindingDescription);

        var magazineIndex = specification.MagazineIndex.ThrowIfNullOrEmpty();
        var summaryRecord = Provider.ByIndex (magazineIndex);
        if (summaryRecord is null)
        {
            // если сводная запись не найдена, все плохо
            _logger.LogError ("Can't find summary record for the magazine");
            throw new IrbisException ("Can't find summary record for the magazine");
        }

        var magazine = MagazineInfo.Parse (summaryRecord); // TODO: брать данные из сводной записи
        if (magazine is null)
        {
            _logger.LogError ("Can't parse the summary record");

            // не удалось распарсить сводную запись
            throw new IrbisException ("Can't");
        }

        foreach (var numberText in numbers)
        {
            var inventory = specification.Inventory.ThrowIfNullOrEmpty();
            var complect = specification.Complect.ThrowIfNullOrEmpty();
            var place = specification.Place.ThrowIfNullOrEmpty();

            // создание записей, если их еще нет.
            var number = numberText.ToString();
            var issueIndex = specification.BuildIndex (number);
            var issueRecord = Provider.ByIndex (issueIndex);
            if (issueRecord is null)
            {
                issueRecord = new Record
                {
                    Database = Provider.EnsureDatabase()
                };

                issueRecord.Add (933, magazineIndex); // поле 933: шифр журнала
                issueRecord.Add (903, issueIndex); // поле 903: шифр выпуска
                issueRecord.Add (934, specification.Year); // поле 934: год выпуска журнала
                issueRecord.Add (936, number); // поле 936: номер, часть журнала
                issueRecord.Add (920, "NJP"); // поле 920: рабочий лист
                issueRecord.Fields.Add
                    (
                        new Field (910) // поле 910: сведения об экземпляре
                            .Add ('a', ExemplarStatus.Bound) // подполе A: статус экземпляра
                            .Add ('b', complect) // подполе B: номер комплекта
                            .Add ('c', "?") // подполе C: дата поступления
                            .Add ('d', place) // подполе D: место хранения
                            .Add ('p', bindingIndex) // подполе P: шифр подшивки
                            .Add ('i', inventory) // подполе I: инвентарный номер подшивки
                    );
            }
            else
            {
                // TODO: проверить существующие выпуски перед подшиванием

                // запись уже есть, нужно подкорректировать статус экземпляра
                var exemplarField = issueRecord.EnumerateField (910)
                    .FirstOrDefault(field => field['b'].SameString (complect)
                        && field['d'].SameString (place));
                if (exemplarField is null)
                {
                    // странно, нет такого экземпляра

                    _logger.LogError ("Can't find the exemplar to bind");
                    throw new IrbisException ("Can't find the exemplar to bind");
                }

                exemplarField
                    .SetSubFieldValue ('a', ExemplarStatus.Bound) // подполе A: статус экземпляра
                    .SetSubFieldValue ('p', bindingIndex) // подполе P: шифр подшивки
                    .SetSubFieldValue ('i', inventory); // подполе I: инвентарный номер подшивки
            }

            // если нет поля 463, добавляем его
            if (issueRecord.GetField (463, 'w', bindingIndex, 0) is null)
            {
                issueRecord.Add
                    (
                        463, // поле 463: издание, в котором опубликована статья
                        'w', // подполе W: шифр документа в базе
                        bindingIndex
                    );
            }

            // меняем рабочий лист на подходящий
            issueRecord.SetValue
                (
                    RecordConfiguration.WorksheetTag,
                    Constants.Njp
                );
            _logger.LogDebug ("Issue {IssueIndex}: worksheet changed", issueIndex);

            Provider.WriteRecord (issueRecord);
            _logger.LogTrace ("Issue {IssueIndex} saved", issueIndex);
        }

        // создание записи подшивки, если ее еще нет
        var bindingRecord = Provider.ByIndex (bindingIndex);
        if (bindingRecord is null)
        {
            bindingRecord = new Record
            {
                Database = Provider.EnsureDatabase()
            };

            bindingRecord.Add (933, specification.MagazineIndex); // поле 933: шифр журнала
            bindingRecord.Add (903, bindingIndex); // поле 903: шифр подписки
            bindingRecord.Add (934, specification.Year); // поле 934: год выпуска журналов в подшивке
            bindingRecord.Add (936, bindingDescription); // поле 936: номер, часть журнала
            bindingRecord.Add (931, specification.IssueNumbers); // поле 931: дополнение к номеру
            // TODO: 931^c - дополнение к номеру (выводится в скобках)
            bindingRecord.Add (920, Constants.Njk); // поле 920: рабочий лист
        }

        bindingRecord.Fields.Add
            (
                new Field { Tag = 910 }                  // поле 910: сведения об экземпляре
                    .Add ('a', ExemplarStatus.Free) // подполе A: статус экземпляра
                    .Add ('b', specification.Inventory)  // подполе B: инвентарный номер
                    .Add ('c', IrbisDate.TodayText)      // подполе C: дата поступления
                    .Add ('d', specification.Place)      // подполе D: место хранения
                    .AddNonEmpty ('h', specification.BindingBarcode) // подполе H: штрих-код или радиометка
            );

        Provider.WriteRecord (bindingRecord);

        // Обновление кумуляции
        // TODO: произвести настоящую кумуляцию
        summaryRecord.Fields.Add
            (
                new Field { Tag = 909 }                    // поле 909: зарегистрированы поступления
                    .Add ('q', specification.Year)         // подполе Q: кумулированные сведения, год
                    .Add ('d', specification.Place)        // подполе D: место хранения
                    .Add ('k', specification.Complect)     // подполе K: номер комплекта
                    .Add ('h', specification.IssueNumbers) // подполе H: кумулированные сведения, номера
            );

        Provider.WriteRecord (summaryRecord);

        _logger.LogInformation ("Done binding");
    }

    /// <inheritdoc cref="IBindingManager.CheckIssue(BindingSpecification,MagazineIssueInfo)"/>
    public bool CheckIssue
        (
            BindingSpecification specification,
            Record record
        )
    {
        Sure.NotNull (specification);
        Sure.NotNull (record);

        return CheckIssue (specification, MagazineIssueInfo.Parse (record));
    }


    /// <inheritdoc cref="IBindingManager.CheckIssue(BindingSpecification,MagazineIssueInfo)"/>
    public bool CheckIssue
        (
            BindingSpecification specification,
            MagazineIssueInfo issue
        )
    {
        Sure.NotNull (specification);
        Sure.NotNull (issue);

        if (!BindingConfiguration.CheckWorksheet (issue.Worksheet))
        {
            // в номере неверный рабочий лист
            _logger.LogDebug ("Bad worksheet");
            return false;
        }

        var exemplars = issue.Exemplars;
        if (exemplars.IsNullOrEmpty())
        {
            // в номере не зарегистрированы экземпляры
            _logger.LogDebug ("No exemplars");
            return false;
        }

        exemplars = exemplars
            .Where (exemplar => exemplar.Number == specification.Complect)
            .ToArray();
        if (exemplars.IsNullOrEmpty())
        {
            // в номере нет требуемого комплекта
            _logger.LogDebug ("No such complect");
            return false;
        }

        if (exemplars.Length != 1)
        {
            // в номере больше одного комплекта
            _logger.LogDebug ("Too many complects");
            return false;
        }

        var place = exemplars.First().Place;
        if (!BindingConfiguration.CheckPlace (place))
        {
            // запрещенное место хранения
            _logger.LogDebug ("Bad place");
            return false;
        }

        if (!place.SameString (specification.Place))
        {
            // различаются места хранения
            _logger.LogDebug ("Place mismatch");
            return false;
        }

        // проверка осуществляется до подшивания экземпляров
        if (!BindingConfiguration.CheckStatus (exemplars.First().Status))
        {
            // недопустимый статус экземпляра
            _logger.LogDebug ("Bad status");
            return false;
        }

        // TODO: проверить попадание номера в спецификацию подшивки

        return true;
    }

    /// <inheritdoc cref="IBindingManager.UnbindMagazines"/>
    public bool UnbindMagazines
        (
            string bindingIndex,
            bool decumulate = true,
            bool deleteBinding = true
        )
    {
        Sure.NotNullNorEmpty (bindingIndex);

        var bindingRecord = Provider.ByIndex (bindingIndex);
        if (bindingRecord is null)
        {
            _logger.LogDebug ("Can't find binding record");
            return false;
        }

        var expression = $"\"II={bindingIndex}\"";
        var issueRecords = Provider.SearchReadRecords (expression);
        if (issueRecords is null)
        {
            _logger.LogDebug ("Can't read issue records");
            return false;
        }

        _logger.LogTrace
            (
                "Binding {Binding}, found issues: {IssueCount}",
                bindingIndex,
                issueRecords.Length
            );

        foreach (var issueRecord in issueRecords)
        {
            var needWrite = false;
            var field463 = issueRecord.GetField (463, 'w', bindingIndex, 0);
            if (field463 is not null)
            {
                issueRecord.Fields.Remove (field463);
                needWrite = true;
            }

            // отыскиваем подшитыве экземпляры и исправляем их
            var exemplar = issueRecord.GetField (910, 'p', bindingIndex, 0);
            if (exemplar is not null)
            {
                exemplar
                    .SetSubFieldValue ('a', ExemplarStatus.Free) // подполе A: статус экземпляра
                    .RemoveSubField ('p') // подполе P: шифр подшивки
                    .RemoveSubField ('i'); // подполе I: инвентарный номер подшивки
                needWrite = true;
            }

            // проверяем, остались ли подшитые экземпляры
            var bound = issueRecord.GetField (910, 'a', ExemplarStatus.Bound);
            if (bound.Length == 0)
            {
                issueRecord.SetValue
                    (
                        RecordConfiguration.WorksheetTag,
                        Constants.Nj
                    );
            }

            // отправляем исправленную запись на сервер
            if (needWrite)
            {
                Provider.WriteRecord (issueRecord);

                var issueIndex = RecordConfiguration.GetIndex (issueRecord);
                _logger.LogInformation ("Issue {IssueIndex} fixed", issueIndex);
            }
        }

        // декумуляция
        if (decumulate)
        {
            // TODO: implement
        }

        // удаление записи подшивки
        if (deleteBinding)
        {
            var bindingRecordMfn = bindingRecord.Mfn;
            bindingRecord.Status |= RecordStatus.LogicallyDeleted;
            Provider.WriteRecord (bindingRecord);
            _logger.LogTrace ("Binding record {Mfn} deleted", bindingRecordMfn);
        }

        _logger.LogInformation ("Done unbinding");

        return true;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // тело метода пока оставлено пустым
    }

    #endregion
}
