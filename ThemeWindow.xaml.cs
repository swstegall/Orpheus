﻿using System.Windows;
using System.Windows.Controls;

namespace Orpheus
{
    public partial class ThemeWindow : Window
    {
        public string ThemeSelectedValue { get; set; }

        public ThemeWindow()
        {
            InitializeComponent();
        }

        private void OkaySelectedCommand(object sender, SelectionChangedEventArgs e)
        {
            this.Close();
        }
    }
}
