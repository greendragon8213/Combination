using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;

namespace CombinationProject
{
    public class MySimpleWrapPanel : WrapPanel
    {
        public TextBox tb1;
        public TextBox tb2;
        public  MySimpleWrapPanel() :base()
        {
            Orientation = Orientation.Vertical;

            tb1 = new TextBox();
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.Height = 23;
            tb1.Width = 33;
            tb1.TextWrapping = TextWrapping.Wrap;
            tb1.VerticalAlignment = VerticalAlignment.Top;
            tb1.Margin = new Thickness(13,15,0,0);

            ToolTip ttSymbol = new ToolTip();
            ttSymbol.Content = "Enter symbol";
            tb1.ToolTip = ttSymbol;

            this.Children.Add(tb1);

            tb2 = new TextBox();
            tb2.HorizontalAlignment = HorizontalAlignment.Left;
            tb2.Height = 23;
            tb2.Width = 33;
            tb2.TextWrapping = TextWrapping.Wrap;
            tb2.VerticalAlignment = VerticalAlignment.Top;
            tb2.Margin = new Thickness(13, 10, 0, 10);
            //Margin="13,10,0,10"

            ToolTip ttNumber = new ToolTip();
            ttNumber.Content = "Enter repeat couter for this symbol";
            tb2.ToolTip = ttNumber;

            this.Children.Add(tb2);
        }
    }

    public struct Symbol
    {
        public string SymbolValue;
        private int symbolId;
        public int RepetitionsCount;

        public Symbol(string s, int Id, int count)
        {
            this.SymbolValue = s;
            this.symbolId = Id;
            this.RepetitionsCount = count;
        }

        public int SymbolId
        {
            get { return symbolId; }
        }

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int CountOfAddedSymbols = 1;
        private int ExpectationForCombinationCount = 1;
        public MainWindow()
        {
            InitializeComponent();
            wrapPanel.Children.Add(new MySimpleWrapPanel());
        }

        private void Generation(List<Symbol> Symbols, ref int p, ref string[] result)
        {
            //string str = "";
            for (int i = 1; i <= Symbols[p].RepetitionsCount; i++)
            {
                string str = "";
                for (int j = 1; j <= i; j++)
                {
                    str += Symbols[p].SymbolValue;
                }

                result[Symbols[p].SymbolId] = str;
                //Console.Write(str+" ");

                int pp = p + 1;

                if (pp < Symbols.Count)
                    Generation(Symbols, ref pp, ref result);
                else
                {
                    StreamWriter SW = new StreamWriter(new FileStream("output.txt", FileMode.Append, FileAccess.Write));
                    foreach (string st in result)
                    {
                        SW.Write(st);
                    }
                    SW.WriteLine();
                    SW.Close();
                }
                
            }
            //ProgressPB.Value += 20;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //ProgressPB.Value = 0;
            Mouse.OverrideCursor = Cursors.Wait;
            
            if (File.Exists("output.txt"))
            {
                File.Delete("output.txt");
            }

            List<Symbol> Symbols = new List<Symbol>();

            int i = 0;
            foreach(MySimpleWrapPanel element in wrapPanel.Children)
            {
                try
                {
                    Symbols.Add(new Symbol(element.tb1.Text, i, Convert.ToInt16(element.tb2.Text)));
                    ExpectationForCombinationCount *= Convert.ToInt16(element.tb2.Text);
                    i++;
                }
                catch 
                { }
            }

            CombCountLB.Content = "Combination count: " + ExpectationForCombinationCount.ToString();

            int p = 0;
            string[] result = new string[Symbols.Count];
            Generation(Symbols, ref p, ref result);

            Mouse.OverrideCursor = null;
        }

        private void AddSymbol_Click(object sender, RoutedEventArgs e)
        {
            if (CountOfAddedSymbols < 10)
            {
                wrapPanel.Children.Add(new MySimpleWrapPanel());
                CountOfAddedSymbols++;
            }
            else
                MessageBox.Show("Added symbols must be <=10");
        }
    }
}
