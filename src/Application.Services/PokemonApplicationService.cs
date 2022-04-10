using Application.Services.Ports.Outbound.DataAccess;
using Domain.Model;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public sealed class PokemonApplicationService
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
    }
}