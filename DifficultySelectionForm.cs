using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oop_template;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace oop_template
{
    public class DifficultySelectionForm : Form
    {
        public int SelectedSize { get; private set; } // Veličina odabrane table
        public GameDifficulty SelectedDifficulty { get; private set; } // Odabrana težina igre

        public DifficultySelectionForm()
        {
            // Podešava osnovne osobine forme
            Text = "Choose Difficulty";
            Size = new Size(300, 200);

            // Kreira dugmad za različite težine igre
            Button easyButton = new Button { Text = "LAKO [6x6]", Dock = DockStyle.Top };
            Button mediumButton = new Button { Text = "SREDNJE [5x5]", Dock = DockStyle.Top };
            Button hardButton = new Button { Text = "TESKO [4x4]", Dock = DockStyle.Top };

            // Dodaje event handlere za klik na dugmad
            easyButton.Click += (sender, e) => { SelectedSize = 6; SelectedDifficulty = GameDifficulty.Lako; DialogResult = DialogResult.OK; };
            mediumButton.Click += (sender, e) => { SelectedSize = 5; SelectedDifficulty = GameDifficulty.Srednje; DialogResult = DialogResult.OK; };
            hardButton.Click += (sender, e) => { SelectedSize = 4; SelectedDifficulty = GameDifficulty.Tesko; DialogResult = DialogResult.OK; };

            // Dodaje dugmad na formu
            Controls.Add(easyButton);
            Controls.Add(mediumButton);
            Controls.Add(hardButton);
        }
    }
}