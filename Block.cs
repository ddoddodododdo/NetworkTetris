﻿using System;
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
        public int blockNum;
        private int bagIndex;
        private int[] bag7 = new int[7];

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
            blockNum = GetBag7();
        }

        public int GetBag7()
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
                num = rand.Next() % bag7.Length;
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
        public void MoveTurn()
        {
            turn = (turn + 1) % 4;
        }
        public void MoveReturn()
        {
            if (turn == 0) turn = 3;
            else turn--;
        }
    }
}
