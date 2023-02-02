// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LinqNode.cs -- жалкое подобие LINQ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/*

  from score in scores //required
  where score > 80 // optional
  orderby score // optional
  select score; //must end with select or group
  
  или

  from person in people //required
  group person by person.Age; // required

 */

/// <summary>
/// Жалкое подобие LINQ.
/// </summary>
internal sealed class LinqNode
    : AtomNode
{
    #region NestedClasses

    internal sealed class Grouped
        : IGrouping<object?, object?>
    {
        #region IGrouping members
    
        public object? Key { get; }
        
        public List<object?> Items { get; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object?> GetEnumerator() => Items.GetEnumerator();

        #endregion

        #region Construction

        public Grouped
            (
                object? key, 
                List<object?> items
            )
        {
            Key = key;
            Items = items;
        }

        #endregion
    }

    internal sealed class GroupedList
        : List<Grouped>
    {
        #region Public methods

        public void Add 
            (
                object? key,
                object? value
            )
        {
            var found = false;
            foreach (var one in this)
            {
                if (OmnipotentComparer.Default.Compare (one.Key, key) == 0)
                {
                    one.Items.Add (value);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var item = new Grouped (key, new List<object?> { value });
                Add (item);
            }
        }

        #endregion
    }

    internal sealed class OrderClause
        : AstNode
    {
        #region Properties

        public AtomNode Expression { get; }

        public bool Descending { get; }

        #endregion

        #region Construction

        public OrderClause
            (
                AtomNode expression,
                bool descending
            )
        {
            Expression = expression;
            Descending = descending;
        }

        #endregion
    }

    internal sealed class GroupClause
    {
        #region Properties

        public AtomNode Expression { get; }
        
        public AtomNode By { get; }

        #endregion

        #region Construction

        public GroupClause
            (
                AtomNode expression, 
                AtomNode by
            )
        {
            Expression = expression;
            By = by;
        }

        #endregion
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LinqNode
        (
            string variableName,
            AtomNode sequence,
            AtomNode? whereClause,
            OrderClause? orderClause,
            AtomNode? selectClause,
            GroupClause? groupClause
        )
    {
        Sure.NotNullNorEmpty (variableName);
        Sure.NotNull (sequence);

        if (selectClause is null && groupClause is null)
        {
            throw new SyntaxException ("Must be select or group clause");
        }

        if (selectClause is not null && groupClause is not null)
        {
            throw new SyntaxException ("Can't handle select and group simultaneously");
        }

        _variableName = variableName;
        _sequence = sequence;
        _whereClause = whereClause;
        _orderClause = orderClause;
        _selectClause = selectClause;
        _groupClause = groupClause;
    }

    #endregion

    #region Private members

    private readonly string _variableName;
    private readonly AtomNode _sequence;
    private readonly AtomNode? _whereClause;
    private readonly OrderClause? _orderClause;
    private readonly AtomNode? _selectClause;
    private readonly GroupClause? _groupClause;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var sequence = _sequence.Compute (context);
        if (sequence is not IEnumerable)
        {
            sequence = new [] { sequence };
        }

        context = context.CreateChildContext();
        var variables = context.Variables;
        variables[_variableName] = null;

        // in и where (если есть)
        var temporary = new BarsikList();
        foreach (var item in sequence)
        {
            variables[_variableName] = item;
            var success = _whereClause is null
                || KotikUtility.ToBoolean (_whereClause.Compute (context));
            if (success)
            {
                temporary.Add (item);
            }
        }

        // orderby
        if (_orderClause is not null)
        {
            var list = new List<KeyValuePair<dynamic, dynamic>>();
            foreach (var one in temporary)
            {
                variables[_variableName] = one;
                var key = _orderClause.Expression.Compute (context);
                var pair = new KeyValuePair<dynamic, dynamic> (key, one);
                list.Add (pair);
            }

            if (_orderClause.Descending)
            {
                list.Sort ((left, right) =>
                    OmnipotentComparer.Default.Compare (right.Key, left.Key));
            }
            else
            {
                list.Sort ((left, right) =>
                    OmnipotentComparer.Default.Compare (left.Key, right.Key));
            }

            temporary.Clear();
            foreach (var pair in list)
            {
                temporary.Add (pair.Value);
            }
        }

        if (_selectClause is not null)
        {
            var result = new BarsikList();
            foreach (var one in temporary)
            {
                variables[_variableName] = one;
                var selected = _selectClause.Compute (context);
                result.Add (selected);
            }

            return result;
        }

        if (_groupClause is not null)
        {
            var result = new GroupedList();
            foreach (var one in temporary)
            {
                var key = _groupClause.By.Compute (context);
                var value = _groupClause.Expression.Compute (context);
                result.Add (key, value);
            }

            return result;
        }

        return null;
    }

    #endregion
}
