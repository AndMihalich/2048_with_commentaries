using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oop_template;

namespace oop_template
{
    public class Engine
    {
        public event Action BoardChanged; // Event koji se aktivira kada se promeni stanje table
        private int[,] _board; // Matrica koja predstavlja tablu
        private List<int[,]> _history; // Lista istorije stanja table za Undo funkcionalnost
        private Random _random; // Generator slučajnih brojeva za dodavanje novih blokova
        private int _size; // Veličina table
        private SoundEffectEngine _soundEffectEngine; // Instanca za puštanje zvukova
        private HighScoreManager _highScoreManager; // Menadžer za high scores
        public int[,] Board => _board; // Javni pristup stanju table

        // Konstruktor klase Engine, inicijalizuje tablu, istoriju, generator slučajnih brojeva i ostale potrebne instance
        public Engine(int size)
        {
            _size = size;
            _board = new int[_size, _size];
            _history = new List<int[,]>();
            _random = new Random();
            _soundEffectEngine = new SoundEffectEngine();
            _highScoreManager = new HighScoreManager();
            NewGame();
        }

        // Postavlja tablu na početno stanje
        public void NewGame()
        {
            Array.Clear(_board, 0, _board.Length); // Čisti tablu
            _history.Clear(); // Čisti istoriju
            AddRandomBlock(); // Dodaje prvi slučajni blok
            AddRandomBlock(); // Dodaje drugi slučajni blok
            OnBoardChanged(); // Aktivira event da se tabla promenila
        }

        // Pomeranje u odabranom pravcu, vraća true ako je došlo do pomeranja
        public bool Move(Direction direction)
        {
            SaveState(); // Čuva trenutno stanje table
            bool moved = false; // Indikator da li je došlo do pomeranja
            bool merged = false; // Indikator da li je došlo do spajanja
            switch (direction)
            {
                case Direction.Up: (moved, merged) = MoveUp(); break;
                case Direction.Down: (moved, merged) = MoveDown(); break;
                case Direction.Left: (moved, merged) = MoveLeft(); break;
                case Direction.Right: (moved, merged) = MoveRight(); break;
            }
            if (moved)
            {
                if (merged)
                {
                    _soundEffectEngine.PlayMergeSound(); // Pušta zvuk spajanja ako je došlo do spajanja
                }
                else
                {
                    _soundEffectEngine.PlayMoveSound(); // Pušta zvuk pomeranja ako nije bilo spajanja
                }
                AddRandomBlock(); // Dodaje novi slučajni blok
                OnBoardChanged(); // Aktivira event da se tabla promenila
            }
            return moved;
        }

        // Čuva trenutno stanje table u istoriji
        private void SaveState()
        {
            _history.Add((int[,])_board.Clone()); // Dodaje kopiju trenutne table u istoriju
            if (_history.Count > 4) // Ako istorija ima više od 4 stanja
            {
                _history.RemoveAt(0); // Uklanja najstarije stanje
            }
        }

        // Vraća stanje table na prethodno ako postoji istorija, vraća true ako je uspešno
        public bool Undo()
        {
            if (_history.Count > 1)
            {
                _board = (int[,])_history[_history.Count - 1].Clone(); // Vraća tablu na prethodno stanje
                _history.RemoveAt(_history.Count - 1); // Uklanja to stanje iz istorije
                OnBoardChanged(); // Aktivira event da se tabla promenila
                return true;
            }
            return false;
        }

        // Dodaje slučajni blok (2 ili 4) na slučajnu praznu poziciju na tabli
        private void AddRandomBlock()
        {
            List<Point> emptyPoints = new List<Point>(); // Lista praznih pozicija
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_board[i, j] == 0)
                    {
                        emptyPoints.Add(new Point(i, j)); // Dodaje praznu poziciju u listu
                    }
                }
            }

            if (emptyPoints.Count > 0)
            {
                Point p = emptyPoints[_random.Next(emptyPoints.Count)]; // Bira slučajnu praznu poziciju
                _board[p.X, p.Y] = _random.NextDouble() < 0.9 ? 2 : 4; // Postavlja blok sa vrednošću 2 ili 4
            }
        }

        // Pomeranje blokova naviše, vraća true ako je bilo pomeranja ili spajanja
        private (bool moved, bool merged) MoveUp()
        {
            bool moved = false;
            bool merged = false;
            for (int j = 0; j < _size; j++)
            {
                for (int i = 1; i < _size; i++)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = i;
                        while (k > 0 && _board[k - 1, j] == 0)
                        {
                            _board[k - 1, j] = _board[k, j];
                            _board[k, j] = 0;
                            k--;
                            moved = true;
                        }
                        if (k > 0 && _board[k - 1, j] == _board[k, j] && _board[k - 1, j] != 2048)
                        {
                            _board[k - 1, j] *= 2;
                            _board[k, j] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        // Pomeranje blokova naniže, vraća true ako je bilo pomeranja ili spajanja
        private (bool moved, bool merged) MoveDown()
        {
            bool moved = false;
            bool merged = false;
            for (int j = 0; j < _size; j++)
            {
                for (int i = _size - 2; i >= 0; i--)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = i;
                        while (k < _size - 1 && _board[k + 1, j] == 0)
                        {
                            _board[k + 1, j] = _board[k, j];
                            _board[k, j] = 0;
                            k++;
                            moved = true;
                        }
                        if (k < _size - 1 && _board[k + 1, j] == _board[k, j] && _board[k + 1, j] != 2048)
                        {
                            _board[k + 1, j] *= 2;
                            _board[k, j] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        // Pomeranje blokova ulevo, vraća true ako je bilo pomeranja ili spajanja
        private (bool moved, bool merged) MoveLeft()
        {
            bool moved = false;
            bool merged = false;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 1; j < _size; j++)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = j;
                        while (k > 0 && _board[i, k - 1] == 0)
                        {
                            _board[i, k - 1] = _board[i, k];
                            _board[i, k] = 0;
                            k--;
                            moved = true;
                        }
                        if (k > 0 && _board[i, k - 1] == _board[i, k] && _board[i, k - 1] != 2048)
                        {
                            _board[i, k - 1] *= 2;
                            _board[i, k] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        // Pomeranje blokova udesno, vraća true ako je bilo pomeranja ili spajanja
        private (bool moved, bool merged) MoveRight()
        {
            bool moved = false;
            bool merged = false;
            for (int i = 0; i < _size; i++)
            {
                for (int j = _size - 2; j >= 0; j--)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = j;
                        while (k < _size - 1 && _board[i, k + 1] == 0)
                        {
                            _board[i, k + 1] = _board[i, k];
                            _board[i, k] = 0;
                            k++;
                            moved = true;
                        }
                        if (k < _size - 1 && _board[i, k + 1] == _board[i, k] && _board[i, k + 1] != 2048)
                        {
                            _board[i, k + 1] *= 2;
                            _board[i, k] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        // Proverava da li je igra gotova (nema više mogućih poteza)
        public bool IsGameOver()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_board[i, j] == 0)
                        return false;
                    if (i < _size - 1 && _board[i, j] == _board[i + 1, j])
                        return false;
                    if (j < _size - 1 && _board[i, j] == _board[i, j + 1])
                        return false;
                }
            }
            return true;
        }

        // Čuva high score igrača
        public void SaveHighScore(string playerName, GameDifficulty difficulty)
        {
            int score = CalculateScore();
            _highScoreManager.SaveHighScore(playerName, score, difficulty);
        }

        // Računa ukupni score na osnovu vrednosti svih blokova na tabli
        private int CalculateScore()
        {
            int score = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    score += _board[i, j];
                }
            }
            return score;
        }

        // Aktivira event da se tabla promenila
        private void OnBoardChanged()
        {
            BoardChanged?.Invoke();
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum GameDifficulty
    {
        Lako,
        Srednje,
        Tesko
    }
}