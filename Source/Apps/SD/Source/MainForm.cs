// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion

namespace SDHelper;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private PromptItem[]? _items;

    private void ComputeResult()
    {
        var builder = new StringBuilder();
        foreach (var item in _items!)
        {
            var value = item.Value;
            if (!string.IsNullOrEmpty(value))
            {
                if (builder.Length > 0)
                {
                    builder.Append(' ');
                }

                builder.Append(value);
            }
        }

        var result = builder.ToString().Trim();
        result = Regex.Replace(result, @"\s{2,}", " ");
        result = Regex.Replace(result, @"\s+,", ",");

        _resultBox.Text = result;

        var words = Regex.Split(result, @"[\w\-]+");
        _countLabel.Text = $"word count: {words.Length}";
    }

    private void MainForm_Load
        (
            object sender,
            EventArgs eventArgs
        )
    {
        var fileName = Path.Combine
            (
                AppContext.BaseDirectory,
                "woman.json"
            );
        _items = PromptItem.LoadFromFile(fileName);
        _bindingSource.DataSource = _items;
        _dataGrid.ShowCellToolTips = false;
    }

    private void BindingSource_CurrentChanged
        (
            object sender,
            EventArgs eventArgs
        )
    {
        ComputeResult();
    }

    private void CopyButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        var resultText = _resultBox.Text;
        if (!string.IsNullOrEmpty (resultText))
        {
            Clipboard.SetText(resultText.Trim());
        }
    }


    private void SelectNextValue()
    {
        _dataGrid.EndEdit();
        if (_bindingSource.Current is PromptItem current)
        {
            if (current.Suggestions is { } suggestions)
            {
                var currentValue = current.Value;
                if (string.IsNullOrEmpty(currentValue))
                {
                    current.Value = suggestions.FirstOrDefault();
                }
                else
                {
                    var index = Array.IndexOf(suggestions, currentValue);
                    if (index < 0 || index >= suggestions.Length - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }

                    current.Value = suggestions[index];
                }

                _dataGrid.Refresh();
                ComputeResult();
            }
        }
    }

    private void RemoveValue()
    {
        _dataGrid.EndEdit();
        if (_bindingSource.Current is PromptItem current)
        {
            current.Value = null;
            _dataGrid.Refresh();
            ComputeResult();
        }
    }

    private void MainForm_PreviewKeyDown
        (
            object sender,
            PreviewKeyDownEventArgs eventArgs
        )
    {
        if (eventArgs.KeyCode == Keys.Space)
        {
            eventArgs.IsInputKey = false;
            SelectNextValue();
        }

        if (eventArgs.KeyCode == Keys.Delete)
        {
            eventArgs.IsInputKey = false;
            RemoveValue();
        }

        if (eventArgs.KeyCode == Keys.F2)
        {
            eventArgs.IsInputKey = false;
            CopyButton_Click (sender, eventArgs);
        }
    }

    private void DataGrid_DataError
        (
            object sender,
            DataGridViewDataErrorEventArgs eventArgs
        )
    {
        eventArgs.ThrowException = false;
    }

    private void DataGrid_CellContentClick
        (
            object sender,
            DataGridViewCellEventArgs eventArgs
        )
    {
        SelectNextValue();
    }
}
