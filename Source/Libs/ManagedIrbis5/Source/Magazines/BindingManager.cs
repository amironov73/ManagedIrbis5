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

using AM;
using AM.Text.Ranges;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Менеджер подшивок.
    /// </summary>
    public sealed class BindingManager
    {
        #region Properties

        /// <summary>
        /// Синхронный провайдер.
        /// </summary>
        public ISyncProvider Provider { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BindingManager
            (
                ISyncProvider provider
            )
        {
            Provider = provider;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Создание либо обновление подшивки по ее спецификации.
        /// </summary>
        public void BindMagazines
            (
                BindingSpecification specification
            )
        {
            Sure.NotNull (specification);

            if (string.IsNullOrEmpty (specification.MagazineIndex)
                || string.IsNullOrEmpty (specification.Year)
                || string.IsNullOrEmpty (specification.IssueNumbers)
                || string.IsNullOrEmpty (specification.Description)
                || string.IsNullOrEmpty (specification.BindingNumber)
                || string.IsNullOrEmpty (specification.Inventory)
                || string.IsNullOrEmpty (specification.Fond)
                || string.IsNullOrEmpty (specification.Complect))
            {
                throw new IrbisException ("Empty binding specification");
            }

            var numbers = NumberRangeCollection.Parse (specification.IssueNumbers);
            if (numbers.Count == 0)
            {
                // если список номеров пустой
                throw new IrbisException();
            }

            var bindingDescription = specification.Description;
            var bindingIndex = string.IsNullOrEmpty (specification.VolumeNumber)
                    ? $"{specification.MagazineIndex}/{specification.Year}/{bindingDescription}"
                    : $"{specification.MagazineIndex}/{specification.Year}/{specification.VolumeNumber}/{bindingDescription}";

            var mainRecord = Provider.ByIndex (specification.MagazineIndex);
            if (mainRecord is null)
            {
                // если сводная запись не найдена, все плохо
                throw new IrbisException();
            }

            var magazine = MagazineInfo.Parse (mainRecord); // TODO: брать данные из сводной записи
            if (magazine is null)
            {
                // если не удалось распарсить сводную запись, все плохо
                throw new IrbisException();
            }

            foreach (var numberText in numbers)
            {
                // Создание записей, если их еще нет.
                var issueIndex = $"{specification.MagazineIndex}/{specification.Year}/{numberText}";
                var issueRecord = Provider.ByIndex (issueIndex);
                if (issueRecord is null)
                {
                    issueRecord = new Record
                    {
                        Database = Provider.Database
                    };

                    issueRecord.Add (933, specification.MagazineIndex); // поле 933: шифр журнала
                    issueRecord.Add (903, issueIndex); // поле 903: шифр выпуска
                    issueRecord.Add (934, specification.Year); // поле 934: год выпуска журнала
                    issueRecord.Add (936, numberText.ToString()); // поле 936: номер, часть журнала
                    issueRecord.Add (920, "NJP"); // поле 920: рабочий лист
                    issueRecord.Fields.Add
                        (
                            new Field { Tag = 910 } // поле 910: сведения об экземпляре
                                .Add ('a', "0") // подполе A: статус экземпляра
                                .Add ('b', specification.Complect) // подполе B: номер комплекта
                                .Add ('c', "?") // подполе C: дата поступления
                                .Add ('d', specification.Fond) // подполе D: место хранения
                                .Add ('p', bindingIndex) // подполе P: шифр подшивки
                                .Add ('i', specification.Inventory) // подполе I: инвентарный номер подшивки
                        );

                } // if

                issueRecord.Fields.Add
                    (
                        new Field { Tag = 463 } // поле 463: издание, в котором опубликована статья
                            .Add ('w', bindingIndex) // подполе W: шифр документа в базе
                    );

                Provider.WriteRecord (issueRecord);

            } // foreach

            // Создание записи подшивки, если ее еще нет
            var bindingRecord = Provider.ByIndex (bindingIndex);
            if (bindingRecord is null)
            {
                bindingRecord = new Record
                {
                    Database = Provider.Database
                };

                bindingRecord.Add (933, specification.MagazineIndex); // поле 933: шифр журнала
                bindingRecord.Add (903, bindingIndex); // поле 903: шифр подписки
                bindingRecord.Add (934, specification.Year); // поле 934: год выпуска журналов в подшивке
                bindingRecord.Add (936, bindingDescription); // поле 936: номер, часть журнала
                bindingRecord.Add (931, specification.IssueNumbers); // поле 931: дополнение к номеру
                // TODO: 931^c - дополнение к номеру (выводится в скобках)
                bindingRecord.Add (920, "NJK"); // поле 920: рабочий лист

            } // if

            bindingRecord.Fields.Add
                (
                    new Field { Tag = 910 }                 // поле 910: сведения об экземпляре
                        .Add ('a', "0")                     // подполе A: статус экземпляра
                        .Add ('b', specification.Inventory) // подполе B: инвентарный номер
                        .Add ('c', IrbisDate.TodayText)     // подполе C: дата поступления
                        .Add ('d', specification.Fond)      // подполе D: место хранения
                    // TODO: 910^h - штрих-код или радиометка подшивки
                );

            Provider.WriteRecord (bindingRecord);

            // Обновление кумуляции
            // TODO: произвести настоящую кумуляцию
            mainRecord.Fields.Add
                (
                    new Field { Tag = 909 }                    // поле 909: зарегистрированы поступления
                        .Add ('q', specification.Year)         // подполе Q: кумулированные сведения, год
                        .Add ('d', specification.Fond)         // подполе D: место хранения
                        .Add ('k', specification.Complect)     // подполе K: номер комплекта
                        .Add ('h', specification.IssueNumbers) // подполе H: кумулированные сведения, номера
                );

            Provider.WriteRecord (mainRecord);

        } // method BindMagazines

        /// <summary>
        /// Расшитие и удаление подшивки по ее индексу.
        /// </summary>
        public void UnbindMagazines
            (
                string bindingIndex
            )
        {
            Sure.NotNullNorEmpty (bindingIndex);

            throw new NotImplementedException();

        } // method UnbindMagazines

        #endregion

    } // class BindingManager

} // namespace ManagedIrbis.Magazines
