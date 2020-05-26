using System.Collections;
using System.Collections.Generic;
using System;

namespace Test{

	public class Node
	{
		private int _value;
		private Node _left;
		private Node _right;
	
		public int Value 
		{ 
			get {return _value;} 
			set {_value = value;} 
		}
		
		public Node Left
		{
			get {return _left;} 
			set {_left = value;}
		}
		public Node Right 
		{ 
			get {return _right;}
			set {_right = value;}
		}
	
		public Node(int value = 0, Node left = null, Node right = null)
		{
			this._value = value;
			this._left = left;
			this._right = right;
		}
	}

	public class Tree
	{
		private Node Root;
		public Tree()
		{
			this.Root = null;
		}

		public Node root
		{
			get{return Root;}
			set{Root = value;}
		}

		//Trocar
		public void Insert(int value)
		{
			this.Root = Add(this.Root, new Node(value));
		}

		private Node Add(Node root, Node node)
		{
			if(root == null)
			{
				return node;
			}
			if(node.Value < root.Value)
			{
				root.Left = Add(root.Left, node);
			}
			else
			{
				root.Right = Add(root.Right, node);
			}
			return root;
		}



		public Node Remove(Node parent, int value)
		{
			if(parent == null)
				return null;
			if(value < parent.Value)
			{
				parent.Left = Remove(parent.Left, value);
				return parent;
			}
			else if(value > parent.Value)
			{
				parent.Right = Remove(parent.Right, value);
				return parent;
			}
			else if(parent.Left == null)
			{
				return parent.Right;
			}
			else if(parent.Right == null)
			{
				return parent.Left;	
			}
			else 
			{
				Remover_sucessor(parent);
				return parent;
			}
		}

		public void Remover_sucessor(Node parent) {
			Node pai = parent; 
			Node t = parent.Right;
			while (t.Left != null)
			{
				pai = t;
				t = t.Left;
			}
			if (pai.Left == t)
			{
				pai.Left = t.Right;
			}
			else
			{
				pai.Right = t.Left;
			}
			parent.Value = t.Value;
		}


		public void imprimir(Node parent)
		{
			if (parent == null)
			{
				return;
			}
			Console.WriteLine(parent.Value);
			imprimir(parent.Left);
			//Console.WriteLine(parent.Value);
			imprimir(parent.Right);	
			//Console.WriteLine(parent.Value);
			return;
		}

		// A partir desse ponto estao as funcoes referentes ao balanceamento
		// Como visto no video indicado pelo link junto a essa atividade, temos 4 tipos de rotacoes:
		// Rotacoes a Direita e Esquerda
		// Rotacoes Dupla a Esquerda e Direita

	    public Node RotacaoDD(Node pai)
        {
            Node pivo = pai.Right;
            pai.Right = pivo.Left;
            pivo.Left = pai;
            return pivo;
        }
        public Node RotacaoEE(Node pai)
        {
            Node pivo = pai.Left;
            pai.Left = pivo.Right;
            pivo.Right = pai;
            return pivo;
        }
        public Node RotacaoED(Node pai)
        {
            Node pivo = pai.Left;
            pai.Left = RotacaoDD(pivo);
            return RotacaoEE(pai);
        }
        public Node RotacaoDE(Node pai)
        {
            Node pivo = pai.Right;
            pai.Right = RotacaoEE(pivo);
            return RotacaoDD(pai);
        }

        

        // Funcao que calcula o Fator de Balanceamento de acordo com o no passado
        public int FatorDeBalanceamento(Node atual)
        {
            int esq = CalculaNivel(atual.Left);
            int dir = CalculaNivel(atual.Right);
            int fatorB = esq - dir;
            return fatorB;
        }

        // Funcao que contabiliza os niveis das sub arvores para o fator de balanceamento
        public int CalculaNivel(Node atual)
        {
            int nivel = 0;
            if (atual != null)
            {
                int esq = CalculaNivel(atual.Left);
                int dir = CalculaNivel(atual.Right);
                int m = max(esq, dir);
                nivel = m + 1;
            }
            return nivel;
        }

        // Funcao que retorna o maior valor dentre duas subArvores
        public int max(int esq, int dir)
        {
            return esq > dir ? esq : dir;
        }

        // Funcao para balancear a arvore , que chama as rotacoes de acordo com as seguintes condicoes:
        // Se o fator estiver no intervalo -1 <= fator <= 1 --> a arvore esta balanceada
        // Caso contrario, verificamos o fator para:
        // Se fator > 1
        // 		Se subArvore direita tem fator < 0
        //			Rotacao Dupla a esquerda
        //		Caso contrario
        //			Rotacao a Esquerda
        // Caso contrario (isto eh, fator < -1 )
        //		Se subArvore esquerda tem fator > 0
        //			Rotacao Dupla a direita
        //		Caso contrario
        //			Rotacao a Direita
		
        public Node BalancearArvore(Node atual)
        {
            int fatorB = FatorDeBalanceamento(atual);
            if (fatorB > 1)
            {
                if (FatorDeBalanceamento(atual.Left) > 0)
                {
                    atual = RotacaoEE(atual);
                }
                else
                {
                    atual = RotacaoED(atual);
                }
            }
            else if (fatorB < -1)
            {
                if (FatorDeBalanceamento(atual.Right) > 0)
                {
                    atual = RotacaoDE(atual);
                }
                else
                {
                    atual = RotacaoDD(atual);
                }
            }
            return atual;
        }
		
		public Node BuscaEBalanceia(Node raiz)
		{
			if(raiz == null)
			{
				return raiz;
			}
			raiz.Left = BuscaEBalanceia(raiz.Left);
			raiz = BalancearArvore(raiz);
			raiz.Right = BuscaEBalanceia(raiz.Right);
			raiz = BalancearArvore(raiz);
			return raiz;
		}
	}

	public class Program
	{
		public static void Main()
		{
			Tree myTree = new Tree();
			Console.WriteLine(myTree);
			myTree.Insert(1);
			myTree.Insert(2);
			myTree.Insert(3);
			myTree.Insert(4);
			myTree.Insert(5);
			myTree.Insert(6);
			myTree.Insert(7);
			myTree.Insert(8);
			myTree.imprimir(myTree.root);
			Console.WriteLine();
			myTree.root = myTree.BuscaEBalanceia(myTree.root);
			myTree.imprimir(myTree.root);
			Console.WriteLine();
		}
	}
}