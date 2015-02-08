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
using LicencjatInformatyka_RMSE_.ViewModelFolder;

namespace LicencjatInformatyka_RMSE_.ViewControls.UserControls
{
    /// <summary>
    /// Interaction logic for LanguageUserControl.xaml
    /// </summary>
    public partial class LanguageUserControl : UserControl
    {
        public LanguageUserControl(ViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}