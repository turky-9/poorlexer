using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuPoorLexer
{
    /// <summary>
    /// 単純なLexer。
    /// </summary>
    public class PoorLexer : AbstractLexer
    {
        public PoorLexer(string s)
            : base(s)
        {
        }

        public override Token NextToken()
        {
            while (this.Idx < this.Src.Length)
            {
                switch (this.Src[this.Idx])
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        WhiteSpace();
                        continue;

                    case '+':
                        this.Consume();
                        return new Token(ETokenType.OpAdd, "+");

                    case '-':
                        this.Consume();
                        return new Token(ETokenType.OpMinus, "-");

                    case '*':
                        this.Consume();
                        return new Token(ETokenType.OpMulti, "*");

                    case '/':
                        this.Consume();
                        return new Token(ETokenType.OpDiv, "/");

                    case ';':
                        this.Consume();
                        return new Token(ETokenType.SemiColon, ";");

                    case '=':
                        this.Consume();
                        return new Token(ETokenType.Equal, "=");

                    case '{':
                        this.Consume();
                        return new Token(ETokenType.LBrace, "{");

                    case '}':
                        this.Consume();
                        return new Token(ETokenType.RBrace, "}");

                    case '(':
                        this.Consume();
                        return new Token(ETokenType.LParen, "(");

                    case ')':
                        this.Consume();
                        return new Token(ETokenType.RParen, ")");

                    default:
                        //整数を返えす
                        if (this.IsNumeric())
                            return this.Number();

                        //シンボルを返えす
                        if (this.IsLetter())
                            return this.Symbol();

                        //文字列を返えす
                        if (this.IsDoubleQuote())
                            return this.StringLiteral();

                        throw new UnKnownTokenException();
                }
            }

            return new Token(ETokenType.Eof, "<EOF>");
        }


        /// <summary>
        /// 整数（負号なし）
        /// </summary>
        /// <returns></returns>
        public Token Number()
        {
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(this.CurrChar);
                this.Consume();
            } while (this.IsNumeric());

            return new Token(ETokenType.Number, sb.ToString());
        }

        /// <summary>
        /// シンボル（アルファベットのみ）
        /// </summary>
        /// <returns></returns>
        public Token Symbol()
        {
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(this.CurrChar);
                this.Consume();
            } while (this.IsLetter());

            return new Token(ETokenType.Symbol, sb.ToString());
        }

        /// <summary>
        /// 文字列（ダブルクォートでくくられ、バックスラッシュのエスケープ）
        /// </summary>
        /// <returns></returns>
        public Token StringLiteral()
        {
            char oldchar = '\0';
            this.Consume();
            StringBuilder sb = new StringBuilder();

            while (!this.IsDoubleQuote() || oldchar == '\\')
            {
                if (this.CurrChar == '\0')
                    throw new BadStringLiteralException();

                sb.Append(this.CurrChar);
                oldchar = this.CurrChar;
                this.Consume();
            }
            this.Consume();


            return new Token(ETokenType.String, sb.ToString());
        }
    }
}
