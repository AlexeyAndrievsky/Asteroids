using System;
using System.Collections.Generic;
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

namespace WpfLesson
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool PlayState = false;
        DispatcherTimer Timer;

        public MainWindow()
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            //sliderPosition.Value = media.Position.Seconds + media.Position.Minutes * 60 + media.Position.Hours * 360;
        }

        private void media_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {

            //label.Content = e.ErrorException.Message;
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            sliderPosition.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PlayState)
            {
                media.Play();
                playButton.Content = "Пауза";
                Timer.Start();
                PlayState = true;
            }
            else
            {
                media.Pause();
                playButton.Content = "Пуск";
                Timer.Stop();
                PlayState = false;
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
            Timer.Stop();
            PlayState = false;
        }

        private void sliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Pause();
            media.Position = TimeSpan.FromSeconds(sliderPosition.Value);
            media.Play();
        }
    }
}
