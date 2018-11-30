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
using System.Windows.Shapes;

namespace ht
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
            Closing += OnSettingsSet;
        }

        private void OnSettingsSet(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*MainWindow.Player1Name = player1TB.Text;
            MainWindow.Player2Name = player2TB.Text;*/
        }
    }
}
