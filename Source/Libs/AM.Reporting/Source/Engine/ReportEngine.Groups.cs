// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Data;

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Engine
{
    public partial class ReportEngine
    {
        #region Private Classes

        private class GroupTreeItem
        {
            #region Fields

            #endregion Fields

            #region Properties

            public GroupHeaderBand Band { get; }

            public List<GroupTreeItem> Items { get; }

            public GroupTreeItem FirstItem => Items.Count == 0 ? null : Items[0];

            public GroupTreeItem LastItem => Items.Count == 0 ? null : Items[Items.Count - 1];

            public int RowNo { get; set; }

            public int RowCount { get; set; }

            #endregion Properties

            #region Constructors

            public GroupTreeItem (GroupHeaderBand band)
            {
                this.Band = band;
                Items = new List<GroupTreeItem>();
            }

            #endregion Constructors

            #region Public Methods

            public GroupTreeItem AddItem (GroupTreeItem item)
            {
                Items.Add (item);
                return item;
            }

            #endregion Public Methods
        }

        #endregion Private Classes

        #region Private Methods

        private void ShowDataHeader (GroupHeaderBand groupBand)
        {
            groupBand.RowNo = 0;

            var header = groupBand.Header;
            if (header != null)
            {
                ShowBand (header);
                if (header.RepeatOnEveryPage)
                {
                    AddReprint (header);
                }
            }

            var footer = groupBand.Footer;
            if (footer != null)
            {
                if (footer.RepeatOnEveryPage)
                {
                    AddReprint (footer);
                }
            }
        }

        private void ShowDataFooter (GroupHeaderBand groupBand)
        {
            var footer = groupBand.Footer;
            RemoveReprint (footer);
            ShowBand (footer);
            RemoveReprint (groupBand.Header);
        }

        private void ShowGroupHeader (GroupHeaderBand header)
        {
            header.AbsRowNo++;
            header.RowNo++;

            if (header.ResetPageNumber && (header.FirstRowStartsNewPage || header.RowNo > 1))
            {
                ResetLogicalPageNumber();
            }

            if (header.KeepTogether)
            {
                StartKeep (header);
            }

            if (header.KeepWithData)
            {
                StartKeep (header.GroupDataBand);
            }

            // start group event
            OnStateChanged (header, EngineState.GroupStarted);

            ShowBand (header);
            if (header.RepeatOnEveryPage)
            {
                AddReprint (header);
            }

            var footer = header.GroupFooter;
            if (footer != null)
            {
                if (footer.RepeatOnEveryPage)
                {
                    AddReprint (footer);
                }
            }
        }

        private void ShowGroupFooter (GroupHeaderBand header)
        {
            // finish group event
            OnStateChanged (header, EngineState.GroupFinished);

            // rollback to previous data row to print the header condition in the footer.
            var dataBand = header.GroupDataBand;
            var dataSource = dataBand.DataSource;
            dataSource.Prior();

            var footer = header.GroupFooter;
            if (footer != null)
            {
                footer.AbsRowNo++;
                footer.RowNo++;
            }

            RemoveReprint (footer);
            ShowBand (footer);
            RemoveReprint (header);

            // restore current row
            dataSource.Next();

            OutlineUp (header);
            if (header.KeepTogether)
            {
                EndKeep();
            }

            if (footer != null && footer.KeepWithData)
            {
                EndKeep();
            }
        }

        private void InitGroupItem (GroupHeaderBand header, GroupTreeItem curItem)
        {
            while (header != null)
            {
                header.ResetGroupValue();
                header.AbsRowNo = 0;
                header.RowNo = 0;

                curItem = curItem.AddItem (new GroupTreeItem (header));
                curItem.RowNo = header.DataSource.CurrentRowNo;
                curItem.RowCount++;
                header = header.NestedGroup;
            }
        }

        private void CheckGroupItem (GroupHeaderBand header, GroupTreeItem curItem)
        {
            while (header != null)
            {
                if (header.GroupValueChanged())
                {
                    InitGroupItem (header, curItem);
                    break;
                }

                header = header.NestedGroup;
                curItem = curItem.LastItem;
                curItem.RowCount++;
            }
        }

        private GroupTreeItem MakeGroupTree (GroupHeaderBand groupBand)
        {
            var rootItem = new GroupTreeItem (null);
            var dataSource = groupBand.DataSource;
            var isFirstRow = true;

            // cycle through rows
            dataSource.First();
            while (dataSource.HasMoreRows)
            {
                if (isFirstRow)
                {
                    InitGroupItem (groupBand, rootItem);
                }
                else
                {
                    CheckGroupItem (groupBand, rootItem);
                }

                dataSource.Next();
                isFirstRow = false;
                if (Report.Aborted)
                {
                    break;
                }
            }

            return rootItem;
        }

        private void ShowGroupTree (GroupTreeItem root)
        {
            if (root.Band != null)
            {
                root.Band.GroupDataBand.DataSource.CurrentRowNo = root.RowNo;
                ShowGroupHeader (root.Band);
            }

            if (root.Items.Count == 0)
            {
                if (root.RowCount != 0)
                {
                    var rowCount = root.RowCount;
                    var maxRows = root.Band.GroupDataBand.MaxRows;
                    if (maxRows > 0 && rowCount > maxRows)
                    {
                        rowCount = maxRows;
                    }

                    var keepFirstRow = NeedKeepFirstRow (root.Band);
                    var keepLastRow = NeedKeepLastRow (root.Band.GroupDataBand);
                    RunDataBand (root.Band.GroupDataBand, rowCount, keepFirstRow, keepLastRow);
                }
            }
            else
            {
                ShowDataHeader (root.FirstItem.Band);

                for (var i = 0; i < root.Items.Count; i++)
                {
                    var item = root.Items[i];
                    item.Band.IsFirstRow = i == 0;
                    item.Band.IsLastRow = i == root.Items.Count - 1;

                    ShowGroupTree (item);
                    if (Report.Aborted)
                    {
                        break;
                    }
                }

                ShowDataFooter (root.FirstItem.Band);
            }

            if (root.Band != null)
            {
                ShowGroupFooter (root.Band);
            }
        }

        private void RunGroup (GroupHeaderBand groupBand)
        {
            var dataSource = groupBand.DataSource;
            if (dataSource != null)
            {
                // init the datasource - set group conditions to sort data rows
                groupBand.InitDataSource();

                // show the group tree
                ShowGroupTree (MakeGroupTree (groupBand));

                // finalize the datasource, remove the group condition
                // from the databand sort
                groupBand.FinalizeDataSource();

                // do not leave the datasource in EOF state to allow print something in the footer
                dataSource.Prior();
            }
        }

        #endregion Private Methods
    }
}
