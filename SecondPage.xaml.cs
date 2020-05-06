﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KUKUTAN
{
    /// <summary>
    /// SecondPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SecondPage : Page
    {
        public SecondPage()
        {
            InitializeComponent();
        }

        private void backbutton_Click(object sender, RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CountDownPage(new SyutudaiPage()));
        }
    }
}
