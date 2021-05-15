using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TMEditorMap.Engine;
using TMFormat.Formats;

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

        public static Texture2D ToTexture2D(this byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
               return Texture2D.FromStream(MapCore.Instance.DeviceManager.GraphicsDevice, stream);
            }
        }

        public static List<DependencyObject> FindAllChildren(this DependencyObject dpo, Predicate<DependencyObject> predicate)
        {
            var results = new List<DependencyObject>();
            if (predicate == null)
                return results;


            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dpo); i++)
            {
                var child = VisualTreeHelper.GetChild(dpo, i);
                if (predicate(child))
                    results.Add(child);

                var subChildren = child.FindAllChildren(predicate);
                results.AddRange(subChildren);
            }
            return results;
        }
    }
}
