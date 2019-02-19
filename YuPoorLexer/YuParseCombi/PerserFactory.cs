using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YuParseCombi
{
    /// <summary>
    /// パーサを表わす関数をデリゲートとして定義しておく
    /// </summary>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public delegate IYuResult YuPerser(IYuContext ctx);

    /// <summary>
    /// 必要な関数をまとめたクラス
    /// </summary>
    public partial class PerserFactory
    {
        #region perser generator
        /// <summary>
        /// 指定した文字を読むパーサ(関数)を返す
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static YuPerser GetPerserChar(char c)
        {
            return (ctx) =>
            {
                if (ctx.IsEol)
                {
                    return new YuResult(false, null, ctx);
                }

                char ret = ctx.Peek();
                if (ret == c)
                {
                    return new YuResult(true, new string(new char[] { ret }), ctx.Consume());
                }

                return new YuResult(false, null, ctx);
            };
        }


        /// <summary>
        /// 指定した文字列を読む
        /// </summary>
        /// <param name="tok"></param>
        /// <returns></returns>
        public static YuPerser GetPerserToken(string tok)
        {
            return (ctx) =>
            {
                if (ctx.IsEol)
                {
                    return new YuResult(false, null, ctx);
                }
                if (ctx.Source.Substring(ctx.Position, tok.Length) == tok)
                {
                    IYuContext ret = null;
                    for (int i = 0; i < tok.Length; i++)
                        ret = ctx.Consume();
                    return new YuResult(true, tok, ret);
                }
                return new YuResult(false, null, ctx);
            };
        }

        /// <summary>
        /// 正規表現
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public static YuPerser GetPerserRegexp(string reg)
        {
            return (ctx) =>
            {
                if (ctx.IsEol)
                {
                    return new YuResult(false, null, ctx);
                }

                var match = Regex.Match(ctx.Source.Substring(ctx.Position), reg);
                if (match.Success == true && match.Index == 0)
                {
                    IYuContext ret = null;
                    string val = match.Value;
                    for (int i = 0; i < val.Length; i++)
                        ret = ctx.Consume();
                    return new YuResult(true, val, ret);
                }

                return new YuResult(false, null, ctx);
            };

        }

        #endregion


        #region parser combinater
        /// <summary>
        /// 複数のパーサを連続するパーサを返す
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static YuPerser Concat(params YuPerser[] ps)
        {
            return (ctx) =>
            {
                IYuContext ret = ctx;
                StringBuilder result = new StringBuilder();
                List<string> parts = new List<string>();
                foreach (YuPerser p in ps)
                {
                    var r = p(ret);
                    if (r.IsSuccess == false)
                    {
                        IYuResult y = new YuResult(false, null, ctx);
                        return y;
                    }

                    result.Append(r.Result);
                    parts.Add(r.Result);
                    ret = r.Context;
                }

                IYuResult x = new YuResult(true, result.ToString(), ret);
                foreach (var s in parts)
                    x.Parts.Add(s);

                return x;
            };
        }

        /// <summary>
        /// 2個のパーサを連続するパーサを返す
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static YuPerser And(YuPerser p1, YuPerser p2)
        {
            return (ctx) =>
            {
                var r1 = p1(ctx);
                if (r1.IsSuccess == false)
                    return r1;

                var r2 = p2(r1.Context);
                if (r2.IsSuccess == false)
                {
                    IYuResult y = new YuResult(false, null, ctx);
                    return y;
                }

                IYuResult x = new YuResult(true, r1.Result + r2.Result, r2.Context);
                x.Parts.Add(r1.Result);
                x.Parts.Add(r2.Result);

                return x;
            };
        }

        /// <summary>
        /// 2つのパーサの内、どちらかが成功すれば良いパーサを返す
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static YuPerser Or(params YuPerser[] ps)
        {
            return (ctx) =>
            {
                foreach (YuPerser p in ps)
                {
                    var r = p(ctx);
                    if (r.IsSuccess == true)
                        return r;
                }

                return new YuResult(false, null, ctx);
            };
        }

        /// <summary>
        /// あってもなくても良いパーサを返す
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static YuPerser Option(YuPerser p)
        {
            return (ctx) =>
            {
                var r = p(ctx);
                if (r.IsSuccess == true)
                    return r;

                return new YuResult(true, null, ctx);
            };
        }

        /// <summary>
        /// 0個以上連続して読むパーサを返す
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static YuPerser Many(YuPerser p)
        {
            return (ctx) =>
            {
                IYuContext ret = ctx;
                StringBuilder result = new StringBuilder();
                List<string> parts = new List<string>();

                while (true)
                {
                    var r = p(ret);
                    if (r.IsSuccess == false)
                        break;

                    result.Append(r.Result);
                    parts.Add(r.Result);
                    ret = r.Context;
                }

                IYuResult x = new YuResult(true, result.ToString(), ret);
                foreach (var s in parts)
                    x.Parts.Add(s);
                return x;
            };
        }

        /// <summary>
        /// 1個以上連続して読むパーサを返す
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static YuPerser Many1(YuPerser p)
        {
            return (ctx) =>
            {
                IYuContext ret = ctx;
                List<string> parts = new List<string>();

                StringBuilder result = new StringBuilder();
                bool isFirst = true;


                while (true)
                {
                    var r = p(ret);
                    if (r.IsSuccess == false)
                        break;

                    result.Append(r.Result);
                    parts.Add(r.Result);
                    isFirst = false;

                    ret = r.Context;
                }

                if(isFirst == true)
                    return new YuResult(false, null, ctx);

                IYuResult x = new YuResult(true, result.ToString(), ret);
                foreach (var s in parts)
                    x.Parts.Add(s);

                return x;
            };
        }

        /// <summary>
        /// 遅延評価
        /// パーサを返す関数を受け取り
        /// 実行時にパーサを取得し、実行する。
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static YuPerser Lazy(Func<YuPerser> f)
        {
            YuPerser p = null;
            return (ctx) =>
            {
                if (p == null)
                    p = f();
                return p(ctx);
            };
        }
        #endregion

    }
}
