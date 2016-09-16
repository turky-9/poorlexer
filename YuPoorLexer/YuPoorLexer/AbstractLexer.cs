using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuPoorLexer
{
    public abstract class AbstractLexer
    {
        protected string _Src;
        public string Src
        {
            get { return this._Src; }
            protected set { this._Src = value; }
        }

        protected int _Idx;
        public int Idx
        {
            get { return this._Idx; }
            protected set { this._Idx = value; }
        }

        public char CurrChar
        {
            get { return this.Idx == this.Src.Length ? '\0' : this.Src[this.Idx]; }
        }

        public AbstractLexer(string src)
        {
            this.Src = src;
            this.Idx = 0;
        }


        /// <summary>
        /// アルファベット？
        /// </summary>
        /// <returns></returns>
        public bool IsLetter()
        {
            char c = this.CurrChar;
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        /// <summary>
        /// 数値？
        /// </summary>
        /// <returns></returns>
        public bool IsNumeric()
        {
            char c = this.CurrChar;
            return (c >= '0' && c <= '9');
        }

        /// <summary>
        /// ダブルクォート
        /// </summary>
        /// <returns></returns>
        public bool IsDoubleQuote()
        {
            char c = this.CurrChar;
            return c == '"';
        }


        /// <summary>
        /// １文字消費する
        /// </summary>
        public void Consume()
        {
            this.Idx++;
        }

        /// <summary>
        /// ホワイトスペースなら１文字消費する
        /// </summary>
        public void WhiteSpace()
        {
            char c = CurrChar;
            if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
                this.Consume();
        }

        public abstract Token NextToken();
    }
}
