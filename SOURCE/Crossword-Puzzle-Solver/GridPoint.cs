using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ANDREICSLIB.ClassExtras;

namespace Crossword_Puzzle_Solver
{
    public class SolvedWord
    {
        public int X;
        public int Y;
        public bool Across;
        public string Word;
        public int SetChars = 0;
        public SolvedWord(int x,int y,bool across,string word,int setChars)
        {
            SetChars = setChars;
            X = x;
            Y = y;
            Across = across;
            Word = word;
        }
    }

    [Serializable]
    public class GridPointLight
    {
        public static GridPointLight[][] Clone(GridPointLight[][] gp, int width, int height)
        {
            var ret = ArrayExtras.InstantiateArray<GridPointLight>(gp[0].Count(), gp.Count());

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ret[y][x] = new GridPointLight(gp[y][x].Enabled, gp[y][x].C);
                }
            }
            return ret;
        }

        public bool Enabled = true;
        public char C;

        public GridPointLight(bool enabled,char c)
        {
            Enabled = enabled;
            C = c;
        }

        public GridPointLight()
        {
            
        }
    }

    public class GridPoint
    {
        public TextBox Tb;
        public bool Enabled = true;
        public char C;
        public int X;
        public int Y;

        public GridPoint()
        {

        }

        public GridPoint(char c, TextBox tb, int x, int y)
        {
            X = x;
            Y = y;
            Tb = tb;
            C = c;
        }

        public GridPoint(TextBox tb, int x, int y)
        {
            Tb = tb;
            X = x;
            Y = y;
        }

        public static GridPointLight[][] ToLight(GridPoint[][] gp,int width,int height)
        {
            var ret=ArrayExtras.InstantiateArray<GridPointLight>(gp[0].Count(), gp.Count());

            for(int y=0;y<height;y++)
            {
                for (int x=0;x<width;x++)
                {
                    ret[y][x] = new GridPointLight(gp[y][x].Enabled, gp[y][x].C);
                }
            }
            return ret;
        }
    }
}
