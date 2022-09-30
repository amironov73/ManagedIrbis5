// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* TermAdapter.cs -- адаптер поисковых терминов для отображения в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms;

/// <summary>
/// Адаптер поисковых терминов для отображения в гриде.
/// </summary>
public class TermAdapter
{
    #region Properties

    /// <summary>
    /// Источник данных.
    /// </summary>
    public BindingSource Source { get; }

    /// <summary>
    /// Текущее значение.
    /// </summary>
    public string CurrentValue
    {
        get
        {
            var term = (Term?) Source.Current;
            if (term is null)
            {
                return string.Empty;
            }

            var result = term.Text ?? string.Empty;

            return result;
        }
    }

    /// <summary>
    /// Префикс плюс текущее значение.
    /// </summary>
    public string FullTerm
    {
        get
        {
            var result = Prefix + CurrentValue;

            return result;
        }
    }

    /// <summary>
    /// Используемое подключение.
    /// </summary>
    public ISyncProvider Connection { get; }

    /// <summary>
    /// Префикс.
    /// </summary>
    public string Prefix { get; }

    /// <summary>
    /// Portion size;
    /// </summary>
    public int Portion { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TermAdapter
        (
            ISyncProvider connection,
            string prefix
        )
    {
        Sure.NotNull (connection);

        Source = new BindingSource (Array.Empty<Term>(), null);
        Portion = 100;
        Connection = connection;
        Prefix = prefix;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка.
    /// </summary>
    public void Clear()
    {
        Source.DataSource = Array.Empty<Term>();
    }

    /// <summary>
    /// Move to next term.
    /// </summary>
    public bool MoveNext()
    {
        var termSource = Source;
        var currencyManager = termSource.CurrencyManager;

        termSource.MoveNext();
        if (currencyManager.Position >= currencyManager.Count - 1)
        {
            return Fill();
        }

        return true;
    }

    /// <summary>
    /// Move to next term.
    /// </summary>
    public bool MoveNext
        (
            int amount
        )
    {
        while (amount > 0)
        {
            amount--;
            if (!MoveNext())
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Move to previous term.
    /// </summary>
    public bool MovePrevious()
    {
        var termSource = Source;
        var currencyManager = termSource.CurrencyManager;

        termSource.MovePrevious();
        if (currencyManager.Position < 1)
        {
            return Fill (null, true);
        }

        return true;
    }

    /// <summary>
    /// Move to previous term.
    /// </summary>
    public bool MovePrevious
        (
            int amount
        )
    {
        while (amount > 0)
        {
            amount--;
            if (!MovePrevious())
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Fill the adapter.
    /// </summary>
    public bool Fill
        (
            string? startTerm = null,
            bool backward = false
        )
    {
        var fullTerm = FullTerm;
        if (startTerm is not null)
        {
            fullTerm = Prefix + startTerm;
        }

        var parameters = new TermParameters
        {
            Database = Connection.Database,
            StartTerm = fullTerm,
            NumberOfTerms = Portion,
            ReverseOrder = backward
        };
        var terms = Connection.ReadTerms (parameters);
        if (terms is null
            || terms.Length <= 1)
        {
            return false;
        }

        if (backward)
        {
            Array.Reverse (terms);
        }

        var prefix = Prefix;
        var prefixLength = prefix.Length;
        if (prefixLength != 0)
        {
            var goodTerms = new List<Term> (terms.Length);
            foreach (var term in terms)
            {
                if (term.Count < 1)
                {
                    continue;
                }

                var termText = term.Text;
                if (!string.IsNullOrEmpty (termText) && termText.StartsWith (prefix))
                {
                    term.Text = termText.Substring (prefixLength);
                    goodTerms.Add (term);
                }
            }

            terms = goodTerms.ToArray();
        }

        if (terms.Length <= 1)
        {
            return false;
        }

        Source.DataSource = terms;
        if (backward)
        {
            Source.Position = Source.Count - 1;
        }
        else
        {
            Source.Position = 0;
        }

        return true;
    }

    #endregion
}
