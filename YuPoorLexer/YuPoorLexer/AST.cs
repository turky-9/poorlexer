using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuPoorLexer
{
    /// <summary>
    /// Compsiteパターンですよ
    /// </summary>
    public interface AST
    {
        Token Token { get; }
    }

    /// <summary>
    /// 葉っぱ
    /// </summary>
    public class ASTLeaf : AST
    {
        private Token _Token;
        public Token Token
        {
            get { return this._Token; }
            private set { this._Token = value; }
        }

        public ASTLeaf(Token t)
        {
            this.Token = t;
        }
    }

    /// <summary>
    /// 節
    /// </summary>
    public class ASTNode : AST
    {
        private Token _Token;
        public Token Token
        {
            get { return this._Token; }
            private set { this._Token = value; }
        }

        private List<AST> _Children;
        public List<AST> Children
        {
            get { return this._Children; }
            private set { this._Children = value; }
        }

        public ASTNode(Token t)
        {
            this.Token = t;
            this.Children = new List<AST>();
        }
    }
}
