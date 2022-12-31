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
using System.Text;
using System.Data;
using System.Globalization;
using System.Collections.Generic;

using AM.Reporting.Data;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// The pseudo-random generator.
    /// </summary>
    public class FRRandom
    {
        #region Fields

        private readonly Random random;

        private static readonly char[] lowerLetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        private static readonly char[] upperLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        #endregion Fields

        #region Public Methods

        /// <summary>
        /// Gets a random letter in same case that source character.
        /// </summary>
        /// <param name="source">The source character.</param>
        /// <returns>The random character.</returns>
        public char NextLetter (char source)
        {
            if (char.IsLower (source))
            {
                return lowerLetters[random.Next (lowerLetters.Length)];
            }
            else if (char.IsUpper (source))
            {
                return upperLetters[random.Next (upperLetters.Length)];
            }

            return source;
        }

        /// <summary>
        /// Gets random int value from <b>0</b> to <b>9</b>.
        /// </summary>
        /// <returns>Random int value.</returns>
        public int NextDigit()
        {
            return random.Next (10);
        }

        /// <summary>
        /// Gets random int value from <b>0</b> to <b>max</b>.
        /// </summary>
        /// <param name="max">The maximum for random digit.</param>
        /// <returns>Random int value.</returns>
        public int NextDigit (int max)
        {
            return random.Next (max + 1);
        }

        /// <summary>
        /// Gets random int value from <b>min</b> to <b>max</b>.
        /// </summary>
        /// <param name="min">The minimum for random digit.</param>
        /// <param name="max">The maximum for random digit.</param>
        /// <returns>Random int value.</returns>
        public int NextDigit (int min, int max)
        {
            return random.Next (min, max + 1);
        }

        /// <summary>
        /// Gets number of random digits from <b>0</b> to <b>9</b>.
        /// </summary>
        /// <param name="number">The number of digits.</param>
        /// <returns>Number of random digits.</returns>
        public string NextDigits (int number)
        {
            if (number <= 0)
            {
                return "";
            }

            var sb = new StringBuilder();
            for (var i = 0; i < number; i++)
            {
                sb.Append (NextDigit());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the random byte value.
        /// </summary>
        /// <returns>Random byte value.</returns>
        public byte NextByte()
        {
            return (byte)random.Next (byte.MaxValue);
        }

        /// <summary>
        /// Gets random byte array with specified number of elements.
        /// </summary>
        /// <param name="number">The number of elements in array.</param>
        /// <returns>Random byte array.</returns>
        public byte[] NextBytes (int number)
        {
            var bytes = new byte[number];
            random.NextBytes (bytes);
            return bytes;
        }

        /// <summary>
        /// Gets the randomized char value.
        /// </summary>
        /// <returns>Random char value.</returns>
        public char NextChar()
        {
            return Convert.ToChar (random.Next (char.MaxValue));
        }

        /// <summary>
        /// Gets the random day from start to DataTime.Today.
        /// </summary>
        /// <param name="start">The starting DateTime value.</param>
        /// <returns>Random DateTime value.</returns>
        public DateTime NextDay (DateTime start)
        {
            if (start > DateTime.Today)
            {
                return DateTime.Today;
            }

            var range = (DateTime.Today - start).Days;
            return start.AddDays (random.Next (range));
        }

        /// <summary>
        /// Gets the randomized TimeSpan value beetwin specified hours.
        /// </summary>
        /// <param name="start">The starting hour (0 - 24).</param>
        /// <param name="end">The ending hour (0 - 24).</param>
        /// <returns>Random TimeSpan value.</returns>
        public TimeSpan NextTimeSpanBetweenHours (int start, int end)
        {
            if (start < 0)
            {
                start = 0;
            }

            if (end > 24)
            {
                end = 24;
            }

            if (start > end)
            {
                var temp = start;
                start = end;
                end = temp;
            }

            var startTs = TimeSpan.FromHours (start);
            var endTs = TimeSpan.FromHours (end);

            var maxMinutes = (int)(endTs - startTs).TotalMinutes;
            var randomMinutes = random.Next (maxMinutes);

            var result = startTs.Add (TimeSpan.FromMinutes (randomMinutes));
            return result;
        }

        /// <summary>
        /// Gets the randomized decimal value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source decimal value.</param>
        /// <returns>Random decimal value based on source.</returns>
        public decimal RandomizeDecimal (decimal source)
        {
            var sb = new StringBuilder();

            string[] parts = source.ToString (CultureInfo.InvariantCulture).ToUpper().Split ('E');
            var e = "";
            if (parts.Length > 1)
            {
                e = "E" + parts[1];
            }

            parts = parts[0].Split ('.');
            if (parts.Length > 0)
            {
                var length = parts[0].Length;
                if (source < 0.0m)
                {
                    sb.Append ("-");
                    length--;
                }

                sb.Append (NextDigit (1, 9));
                sb.Append (NextDigits (length - 1));
            }

            if (parts.Length > 1)
            {
                sb.Append (".");
                sb.Append (NextDigits (parts[1].Length - 1));
                sb.Append (NextDigit (1, 9));
            }

            sb.Append (e);

            var parsed = decimal.TryParse (sb.ToString(), NumberStyles.Float,
                CultureInfo.InvariantCulture, out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets the randomized double value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source double value.</param>
        /// <returns>Random double value based on source.</returns>
        public double RandomizeDouble (double source)
        {
            return (double)RandomizeDecimal ((decimal)source);
        }

        /// <summary>
        /// Gets the randomized Int16 value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source Int16 value.</param>
        /// <returns>Random Int16 value based on source.</returns>
        public short RandomizeInt16 (short source)
        {
            var sb = new StringBuilder();

            var length = source.ToString (CultureInfo.InvariantCulture).Length;
            if (source < 0)
            {
                sb.Append ('-');
                length--;
            }

            var maxLength = short.MaxValue.ToString (CultureInfo.InvariantCulture).Length;
            if (length < maxLength)
            {
                sb.Append (NextDigit (1, 9));
                sb.Append (NextDigits (length - 1));
            }
            else // Guarantee a value less than 32 000.
            {
                var next = NextDigit (1, 3);
                sb.Append (next);
                if (next < 3)
                {
                    sb.Append (NextDigits (maxLength - 1));
                }
                else
                {
                    sb.Append (NextDigit (1));
                    sb.Append (NextDigits (maxLength - 2));
                }
            }

            var parsed = short.TryParse (sb.ToString(), out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets the randomized Int32 value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source Int32 value.</param>
        /// <returns>Random Int32 value based on source.</returns>
        public int RandomizeInt32 (int source)
        {
            var sb = new StringBuilder();

            var length = source.ToString (CultureInfo.InvariantCulture).Length;
            if (source < 0)
            {
                sb.Append ('-');
                length--;
            }

            var maxLength = int.MaxValue.ToString (CultureInfo.InvariantCulture).Length;
            if (length < maxLength)
            {
                sb.Append (NextDigit (1, 9));
                sb.Append (NextDigits (length - 1));
            }
            else // Guarantee a value less than 2 200 000 000.
            {
                var next = NextDigit (1, 2);
                sb.Append (next);
                if (next < 2)
                {
                    sb.Append (NextDigits (maxLength - 1));
                }
                else
                {
                    sb.Append (NextDigit (1));
                    sb.Append (NextDigits (maxLength - 2));
                }
            }

            var parsed = int.TryParse (sb.ToString(), out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets the randomized Int64 value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source Int64 value.</param>
        /// <returns>Random Int64 value based on source.</returns>
        public long RandomizeInt64 (long source)
        {
            var sb = new StringBuilder();

            var length = source.ToString (CultureInfo.InvariantCulture).Length;
            if (source < 0)
            {
                sb.Append ('-');
                length--;
            }

            var maxLength = long.MaxValue.ToString (CultureInfo.InvariantCulture).Length;
            if (length < maxLength)
            {
                sb.Append (NextDigit (1, 9));
                sb.Append (NextDigits (length - 1));
            }
            else // Guarantee a value less than 9 200 000 000 000 000 000.
            {
                var next = NextDigit (1, 9);
                sb.Append (next);
                if (next < 9)
                {
                    sb.Append (NextDigits (maxLength - 1));
                }
                else
                {
                    sb.Append (NextDigit (1));
                    sb.Append (NextDigits (maxLength - 2));
                }
            }

            var parsed = long.TryParse (sb.ToString(), out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets the randomized SByte value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source SByte value.</param>
        /// <returns>Random SByte value based on source.</returns>
        public sbyte RandomizeSByte (sbyte source)
        {
            var sb = new StringBuilder();

            var length = source.ToString (CultureInfo.InvariantCulture).Length;
            if (source < 0)
            {
                sb.Append ('-');
                length--;
            }

            var maxLength = sbyte.MaxValue.ToString (CultureInfo.InvariantCulture).Length;
            if (length < maxLength)
            {
                sb.Append (NextDigits (length));
            }
            else // Guarantee a value less than 128.
            {
                var next = NextDigit (1);
                sb.Append (next);
                if (next < 1)
                {
                    sb.Append (NextDigits (maxLength - 1));
                }
                else
                {
                    sb.Append (NextDigit (2));
                    sb.Append (NextDigit (7));
                }
            }

            var parsed = sbyte.TryParse (sb.ToString(), out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets the randomized Single value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source Single value.</param>
        /// <returns>Random Single value based on source.</returns>
        public float RandomizeFloat (float source)
        {
            return (float)RandomizeDecimal ((decimal)source);
        }

        /// <summary>
        /// Gets the randomized string with same length and same whitespaces that in source string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>Random string based on source string.</returns>
        public string RandomizeString (string source)
        {
            var sb = new StringBuilder();

            foreach (var c in source)
            {
                if (char.IsWhiteSpace (c))
                {
                    sb.Append (c);
                }
                else if (char.IsLetter (c))
                {
                    sb.Append (NextLetter (c));
                }
                else if (char.IsDigit (c))
                {
                    sb.Append (NextDigit());
                }
                else
                {
                    sb.Append (c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the randomized UInt16 value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source UInt16 value.</param>
        /// <returns>Random UInt16 value based on source.</returns>
        public ushort RandomizeUInt16 (ushort source)
        {
            var sb = new StringBuilder();

            var length = source.ToString (CultureInfo.InvariantCulture).Length;
            var maxLength = ushort.MaxValue.ToString (CultureInfo.InvariantCulture).Length;
            if (length < maxLength)
            {
                sb.Append (NextDigit (1, 9));
                sb.Append (NextDigits (length - 1));
            }
            else // Guarantee a value less than 65 000.
            {
                var next = NextDigit (1, 6);
                sb.Append (next);
                if (next < 6)
                {
                    sb.Append (NextDigits (maxLength - 1));
                }
                else
                {
                    sb.Append (NextDigit (4));
                    sb.Append (NextDigits (maxLength - 2));
                }
            }

            var parsed = ushort.TryParse (sb.ToString(), out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets the randomized UInt32 value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source UInt32 value.</param>
        /// <returns>Random UInt32 value based on source.</returns>
        public uint RandomizeUInt32 (uint source)
        {
            var sb = new StringBuilder();

            var length = source.ToString (CultureInfo.InvariantCulture).Length;
            var maxLength = uint.MaxValue.ToString (CultureInfo.InvariantCulture).Length;
            if (length < maxLength)
            {
                sb.Append (NextDigit (1, 9));
                sb.Append (NextDigits (length - 1));
            }
            else // Guarantee a value less than 4 200 000 000.
            {
                var next = NextDigit (1, 4);
                sb.Append (next);
                if (next < 4)
                {
                    sb.Append (NextDigits (maxLength - 1));
                }
                else
                {
                    sb.Append (NextDigit (1));
                    sb.Append (NextDigits (maxLength - 2));
                }
            }

            var parsed = uint.TryParse (sb.ToString(), out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets the randomized UInt64 value with same number of digits that in source value.
        /// </summary>
        /// <param name="source">The source UInt64 value.</param>
        /// <returns>Random UInt64 value based on source.</returns>
        public ulong RandomizeUInt64 (ulong source)
        {
            var sb = new StringBuilder();

            var length = source.ToString (CultureInfo.InvariantCulture).Length;
            var maxLength = ulong.MaxValue.ToString (CultureInfo.InvariantCulture).Length;
            if (length < maxLength)
            {
                sb.Append (NextDigit (1, 9));
                sb.Append (NextDigits (length - 1));
            }
            else // Guarantee a value less than 18 400 000 000 000 000 000.
            {
                sb.Append (1);
                var next = NextDigit (8);
                sb.Append (next);
                if (next < 8)
                {
                    sb.Append (NextDigits (maxLength - 2));
                }
                else
                {
                    sb.Append (NextDigit (3));
                    sb.Append (NextDigits (maxLength - 3));
                }
            }

            var parsed = ulong.TryParse (sb.ToString(), out var result);
            if (parsed)
            {
                return result;
            }

            return source;
        }

        /// <summary>
        /// Gets randomized object based on the source object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="type">The type of object.</param>
        /// <returns>Random object based on source.</returns>
        public object GetRandomObject (object source, Type type)
        {
            try
            {
                if (type == typeof (string))
                {
                    return RandomizeString ((string)source);
                }
                else if (type == typeof (int))
                {
                    return RandomizeInt32 ((int)source);
                }
                else if (type == typeof (double))
                {
                    return RandomizeDouble ((double)source);
                }
                else if (type == typeof (DateTime))
                {
                    return NextDay (new DateTime (1990, 1, 1));
                }
                else if (type == typeof (long))
                {
                    return RandomizeInt64 ((long)source);
                }
                else if (type == typeof (decimal))
                {
                    return RandomizeDecimal ((decimal)source);
                }
                else if (type == typeof (short))
                {
                    return RandomizeInt16 ((short)source);
                }
                else if (type == typeof (float))
                {
                    return RandomizeFloat ((float)source);
                }
                else if (type == typeof (char))
                {
                    return NextChar();
                }
                else if (type == typeof (byte))
                {
                    return NextByte();
                }
                else if (type == typeof (uint))
                {
                    return RandomizeUInt32 ((uint)source);
                }
                else if (type == typeof (ulong))
                {
                    return RandomizeUInt64 ((ulong)source);
                }
                else if (type == typeof (ushort))
                {
                    return RandomizeUInt16 ((ushort)source);
                }
                else if (type == typeof (byte[]))
                {
                    return NextBytes (((byte[])source).Length);
                }
                else if (type == typeof (sbyte))
                {
                    return RandomizeSByte ((sbyte)source);
                }
                else if (type == typeof (TimeSpan))
                {
                    return NextTimeSpanBetweenHours (0, 24);
                }
            }
            catch
            {
                return source;
            }

            return source;
        }

        /// <summary>
        /// Randomizes datasources.
        /// </summary>
        /// <param name="datasources">Collection of datasources.</param>
        public void RandomizeDataSources (DataSourceCollection datasources)
        {
            Dictionary<string, FRColumnInfo> uniquesAndRelations = new Dictionary<string, FRColumnInfo>();

            // Get list of related columns and columns with unique values with their type and length.
            foreach (DataSourceBase datasource in datasources)
            {
                if (datasource is TableDataSource source)
                {
                    var table = source.Table;
                    var ds = table.DataSet;
                    var length = table.Rows.Count;
                    for (var c = 0; c < table.Columns.Count; c++)
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.Unique)
                            {
                                if (!uniquesAndRelations.ContainsKey (column.ColumnName))
                                {
                                    uniquesAndRelations.Add (column.ColumnName,
                                        new FRColumnInfo (column.DataType, length));
                                }
                            }
                        }
                    }

                    foreach (DataRelation dr in ds.Relations)
                    {
                        foreach (var dc in dr.ParentColumns)
                        {
                            if (!uniquesAndRelations.ContainsKey (dc.ColumnName))
                            {
                                uniquesAndRelations.Add (dc.ColumnName, new FRColumnInfo (dc.DataType, length));
                            }
                        }

                        foreach (var dc in dr.ChildColumns)
                        {
                            if (!uniquesAndRelations.ContainsKey (dc.ColumnName))
                            {
                                uniquesAndRelations.Add (dc.ColumnName, new FRColumnInfo (dc.DataType, length));
                            }
                        }
                    }
                }
            }

            Dictionary<string, FRRandomFieldValueCollection> dict =
                new Dictionary<string, FRRandomFieldValueCollection>();
            foreach (KeyValuePair<string, FRColumnInfo> pair in uniquesAndRelations)
            {
                dict.Add (pair.Key, new FRRandomFieldValueCollection());
            }

            // Get values for related columns and columns with unique values.
            foreach (DataSourceBase datasource in datasources)
            {
                if (datasource is TableDataSource source)
                {
                    var table = source.Table;
                    for (var c = 0; c < table.Columns.Count; c++)
                    {
                        var column = table.Columns[c];
                        if (!uniquesAndRelations.ContainsKey (column.ColumnName))
                        {
                            continue;
                        }

                        var type = uniquesAndRelations[column.ColumnName].Type;
                        for (var r = 0; r < table.Rows.Count; r++)
                        {
                            var val = table.Rows[r][c];
                            if (val != null && val is not DBNull && !dict[column.ColumnName].ContainsOrigin (val))
                            {
                                object randomVal;
                                do
                                {
                                    randomVal = GetRandomObject (val, type);
                                } while (dict[column.ColumnName]
                                         .ContainsRandom (new FRRandomFieldValue (val, randomVal)));

                                dict[column.ColumnName].Add (new FRRandomFieldValue (val, randomVal));
                            }
                        }
                    }
                }
            }

            // Randomize all table datasources.
            foreach (DataSourceBase datasource in datasources)
            {
                if (datasource is TableDataSource source)
                {
                    source.StoreData = true;
                    var table = source.Table;
                    for (var c = 0; c < table.Columns.Count; c++)
                    {
                        if (table.Columns[c].ReadOnly)
                        {
                            continue;
                        }

                        var type = table.Columns[c].DataType;
                        for (var r = 0; r < table.Rows.Count; r++)
                        {
                            var val = table.Rows[r][c];
                            if (val != null && val is not DBNull)
                            {
                                if (uniquesAndRelations.ContainsKey (table.Columns[c].ColumnName))
                                {
                                    table.Rows[r][c] = dict[table.Columns[c].ColumnName].GetRandom (val);
                                }
                                else
                                {
                                    table.Rows[r][c] = GetRandomObject (val, type);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FRRandom"/> class.
        /// </summary>
        public FRRandom()
        {
            random = new Random();
        }

        #endregion Constructors
    }

    /// <summary>
    /// Represents information about column.
    /// </summary>
    public class FRColumnInfo
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the type of column.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the length of column.
        /// </summary>
        public int Length { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FRColumnInfo"/> class.
        /// </summary>
        /// <param name="type">The type of column.</param>
        /// <param name="length">The lenght of column.</param>
        public FRColumnInfo (Type type, int length)
        {
            this.Type = type;
            this.Length = length;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Represents random value of field.
    /// </summary>
    public class FRRandomFieldValue
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the original value of field.
        /// </summary>
        public object Origin { get; set; }

        /// <summary>
        /// Gets or sets the random value of field.
        /// </summary>
        public object Random { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FRRandomFieldValue"/> class.
        /// </summary>
        /// <param name="origin">The original value of field.</param>
        /// <param name="random">The random value of field.</param>
        public FRRandomFieldValue (object origin, object random)
        {
            this.Origin = origin;
            this.Random = random;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Represents collection of random values of field.
    /// </summary>
    public class FRRandomFieldValueCollection
    {
        #region Fields

        private readonly List<FRRandomFieldValue> list;

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FRRandomFieldValueCollection"/> class.
        /// </summary>
        public FRRandomFieldValueCollection()
        {
            list = new List<FRRandomFieldValue>();
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Adds an object to the end of this collection.
        /// </summary>
        /// <param name="value">Object to add.</param>
        public void Add (FRRandomFieldValue value)
        {
            list.Add (value);
        }

        /// <summary>
        /// Determines whether an element with the same origin value is in the collection.
        /// </summary>
        /// <param name="origin">The object to locate in the collection.</param>
        /// <returns><b>true</b> if object is found in the collection; otherwise, <b>false</b>.</returns>
        public bool ContainsOrigin (object origin)
        {
            foreach (var value in list)
            {
                if (value.Origin == origin)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether an element with the same random value is in the collection.
        /// </summary>
        /// <param name="random">The object to locate in the collection.</param>
        /// <returns><b>true</b> if object is found in the collection; otherwise, <b>false</b>.</returns>
        public bool ContainsRandom (object random)
        {
            foreach (var value in list)
            {
                if (value.Random == random)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the random value for specified origin.
        /// </summary>
        /// <param name="origin">The origin value.</param>
        /// <returns>The random value.</returns>
        public object GetRandom (object origin)
        {
            foreach (var value in list)
            {
                if (value.Origin == origin)
                {
                    return value.Random;
                }
            }

            return origin;
        }

        #endregion Public Methods
    }
}
