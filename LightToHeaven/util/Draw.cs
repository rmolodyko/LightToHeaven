using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LightToHeaven.util
{
    class Draw
    {
        public void paint(Canvas canvas,Data data){
            canvas.Children.Clear();
            drawLines(canvas);

            double scale = data.scale;
            double x,y;
            double[] f;
            Line line1;

            for (double a = - Math.PI / 2; a < Math.PI / 2; a += data.delta_x)
            {
                /*try
                {*/

                    for (double b = 0; b < Math.PI * 2; b += data.delta_y)
                    {
                        try
                        {
                            f = Solve.run(a, b, Math.PI / 2 - a);
                            x = canvas.Width / 2 - f[0] * scale;
                            y = canvas.Height / 2 + f[1] * scale;
                            line1 = new Line();
                            line1.X1 = x;
                            line1.Y1 = y;
                            line1.X2 = x - 1;
                            line1.Y2 = y - 1;
                            line1.Stroke = System.Windows.Media.Brushes.Gray;
                            line1.StrokeThickness = 1;
                            canvas.Children.Add(line1);
                        }
                        catch (Exception e)
                        {
                            //throw new Exception("next");
                            continue;
                        }
                    }
                /*}
                catch (Exception e) {
                    continue;
                }   */             
            }
        }

        public void drawLines(Canvas canvas)
        {
            Line line = new Line();
            line.Stroke = Brushes.LightSteelBlue;

            line.X1 = canvas.Width/2;
            line.X2 = canvas.Width/2;
            line.Y1 = 0;
            line.Y2 = canvas.Height;

            line.StrokeThickness = 2;
            canvas.Children.Add(line);

            line = new Line();
            line.Stroke = Brushes.LightSteelBlue;

            line.X1 = 0;
            line.X2 = canvas.Width;
            line.Y1 = canvas.Height/2;
            line.Y2 = canvas.Height/2;

            line.StrokeThickness = 2;
            canvas.Children.Add(line);

        }
    }

}
