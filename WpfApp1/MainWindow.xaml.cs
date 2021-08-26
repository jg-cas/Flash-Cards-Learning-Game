﻿using System;
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
using System.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Animation();
        }
     
        private async void Animation()
        {
            var animation = new WelcomeAnimation();
            this.Content = animation;

            //Wait for Animation to Complete
            await Task.Delay(5000);

            animation.PlayerBoxControl.Content = new SelectPlayerControl();

        }
    }
}
