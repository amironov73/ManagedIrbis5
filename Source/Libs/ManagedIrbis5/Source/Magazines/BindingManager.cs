// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BindingManager.cs -- менеджер подшивок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

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
        /// Connection.
        /// </summary>
        public ISyncIrbisProvider Connection { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BindingManager
            (
                ISyncIrbisProvider connection
            )
        {
            Connection = connection;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create or update binding according the specification.
        /// </summary>
        public void BindMagazines
            (
                BindingSpecification specification
            )
        {
            if (string.IsNullOrEmpty(specification.MagazineIndex)
                || string.IsNullOrEmpty(specification.Year)
                || string.IsNullOrEmpty(specification.IssueNumbers)
                || string.IsNullOrEmpty(specification.Description)
                || string.IsNullOrEmpty(specification.BindingNumber)
                || string.IsNullOrEmpty(specification.Inventory)
                || string.IsNullOrEmpty(specification.Fond)
                || string.IsNullOrEmpty(specification.Complect))
            {
                throw new IrbisException("Empty binding specification");
            }

            // TOOD implement

            /*
            NumberRangeCollection collection = NumberRangeCollection.Parse(specification.IssueNumbers);
            string longDescription = string.Format("Подшивка N{0} {1} ({2})", specification.BindingNumber,
                specification.Description, specification.IssueNumbers);
            string bindingIndex = string.Format("{0}/{1}/{2}", specification.MagazineIndex, specification.Year,
                longDescription);
                */
            string longDescription = string.Empty;
            string bindingIndex = string.Empty;
            NumberText[] collection = new NumberText[0];

            /*
            Record mainRecord = Connection.ByIndex(specification.MagazineIndex);
            */
            Record mainRecord = new Record();

            var magazine = MagazineInfo.Parse(mainRecord);

            foreach (NumberText numberText in collection)
            {
                // Создание записей, если их еще нет.
                var issueRecord = new Record
                {
                    Database = Connection.Database
                };

                string issueIndex = string.Format
                    (
                        "{0}/{1}/{2}",
                        specification.MagazineIndex,
                        specification.Year,
                        numberText
                    );
                issueRecord.Add(933, specification.MagazineIndex);
                issueRecord.Add(903, issueIndex);
                issueRecord.Add(934, specification.Year);
                issueRecord.Add(936, numberText.ToString());
                issueRecord.Add(920, "NJP");
                issueRecord.Fields.Add
                    (
                        new Field {Tag = 910}
                            .Add('a', "0")
                            .Add('b', specification.Complect)
                            .Add('c', "?")
                            .Add('d', specification.Fond)
                            .Add('p', bindingIndex)
                            .Add('i', specification.Inventory)
                    );
                issueRecord.Fields.Add
                        (
                            new Field { Tag = 463 }
                                .Add('w', bindingIndex)
                        );

                Connection.WriteRecord(issueRecord);
            }

            // Создание записи подшивки, если ее еще нет
            var bindingRecord = new Record
            {
                Database = Connection.Database
            };

            bindingRecord.Add(933, specification.MagazineIndex);
            bindingRecord.Add(903, bindingIndex);
            bindingRecord.Add(904, specification.Year);
            bindingRecord.Add(936, longDescription);
            bindingRecord.Add(931, specification.IssueNumbers);
            bindingRecord.Add(920, "NJK");
            bindingRecord.Fields.Add
                    (
                        new Field { Tag = 910 }
                            .Add('a', "0")
                            .Add('b', specification.Inventory)
                            .Add('c', "?")
                            .Add('d', specification.Fond)
                    );

            Connection.WriteRecord(bindingRecord);

            // Обновление кумуляции
            mainRecord.Fields.Add
                (
                    new Field { Tag = 909 }
                        .Add('q', specification.Year)
                        .Add('d', specification.Fond)
                        .Add('k', specification.Complect)
                        .Add('h', specification.IssueNumbers)
                );
            Connection.WriteRecord(mainRecord);
        }

        /// <summary>
        /// Расшитие и удаление подшивки по ее индексу.
        /// </summary>
        public void Unbind
            (
                string bindingIndex
            )
        {
            throw new NotImplementedException();
        }

        #endregion

    } // class BindingManager

} // namespace ManagedIrbis.Magazines
