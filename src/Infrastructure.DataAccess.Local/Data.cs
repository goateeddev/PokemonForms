using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using Domain.Model;
using Domain.Resources;

namespace Infrastructure.DataAccess.Local
{
    public class PokeData
    {
        public static object ReadPokedata(string action, string file_path)
        {
            try
            {
                CsvReader reader = new CsvReader(File.OpenText(file_path));
                object list = null;
                switch (action)
                {
                    case "pokemon":
                        list = reader.GetRecords<Pokemon>().ToList();
                        break;
                    case "type":
                        list = reader.GetRecords<Domain.Model.Type>().ToList();
                        break;
                    case "move":
                        list = reader.GetRecords<Move>().ToList();
                        break;
                    case "pokedex":
                        list = reader.GetRecords<PokedexEntry>().ToList();
                        break;
                    case "poketype":
                        list = reader.GetRecords<PokeType>().ToList();
                        break;
                    case "pokemove":
                        list = reader.GetRecords<PokeMove>().ToList();
                        break;
                    case "balls":
                        list = reader.GetRecords<Ball>().ToList();
                        break;
                    default:
                        break;
                }
                reader.Dispose();
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine("READER ERROR: " + e.ToString());
                return null;
            }
        }

        public static List<Pokemon> ReadPokedata(string file_path)
        {
            return File.ReadAllLines(DefaultFilePaths.pokemon_path).Skip(1).Select(line => GetPokemonField(line)).ToList();
        }

        public static Pokemon GetPokemonField(string csv_line)
        {
            string[] values = csv_line.Split(',');
            Pokemon pokemon = new Pokemon();
            pokemon.id = int.Parse(values[0]);
            pokemon.name = values[1];
            pokemon.level = int.Parse(values[2]);
            pokemon.rarity = int.Parse(values[3]);
            return pokemon;
        }

        public static bool WritePokedex(IEnumerable<PokedexEntry> Pokedex)
        {
            StreamWriter csv = new StreamWriter(DefaultFilePaths.pokedex_path);
            List<string> list = new List<string>();
            try
            {
                if (Pokedex != null)
                {
                    string[] columns = { "id", "name", "level", "rarity", "type", "moves" };
                    list.AddRange(columns);
                    string row = string.Join(",", list.Select(CsvFormatter.Escape));
                    list.Clear();
                    csv.WriteLine(row);
                    try
                    {
                        foreach (PokedexEntry p in Pokedex)
                        {
                            list.Add(p.id);
                            list.Add(p.name);
                            list.Add(p.level.ToString());
                            list.Add(p.rarity.ToString());
                            list.Add(p.type);
                            list.Add(p.moves);
                            row = string.Join(",", list.Select(CsvFormatter.Escape));
                            list.Clear();
                            csv.WriteLine(row);
                        }
                        csv.Close();
                        return true;
                    }
                    catch (Exception)
                    {
                        csv.Close();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Data Error: Pokedex.csv cannot be empty.");
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Data Error: There was an error writing to Pokedex.csv.");
                return false;
            }
        }

        public static bool WriteBalls(List<Ball> Balls)
        {
            StreamWriter csv = new StreamWriter(DefaultFilePaths.balls_path);
            List<string> list = new List<string>();
            try
            {
                if (Balls != null)
                {
                    string[] columns = { "id", "name", "type", "power", "count" };
                    list.AddRange(columns);
                    string row = string.Join(",", list.Select(CsvFormatter.Escape));
                    list.Clear();
                    csv.WriteLine(row);
                    try
                    {
                        foreach (Ball b in Balls)
                        {
                            list.Add(b.id.ToString());
                            list.Add(b.name);
                            list.Add(b.type);
                            list.Add(b.power.ToString());
                            list.Add(b.count.ToString());
                            row = string.Join(",", list.Select(CsvFormatter.Escape));
                            list.Clear();
                            csv.WriteLine(row);
                        }
                        csv.Close();
                        return true;
                    }
                    catch (Exception)
                    {
                        csv.Close();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Data Error: balls.csv cannot be empty.");
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Data Error: There was an error writing to balls.csv.");
                return false;
            }
        }
    }

    public static class CsvFormatter
    {
        private const string QUOTE = "\""; // = "
        private const string ESCAPED_QUOTE = "\"\""; // = "" 
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"' };
        public static string Escape(string s)
        {
            if (s.Contains(QUOTE))
                s = s.Replace(QUOTE, ESCAPED_QUOTE);

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
                s = QUOTE + s + QUOTE;

            return s;
        }
    }
}
