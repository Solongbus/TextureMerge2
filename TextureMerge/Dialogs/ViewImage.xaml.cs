using System.Windows;

namespace TextureMerge
{
    public partial class ViewImage : Window
    {
        private readonly TMImage FullImage;

        public ViewImage(TMImage image, string channelName, string channelSource)
        {
            InitializeComponent();

            FullImage = image;

            var thubnail = image.Clone();
            thubnail.Image.Thumbnail(512, 512);

            TheImage.SetImageThumbnail(thubnail);
            Title = $"View image - in {channelName} channel - source {channelSource} - {image.Image.Width}x{image.Image.Height} - {image.FileName}";

            uint width = image.Image.Width;
            uint height = image.Image.Height;
            ResizeToFit(ref width, ref height, 768);

            Width = width;
            Height = height;
        }

        private static void ResizeToFit(ref uint width, ref uint height, uint size)
        {
            double aspectRatio = (double)width / height;

            if (width > size || height > size)
            {
                if (aspectRatio > 1)
                {
                    width = size;
                    height = (uint)(size / aspectRatio);
                }
                else
                {
                    height = size;
                    width = (uint)(size * aspectRatio);
                }
            }
        }


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (FullImage == null)
                return;

            TheImage.SetImageThumbnailAsync(FullImage);
        }
    }
}
