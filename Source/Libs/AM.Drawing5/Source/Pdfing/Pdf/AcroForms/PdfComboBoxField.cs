// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfComboBoxField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Represents the combo box field.
/// </summary>
public sealed class PdfComboBoxField
    : PdfChoiceField
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of PdfComboBoxField.
    /// </summary>
    internal PdfComboBoxField (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    internal PdfComboBoxField (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets or sets the index of the selected item.
    /// </summary>
    public int SelectedIndex
    {
        get
        {
            var value = Elements.GetString (Keys.V);
            return IndexInOptArray (value);
        }
        set
        {
            // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            if (value != -1) //R080325
            {
                var key = ValueInOptArray (value);
                Elements.SetString (Keys.V, key);
                Elements.SetInteger ("/I", value); //R080304 !!!!!!! sonst reagiert die Combobox �berhaupt nicht !!!!!
            }
        }
    }

    /// <summary>
    /// Gets or sets the value of the field.
    /// </summary>

    // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    public override PdfItem Value //R080304
    {
        get { return Elements[Keys.V]; }
        set
        {
            if (ReadOnly)
            {
                throw new InvalidOperationException ("The field is read only.");
            }

            if (value is PdfString || value is PdfName)
            {
                Elements[Keys.V] = value;
                SelectedIndex = SelectedIndex; //R080304 !!!
                if (SelectedIndex == -1)
                {
                    //R080317 noch nicht rund
                    try
                    {
                        //anh�ngen
                        ((PdfArray)(((PdfItem[])(Elements.Values))[2])).Elements.Add (Value);
                        SelectedIndex = SelectedIndex;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                throw new NotImplementedException ("Values other than string cannot be set.");
            }
        }
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// The description comes from PDF 1.4 Reference.
    /// </summary>
    public new class Keys : PdfAcroField.Keys
    {
        // Combo boxes have no additional entries.

        internal static DictionaryMeta Meta
        {
            get
            {
                if (Keys._meta == null)
                {
                    Keys._meta = CreateMeta (typeof (Keys));
                }

                return Keys._meta;
            }
        }

        static DictionaryMeta _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta
    {
        get { return Keys.Meta; }
    }
}
