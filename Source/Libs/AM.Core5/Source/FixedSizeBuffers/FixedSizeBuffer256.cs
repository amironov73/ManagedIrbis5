// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* FixedSizeBuffer256.cs -- буфер фиксированного размера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace AM.FixedSizeBuffers;

/// <summary>
/// Буфер фиксированного размера 256.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct FixedSizeBuffer256<T>
    : IFixedSizeBuffer<T>
{
    /// <summary>A slot in the buffer</summary>
    public T Item1;

    /// <summary>A slot in the buffer</summary>
    public T Item2;

    /// <summary>A slot in the buffer</summary>
    public T Item3;

    /// <summary>A slot in the buffer</summary>
    public T Item4;

    /// <summary>A slot in the buffer</summary>
    public T Item5;

    /// <summary>A slot in the buffer</summary>
    public T Item6;

    /// <summary>A slot in the buffer</summary>
    public T Item7;

    /// <summary>A slot in the buffer</summary>
    public T Item8;

    /// <summary>A slot in the buffer</summary>
    public T Item9;

    /// <summary>A slot in the buffer</summary>
    public T Item10;

    /// <summary>A slot in the buffer</summary>
    public T Item11;

    /// <summary>A slot in the buffer</summary>
    public T Item12;

    /// <summary>A slot in the buffer</summary>
    public T Item13;

    /// <summary>A slot in the buffer</summary>
    public T Item14;

    /// <summary>A slot in the buffer</summary>
    public T Item15;

    /// <summary>A slot in the buffer</summary>
    public T Item16;

    /// <summary>A slot in the buffer</summary>
    public T Item17;

    /// <summary>A slot in the buffer</summary>
    public T Item18;

    /// <summary>A slot in the buffer</summary>
    public T Item19;

    /// <summary>A slot in the buffer</summary>
    public T Item20;

    /// <summary>A slot in the buffer</summary>
    public T Item21;

    /// <summary>A slot in the buffer</summary>
    public T Item22;

    /// <summary>A slot in the buffer</summary>
    public T Item23;

    /// <summary>A slot in the buffer</summary>
    public T Item24;

    /// <summary>A slot in the buffer</summary>
    public T Item25;

    /// <summary>A slot in the buffer</summary>
    public T Item26;

    /// <summary>A slot in the buffer</summary>
    public T Item27;

    /// <summary>A slot in the buffer</summary>
    public T Item28;

    /// <summary>A slot in the buffer</summary>
    public T Item29;

    /// <summary>A slot in the buffer</summary>
    public T Item30;

    /// <summary>A slot in the buffer</summary>
    public T Item31;

    /// <summary>A slot in the buffer</summary>
    public T Item32;

    /// <summary>A slot in the buffer</summary>
    public T Item33;

    /// <summary>A slot in the buffer</summary>
    public T Item34;

    /// <summary>A slot in the buffer</summary>
    public T Item35;

    /// <summary>A slot in the buffer</summary>
    public T Item36;

    /// <summary>A slot in the buffer</summary>
    public T Item37;

    /// <summary>A slot in the buffer</summary>
    public T Item38;

    /// <summary>A slot in the buffer</summary>
    public T Item39;

    /// <summary>A slot in the buffer</summary>
    public T Item40;

    /// <summary>A slot in the buffer</summary>
    public T Item41;

    /// <summary>A slot in the buffer</summary>
    public T Item42;

    /// <summary>A slot in the buffer</summary>
    public T Item43;

    /// <summary>A slot in the buffer</summary>
    public T Item44;

    /// <summary>A slot in the buffer</summary>
    public T Item45;

    /// <summary>A slot in the buffer</summary>
    public T Item46;

    /// <summary>A slot in the buffer</summary>
    public T Item47;

    /// <summary>A slot in the buffer</summary>
    public T Item48;

    /// <summary>A slot in the buffer</summary>
    public T Item49;

    /// <summary>A slot in the buffer</summary>
    public T Item50;

    /// <summary>A slot in the buffer</summary>
    public T Item51;

    /// <summary>A slot in the buffer</summary>
    public T Item52;

    /// <summary>A slot in the buffer</summary>
    public T Item53;

    /// <summary>A slot in the buffer</summary>
    public T Item54;

    /// <summary>A slot in the buffer</summary>
    public T Item55;

    /// <summary>A slot in the buffer</summary>
    public T Item56;

    /// <summary>A slot in the buffer</summary>
    public T Item57;

    /// <summary>A slot in the buffer</summary>
    public T Item58;

    /// <summary>A slot in the buffer</summary>
    public T Item59;

    /// <summary>A slot in the buffer</summary>
    public T Item60;

    /// <summary>A slot in the buffer</summary>
    public T Item61;

    /// <summary>A slot in the buffer</summary>
    public T Item62;

    /// <summary>A slot in the buffer</summary>
    public T Item63;

    /// <summary>A slot in the buffer</summary>
    public T Item64;

    /// <summary>A slot in the buffer</summary>
    public T Item65;

    /// <summary>A slot in the buffer</summary>
    public T Item66;

    /// <summary>A slot in the buffer</summary>
    public T Item67;

    /// <summary>A slot in the buffer</summary>
    public T Item68;

    /// <summary>A slot in the buffer</summary>
    public T Item69;

    /// <summary>A slot in the buffer</summary>
    public T Item70;

    /// <summary>A slot in the buffer</summary>
    public T Item71;

    /// <summary>A slot in the buffer</summary>
    public T Item72;

    /// <summary>A slot in the buffer</summary>
    public T Item73;

    /// <summary>A slot in the buffer</summary>
    public T Item74;

    /// <summary>A slot in the buffer</summary>
    public T Item75;

    /// <summary>A slot in the buffer</summary>
    public T Item76;

    /// <summary>A slot in the buffer</summary>
    public T Item77;

    /// <summary>A slot in the buffer</summary>
    public T Item78;

    /// <summary>A slot in the buffer</summary>
    public T Item79;

    /// <summary>A slot in the buffer</summary>
    public T Item80;

    /// <summary>A slot in the buffer</summary>
    public T Item81;

    /// <summary>A slot in the buffer</summary>
    public T Item82;

    /// <summary>A slot in the buffer</summary>
    public T Item83;

    /// <summary>A slot in the buffer</summary>
    public T Item84;

    /// <summary>A slot in the buffer</summary>
    public T Item85;

    /// <summary>A slot in the buffer</summary>
    public T Item86;

    /// <summary>A slot in the buffer</summary>
    public T Item87;

    /// <summary>A slot in the buffer</summary>
    public T Item88;

    /// <summary>A slot in the buffer</summary>
    public T Item89;

    /// <summary>A slot in the buffer</summary>
    public T Item90;

    /// <summary>A slot in the buffer</summary>
    public T Item91;

    /// <summary>A slot in the buffer</summary>
    public T Item92;

    /// <summary>A slot in the buffer</summary>
    public T Item93;

    /// <summary>A slot in the buffer</summary>
    public T Item94;

    /// <summary>A slot in the buffer</summary>
    public T Item95;

    /// <summary>A slot in the buffer</summary>
    public T Item96;

    /// <summary>A slot in the buffer</summary>
    public T Item97;

    /// <summary>A slot in the buffer</summary>
    public T Item98;

    /// <summary>A slot in the buffer</summary>
    public T Item99;

    /// <summary>A slot in the buffer</summary>
    public T Item100;

    /// <summary>A slot in the buffer</summary>
    public T Item101;

    /// <summary>A slot in the buffer</summary>
    public T Item102;

    /// <summary>A slot in the buffer</summary>
    public T Item103;

    /// <summary>A slot in the buffer</summary>
    public T Item104;

    /// <summary>A slot in the buffer</summary>
    public T Item105;

    /// <summary>A slot in the buffer</summary>
    public T Item106;

    /// <summary>A slot in the buffer</summary>
    public T Item107;

    /// <summary>A slot in the buffer</summary>
    public T Item108;

    /// <summary>A slot in the buffer</summary>
    public T Item109;

    /// <summary>A slot in the buffer</summary>
    public T Item110;

    /// <summary>A slot in the buffer</summary>
    public T Item111;

    /// <summary>A slot in the buffer</summary>
    public T Item112;

    /// <summary>A slot in the buffer</summary>
    public T Item113;

    /// <summary>A slot in the buffer</summary>
    public T Item114;

    /// <summary>A slot in the buffer</summary>
    public T Item115;

    /// <summary>A slot in the buffer</summary>
    public T Item116;

    /// <summary>A slot in the buffer</summary>
    public T Item117;

    /// <summary>A slot in the buffer</summary>
    public T Item118;

    /// <summary>A slot in the buffer</summary>
    public T Item119;

    /// <summary>A slot in the buffer</summary>
    public T Item120;

    /// <summary>A slot in the buffer</summary>
    public T Item121;

    /// <summary>A slot in the buffer</summary>
    public T Item122;

    /// <summary>A slot in the buffer</summary>
    public T Item123;

    /// <summary>A slot in the buffer</summary>
    public T Item124;

    /// <summary>A slot in the buffer</summary>
    public T Item125;

    /// <summary>A slot in the buffer</summary>
    public T Item126;

    /// <summary>A slot in the buffer</summary>
    public T Item127;

    /// <summary>A slot in the buffer</summary>
    public T Item128;

    /// <summary>A slot in the buffer</summary>
    public T Item129;

    /// <summary>A slot in the buffer</summary>
    public T Item130;

    /// <summary>A slot in the buffer</summary>
    public T Item131;

    /// <summary>A slot in the buffer</summary>
    public T Item132;

    /// <summary>A slot in the buffer</summary>
    public T Item133;

    /// <summary>A slot in the buffer</summary>
    public T Item134;

    /// <summary>A slot in the buffer</summary>
    public T Item135;

    /// <summary>A slot in the buffer</summary>
    public T Item136;

    /// <summary>A slot in the buffer</summary>
    public T Item137;

    /// <summary>A slot in the buffer</summary>
    public T Item138;

    /// <summary>A slot in the buffer</summary>
    public T Item139;

    /// <summary>A slot in the buffer</summary>
    public T Item140;

    /// <summary>A slot in the buffer</summary>
    public T Item141;

    /// <summary>A slot in the buffer</summary>
    public T Item142;

    /// <summary>A slot in the buffer</summary>
    public T Item143;

    /// <summary>A slot in the buffer</summary>
    public T Item144;

    /// <summary>A slot in the buffer</summary>
    public T Item145;

    /// <summary>A slot in the buffer</summary>
    public T Item146;

    /// <summary>A slot in the buffer</summary>
    public T Item147;

    /// <summary>A slot in the buffer</summary>
    public T Item148;

    /// <summary>A slot in the buffer</summary>
    public T Item149;

    /// <summary>A slot in the buffer</summary>
    public T Item150;

    /// <summary>A slot in the buffer</summary>
    public T Item151;

    /// <summary>A slot in the buffer</summary>
    public T Item152;

    /// <summary>A slot in the buffer</summary>
    public T Item153;

    /// <summary>A slot in the buffer</summary>
    public T Item154;

    /// <summary>A slot in the buffer</summary>
    public T Item155;

    /// <summary>A slot in the buffer</summary>
    public T Item156;

    /// <summary>A slot in the buffer</summary>
    public T Item157;

    /// <summary>A slot in the buffer</summary>
    public T Item158;

    /// <summary>A slot in the buffer</summary>
    public T Item159;

    /// <summary>A slot in the buffer</summary>
    public T Item160;

    /// <summary>A slot in the buffer</summary>
    public T Item161;

    /// <summary>A slot in the buffer</summary>
    public T Item162;

    /// <summary>A slot in the buffer</summary>
    public T Item163;

    /// <summary>A slot in the buffer</summary>
    public T Item164;

    /// <summary>A slot in the buffer</summary>
    public T Item165;

    /// <summary>A slot in the buffer</summary>
    public T Item166;

    /// <summary>A slot in the buffer</summary>
    public T Item167;

    /// <summary>A slot in the buffer</summary>
    public T Item168;

    /// <summary>A slot in the buffer</summary>
    public T Item169;

    /// <summary>A slot in the buffer</summary>
    public T Item170;

    /// <summary>A slot in the buffer</summary>
    public T Item171;

    /// <summary>A slot in the buffer</summary>
    public T Item172;

    /// <summary>A slot in the buffer</summary>
    public T Item173;

    /// <summary>A slot in the buffer</summary>
    public T Item174;

    /// <summary>A slot in the buffer</summary>
    public T Item175;

    /// <summary>A slot in the buffer</summary>
    public T Item176;

    /// <summary>A slot in the buffer</summary>
    public T Item177;

    /// <summary>A slot in the buffer</summary>
    public T Item178;

    /// <summary>A slot in the buffer</summary>
    public T Item179;

    /// <summary>A slot in the buffer</summary>
    public T Item180;

    /// <summary>A slot in the buffer</summary>
    public T Item181;

    /// <summary>A slot in the buffer</summary>
    public T Item182;

    /// <summary>A slot in the buffer</summary>
    public T Item183;

    /// <summary>A slot in the buffer</summary>
    public T Item184;

    /// <summary>A slot in the buffer</summary>
    public T Item185;

    /// <summary>A slot in the buffer</summary>
    public T Item186;

    /// <summary>A slot in the buffer</summary>
    public T Item187;

    /// <summary>A slot in the buffer</summary>
    public T Item188;

    /// <summary>A slot in the buffer</summary>
    public T Item189;

    /// <summary>A slot in the buffer</summary>
    public T Item190;

    /// <summary>A slot in the buffer</summary>
    public T Item191;

    /// <summary>A slot in the buffer</summary>
    public T Item192;

    /// <summary>A slot in the buffer</summary>
    public T Item193;

    /// <summary>A slot in the buffer</summary>
    public T Item194;

    /// <summary>A slot in the buffer</summary>
    public T Item195;

    /// <summary>A slot in the buffer</summary>
    public T Item196;

    /// <summary>A slot in the buffer</summary>
    public T Item197;

    /// <summary>A slot in the buffer</summary>
    public T Item198;

    /// <summary>A slot in the buffer</summary>
    public T Item199;

    /// <summary>A slot in the buffer</summary>
    public T Item200;

    /// <summary>A slot in the buffer</summary>
    public T Item201;

    /// <summary>A slot in the buffer</summary>
    public T Item202;

    /// <summary>A slot in the buffer</summary>
    public T Item203;

    /// <summary>A slot in the buffer</summary>
    public T Item204;

    /// <summary>A slot in the buffer</summary>
    public T Item205;

    /// <summary>A slot in the buffer</summary>
    public T Item206;

    /// <summary>A slot in the buffer</summary>
    public T Item207;

    /// <summary>A slot in the buffer</summary>
    public T Item208;

    /// <summary>A slot in the buffer</summary>
    public T Item209;

    /// <summary>A slot in the buffer</summary>
    public T Item210;

    /// <summary>A slot in the buffer</summary>
    public T Item211;

    /// <summary>A slot in the buffer</summary>
    public T Item212;

    /// <summary>A slot in the buffer</summary>
    public T Item213;

    /// <summary>A slot in the buffer</summary>
    public T Item214;

    /// <summary>A slot in the buffer</summary>
    public T Item215;

    /// <summary>A slot in the buffer</summary>
    public T Item216;

    /// <summary>A slot in the buffer</summary>
    public T Item217;

    /// <summary>A slot in the buffer</summary>
    public T Item218;

    /// <summary>A slot in the buffer</summary>
    public T Item219;

    /// <summary>A slot in the buffer</summary>
    public T Item220;

    /// <summary>A slot in the buffer</summary>
    public T Item221;

    /// <summary>A slot in the buffer</summary>
    public T Item222;

    /// <summary>A slot in the buffer</summary>
    public T Item223;

    /// <summary>A slot in the buffer</summary>
    public T Item224;

    /// <summary>A slot in the buffer</summary>
    public T Item225;

    /// <summary>A slot in the buffer</summary>
    public T Item226;

    /// <summary>A slot in the buffer</summary>
    public T Item227;

    /// <summary>A slot in the buffer</summary>
    public T Item228;

    /// <summary>A slot in the buffer</summary>
    public T Item229;

    /// <summary>A slot in the buffer</summary>
    public T Item230;

    /// <summary>A slot in the buffer</summary>
    public T Item231;

    /// <summary>A slot in the buffer</summary>
    public T Item232;

    /// <summary>A slot in the buffer</summary>
    public T Item233;

    /// <summary>A slot in the buffer</summary>
    public T Item234;

    /// <summary>A slot in the buffer</summary>
    public T Item235;

    /// <summary>A slot in the buffer</summary>
    public T Item236;

    /// <summary>A slot in the buffer</summary>
    public T Item237;

    /// <summary>A slot in the buffer</summary>
    public T Item238;

    /// <summary>A slot in the buffer</summary>
    public T Item239;

    /// <summary>A slot in the buffer</summary>
    public T Item240;

    /// <summary>A slot in the buffer</summary>
    public T Item241;

    /// <summary>A slot in the buffer</summary>
    public T Item242;

    /// <summary>A slot in the buffer</summary>
    public T Item243;

    /// <summary>A slot in the buffer</summary>
    public T Item244;

    /// <summary>A slot in the buffer</summary>
    public T Item245;

    /// <summary>A slot in the buffer</summary>
    public T Item246;

    /// <summary>A slot in the buffer</summary>
    public T Item247;

    /// <summary>A slot in the buffer</summary>
    public T Item248;

    /// <summary>A slot in the buffer</summary>
    public T Item249;

    /// <summary>A slot in the buffer</summary>
    public T Item250;

    /// <summary>A slot in the buffer</summary>
    public T Item251;

    /// <summary>A slot in the buffer</summary>
    public T Item252;

    /// <summary>A slot in the buffer</summary>
    public T Item253;

    /// <summary>A slot in the buffer</summary>
    public T Item254;

    /// <summary>A slot in the buffer</summary>
    public T Item255;

    /// <summary>A slot in the buffer</summary>
    public T Item256;

    /// <summary>
    /// Gets or sets the element at offset <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index</param>
    /// <exception cref="ArgumentOutOfRangeException">The index was outside the bounds of the buffer</exception>
    /// <returns>The element at offset <paramref name="index"/>.</returns>
    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= 256)
            {
                throw new ArgumentOutOfRangeException (nameof (index));
            }
            return Unsafe.Add(ref Item1, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (index < 0 || index >= 256)
            {
                throw new ArgumentOutOfRangeException (nameof (index));
            }
            Unsafe.Add(ref Item1, index) = value;
        }
    }

    /// <summary>
    /// Returns a <see cref="Span{T}"/> representing the buffer.
    ///
    /// This method is <strong>unsafe</strong>.
    /// You must ensure the <see cref="Span{T}"/> does not outlive the buffer itself.
    /// </summary>
    /// <returns>A <see cref="Span{T}"/> representing the buffer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref Item1, 256);

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> representing the buffer.
    ///
    /// This method is <strong>unsafe</strong>.
    /// You must ensure the <see cref="ReadOnlySpan{T}"/> does not outlive the buffer itself.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> representing the buffer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsReadOnlySpan() => MemoryMarshal.CreateReadOnlySpan(ref Item1, 256);

    /// <summary>
    /// Call this method when you've finished using the buffer.
    ///
    /// Technically this method is a no-op, but calling it ensures that the
    /// buffer is not deallocated before you've finished working with it.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Dispose() { }
}
