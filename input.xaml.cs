using System;
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
    /// input.xaml の相互作用ロジック
    /// </summary>
    public partial class input : UserControl
    {
        public new Brush Background
        {
            set { border.Background = value; }
            get { return border.Background; }
        }
        public new Brush Foreground
        {
            set { label.Foreground = value; }
            get { return label.Foreground; }
        }
        public new object Content
        {
            set {
                label.Content = value;
                if (ShowHint)
                {
                    if (label.Content.ToString().Length == 0)
                    {
                        label.Content = anser;
                        label.Foreground = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                    }
                    else
                    {
                        label.Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
            }
            get {
                if (ShowHint)
                {
                    var brush = label.Foreground as SolidColorBrush;
                    if (brush != null &&
                        brush.Color == Color.FromRgb(224, 224, 224))
                        return "";
                }
                return label.Content;
            }
        }
        public bool ShowHint { set; get; }
        public int anser { set; get; }
        public bool correct
        {
            get
            {
                var brush = label.Foreground as SolidColorBrush;
                if (brush != null && brush.Color == Color.FromRgb(224, 224, 224)) return false;
                if (label.Content.ToString().Length == 0) return false;
                int input;
                if (!int.TryParse(label.Content.ToString(), out input)) return false;
                return (input == anser);
            }
        }
        public input()
        {
            InitializeComponent();
            Content = "";
        }
    }
}
