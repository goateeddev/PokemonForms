using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using AppSettings;
using ClassLayer;
using LogicLayer;

namespace PokemonFormsApplication
{
    public partial class MainForm : Form
    {
        // Intro initialisations
        int next_count;
        string playerName, playerGender;

        // Game initialisations
        int mHorizontal, mVertical, footsteps, tries, pokemonPanel, playerPanel;
        bool intersects, bLeft, bRight, bTop, bBottom, pokemonOnScreen, encounter;
        Panel currentPanel = new Panel();
        PictureBox player = new PictureBox();
        PictureBox pokemonPictureBox = new PictureBox();
        List<PictureBox> CurrentPanelPictureBoxes = new List<PictureBox>();
        List<PictureBox> Panel1PictureBoxes = new List<PictureBox>();
        List<PictureBox> PokemonPositionBoxes = new List<PictureBox>();
        Random rand = new Random();
        Timer timer = new Timer();
        Timer pokeTimer = new Timer();
        Timer toastTimer = new Timer();
        Pokemon currentPokemon = new Pokemon();
        Bag Backpack = Logic.GetBag();

        public MainForm() 
        {
            InitializeComponent();
            Introduction();
            //InitialiseGame();
        }

        // INTRODUCTION
        private void Introduction()
        {
            next_count = 0;
            lbl_intro.Text = DefaultsStrings.msg_welcome;
            tb_intro.Visible = false;
            btn_enter.Visible = false;
            btn_left.Visible = false;
            btn_right.Visible = false;
        }

        private void btn_left_Click(object sender, EventArgs e)
        {
            if (btn_left.Text == "Boy")
            {
                playerGender = btn_left.Text;
                btn_next.Enabled = false;
                lbl_intro.Text = DefaultsStrings.get_name;
                btn_left.Visible = false;
                btn_right.Visible = false;
                tb_intro.Visible = true;
                btn_enter.Visible = true;
                ActiveControl = tb_intro;
            }
            else if (btn_left.Text == "Yes")
            {
                lbl_intro.Text = DefaultsStrings.msg_begin;
                lbl_intro.AutoSize = false;
                tb_intro.Visible = false;
                btn_enter.Visible = false;
                btn_left.Visible = false;
                btn_right.Visible = false;
                btn_next.Enabled = true;
            }
        }

        private void btn_right_Click(object sender, EventArgs e)
        {
            if (btn_right.Text == "Girl")
            {
                playerGender = btn_right.Text;
                btn_next.Enabled = false;
                lbl_intro.Text = DefaultsStrings.get_name;
                btn_left.Visible = false;
                btn_right.Visible = false;
                tb_intro.Visible = true;
                btn_enter.Visible = true;
                ActiveControl = tb_intro;
            }
            else if (btn_right.Text == "No")
            {
                DialogResult result = MessageBox.Show("Are you sure you wanna exit the game, pussy boy?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            next_count += 1;
            switch (next_count)
            {
                case 1:
                    btn_next.Enabled = false;
                    lbl_intro.Text = DefaultsStrings.get_gender;
                    btn_left.Visible = true;
                    btn_left.Text = "Boy";
                    btn_right.Visible = true;
                    btn_right.Text = "Girl";
                    break;
                case 2:
                    InitialiseGame();
                    break;
            }
        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            string response = tb_intro.Text;
            if (IsValidName(response))
            {
                playerName = response;
                lbl_intro.Text = "Hello " + playerName + "!  " + DefaultsStrings.get_embark;
                lbl_intro.MinimumSize = new Size(417, 40);
                lbl_intro.MaximumSize = new Size(417, 40);
                lbl_intro.AutoSize = true;
                btn_next.Enabled = false;
                btn_left.Visible = true;
                btn_left.Text = "Yes";
                btn_right.Visible = true;
                btn_right.Text = "No";
                tb_intro.Visible = false;
                btn_enter.Visible = false;
            }
            else
            {
                MessageBox.Show(DefaultsStrings.error_username, "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidName(string username)
        {
            Regex ValidUsername = new Regex(@"^(?=[a-zA-Z])[-\w.]{0,23}([a-zA-Z\d]|(?<![-.])_)$");

            if (ValidUsername.IsMatch(username) && username.Length > 2 && username.Length < 16)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void tb_intro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_enter.PerformClick();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        // GAME INITIALISATIONS
        private void InitialiseGame()
        {
            pnl_intro.Visible = false;

            mVertical = 16; // = 8; //
            mHorizontal = 12; // = 6; //
            footsteps = 0;
            bLeft = bTop = bRight = bLeft = false;

            intersects = false;
            pokemonOnScreen = false;
            encounter = false;

            pnl_sandygrass.Visible = true;
            currentPanel = pnl_sandygrass;
            SetPictureBoxList(1, 41);

            pb_cover.Visible = false;
            lbl_toast.BringToFront();

            PopulatePokedexListView();
            DrawPlayer();
            PokemonAppearTimer();
        }

        private void DrawPlayer()
        {
            player.Size = new Size(17, 23);
            player.Location = new Point(12, 16);
            player.BackColor = Color.Transparent;
            player.Image = Image.FromFile(DefaultFilePaths.image_path + "characters/player-front.png");
            currentPanel.Controls.Add(player);
        }

        private void PokemonAppearTimer()
        {
            timer.Tick += new EventHandler(AppearTimerEventProcessor);
            timer.Interval = rand.Next(5000, 20000);
            timer.Enabled = true;
            timer.Start();
        }

        private void AppearTimerEventProcessor(object sender, EventArgs e)
        {
            //timer.Interval = rand.Next(5000, 20000);
            if (!pokemonOnScreen)
            {
                DrawRandomPokemon();
            }
            timer.Stop();
        }

        private void SetPictureBoxList(int min, int max)
        {
            CurrentPanelPictureBoxes.Clear();
            // Sandy Grass Panel
            for (int i = min; i < max + 1; i++)
            {
                CurrentPanelPictureBoxes.Add((PictureBox)Controls.Find("pictureBox" + i, true)[0]);
            }
        }

        // MAIN GAME PLAY
        private void DrawRandomPokemon()
        {
            currentPokemon = Logic.GenerateRandomPokemon();
            pokemonPictureBox.Image = Logic.GetPokemonImage(currentPokemon);
            pokemonPictureBox.Size = new Size(39, 45);
            pokemonPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pokemonPictureBox.BackColor = Color.Transparent;
            pokemonPictureBox.Top = rand.Next(0, 423);
            pokemonPictureBox.Left = rand.Next(0, 423);
            pokemonPictureBox.Visible = true;
            currentPanel.Controls.Add(pokemonPictureBox);
            player.BringToFront();
            pokemonOnScreen = true;

            foreach (PictureBox pb in CurrentPanelPictureBoxes)
            {
                if (pokemonPictureBox.Location.X <= -3 || pokemonPictureBox.Location.X >= 402 || pokemonPictureBox.Location.Y <= -3 || pokemonPictureBox.Location.Y >= 402)
                {
                    currentPanel.Controls.Remove(pokemonPictureBox);
                    pokemonOnScreen = false;
                    DrawRandomPokemon();
                }
                else if (IntersectionDetected(pb, pokemonPictureBox) || IntersectionDetected(pb, player))
                {
                    currentPanel.Controls.Remove(pokemonPictureBox);
                    pokemonOnScreen = false;
                    DrawRandomPokemon();
                }
            }

            PokemonDisappearTimer();

            if (currentPanel == pnl_sandygrass)
            {
                pokemonPanel = 1;
            }
            else if (currentPanel == pnl_town)
            {
                pokemonPanel = 2;
            }
            else if (currentPanel == pnl_cave)
            {
                pokemonPanel = 3;
            }
        }

        private void PokemonDisappearTimer()
        {
            pokeTimer.Tick += new EventHandler(DisappearTimerEventHandler);
            pokeTimer.Interval = 8000;
            pokeTimer.Enabled = true;
            pokeTimer.Start();
        }

        private void DisappearTimerEventHandler(object sender, EventArgs e)
        {
            if (!encounter)
            {
                pokemonPictureBox.Visible = false;
                pokemonOnScreen = false;
                PokemonAppearTimer();
            }
            pokeTimer.Stop();
        }

        private void PanelBorderCheck(PictureBox pb)
        {
            if (pb.Location.X <= 0)
            {
                bLeft = true;
            }
            if (pb.Location.X >= 400)
            {
                bRight = true;
            }
            if (pb.Location.Y <= 0)
            {
                bTop = true;
            }
            if (pb.Location.Y >= 400)
            {
                bBottom = true;
            }
        }

        private bool IntersectionDetected(List<PictureBox> PictureBoxes)
        {
            foreach (PictureBox pb in PictureBoxes)
            {
                if (player.Bounds.IntersectsWith(pb.Bounds))
                {
                    intersects = true;
                    break;
                }
                else
                {
                    intersects = false;
                }
            }
            return intersects;
        }

        private bool IntersectionDetected(PictureBox pb1, PictureBox pb2)
        {
            if (pb1.Bounds.IntersectsWith(pb2.Bounds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void pb_pokedex_Click(object sender, EventArgs e)
        {
            if (pnl_pokedex.Visible)
            {
                pnl_pokedex.Visible = false;
            }
            else
            {
                pnl_pokedex.Visible = true;
            }
        }

        private void PopulatePokedexListView()
        {
            foreach (PokedexEntry p in Backpack.pokedex)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = Convert.ToInt16(p.id);
                item.Text = p.id.ToString() + " " + p.name;
                item.SubItems.Add("Type: " + p.type);
                item.SubItems.Add("Moves: " + p.moves);
                lv_pokedex.Items.Add(item);
            }
        }

        private void UpdatePokedexListView()
        {
            PokedexEntry p = Logic.GeneratePokedexEntry(currentPokemon);
            ListViewItem item = new ListViewItem();
            item.ImageIndex = Convert.ToInt16(p.id);
            item.Text = p.id.ToString() + " " + p.name;
            item.SubItems.Add("Type: " + p.type);
            item.SubItems.Add("Moves: " + p.moves);
            lv_pokedex.Items.Add(item);
        }

        private void ChangeMap(Panel panelFrom, Panel panelTo, int mapTransition)
        {
            panelFrom.Visible = false;
            panelTo.Visible = true;
            panelTo.Controls.Add(player);
            currentPanel = panelTo;
            bLeft = bRight = bTop = bBottom = false;
            switch (mapTransition)
            {
                case 12:
                    player.Location = new Point(0, 240);
                    SetPictureBoxList(85, 96);
                    break;
                case 13:
                    player.Location = new Point(60, 16);
                    SetPictureBoxList(42, 84);
                    break;
                case 21:
                    player.Location = new Point(396, 240);
                    SetPictureBoxList(1, 41);
                    break;
                case 31:
                    player.Location = new Point(72, 400);
                    SetPictureBoxList(1, 41);
                    break;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && player.Location.X >= 400 && (player.Location.Y >= 231 && player.Location.Y <= 261))
            {
                if (currentPanel == pnl_sandygrass) ChangeMap(pnl_sandygrass, pnl_town, 12);
            }
            else if (e.KeyCode == Keys.Down && player.Location.Y >= 400 && (player.Location.X >= 44 && player.Location.X <= 81))
            {
                if (currentPanel == pnl_sandygrass) ChangeMap(pnl_sandygrass, pnl_cave, 13);
            }
            else if (e.KeyCode == Keys.Left && player.Location.X <= 0 && (player.Location.Y >= 224 && player.Location.Y <= 256))
            {
                if (currentPanel == pnl_town) ChangeMap(pnl_town, pnl_sandygrass, 21);
            }
            else if (e.KeyCode == Keys.Up && player.Location.Y <= 0 && (player.Location.X >= 47 && player.Location.X <= 77))
            {
                if (currentPanel == pnl_cave) ChangeMap(pnl_cave, pnl_sandygrass, 31);
            }

            PanelBorderCheck(player);

            if (currentPanel == pnl_sandygrass)
            {
                playerPanel = 1;
            }
            else if (currentPanel == pnl_town)
            {
                playerPanel = 2;
            }
            else if (currentPanel == pnl_cave)
            {
                playerPanel = 3;
            }

            if (pokemonPanel == playerPanel && pokemonOnScreen == true && IntersectionDetected(player, pokemonPictureBox) && encounter == false)
            {
                Encounter();
            }
            else if (encounter) { /* Do nothing */ }
            else
            {
                if (e.KeyCode == Keys.Left)
                {
                    player.Image = Image.FromFile(DefaultFilePaths.image_path + "characters/player-left.png");
                    if (!bLeft) player.Left -= mHorizontal;
                    if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Left += mHorizontal;
                    bRight = false;
                    footsteps++;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    player.Image = Image.FromFile(DefaultFilePaths.image_path + "characters/player-right.png");
                    if (!bRight) player.Left += mHorizontal;
                    if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Left -= mHorizontal;
                    bLeft = false;
                    footsteps++;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    player.Image = Image.FromFile(DefaultFilePaths.image_path + "characters/player-back.png");
                    if (!bTop) player.Top -= mVertical;
                    if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Top += mVertical;
                    bBottom = false;
                    footsteps++;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    player.Image = Image.FromFile(DefaultFilePaths.image_path + "characters/player-front.png");
                    if (!bBottom) player.Top += mVertical;
                    if (IntersectionDetected(CurrentPanelPictureBoxes)) player.Top -= mVertical;
                    bTop = false;
                    footsteps++;
                }
            }
            //Console.WriteLine("x = " + player.Location.X);
            //Console.WriteLine("y = " + player.Location.Y);
        }

        // POKEMON ENCOUNTER
        private void Encounter()
        {
            encounter = true;
            pnl_encounter.Visible = true;
            pnl_encounter.BringToFront();
            lbl_encounter.Text = "A wild " + Logic.MakeFirstUpper(currentPokemon.name) + " appeared. What do you want to do?";
        }

        private void btn_catch_Click(object sender, EventArgs e)
        {
            pnl_encounter.Visible = false;
            pnl_catch.Visible = true;
            pnl_catch.BringToFront();

            lbl_pokeball_no.Text = Backpack.balls.Find(b => b.id == 1).count.ToString();
            lbl_greatball_no.Text = Backpack.balls.Find(b => b.id == 2).count.ToString();
            lbl_ultraball_no.Text = Backpack.balls.Find(b => b.id == 3).count.ToString();
            lbl_masterball_no.Text = Backpack.balls.Find(b => b.id == 4).count.ToString();

            tries = 0;
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            pnl_encounter.Visible = false;
            pnl_catch.Visible = false;
            pokemonPictureBox.Visible = false;
            pokemonOnScreen = false;
            encounter = false;
            Toast("You ran from " + Logic.MakeFirstUpper(currentPokemon.name));// + " like a pussy.");
            PokemonAppearTimer();
        }

        private void btn_pokeball_Click(object sender, EventArgs e)
        {
            Ball pokeball = Backpack.balls.Find(b => b.id == 1);
            if (pokeball.count == 0)
            {
                Toast("You have no " + pokeball.name + "'s left :/");
            }
            else
            {
                ThrowBall(pokeball);
                lbl_pokeball_no.Text = Backpack.balls.Find(b => b.id == 1).count.ToString();
            }
        }

        private void btn_greatball_Click(object sender, EventArgs e)
        {
            Ball greatball = Backpack.balls.Find(b => b.id == 2);
            if (greatball.count == 0)
            {
                Toast("You have no " + greatball.name + "'s left :/");
            }
            else
            {
                ThrowBall(greatball);
                lbl_pokeball_no.Text = Backpack.balls.Find(b => b.id == 2).count.ToString();
            }
        }

        private void btn_ultraball_Click(object sender, EventArgs e)
        {
            Ball ultraball = Backpack.balls.Find(b => b.id == 3);
            if (ultraball.count == 0)
            {
                Toast("You have no " + ultraball.name + "'s left :/");
            }
            else
            {
                ThrowBall(ultraball);
                lbl_pokeball_no.Text = Backpack.balls.Find(b => b.id == 3).count.ToString();
            }
        }

        private void btn_masterball_Click(object sender, EventArgs e)
        {
            Ball masterball = Backpack.balls.Find(b => b.id == 4);
            if (masterball.count == 0)
            {
                Toast("You have no " + masterball.name + "'s left :/");
            }
            else
            {
                ThrowBall(masterball);
                lbl_pokeball_no.Text = Backpack.balls.Find(b => b.id == 4).count.ToString();
            }
        }

        private void ThrowBall(Ball ball)
        {
            string outcome = Logic.ThrowBall(ball, currentPokemon, tries);
            tries++;

            switch (outcome)
            {
                case "caught":
                    pnl_catch.Visible = false;
                    pokemonPictureBox.Visible = false;
                    pokemonOnScreen = false;
                    encounter = false;
                    Toast(Logic.MakeFirstUpper(currentPokemon.name) + " was successfully added to your Pokedex!");
                    UpdatePokedexListView();
                    PokemonAppearTimer();
                    break;
                case "broke":
                    pnl_catch.Hide();
                    btn_pokeball.Enabled = btn_greatball.Enabled = btn_ultraball.Enabled = btn_masterball.Enabled = btn_run2.Enabled = false;
                    Toast(Logic.MakeFirstUpper(currentPokemon.name) + " broke free!");
                    break;
                case "ran":
                    pnl_catch.Visible = false;
                    pokemonPictureBox.Visible = false;
                    pokemonOnScreen = false;
                    encounter = false;
                    Toast("You weren't good enough for " + Logic.MakeFirstUpper(currentPokemon.name) + " so it ran.");
                    PokemonAppearTimer();
                    break;
            }
        }

        public void Toast(string message)
        {
            toastTimer.Start();
            toastTimer.Tick += new EventHandler(ToastTimerEventHandler);
            toastTimer.Interval = 2000;
            toastTimer.Enabled = true;
            lbl_toast.AutoSize = false;
            lbl_toast.Visible = true;
            lbl_toast.Text = message;
        }

        private void ToastTimerEventHandler(object sender, EventArgs e)
        {
            lbl_toast.Visible = false;
            if (encounter) pnl_catch.Show();
            btn_pokeball.Enabled = btn_greatball.Enabled = btn_ultraball.Enabled = btn_masterball.Enabled = btn_run2.Enabled = true;
            toastTimer.Stop();
        }
    }
}