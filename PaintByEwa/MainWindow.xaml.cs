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

namespace PaintByEwa
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int drawStyle = 1;
        Point? currentPoint = new Point();
        Point? startPoint = new Point();
        bool first = true;
        bool firstPoint = true;
        Line selected;
        Ellipse start;
        Ellipse end;
        

        int valueRed;
        int valueGreen;
        int valueBlue;
        Color currentColor = Color.FromRgb(0, 0, 0);

        public MainWindow()
        {
            InitializeComponent();
        }

        public void setValueRed(int val)
        {
            valueRed = val;          
            currentColor = Color.FromRgb((byte)valueRed, (byte)valueGreen, (byte)valueBlue); 
            buttonColorPicker.Fill = new SolidColorBrush(currentColor);
        }

        public void setValueGreen(int val)
        {
            valueGreen = val;
            currentColor = Color.FromRgb((byte)valueRed, (byte)valueGreen, (byte)valueBlue);       
            buttonColorPicker.Fill = new SolidColorBrush(currentColor);
        }

        public void setValueBlue(int val)
        {
            valueBlue = val;
            currentColor = Color.FromRgb((byte)valueRed, (byte)valueGreen, (byte)valueBlue);
            buttonColorPicker.Fill = new SolidColorBrush(currentColor);
        }

        private void buttonDraw_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 1;
        }
        
        private void buttonPoints_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 2;
        }

        private void drawSegment_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 3;
        }

        private void editSegment_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 4;
        }
        private void drawElipse_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 5;
        }

        private void drawCircle_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 6;
        }

        private void drawRectangle_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 7;
        }

        private void drawPolygon_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 8;
        }

        private void drawPath_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 9;
            first = true;
        }

        private void drawPlus_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 10;
        }

        private void drawArrow_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 11;
        }

        private void drawDiamond_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 12;
        }

        private void drawStar_Click(object sender, RoutedEventArgs e)
        {
            drawStyle = 13;
            Console.WriteLine(new SolidColorBrush(currentColor));
        }

        private void paintSurface_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && drawStyle == 1)
            {
                Line line = new Line();
                
                line.Stroke = new SolidColorBrush(currentColor);
                
                line.X1 = currentPoint.Value.X;
                line.Y1 = currentPoint.Value.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;

                currentPoint = e.GetPosition(this);

                paintSurface.Children.Add(line);
            }
        }

        private void paintSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double mouseX;
            double mouseY;
            double polySize;
            Point p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12;
           

            switch (drawStyle)
            {
                case 2:
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 6;
                    ellipse.Height = 6;

                    Canvas.SetTop(ellipse, e.GetPosition(this).Y - ellipse.Height / 2);
                    Canvas.SetLeft(ellipse, e.GetPosition(this).X - ellipse.Width / 2);

                    ellipse.Fill = new SolidColorBrush(currentColor);

                    paintSurface.Children.Add(ellipse);
                    break;
                case 3:
                    if (firstPoint)
                    {
                        firstPoint = false;
                    }
                    else
                    {
                        Point currentMousePosition = e.GetPosition(this);
                        startPoint = currentPoint;

                        Line line = new Line
                        {
                            Stroke = new SolidColorBrush(currentColor),
                            StrokeThickness=3,
                            X1 = startPoint.Value.X,
                            Y1 = startPoint.Value.Y,
                            X2 = currentMousePosition.X,
                            Y2 = currentMousePosition.Y
                        };
                        paintSurface.Children.Add(line);
                        firstPoint = true;
                    }
                    break;
                case 4:                 
                     Point clickPoint = e.GetPosition(paintSurface);
                     HitTestResult result = VisualTreeHelper.HitTest(paintSurface, clickPoint);
                        

                      if (result != null && result.VisualHit is Line l)
                      {
                          selected = l;
                          start = new Ellipse();
                          end = new Ellipse();
                          start.Width = start.Height = end.Width = end.Height = 6;

                          Canvas.SetTop(start, l.Y1 - start.Height / 2);
                          Canvas.SetLeft(start, l.X1 - start.Width / 2);
                          Canvas.SetTop(end, l.Y2 - start.Height / 2);
                          Canvas.SetLeft(end, l.X2 - start.Width / 2);

                          start.Fill = new SolidColorBrush(currentColor);
                          end.Fill = new SolidColorBrush(currentColor);
                          paintSurface.Children.Add(start);
                          paintSurface.Children.Add(end);

                          start.MouseDown += StartPoint_MouseDown;
                          end.MouseDown += EndPoint_MouseDown;                                           
                      }

                   
                    break;
                case 5:
                    Ellipse ell = new Ellipse();
                    ell.Width = 60;
                    ell.Height = 40;
                    Canvas.SetTop(ell, e.GetPosition(this).Y - ell.Height / 2);
                    Canvas.SetLeft(ell, e.GetPosition(this).X - ell.Width / 2);
                    ell.Stroke = new SolidColorBrush(currentColor);
                    paintSurface.Children.Add(ell);
                    break;
                case 6:
                    Ellipse el = new Ellipse();
                    el.Width = 40;
                    el.Height = 40;
                    Canvas.SetTop(el, e.GetPosition(this).Y - el.Height / 2);
                    Canvas.SetLeft(el, e.GetPosition(this).X - el.Width / 2);
                    el.Stroke = new SolidColorBrush(currentColor);
                    paintSurface.Children.Add(el);
                    break;
                case 7:
                    Rectangle rect = new Rectangle();
                    rect.Width = 40;
                    rect.Height = 40;
                    Canvas.SetTop(rect, e.GetPosition(this).Y - rect.Height / 2);
                    Canvas.SetLeft(rect, e.GetPosition(this).X - rect.Width / 2);   
                    rect.Stroke = new SolidColorBrush(currentColor);
                    paintSurface.Children.Add(rect);
                    break;
                case 8:
                    Polygon poly = new Polygon();

                    mouseX = e.GetPosition(this).X;
                    mouseY = e.GetPosition(this).Y;

                    polySize = 15;

                    p1 = new Point(mouseX - polySize, mouseY + 2 * polySize);
                    p2 = new Point(mouseX + polySize, mouseY + 2 * polySize);
                    p3 = new Point(mouseX + 2 * polySize, mouseY);
                    p4 = new Point(mouseX + polySize, mouseY - 2 * polySize);
                    p5 = new Point(mouseX - polySize, mouseY - 2 * polySize);
                    p6 = new Point(mouseX - 2 * polySize, mouseY);

                    PointCollection polyPoints = new PointCollection();
                    polyPoints.Add(p1);
                    polyPoints.Add(p2);
                    polyPoints.Add(p3);
                    polyPoints.Add(p4);
                    polyPoints.Add(p5);
                    polyPoints.Add(p6);
                    poly.Points = polyPoints;
                    poly.Stroke = new SolidColorBrush(currentColor);

                    paintSurface.Children.Add(poly);
                    break;
                case 9:
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        Point currentMousePosition = e.GetPosition(this);
                        startPoint = currentPoint;
                        Brush color = new SolidColorBrush(currentColor);

                        Line line = new Line
                        {

                            Stroke = color,
                            X1 = startPoint.Value.X,
                            Y1 = startPoint.Value.Y,
                            X2 = currentMousePosition.X,
                            Y2 = currentMousePosition.Y
                        };
                        paintSurface.Children.Add(line);
                    }
                    
                    break;
                case 10:
                    Polygon pol = new Polygon();

                    mouseX = e.GetPosition(this).X;
                    mouseY = e.GetPosition(this).Y;

                    polySize = 5;

                    p1 = new Point(mouseX - polySize, mouseY - polySize);
                    p2 = new Point(mouseX - polySize, mouseY - 5 * polySize);
                    p3 = new Point(mouseX +  polySize, mouseY-5*polySize);
                    p4 = new Point(mouseX + polySize, mouseY - polySize);
                    p5 = new Point(mouseX +5* polySize, mouseY - polySize);
                    p6 = new Point(mouseX +5* polySize, mouseY+polySize);
                    p7 = new Point(mouseX + polySize, mouseY + polySize);
                    p8 = new Point(mouseX + polySize, mouseY + 5 * polySize);
                    p9 = new Point(mouseX - polySize, mouseY + 5 * polySize);
                    p10 = new Point(mouseX - polySize, mouseY + polySize);
                    p11 = new Point(mouseX - 5*polySize, mouseY + polySize);
                    p12= new Point(mouseX -5*  polySize, mouseY -polySize);

                    PointCollection plusPoints = new PointCollection();
                    plusPoints.Add(p1);
                    plusPoints.Add(p2);
                    plusPoints.Add(p3);
                    plusPoints.Add(p4);
                    plusPoints.Add(p5);
                    plusPoints.Add(p6);
                    plusPoints.Add(p7);
                    plusPoints.Add(p8);
                    plusPoints.Add(p9);
                    plusPoints.Add(p10);
                    plusPoints.Add(p11);
                    plusPoints.Add(p12);
                    pol.Points = plusPoints; 
                    pol.Stroke = new SolidColorBrush(currentColor);

                    paintSurface.Children.Add(pol);
                    break;
                case 11:
                    Polygon arrow = new Polygon();

                    mouseX = e.GetPosition(this).X;
                    mouseY = e.GetPosition(this).Y;

                    polySize = 7;

                    p1 = new Point(mouseX-6*polySize, mouseY +  polySize);
                    p2 = new Point(mouseX-6*polySize, mouseY -  polySize);                  
                    p3 = new Point(mouseX-polySize, mouseY - polySize);
                    p4 = new Point(mouseX - 3*polySize, mouseY-2.2*polySize);
                    p5 = new Point(mouseX-1.5*polySize, mouseY - 3.7 * polySize);
                    p6 = new Point(mouseX + 3*polySize, mouseY);
                    p7 = new Point(mouseX-1.5*polySize, mouseY + 3.7 * polySize);
                    p8 = new Point(mouseX -3* polySize, mouseY+2.2*polySize);
                    p9 = new Point(mouseX - polySize, mouseY+polySize);              

                    PointCollection arrowPoints = new PointCollection();
                    arrowPoints.Add(p1);
                    arrowPoints.Add(p2);
                    arrowPoints.Add(p3);
                    arrowPoints.Add(p4);                  
                    arrowPoints.Add(p5);
                    arrowPoints.Add(p6);
                    arrowPoints.Add(p7);
                    arrowPoints.Add(p8);
                    arrowPoints.Add(p9);
                    arrow.Points = arrowPoints;
                    arrow.Stroke = new SolidColorBrush(currentColor);

                    paintSurface.Children.Add(arrow);
                    break;
                case 12:
                    Polygon diamond = new Polygon();

                    mouseX = e.GetPosition(this).X;
                    mouseY = e.GetPosition(this).Y;

                    polySize = 15;

                    p1 = new Point(mouseX, mouseY - 2 * polySize);
                    p2 = new Point(mouseX + polySize, mouseY);
                    p3 = new Point(mouseX, mouseY+2*polySize);
                    p4 = new Point(mouseX - polySize, mouseY);
                    

                    PointCollection diamondPoints = new PointCollection();
                    diamondPoints.Add(p1);
                    diamondPoints.Add(p2);
                    diamondPoints.Add(p3);
                    diamondPoints.Add(p4);
                    diamond.Points = diamondPoints;
                    diamond.Stroke = new SolidColorBrush(currentColor);

                    paintSurface.Children.Add(diamond);
                    break;
                case 13:
                    Polygon star = new Polygon();

                    mouseX = e.GetPosition(this).X;
                    mouseY = e.GetPosition(this).Y;
           
                    double outerRadius = 30;
                    double innerRadius = 15;

                    p1 = new Point(mouseX, mouseY - outerRadius);
                    p2 = new Point(mouseX + Math.Sin(Math.PI / 5) * innerRadius, mouseY - Math.Cos(Math.PI / 5) * innerRadius);
                    p3 = new Point(mouseX + Math.Sin(2 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(2 * Math.PI / 5) * outerRadius);
                    p4 = new Point(mouseX + Math.Sin(2 * Math.PI / 5) * innerRadius, mouseY + Math.Cos(2 * Math.PI / 5) * innerRadius);
                    p5 = new Point(mouseX + Math.Sin(4 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(4 * Math.PI / 5) * outerRadius);                  
                    p6 = new Point(mouseX, mouseY + innerRadius);
                    p7 = new Point(mouseX - Math.Sin(4 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(4 * Math.PI / 5) * outerRadius);
                    p8 = new Point(mouseX - Math.Sin(2 * Math.PI / 5) * innerRadius, mouseY + Math.Cos(2 * Math.PI / 5) * innerRadius);
                    p9 = new Point(mouseX - Math.Sin(2 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(2 * Math.PI / 5) * outerRadius);
                    p10 = new Point(mouseX - Math.Sin(Math.PI / 5) * innerRadius, mouseY - Math.Cos(Math.PI / 5) * innerRadius);
                   
                    PointCollection starPoints = new PointCollection();
                    starPoints.Add(p1);
                    starPoints.Add(p2);
                    starPoints.Add(p3);
                    starPoints.Add(p4);
                    starPoints.Add(p5);
                    starPoints.Add(p6);
                    starPoints.Add(p7);  
                    starPoints.Add(p8);
                    starPoints.Add(p9);
                    starPoints.Add(p10);
                    star.Points = starPoints;
                    star.Stroke = new SolidColorBrush(currentColor);
                    paintSurface.Children.Add(star);
                    break;
                case 14:
                    paintSurface.Children.Remove(start);
                    paintSurface.Children.Remove(end);
                    selected.X1 = e.GetPosition(this).X;
                    selected.Y1 = e.GetPosition(this).Y;
                    break;
                case 15:
                    paintSurface.Children.Remove(end);
                    paintSurface.Children.Remove(start);
                    selected.X2 = e.GetPosition(this).X;
                    selected.Y2 = e.GetPosition(this).Y;
                    break;
            }                                             
        }

        private void StartPoint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            drawStyle = 14;
        }
        private void EndPoint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            drawStyle = 15;
        }

        private void paintSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }

        private void buttonColorPicker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ColorPicker colorPicker = new ColorPicker(this);
            colorPicker.Show();
        }
    }
}
