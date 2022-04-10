using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Application.Services
{
    public sealed class PokemonService
    {
        private readonly PokemonApplicationService _pokemonApplicationService;

        public PokemonService(PokemonApplicationService pokemonApplicationService)
        {
            _pokemonApplicationService = pokemonApplicationService;
        }

        public static Image GetCurrentPokemon(Canvas canvas)
        {
            return (Image)canvas.Children.Cast<UIElement>()
                .FirstOrDefault(element => element is Image && ((Image)element).Name == "CurrentPokemon");
        }

        public static bool IntersectionDetected(Image source, List<Image> obstructions, double sourceBufferTop = 0, double sourceBufferLeft = 0)
        {
            return obstructions.Any(obstruction => IntersectionDetected(source, obstruction, sourceBufferTop, sourceBufferLeft));
        }

        public static bool IntersectionDetected(Image source, Image destination, double sourceBufferTop = 0, double sourceBufferLeft = 0)
        {
            var sourceTop = Canvas.GetTop(source) - sourceBufferTop;
            var sourceBottom = sourceTop + source.Height;
            var sourceLeft = Canvas.GetLeft(source) - sourceBufferLeft;
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

        public void DrawRandomPokemon(Image player, UserControl landscape, Canvas canvas, List<Image> obstructions, ref bool pokemonOnScreen)
        {
            var pokemon = _pokemonApplicationService.GenerateRandomPokemon();
            var pokemonElement = new Image
            {
                Source = new BitmapImage(new Uri($"Assets\\img\\pokemon\\{pokemon.PokemonId}.png", UriKind.Relative)),
                Width = 39,
                Height = 45,
                Name = "CurrentPokemon"
            };

            RandomisePositionOnCanvas(pokemonElement, landscape);
            canvas.Children.Add(pokemonElement);
            pokemonOnScreen = true;

            if (Canvas.GetLeft(pokemonElement) < Canvas.GetLeft(landscape) ||
                Canvas.GetLeft(pokemonElement) + pokemonElement.Width > Canvas.GetLeft(landscape) + landscape.Width ||
                Canvas.GetTop(pokemonElement) < Canvas.GetTop(landscape) ||
                Canvas.GetTop(pokemonElement) + pokemonElement.Height > Canvas.GetTop(landscape) + landscape.Height)
            {
                canvas.Children.Remove(pokemonElement);
                pokemonOnScreen = false;
                DrawRandomPokemon(player, landscape, canvas, obstructions, ref pokemonOnScreen);
            }
            else if (IntersectionDetected(pokemonElement, obstructions, 17, 175) || IntersectionDetected(pokemonElement, player))
            {
                canvas.Children.Remove(pokemonElement);
                pokemonOnScreen = false;
                DrawRandomPokemon(player, landscape, canvas, obstructions, ref pokemonOnScreen);
            }
        }

        public static void RandomisePositionOnCanvas(Image element, UserControl canvas)
        {
            var canvasTop = (int)Canvas.GetTop(canvas);
            var canvasLeft = (int)Canvas.GetLeft(canvas);

            var maxTop = canvasTop + (int)canvas.Height - (int)element.Height;
            var maxLeft = canvasLeft + (int)canvas.Width - (int)element.Width;

            if (canvasTop >= maxTop || canvasLeft >= maxLeft)
            {
                RandomisePositionOnCanvas(element, canvas);
            }

            var randomTop = new Random().Next(canvasTop, maxTop);
            var randomLeft = new Random().Next(canvasLeft, maxLeft);

            Canvas.SetTop(element, randomTop);
            Canvas.SetLeft(element, randomLeft);
        }
    }
}