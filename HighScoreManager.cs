using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace oop_template
{
    public class HighScoreManager
    {
        // Datoteke u kojima se čuvaju high scores za različite težine igre
        private const string EasyFilePath = "highscores_easy.txt";
        private const string MediumFilePath = "highscores_medium.txt";
        private const string HardFilePath = "highscores_hard.txt";

        // Vraća putanju do odgovarajuće datoteke na osnovu težine igre
        private string GetFilePath(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.Lako: return EasyFilePath;
                case GameDifficulty.Srednje: return MediumFilePath;
                case GameDifficulty.Tesko: return HardFilePath;
                default: throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        // Čuva high score za datog igrača i težinu igre
        public void SaveHighScore(string playerName, int score, GameDifficulty difficulty)
        {
            var highScores = LoadHighScores(difficulty); // Učitava trenutne high scores
            highScores.Add(new KeyValuePair<string, int>(playerName, score)); // Dodaje novi score
            highScores = highScores.OrderByDescending(x => x.Value).ToList(); // Sortira scores opadajuće
            SaveHighScores(highScores, difficulty); // Čuva ažuriranu listu
        }

        // Učitava high scores iz odgovarajuće datoteke
        public List<KeyValuePair<string, int>> LoadHighScores(GameDifficulty difficulty)
        {
            var highScores = new List<KeyValuePair<string, int>>();
            string filePath = GetFilePath(difficulty);

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        highScores.Add(new KeyValuePair<string, int>(parts[0], score));
                    }
                }
            }

            return highScores;
        }

        // Čuva listu high scores u odgovarajuću datoteku
        private void SaveHighScores(List<KeyValuePair<string, int>> highScores, GameDifficulty difficulty)
        {
            string filePath = GetFilePath(difficulty);
            var lines = highScores.Select(x => $"{x.Key}|{x.Value}");
            File.WriteAllLines(filePath, lines);
        }

        // Prikazuje leaderboard za datu težinu igre
        public void DisplayLeaderboard(GameDifficulty difficulty)
        {
            var highScores = LoadHighScores(difficulty);
            string leaderboard = $"Leaderboard ({difficulty}):\n";
            foreach (var highScore in highScores)
            {
                leaderboard += $"{highScore.Key}: {highScore.Value}\n";
            }
            MessageBox.Show(leaderboard, "Leaderboard");
        }
    }
}