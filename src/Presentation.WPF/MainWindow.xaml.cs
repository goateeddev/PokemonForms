using Domain.Resources;
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
using System.Windows.Threading;

namespace Presentation.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            _dispatcherTimer.Tick += InGameListener;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            _dispatcherTimer.Start();
            player.KeyDown += MovePlayer;
        }

        private void InGameListener(object sender, EventArgs e)
        {

        }

        public void MovePlayer(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_left.png", UriKind.Relative));
                    //if (!bLeft) player.Left -= mHorizontal;
                    //if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Left += mHorizontal;
                    //bRight = false;
                    //footsteps++;
                    break;

                case Key.Right:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_right.png", UriKind.Relative));
                    //if (!bRight) player.Left += mHorizontal;
                    //if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Left -= mHorizontal;
                    //bLeft = false;
                    //footsteps++;
                    break;

                case Key.Up:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_back.png", UriKind.Relative));
                    //if (!bTop) player.Top -= mVertical;
                    //if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Top += mVertical;
                    //bBottom = false;
                    //footsteps++;
                    break;

                case Key.Down:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_front.png", UriKind.Relative));
                    //if (!bBottom) player.Top += mVertical;
                    //if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Top -= mVertical;
                    //bTop = false;
                    //footsteps++;
                    break;

                default:
                    break;
            }
        }
    }
}
