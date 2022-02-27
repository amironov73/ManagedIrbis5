// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CoVariantArrayConversion

#region Using directives

using System;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace WinFormsExample;

public sealed class ClientEngine
    : IDisposable
{
    #region Properties

    public ISyncProvider Provider;

    #endregion

    #region Construction

    public ClientEngine()
        : this (new SyncConnection(), "user=librarian;password=secret;")
    {
    }

    public ClientEngine
        (
            ISyncProvider provider,
            string connectionString
        )
    {
        Provider = provider;
        Provider.ParseConnectionString (connectionString);
        Provider.Connect();
    }

    #endregion

    #region Public methods

    public void PopulateDatabases
        (
            DatabaseComboBox comboBox
        )
    {
        var databases = Provider.ListDatabases();
        comboBox.Items.Clear();
        comboBox.Items.AddRange (databases);
        if (databases.Length != 0)
        {
            comboBox.SelectedIndex = 0;
        }
    }

    public void PopulateScenarios
        (
            PrefixComboBox comboBox
        )
    {
        var database = Provider.EnsureDatabase();
        comboBox.FillWithScenarios (Provider, database);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Provider.Dispose();
    }

    #endregion
}
