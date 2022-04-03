using Domain.Model;

namespace Infrastructure.DataAccess.MySQL.Models
{
    internal sealed class PokemonDbModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LowestLevel { get; set; }
        public int? EvolutionLevel { get; set; }
        public int Rarity { get; set; }

        internal Pokemon ToModel()
        {
            return new Pokemon
            {
                Id = Id,
                Name = Name,
                LowestLevel = LowestLevel,
                EvolutionLevel = EvolutionLevel,
                Rarity = Rarity
            };
        }
    }
}