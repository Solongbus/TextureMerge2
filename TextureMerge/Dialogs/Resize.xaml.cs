using System.Windows;

namespace TextureMerge
{
    public partial class Resize : Window
    {
        public uint NewWidth { get; private set; }
        public uint NewHeight { get; private set; }

        public Resize(uint width, uint height)
        {
            InitializeComponent();
            WidthBox.Text = (NewWidth = width).ToString();
            HeightBox.Text = (NewHeight = height).ToString();
        }

        private void OKButton(object sender, RoutedEventArgs e)
        {
            if (uint.TryParse(WidthBox.Text, out uint width) && uint.TryParse(HeightBox.Text, out uint height) && width > 0 && height > 0)
            {
                NewWidth = width;
                NewHeight = height;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageDialog.Show("Invalid input", type: MessageDialog.Type.Error);
            }
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
