using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tetris201770001
{
    class Game
    {
        public const int BX = 12;  //board x 게임 가로 넓이
        public const int BY = 25;
        public const int CELL_SIZE = 20; // 네모칸 크기
        public const int OPTION_SIZE_X = 5;

        public int gameScore;
        public int holdBlockNum;
        
        public Block now = new Block();
        public Block[] nextBlocks = new Block[3];

        public bool[,] gameBoard = new bool[BY, BX];
        public int[,] gameColorBoard = new int[BY, BX];

        static private Random rand = new Random();

        public Game()
        {
            Reset();
        }

        public void Reset()
        {
            now.NewBlock();
            for (int i = 0; i < nextBlocks.Length; i++) nextBlocks[i] = new Block();
            for (int i = 0; i < nextBlocks.Length; i++) SetNewBlock();
            for (int i = 0; i < BY; i++)
                for (int j = 0; j < BX; j++)
                    gameBoard[i, j] = false;
            gameScore = 0;
            holdBlockNum = -1;
        }

        public bool CheckGameOver()
        {
            now.MoveReturn();
            if (!CanTurn()) // 게임오버
            {
                return true;
            }
            return false;
        }


        #region Move()
        public void MoveDrop()
        {
            while (MoveDown()) ;
        }
        public void MoveTurn()
        {
            if(!CanTurn())
                now.MoveReturn();
        }
        public void MoveLeft()
        {
            if (CanLeft())
            {
                now.MoveLeft();
            }
        }
        public void MoveRight()
        {
            if (CanRight())
            {
                now.MoveRight();
            }
        }
        public bool MoveDown()
        {
            if (CanDown())  
            {
                now.MoveDown();
                return true;
            }

            for (int yy = 0; yy < 4; yy++) //안내려가질때 현재 자리에 블럭 채우기
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    if (Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                    {
                        gameBoard[now.y + yy, now.x + xx] = true;
                        gameColorBoard[now.y + yy, now.x + xx] = now.shape;
                    }
                }
            }
            gameScore += 4;
            CheckFullLine();
            SetNewBlock(); // 땅에 닿으면 새로운블럭 호출
            return false;
        }
        #endregion

        #region Can()
        private bool CanTurn()
        {
            now.MoveTurn();
            for (int yy = 0; yy < 4; yy++)
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    if (Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                    {
                        
                        if ((now.x + xx < 0) || (now.x + xx >= BX) || (now.y + yy >= BY) || gameBoard[now.y + yy, now.x + xx])
                        {

                            if (CanLeft())
                            {
                                now.MoveLeft();
                                return true;
                            }
                            if (now.shape == 0 && now.x + 1 < 0)
                            {
                                now.MoveRight();
                                if (CanRight())
                                {
                                    now.MoveRight();
                                    return true;
                                }
                                now.MoveLeft();
                            }
                            else if (CanRight())
                            {
                                now.MoveRight();
                                return true;
                            }
                            
                            if (CanUp())
                            {
                                return true;
                            }
                            if (CanUp())
                            {
                                return true;
                            }now.y += 2;
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private bool CanLeft()
        {
            for (int yy = 0; yy < 4; yy++)
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    if (Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                    {
                        if (now.x + xx - 1 < 0|| (now.y + yy >= BY) || gameBoard[now.y + yy, now.x + xx - 1]) return false;
                    }
                }
            }
            return true;
        }
        private bool CanRight()
        {
            for (int yy = 0; yy < 4; yy++)
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    if (Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                    {
                        if (now.x + xx + 1 >= BX || (now.y + yy >= BY) || gameBoard[now.y + yy, now.x + xx + 1]) return false;
                    }
                }
            }
            return true;
        }
        private bool CanDown()
        {
            for (int yy = 0; yy < 4; yy++)
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    if(Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                    {
                        if ((now.y + yy + 1 > BY - 1) || gameBoard[now.y + yy + 1, now.x + xx])  return false;
                    }
                }
            }

            return true;
        }
        private bool CanUp()
        {
            now.y--;
            for (int yy = 0; yy < 4; yy++)
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    if (Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                    {
                        if (now.x + xx < 0 || now.x + xx >= BX || now.y + yy - 1 < 0 || (now.y + yy >= BY) 
                            || gameBoard[now.y + yy, now.x + xx]) return false;
                    }
                }
            }
            return true;
        }
        #endregion

        public void UseHold()
        {
            gameScore = (gameScore >= 100) ? gameScore - 100 : 0;

            if (holdBlockNum == -1)
            {
                holdBlockNum = now.shape;
                SetNewBlock();
                return;
            }
            int temp = holdBlockNum;
            holdBlockNum = now.shape;
            now.NewBlock();
            now.shape = temp;

            return;
        }
        public void SetNewBlock()
        {
            now.NewBlock();
            now.shape = nextBlocks[0].shape;
            nextBlocks[0].shape = nextBlocks[1].shape;
            nextBlocks[1].shape = nextBlocks[2].shape;
            nextBlocks[2].NewBlock();
        }
        public int GetFloorDistance()
        {
            int dis = 0;
            while (++dis < BY)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        if (Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                        {
                            if (now.y + yy < 0) return 0;
                            if ((now.y + yy + dis > BY - 1) || gameBoard[now.y + yy + dis, now.x + xx]) return dis - 1;
                        }
                    }
                }
            }
            return dis - 1;
        }
        public bool CheckFullLine()
        {
            bool checker = false;
            int point = BX;
            for(int yy = 0; yy < BY; yy++)
            {
                for(int xx = 0; xx < BX; xx++)
                {
                    if (!gameBoard[yy, xx]) break;
                    if (xx == BX - 1)
                    {
                        EraseBlock(yy);
                        gameScore += point *= 2;

                        checker = true;
                    }
                }
            }
            return checker;
        }
        public void EraseBlock(int yy)
        {
            for (; yy >= 0; yy--)
            {
                for (int xx = 0; xx < BX; xx++)
                {
                    if (yy == 0)
                    {
                        gameBoard[yy, xx] = false;
                    }
                    else
                    {
                        gameBoard[yy, xx] = gameBoard[yy - 1, xx];
                        gameColorBoard[yy, xx] = gameColorBoard[yy - 1, xx];
                    }
                }
            }
        }
        public bool BlockCheck(int yy, int xx, int block)
        {
            switch (block) {
                case -1: return Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx];
                case 0: return Block.BLOCK_SHAPE[nextBlocks[0].shape, 0, yy, xx];
                case 1: return Block.BLOCK_SHAPE[nextBlocks[1].shape, 0, yy, xx];
                case 2: return Block.BLOCK_SHAPE[nextBlocks[2].shape, 0, yy, xx];
                case 3: return Block.BLOCK_SHAPE[holdBlockNum, 0, yy, xx];
                default: return false;
            }
        }
        public Rectangle GetRect(int yy, int xx)
        {
            return new Rectangle((now.x + xx) * CELL_SIZE, (now.y + yy) * CELL_SIZE, CELL_SIZE, CELL_SIZE);
        }

        public void Attacked()
        {
            int toothless = rand.Next(BX);

           for(int yy = 0; yy < BY; yy++)
           {
                for(int xx = 0; xx < BX; xx++)
                {
                    if (yy == BY - 1)
                    {
                        if (xx == toothless)
                        {
                            gameBoard[yy, xx] = false;
                            continue;
                        }
                        gameBoard[yy, xx] = true;
                        gameColorBoard[yy, xx] = 7;
                    }
                    else
                    {
                        gameBoard[yy, xx] = gameBoard[yy + 1, xx];
                        gameColorBoard[yy, xx] = gameColorBoard[yy + 1, xx];
                    }
                }
           }
        }
    }
}
