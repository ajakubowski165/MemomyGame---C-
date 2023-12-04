using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        private List<char> cards;
        private List<Button> cardButtons;
        private List<bool> revealed;
        private int pairsFound;
        private bool isBusy = false;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Witaj w grze Memory!";
        }

        private void InitializeGame()
        {
            cards = new List<char> { 'A', 'B', 'C', 'D', 'E', 'A', 'B', 'C', 'D', 'E' };
            Shuffle(cards);

            cardButtons = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9, button10 };
            revealed = new List<bool>(Enumerable.Repeat(false, cards.Count));
            pairsFound = 0;

            for (int i = 0; i < cardButtons.Count; i++)
            {
                int index = i;
                cardButtons[i].Click += (sender, e) => CardButtonClick(index);
            }
        }

        private void CardButtonClick(int index)
        {
            if (isBusy || revealed[index])
                return;

            cardButtons[index].Text = cards[index].ToString();
            revealed[index] = true;

            int revealedCount = revealed.Count(r => r);
            if (revealedCount % 2 == 0)
            {
                int firstIndex = revealed.IndexOf(true);
                int secondIndex = revealed.LastIndexOf(true);

                isBusy = true;

                if (cards[firstIndex] == cards[secondIndex])
                {
                    pairsFound++;

                    cardButtons[firstIndex].BackColor = Color.Green;
                    cardButtons[secondIndex].BackColor = Color.Green;

                    if (pairsFound == cards.Count / 2)
                    {
                        MessageBox.Show("Gratulacje! Odnalaz³eœ wszystkie pary. Koniec gry!");
                        InitializeGame();
                    }
                }
                else
                {
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 1000 };
                    timer.Tick += (sender, e) =>
                    {
                        cardButtons[firstIndex].Text = "";
                        cardButtons[secondIndex].Text = "";
                        revealed[firstIndex] = false;
                        revealed[secondIndex] = false;
                        isBusy = false;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
        }

        private void Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            int n = list.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
