using Application.Services.Ports.Outbound.DataAccess;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Application.Services
{
    public class PokemonApplicationService
    {
        private readonly IPokemonDao _pokemonDao;

        public PokemonApplicationService(IPokemonDao pokemonDao)
        {
            _pokemonDao = pokemonDao;
        }

        public List<Pokemon> GetAll()
        {
            return _pokemonDao.GetAll();
        }

        public Pokemon GenerateRandomPokemon()
        {
            var ran = new Random();
            var match = false;
            var allPokemon = GetAll();

            while (!match)
            {
                foreach (var pokemon in allPokemon)
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

        public static bool IntersectionDetected(Image source, List<Image> obstructions)
        {
            return obstructions.Any(obstruction => IntersectionDetected(source, obstruction));
        }

        public static bool IntersectionDetected(Image source, Image destination)
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