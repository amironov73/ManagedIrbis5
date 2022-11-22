// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EnglishStemmer.cs -- стеммер для английского языка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.AOT.Stemming;

/// <summary>
/// Стеммер для английского языка.
/// </summary>
public sealed class EnglishStemmer
    : IStemmer
{
    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EnglishStemmer()
    {
        sb = new StringBuilder();
        Current = string.Empty;
    }


    private static EnglishStemmer? instance;

    private int bra;
    private int cursor;
    private int ket;
    private int limit;
    private int limit_backward;
    private StringBuilder sb;

    /// <summary>
    /// Gets or sets the current string in the <see cref="sb"/> object.
    /// </summary>
    /// <value>The current.</value>
    private string Current
    {
        get { return sb.ToString(); }
        set
        {
            sb.Remove (0, sb.Length);
            sb.Append (value);
            cursor = 0;
            limit = sb.Length;
            limit_backward = 0;
            bra = cursor;
            ket = limit;
        }
    }

    /// <summary>
    /// Gets the <see cref="EnglishStemmer"/> instance.
    /// </summary>
    /// <value>The <see cref="EnglishStemmer"/> instance.</value>
    public static EnglishStemmer Instance => instance ??= new EnglishStemmer();

    /// <summary>
    /// Reduces the given word into its stem.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <returns>The stemmed word.</returns>
    public string Stem (string word)
    {
        Current = word.ToLowerInvariant();
        CanStem();
        return Current;
    }

    private static readonly Among[] a_0 =
    {
        new ("arsen", -1, -1, null),
        new ("commun", -1, -1, null),
        new ("gener", -1, -1, null)
    };


    private static readonly Among[] a_1 =
    {
        new ("'", -1, 1, null),
        new ("'s'", 0, 1, null),
        new ("'s", -1, 1, null)
    };


    private static readonly Among[] a_2 =
    {
        new ("ied", -1, 2, null),
        new ("s", -1, 3, null),
        new ("ies", 1, 2, null),
        new ("sses", 1, 1, null),
        new ("ss", 1, -1, null),
        new ("us", 1, -1, null)
    };


    private static readonly Among[] a_3 =
    {
        new ("", -1, 3, null),
        new ("bb", 0, 2, null),
        new ("dd", 0, 2, null),
        new ("ff", 0, 2, null),
        new ("gg", 0, 2, null),
        new ("bl", 0, 1, null),
        new ("mm", 0, 2, null),
        new ("nn", 0, 2, null),
        new ("pp", 0, 2, null),
        new ("rr", 0, 2, null),
        new ("at", 0, 1, null),
        new ("tt", 0, 2, null),
        new ("iz", 0, 1, null)
    };


    private static readonly Among[] a_4 =
    {
        new ("ed", -1, 2, null),
        new ("eed", 0, 1, null),
        new ("ing", -1, 2, null),
        new ("edly", -1, 2, null),
        new ("eedly", 3, 1, null),
        new ("ingly", -1, 2, null)
    };


    private static readonly Among[] a_5 =
    {
        new ("anci", -1, 3, null),
        new ("enci", -1, 2, null),
        new ("ogi", -1, 13, null),
        new ("li", -1, 16, null),
        new ("bli", 3, 12, null),
        new ("abli", 4, 4, null),
        new ("alli", 3, 8, null),
        new ("fulli", 3, 14, null),
        new ("lessli", 3, 15, null),
        new ("ousli", 3, 10, null),
        new ("entli", 3, 5, null),
        new ("aliti", -1, 8, null),
        new ("biliti", -1, 12, null),
        new ("iviti", -1, 11, null),
        new ("tional", -1, 1, null),
        new ("ational", 14, 7, null),
        new ("alism", -1, 8, null),
        new ("ation", -1, 7, null),
        new ("ization", 17, 6, null),
        new ("izer", -1, 6, null),
        new ("ator", -1, 7, null),
        new ("iveness", -1, 11, null),
        new ("fulness", -1, 9, null),
        new ("ousness", -1, 10, null)
    };


    private static readonly Among[] a_6 =
    {
        new ("icate", -1, 4, null),
        new ("ative", -1, 6, null),
        new ("alize", -1, 3, null),
        new ("iciti", -1, 4, null),
        new ("ical", -1, 4, null),
        new ("tional", -1, 1, null),
        new ("ational", 5, 2, null),
        new ("ful", -1, 5, null),
        new ("ness", -1, 5, null)
    };


    private static readonly Among[] a_7 =
    {
        new ("ic", -1, 1, null),
        new ("ance", -1, 1, null),
        new ("ence", -1, 1, null),
        new ("able", -1, 1, null),
        new ("ible", -1, 1, null),
        new ("ate", -1, 1, null),
        new ("ive", -1, 1, null),
        new ("ize", -1, 1, null),
        new ("iti", -1, 1, null),
        new ("al", -1, 1, null),
        new ("ism", -1, 1, null),
        new ("ion", -1, 2, null),
        new ("er", -1, 1, null),
        new ("ous", -1, 1, null),
        new ("ant", -1, 1, null),
        new ("ent", -1, 1, null),
        new ("ment", 15, 1, null),
        new ("ement", 16, 1, null)
    };


    private static readonly Among[] a_8 =
    {
        new ("e", -1, 1, null),
        new ("l", -1, 2, null)
    };


    private static readonly Among[] a_9 =
    {
        new ("succeed", -1, -1, null),
        new ("proceed", -1, -1, null),
        new ("exceed", -1, -1, null),
        new ("canning", -1, -1, null),
        new ("inning", -1, -1, null),
        new ("earring", -1, -1, null),
        new ("herring", -1, -1, null),
        new ("outing", -1, -1, null)
    };


    private static readonly Among[] a_10 =
    {
        new ("andes", -1, -1, null),
        new ("atlas", -1, -1, null),
        new ("bias", -1, -1, null),
        new ("cosmos", -1, -1, null),
        new ("dying", -1, 3, null),
        new ("early", -1, 9, null),
        new ("gently", -1, 7, null),
        new ("howe", -1, -1, null),
        new ("idly", -1, 6, null),
        new ("lying", -1, 4, null),
        new ("news", -1, -1, null),
        new ("only", -1, 10, null),
        new ("singly", -1, 11, null),
        new ("skies", -1, 2, null),
        new ("skis", -1, 1, null),
        new ("sky", -1, -1, null),
        new ("tying", -1, 5, null),
        new ("ugly", -1, 8, null)
    };


    private static readonly char[] g_v = { (char)17, (char)65, (char)16, (char)1 };

    private static readonly char[] g_v_WXY = { (char)1, (char)17, (char)65, (char)208, (char)1 };

    private static readonly char[] g_valid_LI = { (char)55, (char)141, (char)2 };

    private bool B_Y_found;
    private int I_p1;
    private int I_p2;

    private bool in_grouping (char[] s, int min, int max)
    {
        if (cursor >= limit) return false;

        //           char ch = current.charAt(cursor);
        int ch = sb[cursor];
        if (ch > max || ch < min) return false;

        //           ch -= min;
        ch -= min;
        if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0) return false;
        cursor++;
        return true;
    }

    private bool in_grouping_b (char[] s, int min, int max)
    {
        if (cursor <= limit_backward) return false;

        //           char ch = current.charAt(cursor - 1);
        int ch = sb[cursor - 1];
        if (ch > max || ch < min) return false;
        ch -= min;
        if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0) return false;
        cursor--;
        return true;
    }

    private bool out_grouping (char[] s, int min, int max)
    {
        if (cursor >= limit) return false;

        //           char ch = current.charAt(cursor);
        int ch = sb[cursor];
        if (ch > max || ch < min)
        {
            cursor++;
            return true;
        }

        ch -= min;
        if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0)
        {
            cursor++;
            return true;
        }

        return false;
    }

    private bool out_grouping_b (char[] s, int min, int max)
    {
        if (cursor <= limit_backward) return false;

        //           char ch = current.charAt(cursor - 1);
        int ch = sb[cursor - 1];
        if (ch > max || ch < min)
        {
            cursor--;
            return true;
        }

        ch -= min;
        if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0)
        {
            cursor--;
            return true;
        }

        return false;
    }

    private bool eq_s (int s_size, string s)
    {
        if (limit - cursor < s_size) return false;
        int i;
        for (i = 0; i != s_size; i++)
        {
            if (sb[cursor + i] != s[i]) return false;

            //               if (current[cursor + i] != s[i]) return false;
        }

        cursor += s_size;
        return true;
    }

    private bool eq_s_b (int s_size, string s)
    {
        if (cursor - limit_backward < s_size) return false;
        int i;
        for (i = 0; i != s_size; i++)
        {
            //               if (current.charAt(cursor - s_size + i) != s.charAt(i)) return false;
            if (sb[cursor - s_size + i] != s[i]) return false;
        }

        cursor -= s_size;
        return true;
    }


    internal int find_among (Among[] v, int v_size)
    {
        var i = 0;
        var j = v_size;

        var c = cursor;
        var l = limit;

        var common_i = 0;
        var common_j = 0;

        var first_key_inspected = false;
        while (true)
        {
            var k = i + ((j - i) >> 1);
            var diff = 0;
            var common = common_i < common_j ? common_i : common_j; // smaller
            var w = v[k];
            int i2;

            for (i2 = common; i2 < w.s_size; i2++)
            {
                if (c + common == l)
                {
                    diff = -1;
                    break;
                }

                diff = sb[c + common] - w.s[i2];
                if (diff != 0) break;
                common++;
            }

            if (diff < 0)
            {
                j = k;
                common_j = common;
            }
            else
            {
                i = k;
                common_i = common;
            }

            if (j - i <= 1)
            {
                if (i > 0) break; // v->s has been inspected
                if (j == i) break; // only one item in v

                // - but now we need to go round once more to get
                // v->s inspected. This looks messy, but is actually
                // the optimal approach.
                if (first_key_inspected) break;
                first_key_inspected = true;
            }
        }

        while (true)
        {
            var w = v[i];
            if (common_i >= w.s_size)
            {
                cursor = c + w.s_size;
                if (w.method == null) return w.result;

                //bool res;
                //try
                //{
                //    Object resobj = w.method.invoke(w.methodobject,new Object[0]);
                //    res = resobj.toString().equals("true");
                //}
                //catch (InvocationTargetException e)
                //{
                //    res = false;
                //    // FIXME - debug message
                //}
                //catch (IllegalAccessException e)
                //{
                //    res = false;
                //// FIXME - debug message
                //}
                //cursor = c + w.s_size;
                //if (res) return w.result;
            }

            i = w.substring_i;
            if (i < 0) return 0;
        }
    }

    //    // find_among_b is for backwards processing. Same comments apply

    internal int find_among_b (Among[] v, int v_size)
    {
        var i = 0;
        var j = v_size;
        var c = cursor;
        var lb = limit_backward;
        var common_i = 0;
        var common_j = 0;
        var first_key_inspected = false;
        while (true)
        {
            var k = i + ((j - i) >> 1);
            var diff = 0;
            var common = common_i < common_j ? common_i : common_j;
            var w = v[k];
            int i2;
            for (i2 = w.s_size - 1 - common; i2 >= 0; i2--)
            {
                if (c - common == lb)
                {
                    diff = -1;
                    break;
                }

                //                   diff = current.charAt(c - 1 - common) - w.s[i2];
                diff = sb[c - 1 - common] - w.s[i2];
                if (diff != 0) break;
                common++;
            }

            if (diff < 0)
            {
                j = k;
                common_j = common;
            }
            else
            {
                i = k;
                common_i = common;
            }

            if (j - i <= 1)
            {
                if (i > 0) break;
                if (j == i) break;
                if (first_key_inspected) break;
                first_key_inspected = true;
            }
        }

        while (true)
        {
            var w = v[i];
            if (common_i >= w.s_size)
            {
                cursor = c - w.s_size;
                if (w.method == null) return w.result;

                //boolean res;
                //try
                //{
                //    Object resobj = w.method.invoke(w.methodobject,
                //        new Object[0]);
                //    res = resobj.toString().equals("true");
                // }
                //catch (InvocationTargetException e)
                //{
                //    res = false;
                //    // FIXME - debug message
                // }
                //catch (IllegalAccessException e)
                //{
                //    res = false;
                //    // FIXME - debug message
                // }
                //cursor = c - w.s_size;
                //if (res) return w.result;
            }

            i = w.substring_i;
            if (i < 0) return 0;
        }
    }

    //    /* to replace chars between c_bra and c_ket in current by the
    //     * chars in s.
    //     */
    private int replace_s (int c_bra, int c_ket, string s)
    {
        var adjustment = s.Length - (c_ket - c_bra);

        //           current.replace(c_bra, c_ket, s);
        sb = StringBufferReplace (c_bra, c_ket, sb, s);
        limit += adjustment;
        if (cursor >= c_ket) cursor += adjustment;
        else if (cursor > c_bra) cursor = c_bra;
        return adjustment;
    }

    private static StringBuilder StringBufferReplace (int start, int end, StringBuilder s, string s1)
    {
        var bufferReplace = new StringBuilder();
        for (var i = 0; i < start; i++)
        {
            bufferReplace.Insert (bufferReplace.Length, s[i]);
        }

        //           for (int i = 1; i < end - start + 1; i++)
        //           {
        bufferReplace.Insert (bufferReplace.Length, s1);

        //           }
        for (var i = end; i < s.Length; i++)
        {
            bufferReplace.Insert (bufferReplace.Length, s[i]);
        }

        return bufferReplace;

        //string temp = s.ToString();
        //temp = temp.Substring(start - 1, end - start + 1);
        //s = s.Replace(temp, s1, start - 1, end - start + 1);
        //return s;
    }

    private void slice_check()
    {
        if (bra < 0 ||
            bra > ket ||
            ket > limit ||
            limit > sb.Length) // this line could be removed
        {
            //System.err.println("faulty slice operation");
            // FIXME: report error somehow.
            /*
                fprintf(stderr, "faulty slice operation:\n");
                debug(z, -1, 0);
                exit(1);
                */
        }
    }

    private void slice_from (string s)
    {
        slice_check();
        replace_s (bra, ket, s);
    }

    private void slice_del()
    {
        slice_from ("");
    }

    private void insert (int c_bra, int c_ket, string s)
    {
        var adjustment = replace_s (c_bra, c_ket, s);
        if (c_bra <= bra) bra += adjustment;
        if (c_bra <= ket) ket += adjustment;
    }


    private bool r_prelude()
    {
        var returnn = false;
        var subroot = false;
        int v_1;
        int v_2;
        int v_3;
        int v_4;
        int v_5;

        // (, line 25
        // unset Y_found, line 26
        B_Y_found = false;

        // do, line 27
        v_1 = cursor;

        //        lab0:
        do
        {
            // (, line 27
            // [, line 27
            bra = cursor;

            // literal, line 27
            if (!(eq_s (1, "'")))
            {
                break;
            }

            // ], line 27
            ket = cursor;

            // delete, line 27
            slice_del();
        } while (false);

        cursor = v_1;

        // do, line 28
        v_2 = cursor;
        do
        {
            // (, line 28
            // [, line 28
            bra = cursor;

            // literal, line 28
            if (!(eq_s (1, "y")))
            {
                break;
            }

            // ], line 28
            ket = cursor;

            // <-, line 28
            slice_from ("Y");

            // set Y_found, line 28
            B_Y_found = true;
        } while (false);

        cursor = v_2;

        // do, line 29
        v_3 = cursor;
        do
        {
            // repeat, line 29
            replab3:
            while (true)
            {
                v_4 = cursor;
                do
                {
                    // (, line 29
                    // goto, line 29
                    while (true)
                    {
                        v_5 = cursor;
                        do
                        {
                            // (, line 29
                            if (!(in_grouping (g_v, 97, 121)))
                            {
                                break;
                            }

                            // [, line 29
                            bra = cursor;

                            // literal, line 29
                            if (!(eq_s (1, "y")))
                            {
                                break;
                            }

                            // ], line 29
                            ket = cursor;
                            cursor = v_5;
                            subroot = true;
                            if (subroot) break;
                        } while (false);

                        if (subroot)
                        {
                            subroot = false;
                            break;
                        }

                        cursor = v_5;
                        if (cursor >= limit)
                        {
                            subroot = true;
                            break;
                        }

                        cursor++;
                    }

                    returnn = true;
                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    // <-, line 29
                    slice_from ("Y");

                    // set Y_found, line 29
                    B_Y_found = true;
                    if (returnn)
                    {
                        goto replab3;
                    }
                } while (false);

                cursor = v_4;
                break;
            }
        } while (false);

        cursor = v_3;
        return true;
    }


    private bool r_mark_regions()
    {
        var subroot = false;
        int v_1;
        int v_2;

        // (, line 32
        I_p1 = limit;
        I_p2 = limit;

        // do, line 35
        v_1 = cursor;
        do
        {
            // (, line 35
            // or, line 41
            do
            {
                v_2 = cursor;
                do
                {
                    // among, line 36
                    if (find_among (a_0, 3) == 0)
                    {
                        break;
                    }

                    subroot = true;
                    if (subroot) break;
                } while (false);

                if (subroot)
                {
                    subroot = false;
                    break;
                }

                cursor = v_2;

                // (, line 41
                // gopast, line 41
                while (true)
                {
                    do
                    {
                        if (!(in_grouping (g_v, 97, 121)))
                        {
                            break;
                        }

                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    if (cursor >= limit)
                    {
                        goto breaklab0;
                    }

                    cursor++;
                }

                // gopast, line 41
                while (true)
                {
                    do
                    {
                        if (!(out_grouping (g_v, 97, 121)))
                        {
                            break;
                        }

                        //                                    break golab5;
                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    if (cursor >= limit)
                    {
                        goto breaklab0;
                    }

                    cursor++;
                }
            } while (false);

            // setmark p1, line 42
            I_p1 = cursor;

            // gopast, line 43
            while (true)
            {
                do
                {
                    if (!(in_grouping (g_v, 97, 121)))
                    {
                        break;
                    }

                    subroot = true;
                    if (subroot) break;
                } while (false);

                if (subroot)
                {
                    subroot = false;
                    break;
                }

                if (cursor >= limit)
                {
                    goto breaklab0;
                }

                cursor++;
            }

            // gopast, line 43
            while (true)
            {
                do
                {
                    if (!(out_grouping (g_v, 97, 121)))
                    {
                        break;
                    }

                    subroot = true;
                    if (subroot) break;
                } while (false);

                if (subroot)
                {
                    subroot = false;
                    break;
                }

                if (cursor >= limit)
                {
                    goto breaklab0;
                }

                cursor++;
            }

            // setmark p2, line 43
            I_p2 = cursor;
        } while (false);

        breaklab0:
        cursor = v_1;
        return true;
    }


    private bool r_shortv()
    {
        var subroot = false;
        int v_1;

        // (, line 49
        // or, line 51
        //        lab0:
        do
        {
            v_1 = limit - cursor;
            do
            {
                // (, line 50
                if (!(out_grouping_b (g_v_WXY, 89, 121)))
                {
                    break;
                }

                if (!(in_grouping_b (g_v, 97, 121)))
                {
                    break;
                }

                if (!(out_grouping_b (g_v, 97, 121)))
                {
                    break;
                }

                subroot = true;
                if (subroot) break;
            } while (false);

            if (subroot)
            {
                subroot = false;
                break;
            }

            cursor = limit - v_1;

            // (, line 52
            if (!(out_grouping_b (g_v, 97, 121)))
            {
                return false;
            }

            if (!(in_grouping_b (g_v, 97, 121)))
            {
                return false;
            }

            // atlimit, line 52
            if (cursor > limit_backward)
            {
                return false;
            }
        } while (false);

        return true;
    }

    private bool r_R1()
    {
        if (!(I_p1 <= cursor))
        {
            return false;
        }

        return true;
    }

    private bool r_R2()
    {
        if (!(I_p2 <= cursor))
        {
            return false;
        }

        return true;
    }


    private bool r_Step_1a()
    {
        var subroot = false;
        int among_var;
        int v_1;
        int v_2;

        // (, line 58
        // try, line 59
        v_1 = limit - cursor;
        do
        {
            // (, line 59
            // [, line 60
            ket = cursor;

            // substring, line 60
            among_var = find_among_b (a_1, 3);
            if (among_var == 0)
            {
                cursor = limit - v_1;
                break;
            }

            // ], line 60
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    cursor = limit - v_1;
                    subroot = true;
                    break;
                case 1:
                    // (, line 62
                    // delete, line 62
                    slice_del();
                    break;
            }

            if (subroot)
            {
                subroot = false;
                break;
            }
        } while (false);

        // [, line 65
        ket = cursor;

        // substring, line 65
        among_var = find_among_b (a_2, 6);
        if (among_var == 0)
        {
            return false;
        }

        // ], line 65
        bra = cursor;
        switch (among_var)
        {
            case 0:
                return false;
            case 1:
                // (, line 66
                // <-, line 66
                slice_from ("ss");
                break;
            case 2:
                // (, line 68
                // or, line 68
                //    lab1:
                do
                {
                    v_2 = limit - cursor;
                    do
                    {
                        // (, line 68
                        // hop, line 68
                        {
                            var c = cursor - 2;
                            if (limit_backward > c || c > limit)
                            {
                                break;
                            }

                            cursor = c;
                        }

                        // <-, line 68
                        slice_from ("i");
                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    cursor = limit - v_2;

                    // <-, line 68
                    slice_from ("ie");
                } while (false);

                break;
            case 3:
                // (, line 69
                // next, line 69
                if (cursor <= limit_backward)
                {
                    return false;
                }

                cursor--;

                // gopast, line 69
                while (true)
                {
                    do
                    {
                        if (!(in_grouping_b (g_v, 97, 121)))
                        {
                            break;
                        }

                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    if (cursor <= limit_backward)
                    {
                        return false;
                    }

                    cursor--;
                }

                // delete, line 69
                slice_del();
                break;
        }

        return true;
    }


    private bool r_Step_1b()
    {
        var subroot = false;
        int among_var;
        int v_1;
        int v_3;
        int v_4;

        // (, line 74
        // [, line 75
        ket = cursor;

        // substring, line 75
        among_var = find_among_b (a_4, 6);
        if (among_var == 0)
        {
            return false;
        }

        // ], line 75
        bra = cursor;
        switch (among_var)
        {
            case 0:
                return false;
            case 1:
                // (, line 77
                // call R1, line 77
                if (!r_R1())
                {
                    return false;
                }

                // <-, line 77
                slice_from ("ee");
                break;
            case 2:
                // (, line 79
                // test, line 80
                v_1 = limit - cursor;

                // gopast, line 80
                while (true)
                {
                    do
                    {
                        if (!(in_grouping_b (g_v, 97, 121)))
                        {
                            break;
                        }

                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    if (cursor <= limit_backward)
                    {
                        return false;
                    }

                    cursor--;
                }

                cursor = limit - v_1;

                // delete, line 80
                slice_del();

                // test, line 81
                v_3 = limit - cursor;

                // substring, line 81
                among_var = find_among_b (a_3, 13);
                if (among_var == 0)
                {
                    return false;
                }

                cursor = limit - v_3;
                switch (among_var)
                {
                    case 0:
                        return false;
                    case 1:
                        // (, line 83
                        // <+, line 83
                    {
                        var c = cursor;
                        insert (cursor, cursor, "e");
                        cursor = c;
                    }
                        break;
                    case 2:
                        // (, line 86
                        // [, line 86
                        ket = cursor;

                        // next, line 86
                        if (cursor <= limit_backward)
                        {
                            return false;
                        }

                        cursor--;

                        // ], line 86
                        bra = cursor;

                        // delete, line 86
                        slice_del();
                        break;
                    case 3:
                        // (, line 87
                        // atmark, line 87
                        if (cursor != I_p1)
                        {
                            return false;
                        }

                        // test, line 87
                        v_4 = limit - cursor;

                        // call shortv, line 87
                        if (!r_shortv())
                        {
                            return false;
                        }

                        cursor = limit - v_4;

                        // <+, line 87
                    {
                        var c = cursor;
                        insert (cursor, cursor, "e");
                        cursor = c;
                    }
                        break;
                }

                break;
        }

        return true;
    }


    private bool r_Step_1c()
    {
        var returnn = false;
        var subroot = false;
        int v_1;
        int v_2;

        // (, line 93
        // [, line 94
        ket = cursor;

        // or, line 94
        //        lab0:
        do
        {
            v_1 = limit - cursor;
            do
            {
                // literal, line 94
                if (!(eq_s_b (1, "y")))
                {
                    break;
                }

                subroot = true;
                if (subroot) break;
            } while (false);

            if (subroot)
            {
                subroot = false;
                break;
            }

            cursor = limit - v_1;

            // literal, line 94
            if (!(eq_s_b (1, "Y")))
            {
                return false;
            }
        } while (false);

        // ], line 94
        bra = cursor;
        if (!(out_grouping_b (g_v, 97, 121)))
        {
            return false;
        }

        // not, line 95
        {
            v_2 = limit - cursor;
            do
            {
                returnn = true;

                // atlimit, line 95
                if (cursor > limit_backward)
                {
                    break;
                }

                if (returnn)
                {
                    return false;
                }
            } while (false);

            cursor = limit - v_2;
        }

        // <-, line 96
        slice_from ("i");
        return true;
    }


    private bool r_Step_2()
    {
        int among_var;

        // (, line 99
        // [, line 100
        ket = cursor;

        // substring, line 100
        among_var = find_among_b (a_5, 24);
        if (among_var == 0)
        {
            return false;
        }

        // ], line 100
        bra = cursor;

        // call R1, line 100
        if (!r_R1())
        {
            return false;
        }

        switch (among_var)
        {
            case 0:
                return false;
            case 1:
                // (, line 101
                // <-, line 101
                slice_from ("tion");
                break;
            case 2:
                // (, line 102
                // <-, line 102
                slice_from ("ence");
                break;
            case 3:
                // (, line 103
                // <-, line 103
                slice_from ("ance");
                break;
            case 4:
                // (, line 104
                // <-, line 104
                slice_from ("able");
                break;
            case 5:
                // (, line 105
                // <-, line 105
                slice_from ("ent");
                break;
            case 6:
                // (, line 107
                // <-, line 107
                slice_from ("ize");
                break;
            case 7:
                // (, line 109
                // <-, line 109
                slice_from ("ate");
                break;
            case 8:
                // (, line 111
                // <-, line 111
                slice_from ("al");
                break;
            case 9:
                // (, line 112
                // <-, line 112
                slice_from ("ful");
                break;
            case 10:
                // (, line 114
                // <-, line 114
                slice_from ("ous");
                break;
            case 11:
                // (, line 116
                // <-, line 116
                slice_from ("ive");
                break;
            case 12:
                // (, line 118
                // <-, line 118
                slice_from ("ble");
                break;
            case 13:
                // (, line 119
                // literal, line 119
                if (!(eq_s_b (1, "l")))
                {
                    return false;
                }

                // <-, line 119
                slice_from ("og");
                break;
            case 14:
                // (, line 120
                // <-, line 120
                slice_from ("ful");
                break;
            case 15:
                // (, line 121
                // <-, line 121
                slice_from ("less");
                break;
            case 16:
                // (, line 122
                if (!(in_grouping_b (g_valid_LI, 99, 116)))
                {
                    return false;
                }

                // delete, line 122
                slice_del();
                break;
        }

        return true;
    }


    private bool r_Step_3()
    {
        int among_var;

        // (, line 126
        // [, line 127
        ket = cursor;

        // substring, line 127
        among_var = find_among_b (a_6, 9);
        if (among_var == 0)
        {
            return false;
        }

        // ], line 127
        bra = cursor;

        // call R1, line 127
        if (!r_R1())
        {
            return false;
        }

        switch (among_var)
        {
            case 0:
                return false;
            case 1:
                // (, line 128
                // <-, line 128
                slice_from ("tion");
                break;
            case 2:
                // (, line 129
                // <-, line 129
                slice_from ("ate");
                break;
            case 3:
                // (, line 130
                // <-, line 130
                slice_from ("al");
                break;
            case 4:
                // (, line 132
                // <-, line 132
                slice_from ("ic");
                break;
            case 5:
                // (, line 134
                // delete, line 134
                slice_del();
                break;
            case 6:
                // (, line 136
                // call R2, line 136
                if (!r_R2())
                {
                    return false;
                }

                // delete, line 136
                slice_del();
                break;
        }

        return true;
    }


    private bool r_Step_4()
    {
        var subroot = false;
        int among_var;
        int v_1;

        // (, line 140
        // [, line 141
        ket = cursor;

        // substring, line 141
        among_var = find_among_b (a_7, 18);
        if (among_var == 0)
        {
            return false;
        }

        // ], line 141
        bra = cursor;

        // call R2, line 141
        if (!r_R2())
        {
            return false;
        }

        switch (among_var)
        {
            case 0:
                return false;
            case 1:
                // (, line 144
                // delete, line 144
                slice_del();
                break;
            case 2:
                // (, line 145
                // or, line 145
                do
                {
                    v_1 = limit - cursor;
                    do
                    {
                        // literal, line 145
                        if (!(eq_s_b (1, "s")))
                        {
                            break;
                        }

                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    cursor = limit - v_1;

                    // literal, line 145
                    if (!(eq_s_b (1, "t")))
                    {
                        return false;
                    }
                } while (false);

                // delete, line 145
                slice_del();
                break;
        }

        return true;
    }


    private bool r_Step_5()
    {
        var returnn = false;
        var subroot = false;
        int among_var;
        int v_1;
        int v_2;

        // (, line 149
        // [, line 150
        ket = cursor;

        // substring, line 150
        among_var = find_among_b (a_8, 2);
        if (among_var == 0)
        {
            return false;
        }

        // ], line 150
        bra = cursor;
        switch (among_var)
        {
            case 0:
                return false;
            case 1:
                // (, line 151
                // or, line 151
                do
                {
                    v_1 = limit - cursor;
                    do
                    {
                        // call R2, line 151
                        if (!r_R2())
                        {
                            break;
                        }

                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    cursor = limit - v_1;

                    // (, line 151
                    // call R1, line 151
                    if (!r_R1())
                    {
                        return false;
                    }

                    // not, line 151
                    {
                        v_2 = limit - cursor;
                        do
                        {
                            returnn = true;

                            // call shortv, line 151
                            if (!r_shortv())
                            {
                                break;
                            }

                            if (returnn)
                            {
                                return false;
                            }
                        } while (false);

                        cursor = limit - v_2;
                    }
                } while (false);

                // delete, line 151
                slice_del();
                break;
            case 2:
                // (, line 152
                // call R2, line 152
                if (!r_R2())
                {
                    return false;
                }

                // literal, line 152
                if (!(eq_s_b (1, "l")))
                {
                    return false;
                }

                // delete, line 152
                slice_del();
                break;
        }

        return true;
    }


    private bool r_exception2()
    {
        // (, line 156
        // [, line 158
        ket = cursor;

        // substring, line 158
        if (find_among_b (a_9, 8) == 0)
        {
            return false;
        }

        // ], line 158
        bra = cursor;

        // atlimit, line 158
        if (cursor > limit_backward)
        {
            return false;
        }

        return true;
    }


    private bool r_exception1()
    {
        int among_var;

        // (, line 168
        // [, line 170
        bra = cursor;

        // substring, line 170
        among_var = find_among (a_10, 18);
        if (among_var == 0)
        {
            return false;
        }

        // ], line 170
        ket = cursor;

        // atlimit, line 170
        if (cursor < limit)
        {
            return false;
        }

        switch (among_var)
        {
            case 0:
                return false;
            case 1:
                // (, line 174
                // <-, line 174
                slice_from ("ski");
                break;
            case 2:
                // (, line 175
                // <-, line 175
                slice_from ("sky");
                break;
            case 3:
                // (, line 176
                // <-, line 176
                slice_from ("die");
                break;
            case 4:
                // (, line 177
                // <-, line 177
                slice_from ("lie");
                break;
            case 5:
                // (, line 178
                // <-, line 178
                slice_from ("tie");
                break;
            case 6:
                // (, line 182
                // <-, line 182
                slice_from ("idl");
                break;
            case 7:
                // (, line 183
                // <-, line 183
                slice_from ("gentl");
                break;
            case 8:
                // (, line 184
                // <-, line 184
                slice_from ("ugli");
                break;
            case 9:
                // (, line 185
                // <-, line 185
                slice_from ("earli");
                break;
            case 10:
                // (, line 186
                // <-, line 186
                slice_from ("onli");
                break;
            case 11:
                // (, line 187
                // <-, line 187
                slice_from ("singl");
                break;
        }

        return true;
    }


    private bool r_postlude()
    {
        var returnn = false;
        var subroot = false;
        int v_1;
        int v_2;

        // (, line 203
        // Boolean test Y_found, line 203
        if (!(B_Y_found))
        {
            return false;
        }

        // repeat, line 203
        replab0:
        while (true)
        {
            v_1 = cursor;
            do
            {
                // (, line 203
                // goto, line 203
                while (true)
                {
                    v_2 = cursor;
                    do
                    {
                        // (, line 203
                        // [, line 203
                        bra = cursor;

                        // literal, line 203
                        if (!(eq_s (1, "Y")))
                        {
                            break;
                        }

                        // ], line 203
                        ket = cursor;
                        cursor = v_2;
                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    cursor = v_2;
                    if (cursor >= limit)
                    {
                        subroot = true;
                        break;
                    }

                    cursor++;
                }

                returnn = true;
                if (subroot)
                {
                    subroot = false;
                    break;
                }

                // <-, line 203
                slice_from ("y");
                if (returnn)
                {
                    goto replab0;
                }
            } while (false);

            cursor = v_1;
            break;
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool CanStem()
    {
        var returnn = true;
        var subroot = false;
        int v_1;
        int v_2;
        int v_3;
        int v_4;
        int v_5;
        int v_6;
        int v_7;
        int v_8;
        int v_9;
        int v_10;
        int v_11;
        int v_12;
        int v_13;

        // (, line 205
        // or, line 207
        do
        {
            v_1 = cursor;
            do
            {
                // call exception1, line 207
                if (!r_exception1())
                {
                    break;
                }

                subroot = true;
                if (subroot) break;
            } while (false);

            if (subroot)
            {
                subroot = false;
                break;
            }

            cursor = v_1;
            do
            {
                // not, line 208
                {
                    v_2 = cursor;
                    do
                    {
                        // hop, line 208
                        {
                            var c = cursor + 3;
                            if (0 > c || c > limit)
                            {
                                break;
                                ;
                            }

                            cursor = c;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);

                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }

                    cursor = v_2;
                }
                returnn = true;
                if (returnn) goto breaklab0;
            } while (false);

            cursor = v_1;

            // (, line 208
            // do, line 209
            v_3 = cursor;
            do
            {
                // call prelude, line 209
                if (!r_prelude())
                {
                    break;
                }
            } while (false);

            cursor = v_3;

            // do, line 210
            v_4 = cursor;
            do
            {
                // call mark_regions, line 210
                if (!r_mark_regions())
                {
                    break;
                }
            } while (false);

            cursor = v_4;

            // backwards, line 211
            limit_backward = cursor;
            cursor = limit;

            // (, line 211
            // do, line 213
            v_5 = limit - cursor;
            do
            {
                // call Step_1a, line 213
                if (!r_Step_1a())
                {
                    break;
                }
            } while (false);

            cursor = limit - v_5;

            // or, line 215
            do
            {
                v_6 = limit - cursor;
                do
                {
                    // call exception2, line 215
                    if (!r_exception2())
                    {
                        break;
                    }

                    subroot = true;
                    if (subroot) break;
                } while (false);

                if (subroot)
                {
                    subroot = false;
                    break;
                }

                cursor = limit - v_6;

                // (, line 215
                // do, line 217
                v_7 = limit - cursor;
                do
                {
                    // call Step_1b, line 217
                    if (!r_Step_1b())
                    {
                        break;
                    }
                } while (false);

                cursor = limit - v_7;

                // do, line 218
                v_8 = limit - cursor;
                do
                {
                    // call Step_1c, line 218
                    if (!r_Step_1c())
                    {
                        break;
                    }
                } while (false);

                cursor = limit - v_8;

                // do, line 220
                v_9 = limit - cursor;
                do
                {
                    // call Step_2, line 220
                    if (!r_Step_2())
                    {
                        break;
                    }
                } while (false);

                cursor = limit - v_9;

                // do, line 221
                v_10 = limit - cursor;
                do
                {
                    // call Step_3, line 221
                    if (!r_Step_3())
                    {
                        break;
                    }
                } while (false);

                cursor = limit - v_10;

                // do, line 222
                v_11 = limit - cursor;
                do
                {
                    // call Step_4, line 222
                    if (!r_Step_4())
                    {
                        break;
                    }
                } while (false);

                cursor = limit - v_11;

                // do, line 224
                v_12 = limit - cursor;
                do
                {
                    // call Step_5, line 224
                    if (!r_Step_5())
                    {
                        break;
                    }
                } while (false);

                cursor = limit - v_12;
            } while (false);

            cursor = limit_backward; // do, line 227
            v_13 = cursor;
            do
            {
                // call postlude, line 227
                if (!r_postlude())
                {
                    break;
                }
            } while (false);

            cursor = v_13;
        } while (false);

        breaklab0:
        return true;
    }
}
