using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuParseCombi
{
    /// <summary>
    /// パーサの結果
    /// </summary>
    public interface IYuResult
    {
        /// <summary>
        /// 成否
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// 結果のオブジェクト
        /// </summary>
        string Result { get; }

        /// <summary>
        /// 結果のオブジェクト
        /// </summary>
        List<string> Parts { get; }

        /// <summary>
        /// 残りのソース
        /// </summary>
        IYuContext Context { get; }
    }

    /// <summary>
    /// パーサの結果の実装
    /// </summary>
    public class YuResult : IYuResult
    {
        /// <summary>
        /// 成否
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 結果のオブジェクト
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// 結果のオブジェクト
        /// </summary>
        public List<string> Parts { get; private set; }

        /// <summary>
        /// 残りのソース
        /// </summary>
        public IYuContext Context { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="succ"></param>
        /// <param name="res"></param>
        /// <param name="src"></param>
        public YuResult(bool succ, string res, IYuContext src)
        {
            this.IsSuccess = succ;
            this.Result = res;
            this.Parts = new List<string>();
            this.Context = src;
        }
    }
}
