using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuPoorLexer
{
    /// <summary>
    /// トークンタイプ
    /// </summary>
    public enum ETokenType
    {
        Eof = -1,
        SemiColon = 0,

        Symbol = 1,

        OpAdd = 50,
        OpMinus = 51,
        OpMulti = 52,
        OpDiv = 53,

        Equal = 54,

        Number = 100,
        String = 101,

        LBrace = 110,
        RBrace = 111,
        LParen = 112,
        RParen = 113,
    }
}
