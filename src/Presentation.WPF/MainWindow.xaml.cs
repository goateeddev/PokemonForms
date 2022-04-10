using Application.Services;
using Domain.Model;
using Domain.Resources;
using Presentation.WPF.Service;
using Presentation.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Presentation.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PokemonService _pokemonService;
        private readonly PokemonApplicationService _pokemonApplicationService;

        private readonly DispatcherTimer _gameTimer = new();
        private readonly DispatcherTimer _spawnTimer = new();
        private readonly DispatcherTimer _despawnTimer = new();

        private Image __player;
        private List<Image> Obstructions = new();
        private readonly List<Pokemon> AllPokemon = new(151);

        private bool encounter;
        private bool pokemonOnScreen;
        private bool spawnTimerActive;

        public MainWindow(PokemonService pokemonService, PokemonApplicationService pokemonApplicationService) 
        {
            _pokemonService = pokemonService;
            _pokemonApplicationService = pokemonApplicationService;

            InitializeComponent();
            InitialiseGame();
        }

        private void InitialiseGame()
        {
            gameText.Text = DisplayText.Game_Description;

            MainCanvas.Focus();
            MainCanvas.KeyDown += MovePlayer;

            __player = PlayerService.InitialisePlayer(MainCanvas);
            Obstructions = LandscapeService.ConfigureObstructions(LandView.Canvas).ToList();

            var allPokemon = _pokemonApplicationService.GetAll();
            AllPokemon.AddRange(allPokemon);

            _gameTimer.Tick += GameLifetime;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Start();
        }

        private void GameLifetime(object sender, EventArgs e)
        {
            if (!spawnTimerActive && !pokemonOnScreen)
            {
                StartSpawnTimer();
            }
        }

        private void StartSpawnTimer()
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
                _pokemonService.DrawRandomPokemon(__player, Landscape, MainCanvas, Obstructions, ref pokemonOnScreen);
                StartDespawnTimer();
            }
            _spawnTimer.Stop();
            spawnTimerActive = false;
        }

        private void MovePlayer(object sender, KeyEventArgs e)
        {
            PlayerService.MovePlayer(e.Key, __player, Landscape, Obstructions);

            var pokemon = PokemonService.GetCurrentPokemon(MainCanvas);

            if (pokemonOnScreen && PokemonService.IntersectionDetected(__player, pokemon) && !encounter)
            {
                Encounter();
                encounter = false;
            }
        }

        private void StartDespawnTimer()
        {
            _despawnTimer.Tick += new EventHandler(DespawnTimerEventHandler);
            _despawnTimer.Interval = new TimeSpan(0, 0, 5);
            _despawnTimer.IsEnabled = true;
            _despawnTimer.Start();
        }

        private void DespawnTimerEventHandler(object sender, EventArgs e)
        {
            if (!encounter)
            {
                var pokemon = PokemonService.GetCurrentPokemon(MainCanvas);
                MainCanvas.Children.Remove(pokemon);
                pokemonOnScreen = false;
                StartSpawnTimer();
            }
            _despawnTimer.Stop();
        }

        private void Encounter()
        {
            encounter = true;
        }
    }
}