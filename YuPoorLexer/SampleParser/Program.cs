using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuPoorLexer;
using System.IO;
using System.Text.RegularExpressions;
using YuParseCombi;

namespace SampleParser
{
    /// <summary>
    /// パースしたついでに評価もする。
    /// </summary>
    public class SampleParser : AbstractParser
    {
        public SampleParser(AbstractLexer lex)
            : base(lex)
        {
        }

        public int Parse()
        {
            Token l = this.Number();
            Token op = this.OpAdd();
            Token r = this.Number();

            return int.Parse(l.Text) + int.Parse(r.Text);
        }

        /// <summary>
        /// stat = defnode | flow
        /// defnode = symbol, '=' ,symbol ,string
        /// flow = '(', symbol, edge, {edge}, ')'
        /// edge = [string] symbol 
        /// </summary>
        /// <param name="tw"></param>
        /// <returns></returns>
        public string Parse2()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("digraph dfdgraph{" + Environment.NewLine);
            sb.Append("\tgraph[" + Environment.NewLine);
            sb.Append("\t\trankdir = LR;" + Environment.NewLine);
            sb.Append("\t]" + Environment.NewLine);

            while (this.CurrToken.Type != ETokenType.Eof)
            {
                if (this.CurrToken.Type == ETokenType.Symbol)
                    sb.Append(this.DefNode());
                else
                    sb.Append(this.Flow());
            }

            sb.Append("}" + Environment.NewLine);

            return sb.ToString();
        }

        private string DefNode()
        {
            StringBuilder sb = new StringBuilder();

            Token name = this.Symbol();
            this.Equal();
            Token shape = this.Symbol();
            Token label = this.StringLiteral();

            sb.Append("\t" + name.Text + "[");
            sb.Append("shape = ");
            switch(shape.Text.ToUpper())
            {
                case "DS":
                    sb.Append("doublecircle");
                    break;
                case "F":
                    sb.Append("note");
                    break;
                default:
                    sb.Append("ellipse");
                    break;
            }
            sb.Append("; label = \"");
            sb.Append(label.Text);
            sb.Append("\"]" + Environment.NewLine);
            return sb.ToString();
        }

        private string Flow()
        {
            StringBuilder sb = new StringBuilder();

            this.LParen();

            string oldNodeName = null;
            Token start = this.Symbol();

            sb.Append("\t" + start.Text);
            sb.Append(this.Edge(ref oldNodeName));

            while (true)
            {
                Token t = this.CurrToken;
                if(t.Type == ETokenType.RParen)
                {
                    this.RParen();
                    break;
                }

                if (!string.IsNullOrEmpty(oldNodeName))
                    sb.Append("\t" + oldNodeName);

                sb.Append(this.Edge(ref oldNodeName));
            }

            return sb.ToString();
        }

        private string Edge(ref string oldNodeName)
        {
            StringBuilder sb = new StringBuilder();

            string edgeLabel = string.Empty;
            if (this.CurrToken.Type == ETokenType.String)
            {
                Token l = this.StringLiteral();
                edgeLabel = "[label = \"" + l.Text + "\"];";
            }

            Token n = this.Symbol();
            sb.Append(" -> " + n.Text + edgeLabel + Environment.NewLine);
            oldNodeName = n.Text;

            return sb.ToString();
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //パーサコンビネータfrom
                string txt = "hogefoo";
                var p = YuParseCombi.PerserFactory.GetPerserToken("hoge");

                var ctx = new YuParseCombi.YuContext(txt);
                var result = p(ctx);


                var p1 = YuParseCombi.PerserFactory.GetPerserChar('h');
                var p2 = YuParseCombi.PerserFactory.GetPerserChar('o');
                var p3 = YuParseCombi.PerserFactory.And(p1, p2);
                
                ctx = new YuParseCombi.YuContext(txt);
                result = p3(ctx);

                var p4 = YuParseCombi.PerserFactory.GetPerserChar('g');
                var p5 = YuParseCombi.PerserFactory.GetPerserChar('e');
                var p6 = YuParseCombi.PerserFactory.Concat(p1, p2, p4, p5);

                ctx = new YuParseCombi.YuContext(txt);
                result = p6(ctx);

                var pmw = YuParseCombi.PerserFactory.GetPerserManyWhileSpaceCRLF();
                ctx = new YuParseCombi.YuContext("  \t\r\r\nhoge");
                result = pmw(ctx);


                string hoge = "\"a\r\nb\\c\\\"d\"";
                Console.WriteLine(hoge);
                var p8 = YuParseCombi.PerserFactory.GetPerserLiteralString();
                ctx = new YuParseCombi.YuContext(hoge);
                result = p8(ctx);
                if (result.IsSuccess)
                    Console.WriteLine(result.Result);

                Match m = Regex.Match("aBd", "[a-z|A-Z]*");

                hoge = "112239";
                var p9 = YuParseCombi.PerserFactory.GetPerserPosiInt();
                ctx = new YuParseCombi.YuContext(hoge);
                result = p9(ctx);

                hoge = "11119";
                var p10 = YuParseCombi.PerserFactory.GetPerserChar('1');
                YuPerser p11 = null;
                YuPerser p12 = PerserFactory.Lazy(() => p11);
                p11 = PerserFactory.Option(PerserFactory.Concat(p10, p12));
                ctx = new YuParseCombi.YuContext(hoge);
                result = p11(ctx);


                var pdgit = PerserFactory.Many1(PerserFactory.GetPerserDigit());
                var pplus = PerserFactory.GetPerserChar('+');
                var pspace = PerserFactory.Many( PerserFactory.GetPerserWhileSpace());
                var pexp = PerserFactory.Concat(pspace, pdgit, pspace, pplus, pspace, pdgit);

                while (true)
                {
                    string bff = Console.ReadLine();
                    if (bff.ToUpper() == "EXIT")
                        break;

                    var ret = pexp(new YuContext(bff));
                    if (ret.IsSuccess == true)
                    {
                        Console.WriteLine(ret.Result);
                        foreach (var s in ret.Parts)
                            Console.WriteLine("[" + s + "]");
                    }
                    else
                    {
                        Console.WriteLine("#FAILED#");
                    }
                }

                Console.Write("hit any key...");
                Console.ReadLine();
                Environment.Exit(0);
                //パーサコンビネータend








                //while (true)
                //{
                //    string buff = Console.ReadLine();
                //    if (buff == "exit")
                //        break;

                //    SampleParser p = new SampleParser(new PoorLexer(buff));
                //    Console.WriteLine("result:" + p.Parse());
                //}

                /*
                StringBuilder buff = new StringBuilder();
                while (true)
                {
                    string tmp = Console.ReadLine();
                    if (tmp == null)
                        break;
                    buff.Append(tmp + Environment.NewLine);
                }

                SampleParser p = new SampleParser(new PoorLexer(buff.ToString()));
                Console.Out.WriteLine(p.Parse2());
                */

                //入力フェーズ
                StringBuilder buff = new StringBuilder();
                while (true)
                {
                    string tmp = Console.ReadLine();
                    if (tmp == null)
                        break;
                    buff.Append(tmp + Environment.NewLine);
                }

                //字句解析フェーズ
                PoorLexer plex = new PoorLexer(buff.ToString());
                Token token = plex.NextToken();
                while (token.Type != ETokenType.Eof)
                {
                    Console.Out.WriteLine(token.ToString());
                    token = plex.NextToken();
                }
                Console.ReadLine();
                    
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().Name);
                Environment.Exit(1);
            }
        }
    }
}
