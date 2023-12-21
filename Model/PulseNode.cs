using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PulseNode
{
    public string type;
    public string label;

    public List<PulseNode> parents = new List<PulseNode>();
    public List<PulseNode> children = new List<PulseNode>();

    public bool lastPulse = false;
    public bool on = false;

    public List<(PulseNode, bool)> ProcessPulse(bool isHigh)
    {
        List < (PulseNode, bool) > retList = new List<(PulseNode, bool) >();
        if (type == "%")
        {
            if (!isHigh)
            {
                on = !on;
                foreach(PulseNode p in children)
                {
                    retList.Add((p, on));
                }
                lastPulse = on;
            }
        } 
        else if(type == "&")
        {
            bool allParents = true;
            foreach(PulseNode p in parents) 
            {
                if (!p.lastPulse)
                {
                    allParents = false;
                    break;
                }
            }

            foreach (PulseNode p in children)
            {
                retList.Add((p, !allParents));
            }
            lastPulse = !allParents;
        }
        else if(type == "b")
        {
            foreach (PulseNode p in children)
            {
                retList.Add((p, isHigh));
            }
            lastPulse = isHigh;
        }

        return retList;
    }

}

