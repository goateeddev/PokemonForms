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

namespace Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for LandView.xaml
    /// </summary>
    public partial class LandView : UserControl
    {
        public static Canvas ViewCanvas { get; private set; }

        public LandView()
        {
            InitializeComponent();
            SetCanvas();
        }

        private void SetCanvas()
        {
            ViewCanvas = landViewCanvas;
        }
    }
}
