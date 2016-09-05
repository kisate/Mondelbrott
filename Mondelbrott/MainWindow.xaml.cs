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
        private QuickGraphics _qg;
        private List<KeyValuePair<Dot, int>> allDots;

        public double p, q ;//, xratio, yratio;

        public Dot CalculateNext(Dot n)
        {
            double x, y;
            x = n.x * n.x - n.y * n.y + p;
            y = 2 * n.x * n.y + q;

            return new Dot(x,y);
        }

        public int CheckDot(double p, double q)
        {
            int i = 0, color = 0;
            Dot c = new Dot(p, q);
            for (i = 0; i < 1000; i++)
            {
                c = CalculateNext(c);
                if (c.x * c.x + c.y * c.y >= 4)
                {
                    //if (i <= 500) allDots.Add(new KeyValuePair<Dot, int>(c, 0));
                    //if (500 <= i && i < 600) allDots.Add(new KeyValuePair<Dot, int>(c, 1));
                    //if (600 <= i && i < 700) allDots.Add(new KeyValuePair<Dot, int>(c, 2));
                    //if (700 <= i && i < 800) allDots.Add(new KeyValuePair<Dot, int>(c, 3));
                    //if (800 <= i && i < 900) allDots.Add(new KeyValuePair<Dot, int>(c, 4));
                    //if (900 <= i && i < 1000) allDots.Add(new KeyValuePair<Dot, int>(c, 5));
                    if (500 <= i && i < 600) color = 1;
                    else if (i < 700) color = 2;
                    else if (i < 800) color = 3;
                    else if (i < 900) color = 4;
                    else if (i < 1000) color = 5;
                    break;
                }
            }

            if (i >= 1000) color = 6;
            //if (i >= 1000) allDots.Add(new KeyValuePair<Dot, int>(c, 6));
            //ChooseNextCoord();

            return color;
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
            //xratio = 2 / imgCanvas.ActualWidth;
            //yratio = 2 / imgCanvas.ActualHeight;

            var width = Math.Floor(imageBorder.ActualWidth);
            var height = (int)Math.Floor(imageBorder.ActualHeight);
            var colors = new Color[] { 
                Colors.Indigo, 
                Colors.MidnightBlue,
                Colors.DarkBlue,
                Colors.Navy,
                Colors.Blue,
                Colors.RoyalBlue,
                Colors.SlateBlue,
                Colors.DeepSkyBlue,
                Colors.SkyBlue,
                Colors.LightSteelBlue,
                Colors.LightSkyBlue,
                Colors.LightBlue,
                Colors.LightSeaGreen,
                Colors.Aquamarine,
                Colors.Aquamarine,
                Colors.White 
            };
                        
            double 
                xmin = double.Parse(txMinX.Text), 
                ymin = double.Parse(txMinY.Text), 
                size = double.Parse(txSize.Text), 
                p, q, ratio;

            int iterations = int.Parse(txIterations.Text);

            // Если просто поделить квадрат (4;4) на ширину высоту экрана, то картинка не сохранит пропорции (не будет квадрата)
            // Можно по разному это решать, я предлагаю dfhbfyn - выбрать меньший из размеров и использовать его для определения ratio
            // тогда 2й размер вычисляется
            if (width < height)
            {
                ratio = size / width;
            }
            else
            {
                ratio = size / height;
            }

            // initialize quick graphics
            _qg = new QuickGraphics(width, height, colors);
            imageDisplay.Source = _qg.GetImage();

            // clear (draw background)
            _qg.Clear(colors[0]);

            var formula = new RecoursiveFormula(iterations, 2.0, colors.Length);

            for (int x = 0; x < width; x++)
            {
                p = xmin + x * ratio;
                for (int y = 0; y < height; y++)
                {
                    q = ymin + y * ratio;
                    // используя мой код - 
                    var colorIndex = formula.Test(new Dot(p, q));
                    // используй твой код: (надо согласовать кол-во цветов)
                    //var colorIndex = CheckDot(p, q);

                    // в любом случае, ось Y на экране - перевернута (начинается вверху, а не внизу)
                    _qg.WritePixel(x, height - 1 - y, colorIndex);
                }
            }
        }

    }
}
