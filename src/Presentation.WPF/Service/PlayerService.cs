using Application.Services;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Presentation.WPF.Service
{
    public sealed class PlayerService
    {
        public static Image InitialisePlayer(Canvas canvas)
        {
            var player = new Image
            {
                Name = "Player",
                Source = new BitmapImage(new Uri($"Assets\\img\\characters\\player_front.png", UriKind.Relative)),
                Width = 25,
                Height = 25
            };

            Canvas.SetLeft(player, 175);
            Canvas.SetTop(player, 17);

            canvas.Children.Add(player);

            return player;
        }

        public static void MovePlayer(Key key, Image player, UserControl landscape, List<Image> obstructions)
        {
            var originalTop = Canvas.GetTop(player);
            var originalLeft = Canvas.GetLeft(player);
            switch (key)
            {
                case Key.Left:
                    player.Source = new BitmapImage(new Uri("Assets\\img\\characters\\player_left.png", UriKind.Relative));
                    if (Canvas.GetLeft(player) > Canvas.GetLeft(landscape))
                    {
                        var newPosition = Canvas.GetLeft(player) - player.Width;
                        Canvas.SetLeft(player, newPosition);
                        if (PokemonService.IntersectionDetected(player, obstructions, 17, 175))
                        {
                            Canvas.SetLeft(player, originalLeft);
                        }
                    }
                    break;

                case Key.Right:
                    player.Source = new BitmapImage(new Uri("Assets\\img\\characters\\player_right.png", UriKind.Relative));
                    if (Canvas.GetLeft(player) + player.Width < Canvas.GetLeft(landscape) + landscape.Width)
                    {
                        var newPosition = Canvas.GetLeft(player) + player.Width;
                        Canvas.SetLeft(player, newPosition);
                        if (PokemonService.IntersectionDetected(player, obstructions, 17, 175))
                        {
                            Canvas.SetLeft(player, originalLeft);
                        }
                    }
                    break;

                case Key.Up:
                    player.Source = new BitmapImage(new Uri("Assets\\img\\characters\\player_back.png", UriKind.Relative));
                    if (Canvas.GetTop(player) > Canvas.GetTop(landscape))
                    {
                        var newPosition = Canvas.GetTop(player) - player.Height;
                        Canvas.SetTop(player, newPosition);
                        if (PokemonService.IntersectionDetected(player, obstructions, 17, 175))
                        {
                            Canvas.SetTop(player, originalTop);
                        }
                    }
                    break;

                case Key.Down:
                    player.Source = new BitmapImage(new Uri("Assets\\img\\characters\\player_front.png", UriKind.Relative));
                    if (Canvas.GetTop(player) + player.Height < Canvas.GetTop(landscape) + landscape.Height)
                    {
                        var newPosition = Canvas.GetTop(player) + player.Height;
                        Canvas.SetTop(player, newPosition);
                        if (PokemonService.IntersectionDetected(player, obstructions, 17, 175))
                        {
                            Canvas.SetTop(player, originalTop);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}