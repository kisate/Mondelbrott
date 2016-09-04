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

namespace Mondelbrott
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<KeyValuePair<Dot, int>> allDots;

        public double p, q, xratio, yratio;

        public Dot CalculateNext(Dot n)
        {
            double x, y;
            x = n.x * n.x - n.y + p;
            y = 2 * n.x * n.y + q;

            return new Dot(x,y);
        }

        public void CheckDot()
        {
            int i = 0;
            Dot c = new Dot(p, q);
            for (i = 0; i < 1000; i++)
            {
                c = CalculateNext(c);
                if (c.x * c.x + c.y * c.y >= 4)
                {
                    if (i <= 500) allDots.Add(new KeyValuePair<Dot, int>(c, 0));
                    if (500 <= i && i < 600) allDots.Add(new KeyValuePair<Dot, int>(c, 1));
                    if (600 <= i && i < 700) allDots.Add(new KeyValuePair<Dot, int>(c, 2));
                    if (700 <= i && i < 800) allDots.Add(new KeyValuePair<Dot, int>(c, 3));
                    if (800 <= i && i < 900) allDots.Add(new KeyValuePair<Dot, int>(c, 4));
                    if (900 <= i && i < 1000) allDots.Add(new KeyValuePair<Dot, int>(c, 5));
                    break;
                }
            }

            if (i >= 1000) allDots.Add(new KeyValuePair<Dot, int>(c, 6));
            ChooseNextCoord();
        }

        public void ChooseNextCoord()
        {

        }

        public MainWindow()
        {
            InitializeComponent();

        }

        private void image1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lblCoordinates.Content = string.Format("{0}x{1}", e.NewSize.Width, e.NewSize.Height);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            xratio = 2 / imgCanvas.ActualWidth;
            yratio = 2 / imgCanvas.ActualHeight;
        }

    }
}
