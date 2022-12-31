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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using AM.Reporting.CrossView;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    internal class DictionaryHelper
    {
        #region Private Fields

        private Dictionary<string, Base> aliases;
        private Dictionary<string, Base> allObjects;
        private Dictionary<string, Base> allReportObjects;
        private Dictionary<string, Base> dataComponents;
        private Dictionary dictionary;
        private Dictionary<string, Base> fullNames;

        #endregion Private Fields

        #region Internal Methods

        internal void RegisterDataSet (DataSet data, string referenceName, bool enabled)
        {
            foreach (DataTable table in data.Tables)
            {
                PRegisterDataTable (table, referenceName + "." + table.TableName, enabled);
            }

            foreach (DataRelation relation in data.Relations)
            {
                PRegisterDataRelation (relation, referenceName + "." + relation.RelationName, enabled);
            }
        }

        internal void ReRegisterData (List<Dictionary.RegDataItem> fRegisteredItems, bool flag)
        {
            foreach (var item in fRegisteredItems)
            {
                PRegisterData (item.data, item.name, flag);
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddBaseToDictionary (Base b)
        {
            if (b is DataComponentBase { ReferenceName: { } } @base)
            {
                dataComponents[@base.ReferenceName] = @base;
            }

            if ((b is DataComponentBase || b is Relation) && (b as DataComponentBase).Alias != null)
            {
                aliases[(b as DataComponentBase).Alias] = b;
            }

            if (b is DataSourceBase sourceBase)
            {
                var fullname = sourceBase.FullName;
                if (fullname != null)
                {
                    fullNames[fullname] = sourceBase;
                }
            }

            if (b.Name != null)
            {
                allObjects[b.Name] = b;
                allReportObjects[b.Name] = b;
            }
        }

        private void AddBaseWithChiledToDictonary (Base b)
        {
            AddBaseToDictionary (b);
            foreach (Base obj in b.AllObjects)
            {
                AddBaseToDictionary (obj);
            }
        }

        private string CreateUniqueAlias (string alias)
        {
            var baseAlias = alias;
            var i = 1;
            while (FindByAlias (alias) != null)
            {
                alias = baseAlias + i.ToString();
                i++;
            }

            return alias;
        }

        private string CreateUniqueName (string name)
        {
            var baseName = name;
            var i = 1;
            while (FindByName (name) != null || FindReportObjectByName (name) != null)
            {
                name = baseName + i.ToString();
                i++;
            }

            return name;
        }

        private DataComponentBase FindByAlias (string alias)
        {
            Base result = null;
            if (aliases.TryGetValue (alias, out result))
            {
                if ((result is DataConnectionBase || result is Relation))
                {
                    return result as DataComponentBase;
                }
            }

            if (fullNames.TryGetValue (alias, out result))
            {
                if (result is DataSourceBase @base)
                {
                    return @base;
                }
            }

            return null;
        }

        private Base FindByName (string name)
        {
            Base result = null;
            if (allObjects.TryGetValue (name, out result))
            {
                if (result is DataConnectionBase || result is DataSourceBase || result is Relation ||
                    (result is Parameter && result.Parent == dictionary) || result is Total)
                {
                    return result;
                }
            }

            return null;
        }

        private DataComponentBase FindDataComponent (string referenceName)
        {
            if (dataComponents.TryGetValue (referenceName, out var c))
            {
                if (c is DataComponentBase @base)
                {
                    return @base;
                }
            }

            return null;
        }

        private Base FindReportObjectByName (string name)
        {
            Base result = null;
            if (allReportObjects.TryGetValue (name, out result))
            {
                return result;
            }

            return null;
        }

        private void PRegisterBusinessObject (IEnumerable data, string referenceName, int maxNestingLevel, bool enabled)
        {
            dictionary.AddRegisteredItem (data, referenceName);

            var dataType = data.GetType();
            if (data is BindingSource bindingSource)
            {
                if (bindingSource.DataSource is Type type)
                {
                    dataType = type;
                }
                else
                {
                    dataType = bindingSource.DataSource.GetType();
                }
            }

            var converter = new BusinessObjectConverter (dictionary);
            if (FindDataComponent (referenceName) is BusinessObjectDataSource source)
            {
                source.Reference = data;
                source.DataType = dataType;
                converter.UpdateExistingObjects (source, maxNestingLevel);
            }
            else
            {
                source = new BusinessObjectDataSource();
                source.ReferenceName = referenceName;
                source.Reference = data;
                source.DataType = dataType;
                source.Name = CreateUniqueName (referenceName);
                source.Alias = CreateUniqueAlias (source.Alias);
                source.Enabled = enabled;
                dictionary.DataSources.Add (source);
                converter.CreateInitialObjects (source, maxNestingLevel);
                AddBaseWithChiledToDictonary (source);
            }
        }

        private void PRegisterData (object data, string name, bool enabled)
        {
            if (data is DataSet set)
            {
                RegisterDataSet (set, name, enabled);
            }
            else if (data is DataTable table)
            {
                PRegisterDataTable (table, name, enabled);
            }
            else if (data is DataView view)
            {
                PRegisterDataView (view, name, enabled);
            }
            else if (data is DataRelation relation)
            {
                PRegisterDataRelation (relation, name, enabled);
            }
            else if (data is IEnumerable enumerable)
            {
                PRegisterBusinessObject (enumerable, name, 1, enabled);
            }
            else if (data is IBaseCubeLink link)
            {
                PRegisterCubeLink (link, name, enabled);
            }
        }

        private void PRegisterDataRelation (DataRelation relation, string referenceName, bool enabled)
        {
            dictionary.AddRegisteredItem (relation, referenceName);
            if (FindDataComponent (referenceName) != null)
            {
                return;
            }

            var rel = new Relation();
            rel.ReferenceName = referenceName;
            rel.Reference = relation;
            rel.Name = relation.RelationName;
            rel.Enabled = enabled;
            rel.ParentDataSource = dictionary.FindDataTableSource (relation.ParentTable);
            rel.ChildDataSource = dictionary.FindDataTableSource (relation.ChildTable);
            string[] parentColumns = new string[relation.ParentColumns.Length];
            string[] childColumns = new string[relation.ChildColumns.Length];
            for (var i = 0; i < relation.ParentColumns.Length; i++)
            {
                parentColumns[i] = relation.ParentColumns[i].Caption;
            }

            for (var i = 0; i < relation.ChildColumns.Length; i++)
            {
                childColumns[i] = relation.ChildColumns[i].Caption;
            }

            rel.ParentColumns = parentColumns;
            rel.ChildColumns = childColumns;
            dictionary.Relations.Add (rel);
            AddBaseWithChiledToDictonary (rel);
        }

        private void PRegisterDataTable (DataTable table, string referenceName, bool enabled)
        {
            dictionary.AddRegisteredItem (table, referenceName);

            if (FindDataComponent (referenceName) is TableDataSource source)
            {
                source.Reference = table;
                source.InitSchema();
                source.RefreshColumns (true);
                AddBaseWithChiledToDictonary (source);
            }
            else
            {
                // check tables inside connections. Are we trying to replace the connection table
                // with table provided by an application?
                source = FindByAlias (referenceName) as TableDataSource;

                // check "Data.TableName" case
                if (source == null && referenceName.StartsWith ("Data."))
                {
                    source = FindByAlias (referenceName.Remove (0, 5)) as TableDataSource;
                }

                if (source != null && (source.Connection != null || source.IgnoreConnection))
                {
                    source.IgnoreConnection = true;
                    source.Reference = table;
                    source.InitSchema();
                    source.RefreshColumns (true);
                    AddBaseWithChiledToDictonary (source);
                }
                else
                {
                    source = new TableDataSource();
                    source.ReferenceName = referenceName;
                    source.Reference = table;
                    source.Name = CreateUniqueName (referenceName.Contains (".") ? table.TableName : referenceName);
                    source.Alias = CreateUniqueAlias (source.Alias);
                    source.Enabled = enabled;
                    source.InitSchema();
                    dictionary.DataSources.Add (source);
                    AddBaseWithChiledToDictonary (source);
                }
            }
        }

        private void PRegisterDataView (DataView view, string referenceName, bool enabled)
        {
            dictionary.AddRegisteredItem (view, referenceName);

            if (FindDataComponent (referenceName) is ViewDataSource source)
            {
                source.Reference = view;
                source.InitSchema();
                source.RefreshColumns();
            }
            else
            {
                source = new ViewDataSource();
                source.ReferenceName = referenceName;
                source.Reference = view;
                source.Name = CreateUniqueName (referenceName);
                source.Alias = CreateUniqueAlias (source.Alias);
                source.Enabled = enabled;
                source.InitSchema();
                dictionary.DataSources.Add (source);
                AddBaseWithChiledToDictonary (source);
            }
        }

        private void PRegisterCubeLink (IBaseCubeLink cubeLink, string referenceName, bool enabled)
        {
            dictionary.AddRegisteredItem (cubeLink, referenceName);

            if (FindDataComponent (referenceName) is CubeSourceBase source)
            {
                source.Reference = cubeLink;

//                source.InitSchema();
            }
            else
            {
                source = new SliceCubeSource();
                source.ReferenceName = referenceName;
                source.Reference = cubeLink;
                source.Name = CreateUniqueName (referenceName);
                source.Alias = CreateUniqueAlias (source.Alias);
                source.Enabled = enabled;

//                source.InitSchema();
                dictionary.CubeSources.Add (source);
                AddBaseWithChiledToDictonary (source);
            }
        }

        #endregion Private Methods

        #region Public Constructors

        public DictionaryHelper (Dictionary dictionary, ObjectCollection AllDictionaryObjects,
            ObjectCollection AllDictionaryReportObjects)
        {
            this.dictionary = dictionary;
            dataComponents = new Dictionary<string, Base>();
            aliases = new Dictionary<string, Base>();
            fullNames = new Dictionary<string, Base>();
            allObjects = new Dictionary<string, Base>();
            allReportObjects = new Dictionary<string, Base>();
            foreach (Base obj in AllDictionaryObjects)
            {
                AddBaseToDictionary (obj);
            }

            foreach (Base obj in AllDictionaryReportObjects)
            {
                if (obj.Name != null)
                {
                    allReportObjects[obj.Name] = obj;
                }
            }
        }

        #endregion Public Constructors
    }
}
