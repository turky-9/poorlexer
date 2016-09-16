using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuPoorLexer;

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
    }



    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    string buff = Console.ReadLine();
                    if (buff == "exit")
                        break;

                    SampleParser p = new SampleParser(new PoorLexer(buff));
                    Console.WriteLine("result:" + p.Parse());
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().Name);
                Console.ReadLine();
            }
        }
    }
}
