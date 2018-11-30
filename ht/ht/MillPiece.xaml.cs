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

namespace ht
{
    public partial class MillPiece : CheckBox
    {
        private List<int> neighbors = new List<int>();
        private List<int> potentialMill1 = new List<int>();
        private List<int> potentialMill2 = new List<int>();

        // the color of the Piece
        public Color PieceColor
        {
            get { return (Color)GetValue(PieceColorProperty); }
            set { SetValue(PieceColorProperty, value); }
        }

        public static readonly DependencyProperty PieceColorProperty =
            DependencyProperty.Register("PieceColor", typeof(Color), typeof(MillPiece),
            new PropertyMetadata(Color.FromRgb(0,0,0)));

        // lists the neighboring spots of this Piece
        public List<int> Neighbors
        { get { return neighbors; } }

        public List<int> PotentialMill1
        { get { return potentialMill1; } }

        public List<int> PotentialMill2
        { get { return potentialMill2; } }

        public MillPiece()
        {
            InitializeComponent();         
        }

        // parses the neighbors and potential partners for mills from
        // Content once the Piece is loaded
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (Content == null) return;
            String content = (String)Content;
            String[] split = content.Split(new char[] { ',' });
            String[] neighborsSplit = split[0].Split(new char[] { ' ' });
            String[] potentialMill1Split = split[1].Split(new char[] { ' ' });
            String[] potentialMill2Split = split[2].Split(new char[] { ' ' });
            foreach (String s in neighborsSplit)
                neighbors.Add(Int32.Parse(s));
            foreach (String s in potentialMill1Split)
                potentialMill1.Add(Int32.Parse(s));
            foreach (String s in potentialMill2Split)
                potentialMill2.Add(Int32.Parse(s));
        }
    }
}
