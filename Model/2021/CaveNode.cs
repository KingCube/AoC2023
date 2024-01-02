using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023.Model._2021
{
    public class CaveNode
    {
        public List<CaveNode> conns = new List<CaveNode>();

        public string id;

        public bool isLarge = false;

        public CaveNode(string id)
        {
            this.id = id;
            isLarge = id == id.ToUpper();
        }
    }
}
