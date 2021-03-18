﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftTokenKind.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Token kind.
    /// </summary>
    public enum PftTokenKind
    {
        /// <summary>
        /// No tokens.
        /// </summary>
        None,

        /// <summary>
        /// a(v200)
        /// </summary>
        A,

        /// <summary>
        /// abs
        /// </summary>
        Abs,

        /// <summary>
        /// all
        /// </summary>
        All,

        /// <summary>
        /// @
        /// </summary>
        At,

        /// <summary>
        /// and
        /// </summary>
        And,

        /// <summary>
        /// any
        /// </summary>
        Any,

        /// <summary>
        /// \
        /// </summary>
        Backslash,

        /// <summary>
        /// !
        /// </summary>
        Bang,

        /// <summary>
        /// blank
        /// </summary>
        Blank,

        /// <summary>
        /// break
        /// </summary>
        Break,

        /// <summary>
        /// c10
        /// </summary>
        C,

        /// <summary>
        /// ceil
        /// </summary>
        Ceil,

        /// <summary>
        /// :
        /// </summary>
        Colon,

        /// <summary>
        /// ,
        /// </summary>
        Comma,

        /// <summary>
        /// /* comment
        /// </summary>
        Comment,

        /// <summary>
        /// 'literal'
        /// </summary>
        ConditionalLiteral,

        /// <summary>
        /// cseval()
        /// </summary>
        CsEval,

        /// <summary>
        /// div
        /// </summary>
        Div,

        /// <summary>
        /// do
        /// </summary>
        Do,

        /// <summary>
        /// ]]]
        /// </summary>
        EatClose,

        /// <summary>
        /// [[[
        /// </summary>
        EatOpen,

        /// <summary>
        /// else
        /// </summary>
        Else,

        /// <summary>
        /// empty
        /// </summary>
        Empty,

        /// <summary>
        /// end
        /// </summary>
        End,

        /// <summary>
        /// =
        /// </summary>
        Equals,

        /// <summary>
        /// eval()
        /// </summary>
        Eval,

        /// <summary>
        /// f(123)
        /// </summary>
        F,

        /// <summary>
        /// f2(123)
        /// </summary>
        Fmt,

        /// <summary>
        /// false
        /// </summary>
        False,

        /// <summary>
        /// fi
        /// </summary>
        Fi,

        /// <summary>
        /// first
        /// </summary>
        First,

        /// <summary>
        /// floor
        /// </summary>
        Floor,

        /// <summary>
        /// for
        /// </summary>
        For,

        /// <summary>
        /// foreach
        /// </summary>
        ForEach,

        /// <summary>
        /// frac
        /// </summary>
        Frac,

        /// <summary>
        /// from
        /// </summary>
        From,

        /// <summary>
        /// global
        /// </summary>
        Global,

        /// <summary>
        /// `
        /// </summary>
        GraveAccent,

        /// <summary>
        /// #
        /// </summary>
        Hash,

        /// <summary>
        /// ^
        /// </summary>
        Hat,

        /// <summary>
        /// have
        /// </summary>
        Have,

        /// <summary>
        /// identifier
        /// </summary>
        Identifier,

        /// <summary>
        /// if
        /// </summary>
        If,

        /// <summary>
        /// in
        /// </summary>
        In,

        /// <summary>
        /// l(format)
        /// </summary>
        L,

        /// <summary>
        /// last
        /// </summary>
        Last,

        /// <summary>
        /// {
        /// </summary>
        LeftCurly,

        /// <summary>
        /// (
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// [
        /// </summary>
        LeftSquare,

        /// <summary>
        /// &lt;
        /// </summary>
        Less,

        /// <summary>
        /// &lt;=
        /// </summary>
        LessEqual,

        /// <summary>
        /// local
        /// </summary>
        Local,

        /// <summary>
        /// mfn
        /// </summary>
        Mfn,

        /// <summary>
        /// mpl, mhl etc
        /// </summary>
        Mpl,

        /// <summary>
        /// -
        /// </summary>
        Minus,

        /// <summary>
        /// &gt;
        /// </summary>
        More,

        /// <summary>
        /// &gt;=
        /// </summary>
        MoreEqual,

        /// <summary>
        /// nl
        /// </summary>
        Nl,

        /// <summary>
        /// not
        /// </summary>
        Not,

        /// <summary>
        /// &lt;&gt;
        /// </summary>
        NotEqual1,

        /// <summary>
        /// !=
        /// </summary>
        NotEqual2,

        /// <summary>
        /// 123.45
        /// </summary>
        Number,

        /// <summary>
        /// or
        /// </summary>
        Or,

        /// <summary>
        /// order
        /// </summary>
        Order,

        /// <summary>
        /// p(v200)
        /// </summary>
        P,

        /// <summary>
        /// parallel
        /// </summary>
        Parallel,

        /// <summary>
        /// %
        /// </summary>
        Percent,

        /// <summary>
        /// +
        /// </summary>
        Plus,

        /// <summary>
        /// pow
        /// </summary>
        Pow,

        /// <summary>
        /// proc
        /// </summary>
        Proc,

        /// <summary>
        /// ?
        /// </summary>
        Question,

        /// <summary>
        /// ravr
        /// </summary>
        Ravr,

        /// <summary>
        /// |literal|
        /// </summary>
        RepeatableLiteral,

        /// <summary>
        /// }
        /// </summary>
        RightCurly,

        /// <summary>
        /// ]
        /// </summary>
        RightSquare,

        /// <summary>
        /// )
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// ref
        /// </summary>
        Ref,

        /// <summary>
        /// rmax
        /// </summary>
        Rmax,

        /// <summary>
        /// rmin
        /// </summary>
        Rmin,

        /// <summary>
        /// round
        /// </summary>
        Round,

        /// <summary>
        /// rsum
        /// </summary>
        Rsum,

        /// <summary>
        /// s(format)
        /// </summary>
        S,

        /// <summary>
        /// select
        /// </summary>
        Select,

        /// <summary>
        /// ;
        /// </summary>
        Semicolon,

        /// <summary>
        /// sign
        /// </summary>
        Sign,

        /// <summary>
        /// /
        /// </summary>
        Slash,

        /// <summary>
        /// *
        /// </summary>
        Star,

        /// <summary>
        /// then
        /// </summary>
        Then,

        /// <summary>
        /// true
        /// </summary>
        True,

        /// <summary>
        /// trunc
        /// </summary>
        Trunc,

        /// <summary>
        /// ~
        /// </summary>
        Tilda,

        /// <summary>
        /// {{{
        /// </summary>
        TripleCurly,

        /// <summary>
        /// &lt;&lt;&lt;
        /// </summary>
        TripleLess,

        /// <summary>
        /// _
        /// </summary>
        Underscore,

        /// <summary>
        /// &amp;uf('0')
        /// </summary>
        Unifor,

        /// <summary>
        /// 'literal'
        /// </summary>
        UnconditionalLiteral,

        /// <summary>
        /// v200
        /// </summary>
        V,

        /// <summary>
        /// val
        /// </summary>
        Val,

        /// <summary>
        /// $variable
        /// </summary>
        Variable,

        /// <summary>
        /// where
        /// </summary>
        Where,

        /// <summary>
        /// while
        /// </summary>
        While,

        /// <summary>
        /// with
        /// </summary>
        With,

        /// <summary>
        /// x10
        /// </summary>
        X
    }
}
