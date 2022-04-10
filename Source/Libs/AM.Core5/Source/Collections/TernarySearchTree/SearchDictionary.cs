// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode

/* SearchDictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

#nullable enable

namespace AM.Collections.TernarySearchTree;

/// <summary>
///
/// </summary>
public class SearchDictionary<TValue>
    : IDictionary<string, TValue>
{
    /// <summary>
    /// Root node.
    /// </summary>
    private Node<TValue> root;

    /// <summary>
    /// Gets the number of items in the dictionary.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets if the dictionary is read-only.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Returns all keys in the dictionary.
    /// </summary>
    public ICollection<string> Keys => new Collection<string> (Tree.GetAllKeys (root, "").ToArray());

    /// <summary>
    /// Returns all values in the dictionary.
    /// </summary>
    public ICollection<TValue> Values => new Collection<TValue> (Tree.GetAllValues (root).ToArray());

    /// <summary>
    /// Get/set value by key.
    /// </summary>
    /// <param name="key">Key to get or set.</param>
    /// <returns>Gets or sets the value associated with the specified key.</returns>
    public TValue this [string key]
    {
        get
        {
            Sure.NotNullNorEmpty (key);

            // Find node.
            var node = Tree.GetNodeWithValue (root, key);

            // If no node was found, throw exception.
            if (node == null)
            {
                throw new KeyNotFoundException ("The given key was not present in the dictionary.");
            }

            // Return value of node.
            return node.Value;
        }

        set
        {
            Sure.NotNullNorEmpty (key);

            var leafNode = Tree.CreateNodes (ref root, key);
            if (leafNode.HasValue == false)
            {
                Count++;
            }

            leafNode.Value = value;
        }
    }

    /// <summary>
    /// Add an item to the dictionary.
    /// </summary>
    /// <param name="key">Key of the item. May not be null or an empty string.</param>
    /// <param name="value">Value to associate with the key.</param>
    public void Add (string key, TValue value)
    {
        Sure.NotNullNorEmpty (key);

        var leafNode = Tree.CreateNodes (ref root, key);
        if (leafNode.HasValue)
        {
            throw new ArgumentException ("An item with the same key has already been added to the dictionary.",
                nameof (key));
        }

        Count++;
        leafNode.Value = value;
    }

    /// <summary>
    /// Add a key value pair to the dictionary.
    /// </summary>
    /// <param name="item">Key value pair to add.</param>
    public void Add (KeyValuePair<string, TValue> item)
    {
        Sure.NotNullNorEmpty (item.Key);

        Add (item.Key, item.Value);
    }

    /// <summary>
    /// Get enumerator.
    /// </summary>
    /// <returns>Enumerator.</returns>
    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        return Tree.GetAllKeyValuePairs (root, "").GetEnumerator();
    }

    /// <summary>
    /// Contains key.
    /// </summary>
    /// <param name="key">Key to get.</param>
    /// <returns>True if key was found and false if the key could not be found.</returns>
    public bool ContainsKey (string key)
    {
        Sure.NotNullNorEmpty (key);

        var node = Tree.GetNodeWithValue (root, key);
        return node != null && node.HasValue;
    }

    /// <summary>
    /// Determines if the dictionary contains the given item.
    /// </summary>
    /// <param name="item">Key value pair to locate in the collection.</param>
    /// <returns>True if item is found and false if the item could not be found.</returns>
    public bool Contains (KeyValuePair<string, TValue> item)
    {
        Sure.NotNullNorEmpty (item.Key);

        var node = Tree.GetNodeWithValue (root, item.Key);

        return node != null && node.HasValue && EqualityComparer<TValue>.Default.Equals (node.Value, item.Value);
    }

    /// <summary>
    /// Try get value.
    /// </summary>
    /// <param name="key">Key to get.</param>
    /// <param name="value">Value if the value can be found.</param>
    /// <returns>True if key was found and false if the key could not be found.</returns>
    public bool TryGetValue (string key, out TValue value)
    {
        Sure.NotNullNorEmpty (key);

        var node = Tree.GetNodeWithValue (root, key);
        if (node == null || node.HasValue == false)
        {
            value = default (TValue);
            return false;
        }

        value = node.Value;
        return true;
    }

    /// <summary>
    /// Gets all items starting with the specified key.
    /// </summary>
    /// <param name="startOfKey">Start of keys to match.</param>
    /// <returns>List of values matching key.</returns>
    public IEnumerable<TValue> StartsWith (string startOfKey)
    {
        Sure.NotNullNorEmpty (startOfKey);

        var node = Tree.GetNode (root, startOfKey);
        if (node == null)
        {
            yield break;
        }

        if (node.HasValue)
        {
            yield return node.Value;
        }

        foreach (var value in Tree.GetAllValues (node.EqualNode))
        {
            yield return value;
        }
    }

    /// <summary>
    /// Copy the dictionary to an array.
    /// </summary>
    /// <param name="array">Array to copy to.</param>
    /// <param name="index">Index in array to copy to.</param>
    public void CopyTo (KeyValuePair<string, TValue>[] array, int index)
    {
        // Argument.IsWithinRange (index >= 0 && array.Length - index >= Count, nameof (index));

        foreach (var pair in Tree.GetAllKeyValuePairs (root, ""))
        {
            array[index++] = pair;
        }
    }

    /// <summary>
    /// Remove a key from the dictionary.
    /// </summary>
    /// <param name="key">Key to get.</param>
    /// <returns></returns>
    public bool Remove (string key)
    {
        Sure.NotNullNorEmpty (key);

        var node = Tree.GetNodeWithValue (root, key);
        if (node == null || node.HasValue == false)
        {
            return false;
        }

        RemoveInternal (key);
        return true;
    }

    /// <summary>
    /// Remove a key from the dictionary.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    public bool Remove (KeyValuePair<string, TValue> item)
    {
        Sure.NotNullNorEmpty (item.Key);

        var node = Tree.GetNodeWithValue (root, item.Key);
        if (node == null || node.HasValue == false ||
            EqualityComparer<TValue>.Default.Equals (node.Value, item.Value) == false)
        {
            return false;
        }

        RemoveInternal (item.Key);
        return true;
    }

    /// <summary>
    /// Removes all items in the dictionary.
    /// </summary>
    public void Clear()
    {
        root = null;
        Count = 0;
    }

    public void Optimize()
    {
        foreach (var node in Tree.GetEqualNodes (root))
        {
            var newEqualNode = Tree.OptimizeEqualNode (node.EqualNode);
            node.EqualNode = newEqualNode;
        }

        root = Tree.OptimizeEqualNode (root);
    }

    private void RemoveInternal (string key)
    {
        Tree.RemoveNode (root, key, 0);
        if (root.CanBeRemoved)
        {
            root = null;
        }

        Count--;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
