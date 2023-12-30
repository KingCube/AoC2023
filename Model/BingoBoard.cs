using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class BingoBoard
{
    int[,] nums = new int[5,5];
    bool[,] marked = new bool[5,5];

    public bool HasWon = false;

    public void FillLine(List<int> data, int row)
    {
        for(int i = 0; i < data.Count; i++)
        {
            nums[row,i] = data[i];
        }

    }

    public int MarkNumber(int num)
    {
        bool foundMatch = false;
        int row = -1;
        int col = -1;

        for(int i = 0; i < nums.GetLength(0); i++)
        {
            if (foundMatch) break;
            for(int j = 0; j <  nums.GetLength(1); j++)
            {
                if (nums[i,j] == num) {  
                    foundMatch = true;
                    marked[i,j] = true;
                    row = i; col = j;
                    break; 
                }
            }
        }

        if (!foundMatch) return 0;

        //Check if we won row
        bool foundWin = true;
        for (int j = 0; j < nums.GetLength(1); j++)
        {
            foundWin = marked[row, j];
            if (!foundWin) break;
            if (j == nums.GetLength(1) - 1 && foundWin) return CalcScore(num);
        }

        // col
        for (int j = 0; j < nums.GetLength(0); j++)
        {
            foundWin = marked[j, col];
            if (!foundWin) break;
            if (j == nums.GetLength(1) - 1 && foundWin) return CalcScore(num);
        }

        /*
        if(row == col)
        {
            for(int i = 0; i < nums.GetLength(0); i++)
            {
                foundWin = marked[i, i];
                if (!foundWin) break;
                if (i == nums.GetLength(1) - 1 && foundWin) return CalcScore(num);
            }
        }

        if(row + col == nums.GetLength(1) - 1)
        {
            for (int i = 0; i < nums.GetLength(0); i++)
            {
                foundWin = marked[nums.GetLength(1)-1-i, i];
                if (!foundWin) break;
                if (i == nums.GetLength(1) - 1 && foundWin) return CalcScore(num);
            }
        }
        */
        return 0;
    }

    public int CalcScore(int latest)
    {
        int sum = 0;
        for (int i = 0; i < nums.GetLength(0); i++)
            for (int j = 0; j < nums.GetLength(1); j++)
                if (!marked[i, j]) sum += nums[i, j];

        HasWon = true;

        return sum * latest;
         
    }

}

