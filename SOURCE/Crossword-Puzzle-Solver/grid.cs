using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ANDREICSLIB;
using ANDREICSLIB.ClassExtras;

namespace Crossword_Puzzle_Solver
{
    public static class Grid
    {
        public static char defaultChar = '\0';
        public static char DisabledChar = '\b';
        public static char BlankChar = '\f';

        public static Form1 Baseform;
        private static int width;
        private static int height;
        /// <summary>
        /// a textbox/char for each grid point
        /// </summary>
        private static GridPoint[][] grid;
        private static PanelReplacement pu;
        private static readonly Color NormalBack = Color.White;
        private static readonly Color NormalFront = Color.Black;
        private static readonly Color DisabledBack = Color.Black;
        private static bool AllowWordsOnlyOnce;
        

        public static Dictionary<int, List<string>> words = new Dictionary<int, List<string>>();

        public static void LoadWords(string filename)
        {
            var filetext = FileExtras.LoadFile(filename);
            if (string.IsNullOrEmpty(filetext))
                return;

            words = new Dictionary<int, List<string>>();

            var wordssplit = filetext.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var w in wordssplit)
            {
                var w2 = w.ToUpper();
                if (words.ContainsKey(w.Length) == false)
                    words[w.Length] = new List<string>();

                if (words[w.Length].Contains(w2) == false)
                    words[w.Length].Add(w2);
            }
        }

        public static void Clear()
        {
              for(int y=0;y<height;y++)
              {
                  for (int x = 0; x < width; x++)
                  {
                      SetGridPointValue(grid[y][x].Tb,'\0'.ToString());
                  }
              }
        }

        public static void InitGrid(string[] rows)
        {
            int heightI = rows.Count();
            int widthI = rows.Max(s => s.Length);
            InitGrid(widthI, heightI, rows);
        }

        /// <summary>
        /// initialise the grid
        /// </summary>
        /// <param name="widthI"></param>
        /// <param name="heightI"></param>
        /// <param name="rows"></param>
        public static void InitGrid(int widthI, int heightI, string[] rows = null)
        {
            width = widthI;
            height = heightI;

            pu.clearControls();
            grid = new GridPoint[heightI][];
            var f = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Regular);

            for (var y = 0; y < heightI; y++)
            {
                grid[y] = new GridPoint[widthI];

                for (int x = 0; x < widthI; x++)
                {
                    //create the grid point and textbox
                    var tb = new TextBox
                    {
                        Width = 30,
                        Height = 16,
                        BorderStyle = BorderStyle.None,
                        BackColor = NormalBack,
                        ForeColor = NormalFront,
                        MaxLength = 1,
                        ContextMenuStrip = Baseform.GridLetterContext,
                        TextAlign = HorizontalAlignment.Center,
                        Font = f,
                    };

                    grid[y][x] = new GridPoint(tb, x, y);

                    tb.Tag = grid[y][x];

                    tb.TextChanged += Baseform.GridTBChangeEvent;
                    pu.addControl(tb, x != (width - 1));

                    //if text has been passed in, set the text now
                    if (rows != null)
                    {
                        if (x >= rows[y].Length)
                            break;
                        if (rows[y].Length >= x)
                        {
                            DeserialiseChar(rows[y][x], ref grid[y][x]);
                        }
                    }
                }
            }
        }

        public static char SerialiseChar(GridPoint gp)
        {
            if (gp.Enabled == false)
                return DisabledChar;

            if (gp.C == defaultChar)
                return BlankChar;

            return gp.C;
        }

        public static void DeserialiseChar(Char c, ref GridPoint gp)
        {
            gp.C = defaultChar;

            if (c == DisabledChar)
            {
                gp.Enabled = false;
            }
            else
            {
                gp.Enabled = true;
                if (c != BlankChar)
                    gp.C = c;
            }

            Baseform.ChangeTextboxText(gp.Tb, gp.C.ToString());
            SetReadonly(gp);
        }

        public static void SaveGridToFile(String filename)
        {
            var fs = new FileStream(filename, FileMode.Create);
            var sw = new StreamWriter(fs);
            for (int y = 0; y < height; y++)
            {
                var s = grid[y].Aggregate("", (a, b) => a + SerialiseChar(b));
                sw.WriteLine(s);
            }
            sw.Close();
            fs.Close();
        }

        public static void SetGridPointValue(TextBox tb, string sFirstOnly, bool setTextboxValue = true)
        {
            var gp = GetGridPointFromTextbox(tb);
            if (gp == null)
                return;

            if (sFirstOnly.Length == 0)
                sFirstOnly = defaultChar.ToString();
            var c2 = sFirstOnly.ToUpper()[0];

            Baseform.ChangeTextboxText(gp.Tb, c2.ToString(CultureInfo.InvariantCulture));
            gp.C = c2;
        }

        public static GridPoint GetGridPointFromTextbox(TextBox tb)
        {
            if (tb.Tag == null || tb.Tag is GridPoint == false)
                return null;

            var gp = tb.Tag as GridPoint;
            return gp;
        }

        public static void InitPanel(PanelReplacement pu)
        {
            Grid.pu = pu;
        }

        /// <summary>
        /// get the very last textbox for panel resize
        /// </summary>
        /// <returns></returns>
        public static TextBox GetLastTextbox()
        {
            var gp = grid[height - 1][width - 1];
            return gp.Tb;
        }

        public static void Toggle(TextBox tb)
        {
            var gp = GetGridPointFromTextbox(tb);
            if (gp == null)
                return;

            Baseform.ChangeTextboxText(gp.Tb,"");
            gp.Enabled = !gp.Enabled;

            SetReadonly(gp);
        }

        public static void SetReadonly(GridPoint gp)
        {
            if (gp.Enabled)
            {
                gp.Tb.BackColor = NormalBack;
                gp.Tb.ForeColor = NormalFront;
            }
            else
            {
                gp.Tb.BackColor = DisabledBack;
            }

            gp.Tb.ReadOnly = !gp.Enabled;
        }

        public static void Solve(bool allowWordsOnlyOnce)
        {
            try
            {
                AllowWordsOnlyOnce = allowWordsOnlyOnce;

                //go through each horizontal and vertical word, and brute force each dictionary word with same length until no spaces are left
                var gridlocal = GridPoint.ToLight(grid, width, height);
                var res = Solve(gridlocal, new List<string>());
                if (res.Item1)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            SetGridPointValue(grid[y][x].Tb, res.Item2[y][x].C.ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("no solution found");
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException==false)
                MessageBox.Show("ERROR:"+ex);
            }
            Baseform.EndThread();
        }

        private static Tuple<bool, GridPointLight[][]> Solve(GridPointLight[][] gridLight,List<string> blockedWords )
        {
            var newBlockedWords = new List<string>();
            List<SolvedWord> wordsI;
            while ((wordsI = GetIncompleteWords(gridLight)).Count != 0)
            {
                foreach (var w in wordsI)
                {
                    var rws = GetRelevantWords(w,blockedWords);
                    foreach (var rw in rws)
                    {
                        var gl = GridPointLight.Clone(gridLight,width,height);
                        SetWord(ref gl, rw, w.X, w.Y, w.Across);
                        if (AllowWordsOnlyOnce)
                            newBlockedWords.Add(rw);

                        var ok = Solve(gl,newBlockedWords);
                        if (ok.Item1)
                        {
                            return ok;
                        }
                    }
                    return new Tuple<bool, GridPointLight[][]>(false, null);
                }
            }
            return new Tuple<bool, GridPointLight[][]>(true, gridLight);
        }

        private static void SetWord(ref GridPointLight[][] gridLight, string sw, int startx, int starty, bool across)
        {
            for (int a = 0; a < sw.Length; a++)
            {
                if (across)
                    gridLight[starty][startx + a].C = sw[a];
                else
                    gridLight[starty + a][startx].C = sw[a];
            }
        }

        private static IEnumerable<string> GetRelevantWords(SolvedWord sw,List<string> blockedWords)
        {
            var r = new Regex(sw.Word);
            var l = words[sw.Word.Length].Where(s => blockedWords.Contains(s)==false&& r.IsMatch(s)).ToList();
            return l;
        }

        private static IEnumerable<SolvedWord> GetIncompleteWords(bool across, GridPointLight[][] gridLight)
        {
            var ret = new List<SolvedWord>();
            int v1m;
            int v2m;
            if (across)
            {
                v1m = height;
                v2m = width;
            }
            else
            {
                v1m = width;
                v2m = height;
            }

            for (int v1 = 0; v1 < v1m; v1++)
            {
                int startv = 0;
                int setChars = 0;
                bool unsetChar = false;
                //word as regular expresion
                string word = "";
                for (int v2 = 0; v2 <= v2m; v2++)
                {
                    GridPointLight g = null;
                    //end of line, or space before
                    if (v2 == v2m || (g = across ? gridLight[v1][v2] : gridLight[v2][v1]).Enabled == false)
                    {
                        //rules out no length words - blacks at start, and words part of vertical
                        //word has at least one unset letter
                        if ((v2 - startv) > 1 && unsetChar)
                        {
                            var sw = new SolvedWord(across ? startv : v1, across ? v1 : startv, across, word,setChars);
                            ret.Add(sw);
                        }
                        if (v2 == v2m)
                            break;
                    }

                    if (g.Enabled == false)
                    {
                        word = "";
                        startv = v2 + 1;
                        setChars = 0;
                        unsetChar = false;
                    }
                    else
                    {
                        if (g.C == defaultChar)
                        {
                            word += ".";
                            unsetChar = true;
                        }
                        else
                        {
                            setChars++;
                            word += g.C;
                        }
                    }
                }
            }
            return ret;
        }

        private static List<SolvedWord> GetIncompleteWords(GridPointLight[][] gridLight)
        {
            var ret = new List<SolvedWord>();
            ret.AddRange(GetIncompleteWords(true, gridLight));
            ret.AddRange(GetIncompleteWords(false, gridLight));
            ret=ret.OrderByDescending(s=>s.SetChars).ToList();
            return ret;
        }


    }
}
