﻿using System;
using System.Collections.Generic;

namespace JogoDaVelha
{
    class Board
    {
        private int[,] cell;
        //0 é vazio, 1 é computador e 2 é humano

        public Board()
        {
            cell = new int[3, 3];
        }

        public void Limpar()
        {
            for (int y = 0; y < GetComprimentoArray(); y++)
            {
                for (int x = 0; x < GetAlturaArray(); x++)
                {
                    //cell[x, y] = 0;
                    ZeraValorCelula(x,y);
                }
            }
        }

        public int[,] GetCell()
        {
            return cell;
        }

        public void SetCell(int[,] cellA)
        {
            cell = cellA;
        }


        public void AcrescentaValor(int x, int y)
        {
            AtribuiValorCelula(x, y, RetornaValorCelula(x, y) + 1);
            //cell[x, y] += 1;
        }

        public void ZeraValorCelula(int x, int y)
        {
            cell[x, y] = 0;
        }

        public void AtribuiValorCelula(int x, int y, int val)
        {
            cell[x, y] = val;
        }

        public int RetornaValorCelula(int x, int y)
        {
            return cell[x, y];
        }

        public int GetAlturaArray()
        {
            return cell.GetLength(0);
        }

        public int GetComprimentoArray()
        {
            return cell.GetLength(1);
        }

        public int GetVencedor()
        {
            int vencedor = -1;

            for (int player = 1; player <= 2; player++)
            {
                for (int y = 0; y < cell.GetLength(1); y++) // linhas
                {
                    if (cell[0, y] == player && cell[1, y] == player && cell[2, y] == player)
                    {
                        vencedor = player;
                    }
                }

                for (int x = 0; x < cell.GetLength(0); x++) // colunas
                {
                    if (cell[x, 0] == player && cell[x, 1] == player && cell[x, 2] == player)
                    {
                        vencedor = player;
                    }
                }

                {//Diagonal
                    if (cell[0, 0] == player && cell[1, 1] == player && cell[2, 2] == player)
                    {
                        vencedor = player;
                    }

                    if (cell[0, 2] == player && cell[1, 1] == player && cell[2, 0] == player)
                    {
                        vencedor = player;
                    }
                }
            }
            if (vencedor == -1)
            {
                vencedor = VerificaEmpate();
            }
            return vencedor;
        }

        private int VerificaEmpate()
        {
            for (int y = 0; y < GetComprimentoArray(); y++)
            {
                for (int x = 0; x < GetAlturaArray(); x++)
                {
                    if (RetornaValorCelula(x,y) == 0)
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }

        static public int Minimax(Board board, int profundidade, int jogador)
        {
            if (board.EhFimDeJogo() || profundidade == 0)
            {
                return board.CalcularScore();
            }

            int valor;

            if (jogador == 2) // humano
            {
                valor = 9999999;
                List<Board> possibilidades = board.GetPossibilidades(2);
                foreach (Board p in possibilidades)
                {
                    valor = Math.Min(valor, Minimax(p, profundidade - 1, 1 /* Computador */));
                }
            }
            else // computador
            {
                valor = -9999999;
                List<Board> possibilidades = board.GetPossibilidades(1);
                foreach (Board p in possibilidades)
                {
                    valor = Math.Max(valor, Minimax(p, profundidade - 1, 2 /* Humano */));
                }
            }

            return valor;
        }

        public bool EhFimDeJogo()
        {
            if (GetVencedor() > 0)
            {
                return true;
            }
            for (int y = 0; y < GetComprimentoArray(); y++)
            {
                for (int x = 0; x < GetAlturaArray(); x++)
                {
                    if (RetornaValorCelula(x, y) == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int CalcularScore()
        {
            if (GetVencedor() == 1)
            {
                return 1;
            }
            else if (GetVencedor() == 2)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public Board GetCopia()
        {
            Board minhaCopia = new Board();

            for (int y = 0; y < GetComprimentoArray(); y++)
            {
                for (int x = 0; x < GetAlturaArray(); x++)
                {
                    //minhaCopia.cell[x, y] = cell[x, y];
                    minhaCopia.AtribuiValorCelula(x, y, RetornaValorCelula(x,y));
                }
            }

            return minhaCopia;
        }

        public List<Board> GetPossibilidades(int jogador)
        {
            List<Board> resultado = new List<Board>();

            for (int y = 0; y < GetComprimentoArray(); y++)
            {
                for (int x = 0; x < GetAlturaArray(); x++)
                {
                    Board minhaCopia = GetCopia();
                    //minhaCopia.cell[x, y] = jogador;
                    minhaCopia.AtribuiValorCelula(x, y, jogador);
                    resultado.Add(minhaCopia);
                }
            }

            return resultado;
        }
    }
}