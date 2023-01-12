// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Reporting.Data;

#endregion

#nullable enable

namespace AM.Reporting.CrossView
{
    /// <summary>
    /// Contains a set of properties and methods to hold and manipulate the CrossView descriptors.
    /// </summary>
    /// <remarks>
    /// This class contains three collections of descriptors such as <see cref="Columns"/>,
    /// <see cref="Rows"/> and <see cref="Cells"/>. Descriptors are filled from FastCube Slice.
    /// </remarks>
    public class CrossViewData
    {
        #region Fields

        internal int[] columnDescriptorsIndexes;
        internal int[] rowDescriptorsIndexes;
        internal int[] columnTerminalIndexes;
        internal int[] rowTerminalIndexes;

        #endregion

        #region FastCube properties (temporary)

        /// <summary>
        ///
        /// </summary>
        public int XAxisFieldsCount => CubeSource?.XAxisFieldsCount ?? 0;

        /// <summary>
        ///
        /// </summary>
        public int YAxisFieldsCount => CubeSource?.YAxisFieldsCount ?? 0;

        /// <summary>
        ///
        /// </summary>
        public int MeasuresCount => CubeSource?.MeasuresCount ?? 0;

        /// <summary>
        ///
        /// </summary>
        public int MeasuresLevel => CubeSource?.MeasuresLevel ?? 0;

        /// <summary>
        ///
        /// </summary>
        public bool MeasuresInXAxis => CubeSource?.MeasuresInXAxis ?? false;

        /// <summary>
        ///
        /// </summary>
        public bool MeasuresInYAxis => CubeSource?.MeasuresInYAxis ?? false;

        /// <summary>
        ///
        /// </summary>
        public int DataColumnCount => CubeSource?.DataColumnCount ?? 0;

        /// <summary>
        ///
        /// </summary>
        public int DataRowCount => CubeSource?.DataRowCount ?? 0;

        /// <summary>
        ///
        /// </summary>
        public bool SourceAssigned => CubeSource != null;

        /// <summary>
        ///
        /// </summary>
        public string ColumnDescriptorsIndexes
        {
            get => string.Join (",", columnDescriptorsIndexes);
            set => columnDescriptorsIndexes = Array.ConvertAll (value.Split (','), int.Parse);
        }

        /// <summary>
        ///
        /// </summary>
        public string RowDescriptorsIndexes
        {
            get => string.Join (",", rowDescriptorsIndexes);
            set => rowDescriptorsIndexes = Array.ConvertAll (value.Split (','), int.Parse);
        }

        /// <summary>
        ///
        /// </summary>
        public string ColumnTerminalIndexes
        {
            get => string.Join (",", columnTerminalIndexes);
            set => columnTerminalIndexes = Array.ConvertAll (value.Split (','), int.Parse);
        }

        /// <summary>
        ///
        /// </summary>
        public string RowTerminalIndexes
        {
            get => string.Join (",", rowTerminalIndexes);
            set => rowTerminalIndexes = Array.ConvertAll (value.Split (','), int.Parse);
        }

        internal CubeSourceBase? CubeSource { get; set; }

        internal void CreateDescriptors()
        {
            columnDescriptorsIndexes = Array.Empty<int>();
            rowDescriptorsIndexes = Array.Empty<int>();
            columnTerminalIndexes = Array.Empty<int>();
            rowTerminalIndexes = Array.Empty<int>();
            CrossViewHeaderDescriptor crossViewHeaderDescriptor;
            Columns.Clear();
            Rows.Clear();
            Cells.Clear();

            if (!SourceAssigned)
            {
                return;
            }

            var cell = 0;
            for (var i = 0; i < XAxisFieldsCount; i++)
            {
                cell = 0;
                if (MeasuresInXAxis && (MeasuresLevel <= i))
                {
                    if (MeasuresLevel == i)
                    {
                        for (var k = 0; k <= i; k++)
                        {
                            for (var j = 0; j < MeasuresCount; j++)
                            {
                                if (k == i)
                                {
                                    crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("",
                                            CubeSource.GetMeasureName (j), false, false, true)
                                        {
                                            cellsize = 1,
                                            levelsize = XAxisFieldsCount - i
                                        };
                                }
                                else
                                {
                                    crossViewHeaderDescriptor = new CrossViewHeaderDescriptor (
                                            CubeSource.GetXAxisFieldName (k), CubeSource.GetMeasureName (j), false,
                                            false,
                                            true)
                                        {
                                            cellsize = (XAxisFieldsCount - i),
                                            levelsize = 1
                                        };
                                }

                                crossViewHeaderDescriptor.level = i;
                                crossViewHeaderDescriptor.cell = cell;
                                cell += crossViewHeaderDescriptor.cellsize;
                                Columns.Add (crossViewHeaderDescriptor);
                                if ((j == 0) && (k == 0))
                                {
                                    Array.Resize (ref rowDescriptorsIndexes, rowDescriptorsIndexes.Length + 1);
                                    rowDescriptorsIndexes[rowDescriptorsIndexes.Length - 1] = Columns.Count - 1;
                                }

                                if ((k == i) || (i == (XAxisFieldsCount - 1)))
                                {
                                    Array.Resize (ref columnTerminalIndexes, columnTerminalIndexes.Length + 1);
                                    columnTerminalIndexes[columnTerminalIndexes.Length - 1] = Columns.Count - 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var j = 0; j < MeasuresCount; j++)
                        {
                            crossViewHeaderDescriptor = new CrossViewHeaderDescriptor (CubeSource.GetXAxisFieldName (i),
                                    "", false, false, false)
                                {
                                    level = i,
                                    levelsize = 1,
                                    cell = cell,
                                    cellsize = XAxisFieldsCount - i
                                };
                            cell += crossViewHeaderDescriptor.cellsize;
                            Columns.Add (crossViewHeaderDescriptor);
                            if (j == 0)
                            {
                                Array.Resize (ref rowDescriptorsIndexes, rowDescriptorsIndexes.Length + 1);
                                rowDescriptorsIndexes[rowDescriptorsIndexes.Length - 1] = Columns.Count - 1;
                            }

                            if (i == 1)
                            {
                                crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("", "", false, true, false);
                            }
                            else if (i == (MeasuresLevel + 1))
                            {
                                crossViewHeaderDescriptor =
                                    new CrossViewHeaderDescriptor (CubeSource.GetXAxisFieldName (i - 2), "", true,
                                        false, false);
                            }
                            else
                            {
                                crossViewHeaderDescriptor =
                                    new CrossViewHeaderDescriptor (CubeSource.GetXAxisFieldName (i - 1), "", true,
                                        false, false);
                            }

                            crossViewHeaderDescriptor.level = i;
                            crossViewHeaderDescriptor.levelsize = XAxisFieldsCount - i;
                            crossViewHeaderDescriptor.cell = cell;
                            crossViewHeaderDescriptor.cellsize = 1;
                            cell += crossViewHeaderDescriptor.cellsize;
                            Columns.Add (crossViewHeaderDescriptor);

                            if ((crossViewHeaderDescriptor.level + crossViewHeaderDescriptor.levelsize ==
                                 XAxisFieldsCount))
                            {
                                Array.Resize (ref columnTerminalIndexes, columnTerminalIndexes.Length + 1);
                                columnTerminalIndexes[columnTerminalIndexes.Length - 1] = Columns.Count - 1;
                            }
                        }
                    }
                }
                else
                {
                    crossViewHeaderDescriptor =
                        new CrossViewHeaderDescriptor (CubeSource.GetXAxisFieldName (i), "", false, false, false)
                        {
                            level = i,
                            levelsize = 1,
                            cell = cell
                        };
                    if (MeasuresInXAxis)
                    {
                        crossViewHeaderDescriptor.cellsize = (XAxisFieldsCount - i - 1) * MeasuresCount;
                        if (crossViewHeaderDescriptor.cellsize == 0)
                        {
                            crossViewHeaderDescriptor.cellsize = MeasuresCount;
                        }
                    }
                    else
                    {
                        crossViewHeaderDescriptor.cellsize = XAxisFieldsCount - i;
                    }

                    cell += crossViewHeaderDescriptor.cellsize;
                    Columns.Add (crossViewHeaderDescriptor);
                    if ((crossViewHeaderDescriptor.level + crossViewHeaderDescriptor.levelsize == XAxisFieldsCount))
                    {
                        Array.Resize (ref columnTerminalIndexes, columnTerminalIndexes.Length + 1);
                        columnTerminalIndexes[columnTerminalIndexes.Length - 1] = Columns.Count - 1;
                    }

                    Array.Resize (ref rowDescriptorsIndexes, rowDescriptorsIndexes.Length + 1);
                    rowDescriptorsIndexes[rowDescriptorsIndexes.Length - 1] = Columns.Count - 1;

                    if (i == 0)
                    {
                        crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("", "", false, true, false);
                    }
                    else
                    {
                        crossViewHeaderDescriptor = new CrossViewHeaderDescriptor (CubeSource.GetXAxisFieldName (i - 1),
                            "", true, false, false);
                    }

                    crossViewHeaderDescriptor.level = i;
                    crossViewHeaderDescriptor.cell = cell;
                    if (MeasuresInXAxis)
                    {
                        crossViewHeaderDescriptor.levelsize = MeasuresLevel - i;
                        crossViewHeaderDescriptor.cellsize = MeasuresCount;
                    }
                    else
                    {
                        crossViewHeaderDescriptor.levelsize = XAxisFieldsCount - i;
                        crossViewHeaderDescriptor.cellsize = 1;
                    }

                    cell += crossViewHeaderDescriptor.cellsize;
                    Columns.Add (crossViewHeaderDescriptor);

                    if ((crossViewHeaderDescriptor.level + crossViewHeaderDescriptor.levelsize == XAxisFieldsCount))
                    {
                        Array.Resize (ref columnTerminalIndexes, columnTerminalIndexes.Length + 1);
                        columnTerminalIndexes[columnTerminalIndexes.Length - 1] = Columns.Count - 1;
                    }
                }
            }

            if (Columns.Count == 0)
            {
                crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("", "", false, true, false)
                {
                    level = 0,
                    levelsize = 1,
                    cell = 0,
                    cellsize = 1
                };
                Columns.Add (crossViewHeaderDescriptor);
                Array.Resize (ref rowDescriptorsIndexes, rowDescriptorsIndexes.Length + 1);
                rowDescriptorsIndexes[rowDescriptorsIndexes.Length - 1] = Columns.Count - 1;
                Array.Resize (ref columnTerminalIndexes, columnTerminalIndexes.Length + 1);
                columnTerminalIndexes[columnTerminalIndexes.Length - 1] = Columns.Count - 1;
            }

            for (var i = 0; i < YAxisFieldsCount; i++)
            {
                cell = 0;
                if (MeasuresInYAxis && (MeasuresLevel <= i))
                {
                    if (MeasuresLevel == i)
                    {
                        for (var k = 0; k <= i; k++)
                        {
                            for (var j = 0; j < MeasuresCount; j++)
                            {
                                if (k == i)
                                {
                                    crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("",
                                            CubeSource.GetMeasureName (j), false, false, true)
                                        {
                                            cellsize = 1,
                                            levelsize = YAxisFieldsCount - i
                                        };
                                }
                                else
                                {
                                    crossViewHeaderDescriptor = new CrossViewHeaderDescriptor (
                                            CubeSource.GetYAxisFieldName (k), CubeSource.GetMeasureName (j), false,
                                            false,
                                            true)
                                        {
                                            cellsize = (YAxisFieldsCount - i),
                                            levelsize = 1
                                        };
                                }

                                crossViewHeaderDescriptor.level = i;
                                crossViewHeaderDescriptor.cell = cell;
                                cell += crossViewHeaderDescriptor.cellsize;
                                Rows.Add (crossViewHeaderDescriptor);
                                if ((j == 0) && (k == 0))
                                {
                                    Array.Resize (ref columnDescriptorsIndexes, columnDescriptorsIndexes.Length + 1);
                                    columnDescriptorsIndexes[columnDescriptorsIndexes.Length - 1] = Rows.Count - 1;
                                }

                                if ((k == i) || (i == (YAxisFieldsCount - 1)))
                                {
                                    Array.Resize (ref rowTerminalIndexes, rowTerminalIndexes.Length + 1);
                                    rowTerminalIndexes[rowTerminalIndexes.Length - 1] = Rows.Count - 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var j = 0; j < MeasuresCount; j++)
                        {
                            crossViewHeaderDescriptor = new CrossViewHeaderDescriptor (CubeSource.GetYAxisFieldName (i),
                                    "", false, false, false)
                                {
                                    level = i,
                                    levelsize = 1,
                                    cell = cell,
                                    cellsize = YAxisFieldsCount - i
                                };
                            cell += crossViewHeaderDescriptor.cellsize;
                            Rows.Add (crossViewHeaderDescriptor);
                            if (j == 0)
                            {
                                Array.Resize (ref columnDescriptorsIndexes, columnDescriptorsIndexes.Length + 1);
                                columnDescriptorsIndexes[columnDescriptorsIndexes.Length - 1] = Rows.Count - 1;
                            }

                            if (i == 1)
                            {
                                crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("", "", false, true, false);
                            }
                            else if (i == (MeasuresLevel + 1))
                            {
                                crossViewHeaderDescriptor =
                                    new CrossViewHeaderDescriptor (CubeSource.GetYAxisFieldName (i - 2), "", true,
                                        false, false);
                            }
                            else
                            {
                                crossViewHeaderDescriptor =
                                    new CrossViewHeaderDescriptor (CubeSource.GetYAxisFieldName (i - 1), "", true,
                                        false, false);
                            }

                            crossViewHeaderDescriptor.level = i;
                            crossViewHeaderDescriptor.levelsize = YAxisFieldsCount - i;
                            crossViewHeaderDescriptor.cell = cell;
                            crossViewHeaderDescriptor.cellsize = 1;
                            cell += crossViewHeaderDescriptor.cellsize;
                            Rows.Add (crossViewHeaderDescriptor);

                            if ((crossViewHeaderDescriptor.level + crossViewHeaderDescriptor.levelsize ==
                                 YAxisFieldsCount))
                            {
                                Array.Resize (ref rowTerminalIndexes, rowTerminalIndexes.Length + 1);
                                rowTerminalIndexes[rowTerminalIndexes.Length - 1] = Rows.Count - 1;
                            }
                        }
                    }
                }
                else
                {
                    crossViewHeaderDescriptor =
                        new CrossViewHeaderDescriptor (CubeSource.GetYAxisFieldName (i), "", false, false, false)
                        {
                            level = i,
                            levelsize = 1,
                            cell = cell
                        };
                    if (MeasuresInYAxis)
                    {
                        crossViewHeaderDescriptor.cellsize = (YAxisFieldsCount - i - 1) * MeasuresCount;
                        if (crossViewHeaderDescriptor.cellsize == 0)
                        {
                            crossViewHeaderDescriptor.cellsize = MeasuresCount;
                        }
                    }
                    else
                    {
                        crossViewHeaderDescriptor.cellsize = YAxisFieldsCount - i;
                    }

                    cell += crossViewHeaderDescriptor.cellsize;
                    Rows.Add (crossViewHeaderDescriptor);
                    if ((crossViewHeaderDescriptor.level + crossViewHeaderDescriptor.levelsize == YAxisFieldsCount))
                    {
                        Array.Resize (ref rowTerminalIndexes, rowTerminalIndexes.Length + 1);
                        rowTerminalIndexes[rowTerminalIndexes.Length - 1] = Rows.Count - 1;
                    }

                    Array.Resize (ref columnDescriptorsIndexes, columnDescriptorsIndexes.Length + 1);
                    columnDescriptorsIndexes[columnDescriptorsIndexes.Length - 1] = Rows.Count - 1;

                    if (i == 0)
                    {
                        crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("", "", false, true, false);
                    }
                    else
                    {
                        crossViewHeaderDescriptor = new CrossViewHeaderDescriptor (CubeSource.GetYAxisFieldName (i - 1),
                            "", true, false, false);
                    }

                    crossViewHeaderDescriptor.level = i;
                    crossViewHeaderDescriptor.cell = cell;
                    if (MeasuresInYAxis)
                    {
                        crossViewHeaderDescriptor.levelsize = MeasuresLevel - i;
                        crossViewHeaderDescriptor.cellsize = MeasuresCount;
                    }
                    else
                    {
                        crossViewHeaderDescriptor.levelsize = YAxisFieldsCount - i;
                        crossViewHeaderDescriptor.cellsize = 1;
                    }

                    cell += crossViewHeaderDescriptor.cellsize;
                    Rows.Add (crossViewHeaderDescriptor);

                    if ((crossViewHeaderDescriptor.level + crossViewHeaderDescriptor.levelsize == YAxisFieldsCount))
                    {
                        Array.Resize (ref rowTerminalIndexes, rowTerminalIndexes.Length + 1);
                        rowTerminalIndexes[rowTerminalIndexes.Length - 1] = Rows.Count - 1;
                    }
                }
            }

            if (Rows.Count == 0)
            {
                crossViewHeaderDescriptor = new CrossViewHeaderDescriptor ("", "", false, true, false)
                {
                    level = 0,
                    levelsize = 1,
                    cell = 0,
                    cellsize = 1
                };
                Rows.Add (crossViewHeaderDescriptor);
                Array.Resize (ref columnDescriptorsIndexes, columnDescriptorsIndexes.Length + 1);
                columnDescriptorsIndexes[columnDescriptorsIndexes.Length - 1] = Rows.Count - 1;
                Array.Resize (ref rowTerminalIndexes, rowTerminalIndexes.Length + 1);
                rowTerminalIndexes[rowTerminalIndexes.Length - 1] = Rows.Count - 1;
            }

            CrossViewCellDescriptor crossViewCellDescriptor;
            for (var i = 0; i < columnTerminalIndexes.Length; i++)
            {
                for (var j = 0; j < rowTerminalIndexes.Length; j++)
                {
                    crossViewCellDescriptor = new CrossViewCellDescriptor (Columns[columnTerminalIndexes[i]].fieldName,
                            Rows[rowTerminalIndexes[j]].fieldName,
                            Columns[columnTerminalIndexes[i]].measureName + Rows[rowTerminalIndexes[j]].measureName,
                            Columns[columnTerminalIndexes[i]].isTotal, Rows[rowTerminalIndexes[j]].isTotal,
                            Columns[columnTerminalIndexes[i]].isGrandTotal, Rows[rowTerminalIndexes[j]].isGrandTotal)
                        {
                            X = i,
                            Y = j
                        };
                    Cells.Add (crossViewCellDescriptor);
                }
            }
        }

        internal CrossViewHeaderDescriptor GetRowDescriptor (int index)
        {
            var tempXAxisFieldsCount = (!SourceAssigned) ? 1 : XAxisFieldsCount;
            if (index < tempXAxisFieldsCount)
            {
                return Columns[rowDescriptorsIndexes[index]];
            }
            else
            {
                return Rows[rowTerminalIndexes[index - tempXAxisFieldsCount]];
            }
        }

        internal CrossViewHeaderDescriptor GetColumnDescriptor (int index)
        {
            var tempYAxisFieldsCount = (!SourceAssigned) ? 1 : YAxisFieldsCount;
            if (index < tempYAxisFieldsCount)
            {
                return Rows[columnDescriptorsIndexes[index]];
            }
            else
            {
                return Columns[columnTerminalIndexes[index - tempYAxisFieldsCount]];
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a collection of column descriptors.
        /// </summary>
        /// <remarks>
        /// Note: after you change something in this collection, call the
        /// <see cref="CrossViewObject.BuildTemplate"/> method to refresh the CrossView.
        /// </remarks>
        public CrossViewHeader Columns { get; }

        /// <summary>
        /// Gets a collection of row descriptors.
        /// </summary>
        /// <remarks>
        /// Note: after you change something in this collection, call the
        /// <see cref="CrossViewObject.BuildTemplate"/> method to refresh the CrossView.
        /// </remarks>
        public CrossViewHeader Rows { get; }

        /// <summary>
        /// Gets a collection of data cell descriptors.
        /// </summary>
        /// <remarks>
        /// Note: after you change something in this collection, call the
        /// <see cref="CrossViewObject.BuildTemplate"/> method to refresh the CrossView.
        /// </remarks>
        public CrossViewCells Cells { get; }

        #endregion

        #region Public Methods

        #endregion

        internal CrossViewData()
        {
            Columns = new CrossViewHeader
            {
                Name = "CrossViewColumns"
            };
            Rows = new CrossViewHeader
            {
                Name = "CrossViewRows"
            };
            Cells = new CrossViewCells
            {
                Name = "CrossViewCells"
            };
            CreateDescriptors();
        }
    }
}
