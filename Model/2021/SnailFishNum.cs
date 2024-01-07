using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023.Model._2021
{
    public class SnailFishNum
    {
        public enum LR{
            Left, 
            Right
            };

        public long numLeft;
        public long numRight;

        public SnailFishNum parent;
        public SnailFishNum left;
        public SnailFishNum right;

        public List<SnailFishNum> allNums;

        public int level;

        public SnailFishNum(List<SnailFishNum> allNums, SnailFishNum parent)
        {
            this.allNums = allNums;
            this.parent = parent;
            this.level = parent.level + 1;
        }

        public SnailFishNum GetChild(LR type) => type == LR.Left ? left : right;
        public bool isNum(LR type) => type == LR.Left ? left == null : right == null;

        public bool Explode()
        {
            if (level != 5) return false;

            bool childIsLeft = parent.left == this;

            if (childIsLeft)
            {
                parent.left = null;
                parent.numLeft = 0;

                int idx = allNums.IndexOf(this);

                
            }



            return true;
        }

   
        void AddNumToCertainVal(LR type, long num)
        {
            if (type == LR.Left)
                numLeft += num;
            else
                numRight += num;
        }

    }
}
