using Application.Services.Ports.Outbound.DataAccess;
using Domain.Core.Extensions;
using Domain.Model;
using Infrastructure.DataAccess.MySQL.Models;
using System.Collections.Generic;
using System.Linq;
using TradeUB.Infrastructure.DataAccess.Database;

namespace Infrastructure.DataAccess.MySQL
{
    public sealed class PokemonDao : IPokemonDao
    {
        private readonly DbClient _dbClient;

        public PokemonDao(DbClient dbClient)
        {
            _dbClient = dbClient;
        }

        public List<Pokemon> GetAll()
        {
            var sql = @"SELECT pokemon_id AS id,
                               name,
                               lowest_level,
                               evolution_level,
                               rarity
                        FROM pokemonforms.pokemon;";

            return _dbClient.Query<PokemonDbModel>(sql, null).ToList(dbm => dbm.ToModel());
        }
    }
}