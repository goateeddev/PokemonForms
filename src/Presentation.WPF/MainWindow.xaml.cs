﻿using Application.Services.Ports.Outbound.DataAccess;
using Domain.Model;
using Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Presentation.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IPokemonDao _pokemonDao;

        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        private readonly DispatcherTimer _spawnTimer = new DispatcherTimer();
        private readonly DispatcherTimer _despawnTimer = new DispatcherTimer();

        private readonly List<Image> Obstructions = new List<Image>();
        private List<Pokemon> AllPokemon = new List<Pokemon>(151);

        private bool encounter;
        private bool pokemonOnScreen;
        private bool spawnTimerActive;

        public MainWindow(IPokemonDao pokemonDao)
        {
            _pokemonDao = pokemonDao;

            InitializeComponent();
            InitialiseGame();
        }

        private void InitialiseGame()
        {
            canvas.Focus();
            canvas.KeyDown += MovePlayer;

            var allPokemon = _pokemonDao.GetAll();
            AllPokemon.AddRange(allPokemon);

            foreach (var child in canvas.Children.Cast<Image>().Where(image => image.Uid == "obstruction"))
            {
                Obstructions.Add(child);
            }

            _gameTimer.Tick += GameLifetime;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Start();
        }

        private void GameLifetime(object sender, EventArgs e)
        {
            if (!spawnTimerActive && !pokemonOnScreen)
            {
                SpawnPokemon();
            }
        }

        private void SpawnPokemon()
        {
            _spawnTimer.Tick += new EventHandler(SpawnEventProcessor);
            _spawnTimer.Interval = new TimeSpan(0, 0, new Random().Next(5, 20));
            _spawnTimer.Start();
            spawnTimerActive = true;
        }

        private void SpawnEventProcessor(object sender, EventArgs e)
        {
            if (!pokemonOnScreen)
            {
                DrawRandomPokemon();
            }
            _spawnTimer.Stop();
            spawnTimerActive = false;
        }

        private void DrawRandomPokemon()
        {
            var pokemon = GenerateRandomPokemon();
            var pokemonElement = new Image
            {
                Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + $"pokemon\\{pokemon.PokemonId}.png", UriKind.Relative)),
                Width = 39,
                Height = 45,
                Name = "CurrentPokemon"
            };

            AddToCanvas(pokemonElement);
            canvas.Children.Add(pokemonElement);
            pokemonOnScreen = true;

            if (Canvas.GetLeft(pokemonElement) < Canvas.GetLeft(land) ||
                Canvas.GetLeft(pokemonElement) + pokemonElement.Width > Canvas.GetLeft(land) + land.Width ||
                Canvas.GetTop(pokemonElement) < Canvas.GetTop(land) ||
                Canvas.GetTop(pokemonElement) + pokemonElement.Height > Canvas.GetTop(land) + land.Height)
            {
                canvas.Children.Remove(pokemonElement);
                pokemonOnScreen = false;
                DrawRandomPokemon();
            }
            else if (IntersectionDetected(pokemonElement) || IntersectionDetected(pokemonElement, player))
            {
                canvas.Children.Remove(pokemonElement);
                pokemonOnScreen = false;
                DrawRandomPokemon();
            }

            PokemonDisappearTimer();
        }

        public void MovePlayer(object sender, KeyEventArgs e)
        {
            var originalTop = Canvas.GetTop(player);
            var originalLeft = Canvas.GetLeft(player);
            switch (e.Key)
            {
                case Key.Left:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_left.png", UriKind.Relative));
                    if (Canvas.GetLeft(player) > Canvas.GetLeft(land))
                    {
                        var newPosition = Canvas.GetLeft(player) - player.Width;
                        Canvas.SetLeft(player, newPosition);
                        if (IntersectionDetected(player))
                        {
                            Canvas.SetLeft(player, originalLeft);
                        }
                    }
                    break;

                case Key.Right:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_right.png", UriKind.Relative));
                    if (Canvas.GetLeft(player) + player.Width < Canvas.GetLeft(land) + land.Width)
                    {
                        var newPosition = Canvas.GetLeft(player) + player.Width;
                        Canvas.SetLeft(player, newPosition);
                        if (IntersectionDetected(player))
                        {
                            Canvas.SetLeft(player, originalLeft);
                        }
                    }
                    break;

                case Key.Up:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_back.png", UriKind.Relative));
                    if (Canvas.GetTop(player) > Canvas.GetTop(land))
                    {
                        var newPosition = Canvas.GetTop(player) - player.Height;
                        Canvas.SetTop(player, newPosition);
                        if (IntersectionDetected(player))
                        {
                            Canvas.SetTop(player, originalTop);
                        }
                    }
                    break;

                case Key.Down:
                    player.Source = new BitmapImage(new Uri(DefaultFilePaths.image_path + "characters\\player_front.png", UriKind.Relative));
                    if (Canvas.GetTop(player) + player.Height < Canvas.GetTop(land) + land.Height)
                    {
                        var newPosition = Canvas.GetTop(player) + player.Height;
                        Canvas.SetTop(player, newPosition);
                        if (IntersectionDetected(player))
                        {
                            Canvas.SetTop(player, originalTop);
                        }
                    }
                    break;

                default:
                    break;
            }

            var pokemon = canvas.Children.Cast<Image>().FirstOrDefault(child => child.Name == "CurrentPokemon");
            if (pokemonOnScreen && IntersectionDetected(player, pokemon) && !encounter)
            {
                Encounter();
                encounter = false;
            }
        }

        private void PokemonDisappearTimer()
        {
            _despawnTimer.Tick += new EventHandler(DisappearTimerEventHandler);
            _despawnTimer.Interval = new TimeSpan(0, 0, 8);
            _despawnTimer.IsEnabled = true;
            _despawnTimer.Start();
        }

        private void DisappearTimerEventHandler(object sender, EventArgs e)
        {
            if (!encounter)
            {
                var pokemon = canvas.Children.Cast<Image>().FirstOrDefault(child => child.Name == "CurrentPokemon");
                canvas.Children.Remove(pokemon);
                pokemonOnScreen = false;
                SpawnPokemon();
            }
            _despawnTimer.Stop();
        }

        public Pokemon GenerateRandomPokemon()
        {
            var ran = new Random();
            var match = false;

            while (!match)
            {
                foreach (var pokemon in AllPokemon)
                {
                    int range = ran.Next(1, 152 * pokemon.Rarity);
                    if (range == pokemon.Id)
                    {
                        match = true;
                        return pokemon;
                    }
                }
            }

            return default;
        }

        private void Encounter()
        {
            encounter = true;
            //throw new NotImplementedException();
        }

        private void AddToCanvas(Image element)
        {
            var canvasTop = (int)Canvas.GetTop(land);
            var canvasLeft = (int)Canvas.GetLeft(land);

            var randomTop = new Random().Next(canvasTop, canvasTop + (int)land.Height - (int)element.Height);
            var randomLeft = new Random().Next(canvasLeft, canvasLeft + (int)land.Width - (int)element.Width);

            Canvas.SetTop(element, randomTop);
            Canvas.SetLeft(element, randomLeft);
        }

        private bool IntersectionDetected(Image source)
        {
            return Obstructions.Any(obstruction => IntersectionDetected(source, obstruction));
        }

        private bool IntersectionDetected(Image source, Image destination)
        {
            var sourceTop = Canvas.GetTop(source);
            var sourceBottom = sourceTop + source.Height;
            var sourceLeft = Canvas.GetLeft(source);
            var sourceRight = sourceLeft + source.Width;

            var destinationTop = Canvas.GetTop(destination);
            var destinationBottom = destinationTop + destination.Height;
            var destinationLeft = Canvas.GetLeft(destination);
            var destinationRight = destinationLeft + destination.Width;

            var topIntersects = sourceTop > destinationTop && sourceTop < destinationBottom &&
                sourceRight > destinationLeft && sourceRight < destinationRight;

            var bottomIntersects = sourceBottom > destinationTop && sourceBottom < destinationBottom &&
                sourceRight > destinationLeft && sourceRight < destinationRight;

            var leftIntersects = sourceLeft > destinationLeft && sourceLeft < destinationRight &&
                sourceTop >= destinationTop && sourceTop < destinationBottom;

            var rightIntersects = sourceRight > destinationLeft && sourceRight < destinationRight &&
                sourceTop > destinationTop && sourceTop < destinationBottom;

            return topIntersects || bottomIntersects || leftIntersects || rightIntersects;
        }
    }
}
