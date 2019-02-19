using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuParseCombi
{
    public partial class PerserFactory
    {
        /// <summary>
        /// 何でも良いので1文字読む
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserAnyChar()
        {
            return (ctx) =>
            {
                if (ctx.IsEol)
                {
                    return new YuResult(false, null, ctx);
                }

                var ret = ctx.Consume();
                return new YuResult(true, new string(new char[] { ret.Source[ret.Position] }), ctx);
            };
        }

        /// <summary>
        /// セミコロン
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserSemiColon()
        {
            return GetPerserChar(';');
        }

        /// <summary>
        /// コロン
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserColon()
        {
            return GetPerserChar(':');
        }

        /// <summary>
        /// ピリオド
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserPeriod()
        {
            return GetPerserChar('.');
        }

        /// <summary>
        /// コンマ
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserComma()
        {
            return GetPerserChar(',');
        }

        /// <summary>
        /// 左括弧'('
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserLeftParen()
        {
            return GetPerserChar('(');
        }

        /// <summary>
        /// 右括弧')'
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserRightParen()
        {
            return GetPerserChar(')');
        }

        /// <summary>
        /// 左中括弧'{'
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserLeftBrace()
        {
            return GetPerserChar('{');
        }

        /// <summary>
        /// 右中括弧'}'
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetParserRightBrace()
        {
            return GetPerserChar('}');
        }

        /// <summary>
        /// 半角スペース
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserSpace()
        {
            return GetPerserChar(' ');
        }
        /// <summary>
        /// タブ
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserTab()
        {
            return GetPerserChar('\t');
        }
        /// <summary>
        /// CR
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserCR()
        {
            return GetPerserChar('\r');
        }
        /// <summary>
        /// LF
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserLF()
        {
            return GetPerserChar('\n');
        }

        /// <summary>
        /// スペース or タブを読む
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserWhileSpace()
        {
            return Or(GetPerserSpace(), GetPerserTab());
        }

        /// <summary>
        /// スペース or タブ or CR or LFを読む
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserWhileSpaceCRLF()
        {
            return Or(GetPerserSpace(), GetPerserTab(), GetPerserCR(), GetPerserLF());
        }

        /// <summary>
        /// スペース or タブを0個以上読む
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserManyWhileSpace()
        {
            return Many(GetPerserWhileSpace());
        }

        /// <summary>
        /// スペース or タブ or CR or LFを0個以上読む
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserManyWhileSpaceCRLF()
        {
            return Many(GetPerserWhileSpaceCRLF());
        }

        /// <summary>
        /// 数字
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserDigit()
        {
            string reg = @"[0-9]";
            return GetPerserRegexp(reg);
        }

        /// <summary>
        /// アルファベット
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserAlphabet()
        {
            string reg = @"[a-z|A-Z]";
            return GetPerserRegexp(reg);
        }

        /// <summary>
        /// 文字列リテラル
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserLiteralString()
        {
            string reg = @"""([^\\""]|\\.)*""";
            return GetPerserRegexp(reg);
        }

        /// <summary>
        /// 正の整数
        /// </summary>
        /// <returns></returns>
        public static YuPerser GetPerserPosiInt()
        {
            return Many1(GetPerserDigit());
        }
    }
}
