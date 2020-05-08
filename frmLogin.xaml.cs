using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KUKUTAN
{
    /// <summary>
    /// frmLogin.xaml の相互作用ロジック
    /// </summary>
    public partial class frmLogin : Window
    {
        public bool pass = false;
        public frmLogin()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (passwordEditBox.Password != "9999")
            {
                new SoundPlayer(Properties.Resources.BUBU).Play();
                return;
            }
            pass = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            passwordEditBox.Focus();
        }
    }
}
