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
    public partial class MillBoard : UserControl
    {
        List<MillPiece> pieces = new List<MillPiece>();
        Random r = new Random();
        // player one is black; two is white
        public static Color[] players = { Color.FromRgb(0,0,0), Color.FromRgb(255,255,255) };
        private Color baseColor = Color.FromRgb(143, 188, 143);
        private Color moveColor = Color.FromRgb(220, 20, 60);
        public static int addCounter = 0;
        public static bool insert = false;
        public static bool move = false;
        public static bool remove = false;
        private MillPiece beingMoved;

        public MillBoard()
        {
            InitializeComponent();
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            SizeChanged += MillBoard_SizeChanged;
            Turn = players[r.Next(0, 2)];
        }

        // scales the board's elements to window size
        private void MillBoard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CellWidth = ActualWidth/7;
            CellHeight = ActualHeight/7;
            HalfCellHeight = CellHeight/2;
            HalfCellWidth = CellWidth/2;
        }

        // indicates if turn is on player 1 or 2
        public Color Turn
        {
            get { return (Color)GetValue(TurnProperty); }
            set { SetValue(TurnProperty, value); }
        }

        public static readonly DependencyProperty TurnProperty =
            DependencyProperty.Register("Turn", typeof(Color), typeof(MillBoard),
            new PropertyMetadata(Color.FromRgb(0,0,0)));

        // contains information on the current game state
        public String Notification
        {
            get { return (String)GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(String),
            typeof(MillBoard), new PropertyMetadata(""));

        public double CellWidth
        {
            get { return (double)GetValue(CellWidthProperty); }
            set { SetValue(CellWidthProperty, value); }
        }

        public static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register("CellWidth", typeof(double),
            typeof(MillBoard), new PropertyMetadata(0.0));

        public double CellHeight
        {
            get { return (double)GetValue(CellHeightProperty); }
            set { SetValue(CellHeightProperty, value); }
        }

        public static readonly DependencyProperty CellHeightProperty =
            DependencyProperty.Register("CellHeight", typeof(double),
            typeof(MillBoard), new PropertyMetadata(0.0));


        public double HalfCellWidth
        {
            get { return (double)GetValue(HalfCellWidthProperty); }
            set { SetValue(HalfCellWidthProperty, value); }
        }

        public static readonly DependencyProperty HalfCellWidthProperty =
            DependencyProperty.Register("HalfCellWidth", typeof(double),
            typeof(MillBoard), new PropertyMetadata(0.0));

        public double HalfCellHeight
        {
            get { return (double)GetValue(HalfCellHeightProperty); }
            set { SetValue(HalfCellHeightProperty, value); }
        }

        public static readonly DependencyProperty HalfCellHeightProperty =
            DependencyProperty.Register("HalfCellHeight", typeof(double),
            typeof(MillBoard), new PropertyMetadata(0.0));

        // adds the pieces on the board to a list once the board is loaded
        private void BoardGrid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Object o in BoardGrid.Children)
                if (o is MillPiece)
                    pieces.Add((MillPiece)o);
        }

        // begins a new game
        public void NewGame()
        {
            addCounter = 0;
            Turn = players[r.Next(0, 2)];
            foreach (MillPiece mp in pieces)
                mp.PieceColor = baseColor;
        }

        // changes the turn from one player to another
        private void ChangeTurn()
        {
            Turn = Turn == players[0] ? players[1] : players[0];
        }

        // checks if a move has formed a mill
        private void MillFormed(MillPiece moved)
        {
            if (IsInMill(moved))
                RemovePiece();
            else ChangeTurn();
        }

        // checks if a piece is a part of a mill
        private bool IsInMill(MillPiece target)
        {
            if ((pieces[target.PotentialMill1[0]-1].PieceColor == target.PieceColor &&
                pieces[target.PotentialMill1[1]-1].PieceColor == target.PieceColor) ||
                (pieces[target.PotentialMill2[0]-1].PieceColor == target.PieceColor &&
                pieces[target.PotentialMill2[1]-1].PieceColor == target.PieceColor))
                return true;
            return false;
        }

        // checks if all pieces of one color are in a mill
        private bool AreAllInMill()
        {
            foreach (MillPiece mp in pieces)
                if (mp.PieceColor != Turn && mp.PieceColor != baseColor && !IsInMill(mp))
                    return false;    
            return true;
        }

        // checks if a color has less than three remaining pieces
        private bool ThreeRemaining()
        {
            if (addCounter < 18) return false;
            int remaining = 0;
            foreach (MillPiece mp in pieces)
                if (mp.PieceColor == Turn) remaining++;
            if (remaining <= 3) return true;
            return false;
        }

        private void RemovePiece()
        {
            Notification = "Mill formed! Click on an opponent's piece to remove it.";
            remove = true;
        }

        // handles all inserting, moving and removing
        private void Piece_Click(object sender, RoutedEventArgs e)
        {
            MillPiece target = (MillPiece)sender;
            if (remove)
            {
                if (target.PieceColor == Turn || target.PieceColor == baseColor) return;
                if (IsInMill(target) && !AreAllInMill())
                {
                    Notification = "Cannot remove from a mill.";
                    return;
                }
                target.PieceColor = baseColor; remove = false; Notification = ""; ChangeTurn();
                if (addCounter == 18)
                {
                    int counter = 0;
                    foreach (MillPiece mp in pieces)
                        if (mp.PieceColor == Turn) counter++;
                    if (counter < 3) Notification = "Game over.";
                } return;
            }

            if (insert)
            {
                if (target.PieceColor == players[0] || target.PieceColor == players[1]) return;
                addCounter++; target.PieceColor = Turn; insert = false; 
                MillFormed(target); return;
            }

            if (move)
            {
                if (beingMoved == null)
                {
                    if (target.PieceColor != Turn) return;
                    else { beingMoved = target; beingMoved.PieceColor = moveColor; return; }
                }

                else if (target.PieceColor != players[0] && target.PieceColor != players[1] &&
                         target.PieceColor != moveColor &&
                         (beingMoved.Neighbors.Contains(pieces.IndexOf(target)+1) ||
                         ThreeRemaining()))
                {
                    target.PieceColor = Turn;
                    beingMoved.PieceColor = baseColor;
                    move = false; beingMoved = null; MillFormed(target); return;
                }
                else
                {
                    beingMoved.PieceColor = Turn;
                    move = false; beingMoved = null; return;
                }
            }
        }
    }

    // converts a Color to Brush
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Color)
                return new SolidColorBrush((Color)value);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { throw new NotImplementedException(); }
    }

    // converts a Color to String
    public class ColorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Color)
                return (Color)value == MillBoard.players[0] ? "Black's turn." : "White's turn.";
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { throw new NotImplementedException(); }
    }
}
