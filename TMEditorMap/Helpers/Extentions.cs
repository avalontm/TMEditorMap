using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TMEditorMap.Helpers
{
   public static class Extentions
    {
        public static ImageSource FromFile(string file)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(file);
            image.EndInit();
            return image;
        }

        public static Stream ToStream(this ImageSource source)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)source));
            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);

            return stream;
        }

        public static ImageSource ToImage(this byte[] byteArray)
        {
            Image image = new Image();

            try
            {
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    return image.Source;
                }
            }
            catch
            {
                return null;
            }
        }


    }
}
