using LightToHeaven.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace LightToHeaven
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            init();
            
        }

        private void init() {
            Data data = Data.getInstance();
            data.a = Double.Parse(prop_a.Text);
            data.b = Double.Parse(prop_b.Text);
            data.delta_x = Double.Parse(prop_delta_x.Text);
            data.delta_y = Double.Parse(prop_delta_y.Text);
            data.init_x = Double.Parse(prop_init_x.Text);
            data.init_y = Double.Parse(prop_init_y.Text);
            data.scale = Double.Parse(prop_scale.Text);
            data.label_status = label_status;
        }

        private void draw_Click(object sender, RoutedEventArgs e)
        {
            Data data = Data.getInstance();
            init();
            Thread.Sleep(500);

            data.e = Math.Sqrt(1 - (Math.Pow(data.a, 2) / Math.Pow(data.b, 2)));
            data.c = data.b * data.e;

            //data.init_x = 2;
            data.init_z = -data.c;
            //data.init_y = 4;

            //double[] item = Solve.run(Math.PI * 210 / 180, Math.PI / 2, Math.PI * 300 / 180);
            //Console.Write(item);

            Draw d = new Draw();
            d.paint(canvas,data);
        }
    }
}
