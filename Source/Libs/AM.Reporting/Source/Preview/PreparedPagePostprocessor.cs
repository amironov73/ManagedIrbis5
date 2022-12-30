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

using System;
using System.Collections.Generic;
using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Preview
{
    internal class PreparedPagePosprocessor
    {
        private Dictionary<string, List<TextObjectBase>> duplicates;
        private Dictionary<int, Base> bands;
        int iBand;

        private void ProcessDuplicates (TextObjectBase obj)
        {
            if (duplicates.ContainsKey (obj.Name))
            {
                List<TextObjectBase> list = duplicates[obj.Name];
                var lastObj = list[list.Count - 1];

                var isDuplicate = true;

                // compare Text
                if (obj.Text != lastObj.Text)
                {
                    isDuplicate = false;
                }
                else
                {
                    var lastObjBottom = (lastObj.Parent as ReportComponentBase).Bottom;
                    var objTop = (obj.Parent as ReportComponentBase).Top;
                    if (Math.Abs (objTop - lastObjBottom) > 0.5f)
                    {
                        isDuplicate = false;
                    }
                }

                if (isDuplicate)
                {
                    list.Add (obj);
                }
                else
                {
                    // close duplicates
                    CloseDuplicates (list);

                    // add new obj
                    list.Clear();
                    list.Add (obj);
                }
            }
            else
            {
                List<TextObjectBase> list = new List<TextObjectBase>();
                list.Add (obj);
                duplicates.Add (obj.Name, list);
            }
        }

        private void CloseDuplicates()
        {
            foreach (List<TextObjectBase> list in duplicates.Values)
            {
                CloseDuplicates (list);
            }
        }

        private void CloseDuplicates (List<TextObjectBase> list)
        {
            if (list.Count == 0)
            {
                return;
            }

            var duplicates = list[0].Duplicates;
            switch (duplicates)
            {
                case Duplicates.Clear:
                    CloseDuplicatesClear (list);
                    break;
                case Duplicates.Hide:
                    CloseDuplicatesHide (list);
                    break;
                case Duplicates.Merge:
                    CloseDuplicatesMerge (list);
                    break;
            }
        }

        private void CloseDuplicatesClear (List<TextObjectBase> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    list[i].Text = "";
                }
            }
        }

        private void CloseDuplicatesHide (List<TextObjectBase> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    list[i].Dispose();
                }
            }
        }

        private void CloseDuplicatesMerge (List<TextObjectBase> list)
        {
            var top = list[0].AbsTop;

            // dispose all objects except the last one
            for (var i = 0; i < list.Count - 1; i++)
            {
                list[i].Dispose();
            }

            // stretch the last object
            var lastObj = list[list.Count - 1];
            var delta = lastObj.AbsTop - top;
            lastObj.Top -= delta;
            lastObj.Height += delta;
        }

        public void Postprocess (ReportPage page)
        {
            page.ExtractMacros();
            var allObjects = page.AllObjects;
            for (var i = 0; i < allObjects.Count; i++)
            {
                var c = allObjects[i];
                if (c.Report == null)
                {
                    c.SetReport (page.Report);
                }

                c.ExtractMacros();

                if (c is BandBase @base)
                {
                    @base.UpdateWidth();
                }

                if (c is TextObjectBase objectBase && objectBase.Duplicates != Duplicates.Show)
                {
                    ProcessDuplicates (objectBase);
                }
            }

            CloseDuplicates();
        }

        public PreparedPagePosprocessor()
        {
            duplicates = new Dictionary<string, List<TextObjectBase>>();
            bands = new Dictionary<int, Base>();
            iBand = 0;
        }

        public void PostprocessUnlimited (PreparedPage preparedPage, ReportPage page)
        {
            var flag = false;
            var i = 0;
            foreach (var b in preparedPage.GetPageItems (page, true))
            {
                foreach (Base c in b.AllObjects)
                {
                    if (c is TextObjectBase @base && @base.Duplicates != Duplicates.Show)
                    {
                        ProcessDuplicates (@base);
                        flag = true; //flag for keep in dictionary
                    }
                }

                i++;
                if (flag)
                {
                    b.ExtractMacros();
                    bands[i - 1] = b;
                }
                else
                {
                    b.Dispose();
                }
            }

            CloseDuplicates();
        }

        public Base PostProcessBandUnlimitedPage (Base band)
        {
            if (bands.ContainsKey (iBand))
            {
                var replaceBand = bands[iBand];
                var parent = band.Parent;
                band.Parent = null;
                replaceBand.Parent = parent;
                band.Dispose();
                iBand++;
                return replaceBand;
            }

            band.ExtractMacros();
            iBand++;
            return band;
        }
    }
}
