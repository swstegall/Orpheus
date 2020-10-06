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

namespace Orpheus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Build a new JSONHandler object that will take care of the JSON interactions  - Isaac
            JSONHandler handler = new JSONHandler();
            //Returns the list of songs from the JSON file or an empty SongList object if none existed or it failed  - Isaac
            SongList ListOfSongs = handler.ReadJsonFile();
        }
    }
}
