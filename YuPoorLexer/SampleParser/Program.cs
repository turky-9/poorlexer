using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuPoorLexer;
using System.IO;

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
                //while (true)
                //{
                //    string buff = Console.ReadLine();
                //    if (buff == "exit")
                //        break;

                //    SampleParser p = new SampleParser(new PoorLexer(buff));
                //    Console.WriteLine("result:" + p.Parse());
                //}

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
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().Name);
                Environment.Exit(1);
            }
        }
    }
}
