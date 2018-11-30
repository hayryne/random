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
    public partial class MainWindow : Window
    {    
        public MainWindow()
        {
            InitializeComponent();          
        }

        private void CloseCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void NewCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            Board.NewGame();
        }

        private void HelpCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            HelpDialog hd = new HelpDialog();
            hd.ShowDialog();
        }

        private void AboutCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            AboutDialog ad = new AboutDialog();
            ad.ShowDialog();
        }

        private void SettingsCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            SettingsDialog sd = new SettingsDialog();
            sd.ShowDialog();
        }

        private void CanInsert(object target, CanExecuteRoutedEventArgs e)
        {
            if (MillBoard.addCounter < 18) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void InsertCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            MillBoard.insert = true;
        }

        private void CanMove(object target, CanExecuteRoutedEventArgs e)
        {
            if (MillBoard.addCounter == 18) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void MoveCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            MillBoard.move = true;
        }
    }
}
