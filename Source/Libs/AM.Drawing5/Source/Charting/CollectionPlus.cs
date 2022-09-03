// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CollectionPlus.cs -- базовый класс коллекции для элементов диаграммы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Базовый класс коллекции, содержащий базовые дополнительные
/// функции, которые будут наследоваться <see cref="CurveList"/>,
/// <see cref="IPointList"/>, <see cref="GraphObjList"/>.
/// </summary>
/// <remarks>The methods in this collection operate on basic
/// <see cref="object"/> types.  Therefore, in order to make sure that
/// the derived classes remain strongly-typed, there are no Add() or
/// Insert() methods here, and no methods that return an object.
/// Only Remove(), Move(), IndexOf(), etc. methods are included.
/// </remarks>
[Serializable]
public class CollectionPlus
    : CollectionBase
{
    #region Public methods

    /// <inheritdoc cref="IList.IndexOf"/>
    public int IndexOf
        (
            object? item
        )
    {
        if (item is not null)
        {
            return List.IndexOf (item);
        }

        return -1;
    }

    /// <inheritdoc cref="IList.RemoveAt"/>
    public void Remove
        (
            int index
        )
    {
        if (index >= 0 && index < List.Count)
        {
            List.RemoveAt (index);
        }
    }

    /// <inheritdoc cref="IList.Remove"/>
    public void Remove
        (
            object? item
        )
    {
        if (item is not null)
        {
            List.Remove (item);
        }
    }

    /// <summary>
    /// Move the position of the object at the specified index
    /// to the new relative position in the list.</summary>
    /// <remarks>For Graphic type objects, this method controls the
    /// Z-Order of the items.  Objects at the beginning of the list
    /// appear in front of objects at the end of the list.</remarks>
    /// <param name="index">The zero-based index of the object
    /// to be moved.</param>
    /// <param name="relativePos">The relative number of positions to move
    /// the object.  A value of -1 will move the
    /// object one position earlier in the list, a value
    /// of 1 will move it one position later.  To move an item to the
    /// beginning of the list, use a large negative value (such as -999).
    /// To move it to the end of the list, use a large positive value.
    /// </param>
    /// <returns>The new position for the object, or -1 if the object
    /// was not found.</returns>
    public int Move
        (
            int index,
            int relativePos
        )
    {
        if (index < 0 || index >= List.Count)
        {
            return -1;
        }

        var obj = List[index];
        List.RemoveAt (index);
        index += relativePos;
        if (index < 0)
        {
            index = 0;
        }

        if (index > List.Count)
        {
            index = List.Count;
        }

        List.Insert (index, obj);
        return index;
    }

    #endregion

    /*
    #region Serialization
        /// <summary>
        /// Current schema value that defines the version of the serialized file
        /// </summary>
        public const int schema = 1;

        /// <summary>
        /// Constructor for deserializing objects
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
        /// </param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
        /// </param>
        protected CollectionPlus( SerializationInfo info, StreamingContext context ) : base( info, context )
        {
            // The schema value is just a file version parameter.  You can use it to make future versions
            // backwards compatible as new member variables are added to classes
            int sch = info.GetInt32( "schema" );

        }
        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
        public virtual void GetObjectData
            (
                SerializationInfo info,
                StreamingContext context
            )
        {
            base.GetObjectData( info, context );

            info.AddValue( "schema", schema );
        }
    #endregion
*/
}
