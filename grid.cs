using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ANDREICSLIB;
using ANDREICSLIB.ClassExtras;
using ANDREICSLIB.ClassReplacements;

namespace Crossword_Puzzle_Solver
{
    public static class Grid
    {
        public static char DefaultChar = '\0';
        public static char DisabledChar = '\b';
        public static char BlankChar = '\f';

        public static Form1 Baseform;
        private static int _width;
        private static int _height;

        /// <summary>
        /// a textbox/char for each grid point
        /// </summary>
        private static GridPoint[][] _grid;

        private static PanelReplacement _pu;
        private static readonly Color NormalBack = Color.White;
        private static readonly Color NormalFront = Color.Black;
        private static readonly Color DisabledBack = Color.Black;
        private static bool _allowWordsOnlyOnce;
        private static Random r = new Random();

        public static void Randomise()
        {
            var gridlocal = GridPoint.ToLight(_grid, _width, _height);
            var res = Randomise(gridlocal);
            if (res.Item1)
                CopyToMain(res.Item2);
            SetReadonly();
        }

        public static Tuple<bool, GridPointLight[][]> Randomise(GridPointLight[][] g)
        {
            retry:
            int count = g.Sum(s => s.Count(s2 => s2.Enabled == false));
            int totalSquares = _width * _height;
            var target = totalSquares / 2;
            var startTesting = totalSquares / 4;

            int fail = 0;
            int failmax = 100;
            while (count < target)
            {
                GridPointLight[][] gl = null;
                int rx = -1;
                int ry = -1;
                bool ok = false;

                while (ok == false)
                {
                    rx = r.Next() % _width;
                    ry = r.Next() % _height;
                    gl = GridPointLight.Clone(g, _width, _height);
                    gl[ry][rx].Enabled = false;
                    ok = (rx == -1 || ry == -1 || !gl[ry][rx].Enabled || AllowableGrid(gl) == false);
                    if (ok == false)
                        gl[ry][rx].Enabled = true;
                    else
                        break;
                }

                //dont bother trying to solve until at least a quarter full
                if (count < startTesting)
                {
                    count++;
                    fail = 0;
                }
                else
                {
                    var oksolve = Solve(gl, new List<string>());
                    if (oksolve.Item1)
                        return new Tuple<bool, GridPointLight[][]>(true, gl);

                    gl[ry][rx].Enabled = true;
                    fail++;
                    //pop stack
                    if (fail > failmax)
                    {
                        //goto retry;
                        return new Tuple<bool, GridPointLight[][]>(true, gl);
                    }
                }
            }
            return new Tuple<bool, GridPointLight[][]>(false, null);
        }

        private static bool AllowableGrid(GridPointLight[][] g)
        {
            var iw = GetIncompleteWords(g);
            return (iw.All(s => s.Word.Length >= 3));
        }


        public static Dictionary<int, List<string>> words = new Dictionary<int, List<string>>();

        public static void LoadWords(string filename)
        {
            var filetext = FileExtras.LoadFile(filename);
            if (string.IsNullOrEmpty(filetext))
                return;

            words = new Dictionary<int, List<string>>();

            var wordssplit = filetext.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

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
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    SetGridPointValue(_grid[y][x].Tb, '\0'.ToString());
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
            _width = widthI;
            _height = heightI;

            _pu.ClearControls();
            _grid = new GridPoint[heightI][];
            var f = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Regular);

            for (var y = 0; y < heightI; y++)
            {
                _grid[y] = new GridPoint[widthI];

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

                    _grid[y][x] = new GridPoint(tb, x, y);

                    tb.Tag = _grid[y][x];

                    tb.TextChanged += Baseform.GridTBChangeEvent;
                    _pu.AddControl(tb, x != (_width - 1));

                    //if text has been passed in, set the text now
                    if (rows != null)
                    {
                        if (x >= rows[y].Length)
                            break;
                        if (rows[y].Length >= x)
                        {
                            DeserialiseChar(rows[y][x], ref _grid[y][x]);
                        }
                    }
                }
            }
        }

        public static char SerialiseChar(GridPoint gp)
        {
            if (gp.Enabled == false)
                return DisabledChar;

            if (gp.C == DefaultChar)
                return BlankChar;

            return gp.C;
        }

        public static void DeserialiseChar(Char c, ref GridPoint gp)
        {
            gp.C = DefaultChar;

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
            for (int y = 0; y < _height; y++)
            {
                var s = _grid[y].Aggregate("", (a, b) => a + SerialiseChar(b));
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
                sFirstOnly = DefaultChar.ToString();
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
            Grid._pu = pu;
        }

        /// <summary>
        /// get the very last textbox for panel resize
        /// </summary>
        /// <returns></returns>
        public static TextBox GetLastTextbox()
        {
            var gp = _grid[_height - 1][_width - 1];
            return gp.Tb;
        }

        public static void Toggle(TextBox tb)
        {
            var gp = GetGridPointFromTextbox(tb);
            if (gp == null)
                return;

            Baseform.ChangeTextboxText(gp.Tb, "");
            gp.Enabled = !gp.Enabled;

            SetReadonly(gp);
        }

        public static void SetReadonly()
        {
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    SetReadonly(_grid[y][x]);
                }
            }
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
                _allowWordsOnlyOnce = allowWordsOnlyOnce;

                //go through each horizontal and vertical word, and brute force each dictionary word with same length until no spaces are left
                var gridlocal = GridPoint.ToLight(_grid, _width, _height);
                var res = Solve(gridlocal, new List<string>());
                if (res.Item1)
                    CopyToMain(res.Item2);
                else
                {
                    MessageBox.Show("no solution found");
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException == false)
                    MessageBox.Show("ERROR:" + ex);
            }
            Baseform.EndThread();
        }

        private static void CopyToMain(GridPointLight[][] g)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _grid[y][x].Enabled = g[y][x].Enabled;
                    SetGridPointValue(_grid[y][x].Tb, g[y][x].C.ToString());
                }
            }
        }


        private static Tuple<bool, GridPointLight[][]> Solve(GridPointLight[][] gridLight, List<string> blockedWords)
        {
            var newBlockedWords = new List<string>();
            List<SolvedWord> wordsI;
            while ((wordsI = GetIncompleteWords(gridLight)).Count != 0)
            {
                foreach (var w in wordsI)
                {
                    var rws = GetRelevantWords(w, blockedWords);
                    foreach (var rw in rws)
                    {
                        var gl = GridPointLight.Clone(gridLight, _width, _height);
                        SetWord(ref gl, rw, w.X, w.Y, w.Across);
                        if (_allowWordsOnlyOnce)
                            newBlockedWords.Add(rw);

                        var ok = Solve(gl, newBlockedWords);
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

        private static IEnumerable<string> GetRelevantWords(SolvedWord sw, List<string> blockedWords)
        {
            var r = new Regex(sw.Word);
            if (words.ContainsKey(sw.Word.Length) == false)
                return new List<string>();

            var l = words[sw.Word.Length].Where(s => blockedWords.Contains(s) == false && r.IsMatch(s)).ToList();
            return l;
        }

        private static IEnumerable<SolvedWord> GetIncompleteWords(bool across, GridPointLight[][] gridLight)
        {
            var ret = new List<SolvedWord>();
            int v1m;
            int v2m;
            if (across)
            {
                v1m = _height;
                v2m = _width;
            }
            else
            {
                v1m = _width;
                v2m = _height;
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
                            var sw = new SolvedWord(across ? startv : v1, across ? v1 : startv, across, word, setChars);
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
                        if (g.C == DefaultChar)
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
            ret = ret.OrderByDescending(s => s.SetChars).ToList();
            return ret;
        }
    }
}
