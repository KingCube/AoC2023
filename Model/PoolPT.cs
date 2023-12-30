using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Dynamic;

public class PoolPT
{
    ConcurrentStack<PathTracker> available = new ConcurrentStack<PathTracker>();

    public PoolPT()
    {
        for(int i = 0; i < 10000; i++)
            available.Push(new PathTracker());
    }

    public PathTracker GetItem()
    {
        if(available.Count == 0)
            return new PathTracker();
        else
        {
            PathTracker pt;
            available.TryPop(out pt);
            if(pt != null) return pt;
            return new PathTracker();
        }
    }

    public void RetireItem(PathTracker item)
    {
        item.traversed.Clear();
        available.Push(item);
    }

}

