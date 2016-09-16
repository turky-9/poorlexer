using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuPoorLexer
{
    /// <summary>
    /// 文法をチェックするParseの為の基底クラス。
    /// ちゃんと実装するならばASTを返すようにした方が良いよ。
    /// そして、ASTを評価するEVALを実装する。
    /// EVALはSymbolTableとか色々・・・
    /// </summary>
    public class AbstractParser
    {
        protected AbstractLexer _Lex;
        public AbstractLexer Lex
        {
            get { return this._Lex; }
            protected set { this._Lex = value; }
        }

        protected Token _CurrToken;
        public Token CurrToken
        {
            get { return this._CurrToken; }
            protected set { this._CurrToken = value; }
        }

        public AbstractParser(AbstractLexer lex)
        {
            this.Lex = lex;
            this.CurrToken = this.Lex.NextToken();
        }

        /// <summary>
        /// マッチしていればトークンを消費する
        /// </summary>
        /// <param name="type"></param>
        protected void Match(ETokenType type)
        {
            if (this.CurrToken.Type == type)
                this.Consume();
            else
                throw new UnExceptedTokenException();
        }

        /// <summary>
        /// トークンを消費する
        /// </summary>
        protected void Consume()
        {
            this.CurrToken = this.Lex.NextToken();
        }

        /// <summary>
        /// NumberならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token Number()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.Number);
            return ret;
        }

        /// <summary>
        /// SymbolならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token Symbol()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.Symbol);
            return ret;
        }

        /// <summary>
        /// StringLiteralならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token StringLiteral()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.String);
            return ret;
        }

        /// <summary>
        /// OpAddならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token OpAdd()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.OpAdd);
            return ret;
        }

        /// <summary>
        /// OpMinusならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token OpMinus()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.OpMinus);
            return ret;
        }

        /// <summary>
        /// OpMultiならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token OpMulti()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.OpMulti);
            return ret;
        }

        /// <summary>
        /// OpDivならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token OpDiv()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.OpDiv);
            return ret;
        }

        /// <summary>
        /// SemiColonならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token SemiColon()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.SemiColon);
            return ret;
        }

        /// <summary>
        /// EqualならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token Equal()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.Equal);
            return ret;
        }

        /// <summary>
        /// LBraceならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token LBrace()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.LBrace);
            return ret;
        }

        /// <summary>
        /// RBraceならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token RBrace()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.RBrace);
            return ret;
        }

        /// <summary>
        /// LParenならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token LParen()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.LParen);
            return ret;
        }

        /// <summary>
        /// RParenならTokenを返す。そうでなければ例外を発生させる。
        /// </summary>
        /// <returns></returns>
        protected Token RParen()
        {
            Token ret = this.CurrToken;
            this.Match(ETokenType.RParen);
            return ret;
        }
    }
}
