using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuParseCombi
{
    /// <summary>
    /// パーサのソース
    /// </summary>
    public interface IYuContext
    {
        /// <summary>
        /// パースするテキスト
        /// </summary>
        string Source { get; }

        /// <summary>
        /// テキストのポジション
        /// </summary>
        int Position { get; }

        /// <summary>
        /// 最後までパースしたかどうか
        /// </summary>
        bool IsEol { get; }

        /// <summary>
        /// ポジションを変更せず1文字読む
        /// </summary>
        /// <returns></returns>
        char Peek();

        /// <summary>
        /// ポジションを変更して1文字読む
        /// </summary>
        /// <returns></returns>
        IYuContext Consume();
    }

    /// <summary>
    /// パーサのソースの実装
    /// </summary>
    public class YuContext : IYuContext
    {
        /// <summary>
        /// パースするテキスト
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// テキストのポジション
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// 最後までパースしたかどうか
        /// </summary>
        public bool IsEol
        {
            get
            {
                if (this.Position >= this.Length)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// ポジションを変更せず1文字読む
        /// </summary>
        /// <returns></returns>
        public char Peek()
        {
            if (this.IsEol)
                throw new Exception("source is eol");

            return this.Source[this.Position];
        }

        /// <summary>
        /// ポジションを変更して1文字読む
        /// </summary>
        /// <returns></returns>
        public IYuContext Consume()
        {
            if (this.IsEol)
                throw new Exception("source is eol");

            //char ret = this.Peek();
            //this.Position++;
            //return ret;

            return new YuContext(this.Source, this.Position + 1, this.Length);
        }


        /// <summary>
        /// ソースの長さ
        /// </summary>
        private int Length { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="src"></param>
        public YuContext(string src)
        {
            this.Source = src;
            this.Position = 0;
            this.Length = src.Length;
        }

        private YuContext(string src, int pos, int len)
        {
            this.Source = src;
            this.Position = pos;
            this.Length = len;
        }
    }


}
