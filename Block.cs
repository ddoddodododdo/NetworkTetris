using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Tetris201770001
{
    class Block
    {
        public int x;
        public int y;
        public int turn;
        public int shape;
        private int bagIndex;
        private int[] bag7 = new int[7];

        //public static readonly bool[,,,] BLOCK_SHAPE; 아래에 있음
        static Random rand = new Random();

        public Block()
        {
            ResetBag();
            NewBlock();
        }

        public void NewBlock()
        {
            x = Game.BX/2 - 2;
            y = 0;
            turn = 0;
            shape = GetBlockShape();
        }

        private int GetBlockShape()
        {
            if (bagIndex > bag7.Length - 1) {
                bagIndex = 0;
                ResetBag();
            }
            return bag7[bagIndex++];
        }

        public void ResetBag()
        {
            int num, temp;
            for (int i = 0; i < bag7.Length; i++) bag7[i] = i; //블럭 넣고
            for(int i = 0; i < bag7.Length; i++)               //섞기
            {
                num = rand.Next(bag7.Length);
                temp = bag7[i];
                bag7[i] = bag7[num];
                bag7[num] = temp;
            }
        }

        public void MoveLeft()
        {
            x--;
        }
        public void MoveRight()
        {
            x++;
        }

        public void MoveDown()
        {
            y++;
        }

        public void MoveUp()
        {
            y--;
        }

        public void MoveTurn()
        {
            turn = (turn + 1) % 4;
        }
        public void MoveReturn()
        {
            if (turn == 0) turn = 3;
            else turn--;
        }

        public static readonly bool[,,,] BLOCK_SHAPE = new bool[7, 4, 4, 4]
        {
            {               //ㅁㅁㅁㅁ
                {
                    {false, false, true, false },
                    {false, false, true, false },
                    {false, false, true, false },
                    {false, false, true, false }
                },
                {
                    {false, false, false, false },
                    {true,  true,  true,  true },
                    {false, false, false, false },
                    {false, false, false, false }
                },
                {
                    {false, false, true, false },
                    {false, false, true, false },
                    {false, false, true, false },
                    {false, false, true, false }
                },
                {
                    {false, false, false, false },
                    {true,  true,  true,  true },
                    {false, false, false, false },
                    {false, false, false, false }
                }
            },


            {             //ㅁㅁ                           
                {        // ㅁㅁ
                    {false, false, false, false },
                    {false, true,  true,  false },
                    {false, true,  true,  false },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, true,  true,  false },
                    {false, true,  true,  false },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, true,  true,  false },
                    {false, true,  true,  false },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, true,  true,  false },
                    {false, true,  true,  false },
                    {false, false, false, false }
                }
            },


            {            //ㅁ
                {        //ㅁㅁㅁ
                    {false, false, false, false },
                    {false, true,  false, false },
                    {false, true,  true,  true  },
                    {false, false, false, false }
                },
                {
                    {false, true,  true,  false },
                    {false, true,  false, false },
                    {false, true,  false, false },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, true,  true,  true },
                    {false, false, false, true },
                    {false, false, false, false }
                },
                {
                    {false, false, false, true },
                    {false, false, false, true },
                    {false, false, true,  true },
                    {false, false, false, false }
                }
            },


            {         //    ㅁ
                {     //ㅁㅁㅁ
                    {false, false, false, false },
                    {false, false, false, true },
                    {false, true, true, true },
                    {false, false, false, false }
                },
                {
                    {false, false, true, false },
                    {false, false, true, false },
                    {false, false, true, true },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, true, true, true },
                    {false, true, false, false },
                    {false, false, false, false }
                },
                {
                    {false, true, true, false },
                    {false, false, true, false },
                    {false, false, true, false },
                    {false, false, false, false }
                }
            },

                         //   ㅁ
            {            // ㅁㅁ
                {        // ㅁ
                    {false, false, true, false },
                    {false, true, true, false },
                    {false, true, false, false },
                    {false, false, false, false }
                },
                {
                    {false, true,  true,  false},
                    {false, false, true,  true },
                    {false, false, false, false},
                    {false, false, false, false}
                },
                {
                    {false, false, true, false },
                    {false, true, true, false },
                    {false, true, false, false },
                    {false, false, false, false }
                },
                {
                    {false, true,  true,  false},
                    {false, false, true,  true },
                    {false, false, false, false},
                    {false, false, false, false}
                }
            },

                        //ㅁ
            {           //ㅁㅁ
                {       //  ㅁ
                    {false, true, false, false },
                    {false, true, true, false },
                    {false, false, true, false },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, false, true, true },
                    {false, true, true, false },
                    {false, false, false, false }
                },
                {
                    {false, true, false, false },
                    {false, true, true, false },
                    {false, false, true, false },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, false, true, true },
                    {false, true, true, false },
                    {false, false, false, false }
                }
            },


            {           //  ㅁ
                {       //ㅁㅁㅁ
                    {false, false, true, false },
                    {false, true, true, true },
                    {false, false, false, false },
                    {false, false, false, false }
                },
                {
                    {false, false, true, false },
                    {false, false, true, true },
                    {false, false, true, false },
                    {false, false, false, false }
                },
                {
                    {false, false, false, false },
                    {false, true, true, true },
                    {false, false, true, false },
                    {false, false, false, false }
                },
                {
                    {false, false, true, false },
                    {false, true, true, false },
                    {false, false, true, false },
                    {false, false, false, false }
                }
            }

        };

    }
}
