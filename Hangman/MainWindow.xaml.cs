using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Hangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XmlDocument xmlDoc = null;
        private List<CharHolder> charHolders = null;
        private int letterCounter;
        private int numberOfGuesses;
        public MainWindow()
        {
            InitializeComponent();
            GameInit();
        }

        private void GameInit()
        {
            letterCounter = 0;
            numberOfGuesses = 0;

            xmlDoc = new XmlDocument();
            xmlDoc.Load("Words.xml");

            int wordsCount = xmlDoc.DocumentElement.ChildNodes.Count - 1;

            Random random = new Random();
            int index = random.Next(0, wordsCount);

            XmlNode node = xmlDoc.DocumentElement.ChildNodes.Item(index);
            char[] characters = node.InnerText.ToCharArray();

            charHolders = new List<CharHolder>();

            int i = 0;
            foreach (char c in characters)
            {
                CharHolder holder = new CharHolder();
                holder.lblCharHolder.Content = c;
                holder.lblCharHolder.Visibility = Visibility.Hidden;
                charsPanel.Children.Add(holder);
                charHolders.Add(holder);
            }
        }

        private void newGame(object sender, RoutedEventArgs e)
        {
            foreach (CharHolder holder in charHolders)
            {
                charsPanel.Children.Remove(holder);
            }

            GameInit();

            btnSubmit.Click -= newGame;
            btnSubmit.Click += btnSubmit_Click;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (CharHolder holder in charHolders)
            {
                if (holder.lblCharHolder.Content.ToString().Equals(txtInputLetter.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    i++;
                    holder.lblCharHolder.Visibility = Visibility.Visible;
                }
            }

            if (i == 0)
            {
                numberOfGuesses++;
                if (numberOfGuesses == 7)
                {
                    MessageBox.Show("You lose!");
                    foreach (CharHolder holder in charHolders)
                    {
                        holder.lblCharHolder.Visibility = Visibility.Visible;
                    }
                    RemoveSubmitHandler();
                }
                else
                {
                    MessageBox.Show("Bad guess! You have " + (7 - numberOfGuesses) + " remaining atempts.");
                }
            } else
            {
                if (letterCounter == charHolders.Count)
                {
                    MessageBox.Show("You won! Nice job!");
                    RemoveSubmitHandler();
                }
            }
        }

        private void RemoveSubmitHandler()
        {
            btnSubmit.Content = "New Game";
            btnSubmit.Click -= this.btnSubmit_Click;
            btnSubmit.Click += newGame;
        }
    }
}
