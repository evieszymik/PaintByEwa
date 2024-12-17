using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using System.Windows.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Reg;
using Emgu.CV.Structure;
using Microsoft.Win32;


namespace PaintByEwa
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int drawStyle = 1;
        System.Windows.Point? currentPoint = new System.Windows.Point();
        System.Windows.Point? startPoint = new System.Windows.Point();
        bool first = true;
        bool firstPoint = true;
        Line selected;
        System.Windows.Shapes.Ellipse start;
        System.Windows.Shapes.Ellipse end;
        string imagePath;
        

        int valueRed;
        int valueGreen;
        int valueBlue;
        System.Windows.Media.Color currentColor = System.Windows.Media.Color.FromRgb(0, 0, 0);

        bool imageAdded = false;
        private Mat originalImage;
        private Mat processedImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void setValueRed(int val)
        {
            valueRed = val;          
            currentColor = System.Windows.Media.Color.FromRgb((byte)valueRed, (byte)valueGreen, (byte)valueBlue); 
            buttonColorPicker.Fill = new SolidColorBrush(currentColor);
        }

        public void setValueGreen(int val)
        {
            valueGreen = val;
            currentColor = System.Windows.Media.Color.FromRgb((byte)valueRed, (byte)valueGreen, (byte)valueBlue);       
            buttonColorPicker.Fill = new SolidColorBrush(currentColor);
        }

        public void setValueBlue(int val)
        {
            valueBlue = val;
            currentColor = System.Windows.Media.Color.FromRgb((byte)valueRed, (byte)valueGreen, (byte)valueBlue);
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
            System.Windows.Point p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12;
           

            switch (drawStyle)
            {
                case 2:
                    System.Windows.Shapes.Ellipse ellipse = new System.Windows.Shapes.Ellipse();
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
                        System.Windows.Point currentMousePosition = e.GetPosition(this);
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
                    System.Windows.Point clickPoint = e.GetPosition(paintSurface);
                     HitTestResult result = VisualTreeHelper.HitTest(paintSurface, clickPoint);
                        

                      if (result != null && result.VisualHit is Line l)
                      {
                          selected = l;
                          start = new System.Windows.Shapes.Ellipse();
                          end = new System.Windows.Shapes.Ellipse();
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
                    System.Windows.Shapes.Ellipse ell = new System.Windows.Shapes.Ellipse();
                    ell.Width = 60;
                    ell.Height = 40;
                    Canvas.SetTop(ell, e.GetPosition(this).Y - ell.Height / 2);
                    Canvas.SetLeft(ell, e.GetPosition(this).X - ell.Width / 2);
                    ell.Stroke = new SolidColorBrush(currentColor);
                    paintSurface.Children.Add(ell);
                    break;
                case 6:
                    System.Windows.Shapes.Ellipse el = new System.Windows.Shapes.Ellipse();
                    el.Width = 40;
                    el.Height = 40;
                    Canvas.SetTop(el, e.GetPosition(this).Y - el.Height / 2);
                    Canvas.SetLeft(el, e.GetPosition(this).X - el.Width / 2);
                    el.Stroke = new SolidColorBrush(currentColor);
                    paintSurface.Children.Add(el);
                    break;
                case 7:
                    System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
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

                    p1 = new System.Windows.Point(mouseX - polySize, mouseY + 2 * polySize);
                    p2 = new System.Windows.Point(mouseX + polySize, mouseY + 2 * polySize);
                    p3 = new System.Windows.Point(mouseX + 2 * polySize, mouseY);
                    p4 = new System.Windows.Point(mouseX + polySize, mouseY - 2 * polySize);
                    p5 = new System.Windows.Point(mouseX - polySize, mouseY - 2 * polySize);
                    p6 = new System.Windows.Point(mouseX - 2 * polySize, mouseY);

                    System.Windows.Media.PointCollection polyPoints = new System.Windows.Media.PointCollection();
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
                        System.Windows.Point currentMousePosition = e.GetPosition(this);
                        startPoint = currentPoint;
                        System.Windows.Media.Brush color = new SolidColorBrush(currentColor);

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

                    p1 = new System.Windows.Point(mouseX - polySize, mouseY - polySize);
                    p2 = new System.Windows.Point(mouseX - polySize, mouseY - 5 * polySize);
                    p3 = new System.Windows.Point(mouseX +  polySize, mouseY-5*polySize);
                    p4 = new System.Windows.Point(mouseX + polySize, mouseY - polySize);
                    p5 = new System.Windows.Point(mouseX +5* polySize, mouseY - polySize);
                    p6 = new System.Windows.Point(mouseX +5* polySize, mouseY+polySize);
                    p7 = new System.Windows.Point(mouseX + polySize, mouseY + polySize);
                    p8 = new System.Windows.Point(mouseX + polySize, mouseY + 5 * polySize);
                    p9 = new System.Windows.Point(mouseX - polySize, mouseY + 5 * polySize);
                    p10 = new System.Windows.Point(mouseX - polySize, mouseY + polySize);
                    p11 = new System.Windows.Point(mouseX - 5*polySize, mouseY + polySize);
                    p12= new System.Windows.Point(mouseX -5*  polySize, mouseY -polySize);

                    System.Windows.Media.PointCollection plusPoints = new System.Windows.Media.PointCollection();
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

                    p1 = new System.Windows.Point(mouseX-6*polySize, mouseY +  polySize);
                    p2 = new System.Windows.Point(mouseX-6*polySize, mouseY -  polySize);                  
                    p3 = new System.Windows.Point(mouseX-polySize, mouseY - polySize);
                    p4 = new System.Windows.Point(mouseX - 3*polySize, mouseY-2.2*polySize);
                    p5 = new System.Windows.Point(mouseX-1.5*polySize, mouseY - 3.7 * polySize);
                    p6 = new System.Windows.Point(mouseX + 3*polySize, mouseY);
                    p7 = new System.Windows.Point(mouseX-1.5*polySize, mouseY + 3.7 * polySize);
                    p8 = new System.Windows.Point(mouseX -3* polySize, mouseY+2.2*polySize);
                    p9 = new System.Windows.Point(mouseX - polySize, mouseY+polySize);

                    System.Windows.Media.PointCollection arrowPoints = new System.Windows.Media.PointCollection();
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

                    p1 = new System.Windows.Point(mouseX, mouseY - 2 * polySize);
                    p2 = new System.Windows.Point(mouseX + polySize, mouseY);
                    p3 = new System.Windows.Point(mouseX, mouseY+2*polySize);
                    p4 = new System.Windows.Point(mouseX - polySize, mouseY);


                    System.Windows.Media.PointCollection diamondPoints = new System.Windows.Media.PointCollection();
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

                    p1 = new System.Windows.Point(mouseX, mouseY - outerRadius);
                    p2 = new System.Windows.Point(mouseX + Math.Sin(Math.PI / 5) * innerRadius, mouseY - Math.Cos(Math.PI / 5) * innerRadius);
                    p3 = new System.Windows.Point(mouseX + Math.Sin(2 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(2 * Math.PI / 5) * outerRadius);
                    p4 = new System.Windows.Point(mouseX + Math.Sin(2 * Math.PI / 5) * innerRadius, mouseY + Math.Cos(2 * Math.PI / 5) * innerRadius);
                    p5 = new System.Windows.Point(mouseX + Math.Sin(4 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(4 * Math.PI / 5) * outerRadius);                  
                    p6 = new System.Windows.Point(mouseX, mouseY + innerRadius);
                    p7 = new System.Windows.Point(mouseX - Math.Sin(4 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(4 * Math.PI / 5) * outerRadius);
                    p8 = new System.Windows.Point(mouseX - Math.Sin(2 * Math.PI / 5) * innerRadius, mouseY + Math.Cos(2 * Math.PI / 5) * innerRadius);
                    p9 = new System.Windows.Point(mouseX - Math.Sin(2 * Math.PI / 5) * outerRadius, mouseY - Math.Cos(2 * Math.PI / 5) * outerRadius);
                    p10 = new System.Windows.Point(mouseX - Math.Sin(Math.PI / 5) * innerRadius, mouseY - Math.Cos(Math.PI / 5) * innerRadius);

                    System.Windows.Media.PointCollection starPoints = new System.Windows.Media.PointCollection();
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

      
        private void readImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.gif; *.png; *.bmp) | *.jpg; *.jpeg; *.gif; *.png;  *.bmp";
            openFileDialog.InitialDirectory = "C:\\Users\\ewka0\\OneDrive\\Pulpit\\Studia\\Grafika komputerowa";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    imagePath = openFileDialog.FileName;
                    Uri fileUri = new Uri(imagePath);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = fileUri;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    orgImg.Source = bitmap;
                    imageAdded = true;

                    originalImage = CvInvoke.Imread(imagePath, ImreadModes.Color);
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Błąd ładowania obrazu");
                }
                    
            }
        }

        private void sobel_Click(object sender, RoutedEventArgs e)
        {
            if (imageAdded)
            {
                try
                {
                    Image<Bgr, byte> img = new Image<Bgr, byte>(imagePath);
                    
                    Image<Gray, byte> img2 = img.Convert<Gray, byte> ();
                    Image<Gray, Single> img_final = (img2.Sobel(1,1,5));                  

                    CvInvoke.Imshow("Filtr Sobel", img_final);
                    CvInvoke.WaitKey(0);

                    
                    //Bitmap bitmap = img_final.ToBitmap();
                    //MemoryStream memoryStream = new MemoryStream();

                    //bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    //memoryStream.Position = 0; 
                
                    //BitmapImage bitmapImage = new BitmapImage();
                    //bitmapImage.BeginInit();
                    //bitmapImage.StreamSource = memoryStream;
                    //bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    //bitmapImage.EndInit();
                    //bitmapImage.Freeze();
                    //orgImg.Source = bitmapImage;        
               }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas przetwarzania obrazu: {ex.Message}");
                }

            }
            else
            {
                MessageBox.Show("Nie dodano obrazu");
            }
        }
    }
}
