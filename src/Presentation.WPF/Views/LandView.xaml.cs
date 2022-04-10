using System.Windows.Controls;

namespace Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for LandView.xaml
    /// </summary>
    public partial class LandView : UserControl
    {
        public static Canvas Canvas { get; private set; }

        public LandView()
        {
            InitializeComponent();
            SetCanvas();
        }

        private void SetCanvas()
        {
            Canvas = LandViewCanvas;
        }
    }
}