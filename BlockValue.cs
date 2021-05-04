using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris201770001
{
    class BlockValue
    {

        public bool[,,,] blockShape = new bool[7, 4, 4, 4]
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
