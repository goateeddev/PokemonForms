using System.Collections.Generic;

namespace ClassLayer
{
    public class Player
    {
        public string name { get; set; }
        public string gender { get; set; }
        public int level { get; set; }

        public Player(string name, string gender, int level)
        {
            this.name = name;
            this.gender = gender;
            this.level = level;
        }

        public Player()
        {
        }
    }

    public class Ball
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int power { get; set; }
        public int count { get; set; }

        public Ball(int id, string name, string type, int power, int count)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.power = power;
            this.count = count;
        }

        public Ball()
        {
        }
    }

    public class Bag
    {
        public List<Ball> balls { get; set; }
        public List<PokedexEntry> pokedex { get; set; }

        public Bag(List<Ball> balls, List<PokedexEntry> pokedex)
        {
            this.balls = balls;
            this.pokedex = pokedex;
        }

        public Bag()
        {
        }
    }

    public class PokedexEntry : Pokemon
    {
        public new string id { get; set; }
        public string type { get; set; }
        public string moves { get; set; }

        public PokedexEntry(string id, string name, int level, int rarity, string type, string moves)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.moves = moves;
            this.level = level;
            this.rarity = rarity;
        }

        public PokedexEntry()
        {
        }
    }

    public class Pokemon
    {
        public int id { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public int rarity { get; set; }

        public Pokemon(int id, string name, int level, int rarity)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.rarity = rarity;
        }

        public Pokemon()
        {
        }
    }

    public class Move
    {
        public int id { get; set; }
        public string name { get; set; }
        public int power { get; set; }
        public int type_id { get; set; }

        public Move(int id, string name, int power, int type_id)
        {
            this.id = id;
            this.name = name;
            this.power = power;
            this.type_id = type_id;
        }

        public Move()
        {
        }
    }

    public class Type
    {
        public int id { get; set; }
        public string name { get; set; }

        public Type(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Type()
        {
        }
    }

    public class PokeType
    {
        public int pokemon_id { get; set; }
        public int type_id { get; set; }
        public int slot { get; set; }

        public PokeType(int pokemon_id, int type_id, int slot)
        {
            this.pokemon_id = pokemon_id;
            this.type_id = type_id;
            this.slot = slot;
        }

        public PokeType()
        {
        }
    }

    public class PokeMove
    {
        public int pokemon_id { get; set; }
        public int move_id { get; set; }
        public int pokemon_lvl { get; set; }

        public PokeMove(int pokemon_id, int move_id, int pokemon_lvl)
        {
            this.pokemon_id = pokemon_id;
            this.move_id = move_id;
            this.pokemon_lvl = pokemon_lvl;
        }

        public PokeMove()
        {
        }
    }
}
