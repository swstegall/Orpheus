using System.Windows;
using System.Windows.Controls;

namespace Orpheus.Windows
{
    public partial class ThemeWindow : Window
    {
        public string ThemeSelectedValue { get; set; }

        public ThemeWindow()
        {
            InitializeComponent();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            var senderBtn = sender as ComboBoxItem;
            ThemeSelectedValue = senderBtn.Content.ToString();
            this.Close();
        }
    }
}
