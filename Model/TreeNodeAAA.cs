using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TreeNodeAAA
{
    public string id;

    public TreeNodeAAA left;
    public TreeNodeAAA right;

    public TreeNodeAAA GetNode(char dir)
    {
        if (dir == 'L')
            return left;
        else
        {
            return right;
        }
    }

}

