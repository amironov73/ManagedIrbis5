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

using AM.Reporting.Table;
using AM.Reporting.Utils;

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Preview
{
    internal class Dictionary
    {
        private SortedList<string, DictionaryItem> names;

        //private SortedDictionary<string, DictionaryItem> FNames;
        private Hashtable baseNames;
        private PreparedPages preparedPages;

        private void AddBaseName (string name)
        {
            for (var i = 0; i < name.Length; i++)
            {
                if (name[i] >= '0' && name[i] <= '9')
                {
                    var baseName = name.Substring (0, i);
                    var num = int.Parse (name.Substring (i));
                    if (baseNames.ContainsKey (baseName))
                    {
                        var maxNum = (int)baseNames[baseName];
                        if (num < maxNum)
                        {
                            num = maxNum;
                        }
                    }

                    baseNames[baseName] = num;
                    return;
                }
            }
        }

        public string CreateUniqueName (string baseName)
        {
            var num = 1;
            if (baseNames.ContainsKey (baseName))
            {
                num = (int)baseNames[baseName] + 1;
            }

            baseNames[baseName] = num;
            return baseName + num.ToString();
        }

        private void Add (string name, string sourceName, Base obj)
        {
            names.Add (name, new DictionaryItem (sourceName, obj));
        }

        public string AddUnique (string baseName, string sourceName, Base obj)
        {
            var result = CreateUniqueName (baseName);
            Add (result, sourceName, obj);
            return result;
        }

        public Base GetObject (string name)
        {
            if (names.TryGetValue (name, out var item))
            {
                //                return item.CloneObject(name);
                if (item.OriginalComponent != null)
                {
                    item.OriginalComponent.SetReport (preparedPages.Report);
                }

                var result = item.CloneObject (name);

                //result.SetReport(this);
                return result;
            }
            else
            {
                return null;
            }

            //int i = FNames.IndexOfKey(name);
            //if (i == -1)
            //  return null;
            //return FNames.Values[i].CloneObject(name);
        }

        public Base GetOriginalObject (string name)
        {
            if (names.TryGetValue (name, out var item))
            {
                return item.OriginalComponent;
            }
            else
            {
                return null;
            }

            //  int i = FNames.IndexOfKey(name);
            //if (i == -1)
            //  return null;
            //return FNames.Values[i].OriginalComponent;
        }

        public void Clear()
        {
            names.Clear();
            baseNames.Clear();
        }

        public void Save (XmlItem rootItem)
        {
            rootItem.Clear();
            foreach (KeyValuePair<string, DictionaryItem> pair in names)
            {
                var xi = rootItem.Add();
                xi.Name = pair.Key;
                xi.ClearProps();
                xi.SetProp ("name", pair.Value.SourceName);

                //xi.Text = String.Concat("name=\"", pair.Value.SourceName, "\"");
            }

            //for (int i = 0; i < FNames.Count; i++)
            //{
            //  XmlItem xi = rootItem.Add();
            //  xi.Name = FNames.Keys[i];
            //  xi.Text = "name=\"" + FNames.Values[i].SourceName + "\"";
            //}
        }

        public void Load (XmlItem rootItem)
        {
            Clear();
            for (var i = 0; i < rootItem.Count; i++)
            {
                // rootItem[i].Name is 's1', rootItem[i].Text is 'name="Page0.Shape1"'
                var sourceName = rootItem[i].GetProp ("name");

                // split to Page0, Shape1
                string[] objName = sourceName.Split ('.');

                // get page number
                var pageN = int.Parse (objName[0].Substring (4));

                // get the object
                Base obj = null;
                if (objName.Length == 2)
                {
                    obj = preparedPages.SourcePages[pageN].FindObject (objName[1]);
                }
                else
                {
                    obj = preparedPages.SourcePages[pageN];
                }

                // add s1, Page0.Shape1, object
                var name = rootItem[i].Name;
                Add (name, sourceName, obj);
                AddBaseName (name);
            }
        }

        public Dictionary (PreparedPages preparedPages)
        {
            this.preparedPages = preparedPages;
            names = new SortedList<string, DictionaryItem>();

            //FNames = new SortedDictionary<string, DictionaryItem>();
            baseNames = new Hashtable();
        }

        private class DictionaryItem
        {
            public string SourceName { get; }

            public Base OriginalComponent { get; }

            public Base CloneObject (string alias)
            {
                Base result = null;
                var type = OriginalComponent.GetType();

                // try frequently used objects first. The CreateInstance method is very slow.
                if (type == typeof (TextObject))
                {
                    result = new TextObject();
                }
                else if (type == typeof (TableCell))
                {
                    result = new TableCell();
                }
                else if (type == typeof (DataBand))
                {
                    result = new DataBand();
                }
                else
                {
                    result = Activator.CreateInstance (type) as Base;
                }

                result.Assign (OriginalComponent);
                result.OriginalComponent = OriginalComponent;
                result.Alias = alias;
                result.SetName (OriginalComponent.Name);
                if (result is ReportComponentBase @base)
                {
                    @base.AssignPreviewEvents (OriginalComponent);
                }

                return result;
            }

            public DictionaryItem (string name, Base obj)
            {
                SourceName = name;
                OriginalComponent = obj;
            }
        }
    }
}
