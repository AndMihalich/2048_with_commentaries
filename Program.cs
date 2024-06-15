using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using oop_template;

namespace _2048
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Kreiranje forme za odabir težine igre
            DifficultySelectionForm difficultySelectionForm = new DifficultySelectionForm();
            if (difficultySelectionForm.ShowDialog() == DialogResult.OK)
            {
                int size = difficultySelectionForm.SelectedSize;
                GameDifficulty difficulty = difficultySelectionForm.SelectedDifficulty;
                EngineUI engineUI = new EngineUI(size, difficulty);
                Application.Run(engineUI);
            }
        }
    }
}