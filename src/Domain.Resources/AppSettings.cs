namespace Domain.Resources
{
    public static class DefaultFilePaths
    {
        public static string pokedex_path = @"csv\Pokedex.csv";
        public static string pokedex_empty_path = @"csv\Pokedex_empty.csv";

        public static string pokemon_path = @"csv\pokemon.csv";
        public static string types_path = @"csv\types.csv";
        public static string moves_path = @"csv\moves.csv";
        public static string balls_path = @"csv\balls.csv";

        public static string pokemon_types_path = @"csv\pokemon_types.csv";
        public static string pokemon_moves_path = @"csv\pokemon_moves.csv";

        public static string pokemon_image_path = @"images\pokemon\";
        public static string image_path = @"images\";
    }

    public static class DefaultsStrings
    {
        public static string msg_welcome = "Welcome to Pokemon Forms Application!";
        public static string msg_starter = "You have a choice of 3 starter pokemon!";
        public static string msg_begin = "Your adventure is about to begin...";
        public static string msg_poff = "Piss off then.";

        public static string get_gender = "Are you a boy or are you a girl?";
        public static string get_name = "And what is your name?";
        public static string get_embark = "Are you ready to embark on a pokemon adventure?!";

        public static string show_starter_choices = "1 = Bulbasaur" + "\n" + "2 = Charmander" + "\n" + "3 = Squirtle";

        public static string error_yes_no = "Huh? It's a yes or no question bro";
        public static string error_not_option = "Thats not one of the options I gave you...";
        public static string error_username = "I don't like that username. Choose another one.";
        public static string error_no_balls = "You have none left. Choose a different one.";
    }
}
