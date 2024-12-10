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
using System.Windows.Shapes;

namespace PaintByEwa
{
    /// <summary>
    /// Logika interakcji dla klasy ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        bool correctRed = true;
        bool correctBlue = true;
        bool correctGreen = true;

        int vr = 0, vg = 0, vb = 0;
        
        MainWindow main;

        public ColorPicker(MainWindow mainWindow)
        {
            InitializeComponent();
            main = mainWindow;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!(correctRed && correctBlue && correctGreen))
            {
                e.Cancel = true;
                MessageBox.Show("Wprowadź poprawne wartości");
            }
          
            
        }

        private (int H, int S, int V) ConvertToHSV(int r, int g, int b)
        {
            double Rn = r / 255.0;
            double Gn = g / 255.0;
            double Bn = b / 255.0;

            double mMax = Math.Max(Rn, Math.Max(Gn,Bn));
            double mMin = Math.Min(Rn, Math.Min(Gn, Bn));
            double delta = mMax - mMin;

            double H = 0, S = 0, V = 0;

            V = mMax;
            if (mMax != 0)
            {
                S = delta / mMax;
            }

            if (delta != 0)
            {
                if (mMax == Rn)
                {
                    H = ((Gn - Bn) / delta)%6;

                } else if (mMax == Gn)
                {
                    H = (Bn - Rn) / delta + 2;

                }
                else
                {
                    H = (Rn - Gn) / delta + 4;

                }       
            }
           
            H *= 60;
            S *= 100;
            V *= 100;
            if (H < 0)
                H += 360;

            return ((int)Math.Round(H), (int)Math.Round(S), (int)Math.Round(V));
        }
    
       

        private void tbRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(int.TryParse(tbRed.Text, out int red)) {
                if (red>-1 && red < 256)
                {
                    err1.Visibility = Visibility.Hidden;
                    correctRed = true;
                    main.setValueRed(red);
                    vr = red;
                    var (H, S, V) = ConvertToHSV(vr, vg, vb);
                    
                    updateHSV(H, S, V);
                }
                else
                {
                    err1.Visibility = Visibility.Visible;
                    correctRed = false;
                }
                
            }
            else {
                err1.Visibility = Visibility.Visible;
                correctRed = false;
            }
            
        }

        private void tbGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(tbGreen.Text, out int green)) {
                if (green >-1 && green < 256)
                {
                    err2.Visibility = Visibility.Hidden;
                    correctGreen = true;
                    main.setValueGreen(green);
                    vg = green;
                    var (H, S, V) = ConvertToHSV(vr, vg, vb);
                    updateHSV(H, S, V);
                }
                else
                {
                    err2.Visibility = Visibility.Visible;
                    correctGreen = false;
                }
                
            } else {
                err2.Visibility = Visibility.Visible;
                correctGreen = false;
            }

        }

        private void tbBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(tbBlue.Text, out int blue)) {
                if (blue>-1 && blue < 256)
                {
                    err3.Visibility = Visibility.Hidden;
                    correctBlue = true;
                    main.setValueBlue(blue);
                    vb = blue;
                    var (H, S, V) = ConvertToHSV(vr, vg, vb);
                    
                    updateHSV(H, S, V);
                }
                else
                {
                    err3.Visibility = Visibility.Visible;
                    correctBlue = false;
                }
                
            } else {
                err3.Visibility = Visibility.Visible;
                correctBlue = false;
            }

        }

        private void updateHSV(int h, int s, int v)
        {
            hsv.Text = $"{h}°, {s}%, {v}%";
        }


    }
}
