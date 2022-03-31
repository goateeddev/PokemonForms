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
        private int footsteps;

        public MainWindow()
        {
            InitializeComponent();
            canvas.Focus();
            canvas.KeyDown += MovePlayer;

            _dispatcherTimer.Tick += GameLifetime;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            _dispatcherTimer.Start();
        }

        private void GameLifetime(object sender, EventArgs e)
        {
            
        }

        public void MovePlayer(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_left.png", UriKind.Relative));
                    if (Canvas.GetLeft(player) > Canvas.GetLeft(land))
                    {
                        Canvas.SetLeft(player, Canvas.GetLeft(player) - player.Width);
                    }
                    footsteps++;
                    break;

                case Key.Right:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_right.png", UriKind.Relative));
                    if (Canvas.GetLeft(player) + player.Width < Canvas.GetLeft(land) + land.Width)
                    {
                        Canvas.SetLeft(player, Canvas.GetLeft(player) + player.Width);
                    }
                    footsteps++;
                    break;

                case Key.Up:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_back.png", UriKind.Relative));
                    if (Canvas.GetTop(player) > Canvas.GetTop(land))
                    {
                        Canvas.SetTop(player, Canvas.GetTop(player) - player.Height);
                    }
                    footsteps++;
                    break;

                case Key.Down:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_front.png", UriKind.Relative));
                    if (Canvas.GetTop(player) + player.Height < Canvas.GetTop(land) + land.Height)
                    {
                        Canvas.SetTop(player, Canvas.GetTop(player) + player.Height);
                    }
                    footsteps++;
                    break;

                default:
                    break;
            }
        }
    }
}
