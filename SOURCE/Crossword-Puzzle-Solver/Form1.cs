using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ANDREICSLIB;
using ANDREICSLIB.ClassExtras;

namespace Crossword_Puzzle_Solver
{
    public partial class Form1 : Form
    {
        #region licensing

        private const string AppTitle = "Crossword Puzzle Solver";
        private const double AppVersion = 0.1;
        private const String HelpString = "";

        private const String UpdatePath = "https://github.com/EvilSeven/Crossword-Puzzle-Solver/zipball/master";
        private const String VersionPath = "https://raw.github.com/EvilSeven/Crossword-Puzzle-Solver/master/INFO/version.txt";
        private const String ChangelogPath = "https://raw.github.com/EvilSeven/Crossword-Puzzle-Solver/master/INFO/changelog.txt";

        private readonly String OtherText =
            @"©" + DateTime.Now.Year +
            @" Andrei Gec (http://www.andreigec.net)

Licensed under GNU LGPL (http://www.gnu.org/)

Zip Assets © SharpZipLib (http://www.sharpdevelop.net/OpenSource/SharpZipLib/)
";
        #endregion

        #region locks
        public static bool AllowLookupEvents = true;
        public static bool AllowTbChangeEvent = true;
        #endregion locks

        private Thread solvingThread;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryExtras.SetCurrentDirectoryToDefault();
            var sd = new Licensing.SolutionDetails(HelpString, AppTitle, AppVersion, OtherText, VersionPath, UpdatePath,
                                                   ChangelogPath);
            Licensing.CreateLicense(this, sd, menuStrip1);
            Grid.Baseform = this;
            Grid.InitPanel(grid);
            Grid.LoadWords("words.dict");
            InitGrid(8, 8);
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void createbutton_Click(object sender, EventArgs e)
        {
            try
            {
                int w = int.Parse(createwidthTB.Text);
                int h = int.Parse(createheightTB.Text);

                InitGrid(w, h);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error creating grid:" + ex);
            }
            return;
        }

        private void InitGrid(int width, int height)
        {
            Grid.InitGrid(width, height);
            ResizeWindow();
        }

        private void InitGrid(string[] rows)
        {
            Grid.InitGrid(rows);
            ResizeWindow();
        }

        private void ResizeWindow()
        {
            int w = MinimumSize.Width;
            int h = MinimumSize.Height;

            var lastTb = Grid.GetLastTextbox();
            int lx = lastTb.Location.X;
            lx += 340;

            int ly = Grid.GetLastTextbox().Location.Y;
            ly += 380;

            if (lx > w)
                w = lx;

            if (ly > h)
                h = ly;

            Size = new Size(w, h);
        }

        public void GridTBChangeEvent(object sender, EventArgs e)
        {
            if (AllowTbChangeEvent)
            {
                AllowTbChangeEvent = false;
                var tb = sender as TextBox;
                Grid.SetGridPointValue(tb, tb.Text, false);

                AllowTbChangeEvent = true;
            }
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            Grid.Clear();
        }

        private void toggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tb = (TextBox)ContextMenuStripExtras.GetContextParent(sender, typeof(TextBox));
            if (tb == null)
                return;
            Grid.Toggle(tb);
        }

        private void loadwordgrid_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "|*.txt";
            var res = ofd.ShowDialog();
            if (res != DialogResult.OK)
                return;

            LoadGridFromFile(ofd.FileName);
        }

        public void LoadGridFromFile(String filename)
        {
            AllowLookupEvents = AllowTbChangeEvent = false;
            var f=FileExtras.LoadFile(filename);
            if (f == null)
                return;

            var rows = f.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Any(s=>s.Length>20))
            {
                var resp =
                    MessageBox.Show(
                        "Some of the rows are more than 20 chars in length, continue importing this file?","question",MessageBoxButtons.YesNo);
                if (resp == DialogResult.No)
                    return;
            }
            InitGrid(rows);
            AllowLookupEvents = AllowTbChangeEvent = true;
        }

        private void savebutton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.Title = "Choose a file name for the new grid";
            sfd.Filter = "|*.txt";
            var res = sfd.ShowDialog();
            if (res != DialogResult.OK)
                return;

            Grid.SaveGridToFile(sfd.FileName);
        }
        /*
        private void loadfromimageB_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "|*.*";
            ofd.Title = "choose file to load letters from";
            var res = ofd.ShowDialog();
            if (res != DialogResult.OK)
                return;

            LoadFromImage(ofd.FileName);
                   }
        

        private void LoadFromImage(string fn)
        {
            AllowLookupEvents = AllowTbChangeEvent = false;
            Bitmap b;
            try
            {
                b = new Bitmap(fn);
                string[] rows = null;

                //trim any white
                b = BitmapExtras.OnlyAllowBlackAndColour(b, 0, 0, 0);
                b = BitmapExtras.RemoveExcessWhitespace(b, false);
                b.Save("test.bmp");
                //InitGrid(rows);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error:" + ex);
            }

            AllowLookupEvents = AllowTbChangeEvent = true;
        }
        */

       private void button1_Click(object sender, EventArgs e)
        {
            ToggleSolve(); 
        }

        private void ToggleSolve()
        {
            if (solvingThread==null)
            {
                solveB.Text = "Abort";
                solvingThread = new Thread(() => Grid.Solve(solvedWordsCanOnlyBeUsedOnceToolStripMenuItem.Checked));
                solvingThread.Start();
            }
            else
            {
                solveB.Text = "Solve";
                solvingThread.Abort();
                solvingThread = null;
            }
        }

        public void ChangeTextboxText(TextBox tb,string s)
        {
            tb.Invoke(new MethodInvoker(delegate { tb.Text = s; }));
        }

        public void EndThread()
        {
            solveB.Invoke(new MethodInvoker(ToggleSolve));
        }


    }
}
