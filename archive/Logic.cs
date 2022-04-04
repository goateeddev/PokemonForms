using ClassLayer;
using DataLayer;
using AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace LogicLayer
{
    public class Logic
    {
        static List<Pokemon> AllPokemon = PokeData.ReadPokedata(DefaultFilePaths.pokemon_path);
        static List<ClassLayer.Type> AllTypes = PokeData.ReadPokedata("type", DefaultFilePaths.types_path) as List<ClassLayer.Type>;
        static List<Move> AllMoves = PokeData.ReadPokedata("move", DefaultFilePaths.moves_path) as List<Move>;

        static List<PokeType> PokeTypes = PokeData.ReadPokedata("poketype", DefaultFilePaths.pokemon_types_path) as List<PokeType>;
        static List<PokeMove> PokeMoves = PokeData.ReadPokedata("pokemove", DefaultFilePaths.pokemon_moves_path) as List<PokeMove>;

        static List<PokedexEntry> Pokedex = PokeData.ReadPokedata("pokedex", DefaultFilePaths.pokedex_path) as List<PokedexEntry>;
        static List<Ball> Balls = PokeData.ReadPokedata("balls", DefaultFilePaths.balls_path) as List<Ball>;

        static Random rand = new Random();

        public static Bag GetBag()
        {
            return new Bag(Balls, Pokedex);
        }

        public static Pokemon GenerateRandomPokemon()
        {
            Random ran = new Random();
            Pokemon pokemon = new Pokemon();
            bool match = false;

            while (!match)
            {
                foreach (Pokemon p in AllPokemon)
                {
                    int range = ran.Next(1, 152 * p.rarity);
                    if (range == p.id)
                    {
                        pokemon = p;
                        match = true;
                    }
                }
            }
            return pokemon;
        }

        public static Image GetPokemonImage(Pokemon pokemon)
        {
            string imgName = "";

            switch (pokemon.id.ToString().Length)
            {
                case 1:
                    imgName = 0 + "" + 0 + "" + pokemon.id;
                    break;
                case 2:
                    imgName = 0 + "" + pokemon.id;
                    break;
                case 3:
                    imgName = "" + pokemon.id;
                    break;
            }
            try
            {
                return Image.FromFile(DefaultFilePaths.pokemon_image_path + imgName + ".png");
            }
            catch (Exception)
            {
                return Image.FromFile(DefaultFilePaths.pokemon_image_path + "000.png");
            }

        }

        public static PokedexEntry GeneratePokedexEntry(Pokemon pokemon)
        {
            PokedexEntry pokedex_entry = new PokedexEntry();
            pokedex_entry.id = pokemon.id.ToString().PadLeft(3, '0');
            pokedex_entry.name = pokemon.name;
            pokedex_entry.level = pokemon.level;
            pokedex_entry.rarity = pokemon.rarity;
            pokedex_entry.type = GetPokemonType(pokemon);
            pokedex_entry.moves = GetPokemonMoves(pokemon);
            return pokedex_entry;
        }

        private static string GetPokemonType(Pokemon pokemon)
        {
            List<string> types = new List<string>();
            foreach (PokeType type in PokeTypes)
            {
                if (pokemon.id == type.pokemon_id)
                {
                    types.Add((from t in AllTypes where t.id == type.type_id select t.name).First().ToString());
                }
            }
            return string.Join(", ", types);
        }

        private static string GetPokemonMoves(Pokemon pokemon)
        {
            IEnumerable<PokeMove> possible_moves = from row in PokeMoves where row.pokemon_id == pokemon.id select (PokeMove)row;
            List<string> move_list = new List<string>();
            Random ran = new Random();
            if (possible_moves.Count() < 4)
            {
                foreach (PokeMove pokemove in possible_moves)
                {
                    string move = (from row in AllMoves where row.id == pokemove.move_id select row.name).First().ToString();
                    move_list.Add(move);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    bool match = true;
                    int id = ran.Next(1, 166);
                    foreach (PokeMove pokemove in possible_moves)
                    {
                        if (pokemove.move_id == id)
                        {
                            string move = (from row in AllMoves where row.id == pokemove.move_id select row.name).First().ToString();
                            if (!move_list.Contains(move))
                            {
                                move_list.Add(move);
                                match = false;
                            }
                            else if (move_list.Contains(move))
                            {
                                match = true;
                            }
                        }
                    }
                    if (match)
                    {
                        i--;
                    }
                }
                if (move_list.Count > 4)
                {
                    for (int i = 4; i < move_list.Count; i++)
                    {
                        move_list.RemoveAt(i);
                        i--;
                    }
                }
            }
            return string.Join(", ", move_list);
        }

        public static string ThrowBall(Ball ball, Pokemon pokemon, int tries)
        {
            ball.count--;
            PokeData.WriteBalls(Balls);

            int number = rand.Next(1, 10 * pokemon.rarity);

            if (Ran(pokemon, tries))
            {
                return "ran";
            }
            else if (number % ball.power == 0)
            {
                PokedexEntry entry = GeneratePokedexEntry(pokemon);
                UpdatePokedex(entry);
                return "caught";
            }
            else if (number % ball.power != 0)
            {
                return "broke";
            }
            else return null;
        }

        private static bool Ran(Pokemon pokemon, int tries)
        {
            bool outcome = false;

            switch (pokemon.rarity)
            {
                case 1:
                    outcome = tries >= 10;
                    break;
                case 2:
                    outcome = tries >= 10;
                    break;
                case 5:
                    outcome = tries >= 10;
                    break;
                case 10:
                    outcome = tries >= 20;
                    break;
                case 15:
                    outcome = tries >= 20;
                    break;
                case 20:
                    outcome = tries >= 30;
                    break;
                case 50:
                    outcome = tries >= 30;
                    break;
                case 75:
                    outcome = tries >= 40;
                    break;
                case 100:
                    outcome = tries >= 40;
                    break;
            }
            return outcome;
        }

        private static bool UpdatePokedex(PokedexEntry pokedex_entry)
        {
            Pokedex.Add(pokedex_entry);
            return PokeData.WritePokedex(Pokedex) ? true : false;
        }

        public static string MakeFirstUpper(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
