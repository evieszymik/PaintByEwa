using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;

namespace PaintByEwa
{
    /// <summary>
    /// Logika interakcji dla klasy ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        MainWindow mainWin;
        public ImageWindow(MainWindow main)
        {
            InitializeComponent();
            mainWin = main;
        }

        private void applyFilter_Click(object sender, RoutedEventArgs e)
        {            
            Mat src = CvInvoke.Imread(mainWin.imagePath, ImreadModes.Color);

            Mat kernel = new Mat(3, 3, DepthType.Cv32F, 1);

            float[] matrix = new float[9];
            kernel.CopyTo(matrix);
           
            try
            {
                matrix[0] = int.Parse(f1.Text);
                matrix[1] = int.Parse(f2.Text);
                matrix[2] = int.Parse(f3.Text);
                matrix[3] = int.Parse(f4.Text);
                matrix[4] = int.Parse(f5.Text);
                matrix[5] = int.Parse(f6.Text);
                matrix[6] = int.Parse(f7.Text);
                matrix[7] = int.Parse(f8.Text);
                matrix[8] = int.Parse(f9.Text);

                float sum = 0;
                for(int i = 0; i < 9; i++)
                {
                    sum += matrix[i];
                }

                kernel.SetTo(matrix);
                if(sum!=0)
                    kernel = kernel / sum;              

                Mat dst = new Mat(src.Size, DepthType.Cv8U, 3);
                CvInvoke.Filter2D(src, dst, kernel, new System.Drawing.Point(-1, -1));

                CvInvoke.Imshow("Filtred Image", dst);
                CvInvoke.WaitKey(0);
            }
            catch (Exception)
            {
                MessageBox.Show("Nieprawidłowe wartości");
            }                     
        }
    }
}
