using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuPoorLexer
{
    /// <summary>
    /// トークンを表すクラス。イミュータブル。
    /// </summary>
    public class Token
    {
        /// <summary>
        /// トークンタイプ
        /// </summary>
        private ETokenType _Type;
        public ETokenType Type
        {
            get { return this._Type; }
            private set { this._Type = value; }
        }

        /// <summary>
        /// トークンそのもの
        /// </summary>
        private string _Text;
        public string Text
        {
            get { return this._Text; }
            private set { this._Text = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type"></param>
        /// <param name="txt"></param>
        public Token(ETokenType type, string txt)
        {
            this.Type = type;
            this.Text = txt;
        }

        /// <summary>
        /// このトークンの文字列表現
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "<'" + this.Text + "', '" + this.Type.ToString() + "'>";
        }
    }
}
