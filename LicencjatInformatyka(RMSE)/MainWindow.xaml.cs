﻿using System.Windows;
using LicencjatInformatyka_RMSE_.ViewControls.AskWindows;
using LicencjatInformatyka_RMSE_.ViewControls.UserControls;
using LicencjatInformatyka_RMSE_.ViewModelFolder;
using WpfRichText;

namespace LicencjatInformatyka_RMSE_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
         
            InitializeComponent();
            GeneralControl.Content = new RuleBaseUserControl(DataContext as ViewModel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Some text", "ddd", MessageBoxButton.YesNo);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new RuleBaseUserControl(DataContext as ViewModel);
            GeneralControl.Visibility = Visibility.Visible;
           

        }
   

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new ConstrainBaseUserControll(DataContext as ViewModel);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new ModelBaseUserControll(DataContext as ViewModel);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
        
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new ConcludeUserControl(DataContext as ViewModel);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new GraphicBaseUserControll(DataContext as ViewModel);

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new FlatterUserControll();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new AdviceUserControl(DataContext as ViewModel);

        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new SoundUserControl();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            GeneralControl.Content = new LanguageUserControl(DataContext as ViewModel);
        }
       
    }
}
