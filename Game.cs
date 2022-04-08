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
            now.ResetBlock();
            for (int i = 0; i < nextBlocks.Length; i++) nextBlocks[i] = new Block();
            for (int i = 0; i < nextBlocks.Length; i++) SetNewBlock();
            for (int i = 0; i < BY; i++) for (int j = 0; j < BX; j++) gameBoard[i, j] = false;
            gameScore = 0;
            holdBlockNum = -1;
        }

        public bool CheckGameOver()
        {
            return !CanHere();
        }


        #region Move()
        private bool CanHere()
        {
            int blockX;
            int blockY;
            for (int yy = 0; yy < 4; yy++)
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    if (Block.BLOCK_SHAPE[now.shape, now.turn, yy, xx])
                    {
                        blockX = now.x + xx;
                        blockY = now.y + yy;
                        if (blockX < 0 || BX <= blockX || blockY < 0 || BY <= blockY || gameBoard[blockY, blockX]) return false;
                    }
                }
            }
            return true;
        }

        public bool MoveLeft()
        {
            now.MoveLeft();
            if (CanHere()) return true;

            now.MoveRight();
            return false;
        }

        public bool MoveRight()
        {
            now.MoveRight();
            if (CanHere()) return true;

            now.MoveLeft();
            return false;
        }

        public bool MoveUp()
        {
            now.MoveUp();
            if (CanHere()) return true;

            now.MoveDown();
            return false;
        }
        public void MoveTurn()
        {
            now.MoveTurn();
            if (CanHere()) return;
            if (MoveLeft()) return;
            if (MoveRight()) return;
            if (MoveUp()) return;
            now.MoveReturn();
        }
        
        public bool MoveDown()
        {
            now.MoveDown();
            if (CanHere()) return true;
            now.MoveUp();

            //땅에 닿으면
            FillLandBlock();
            gameScore += 4;
            CheckFullLine();
            SetNewBlock(); 
            return false;
        }

        public void MoveDrop()
        {
            while (MoveDown()) ;
        }

        #endregion

        private void FillLandBlock()
        {
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
        }

        public void UseHold()
        {
            gameScore = (gameScore >= 100) ? gameScore - 100 : 0;

            if (holdBlockNum == -1) // Hold 해준 블럭이 없을 경우
            {
                holdBlockNum = now.shape;
                SetNewBlock();
                return;
            }
            int temp = holdBlockNum;
            holdBlockNum = now.shape;
            now.ResetBlock();
            now.shape = temp;

            return;
        }

        public void SetNewBlock()
        {
            now.ResetBlock();
            now.shape = nextBlocks[0].shape;
            nextBlocks[0].shape = nextBlocks[1].shape;
            nextBlocks[1].shape = nextBlocks[2].shape;
            nextBlocks[2].ResetBlock();
        }

        public int GetFloorDistance()
        {
            int dis = 0;
            while (++dis < BY)
            {
                now.y++;
                if (!CanHere())
                {
                    now.y -= dis;
                    return dis - 1;
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
