using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LahmaOnline.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms.Internals;

[assembly: ExportRenderer(typeof(Image), typeof(FixedImageRenderer))]

namespace LahmaOnline.UWP
{
    // Image renderer based on The Xamarin.Forms UWP ImageRenderer
    // https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Platform.WinRT/ImageRenderer.cs
    // Ensures images are loaded from an Images folder in the project instead of the root and they have the .png extension which is required for UWP but not iOS or Android.
    public class FixedImageRenderer : ImageRenderer
    {
        bool _disposed;

        private string _imagePrefix = "Assets\\";

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _disposed = true;
        }

        protected override async Task TryUpdateSource()
        {
            // By default we'll just catch and log any exceptions thrown by UpdateSource so we don't bring down
            // the application; a custom renderer can override this method and handle exceptions from
            // UpdateSource differently if it wants to

            try
            {
                await UpdateSource2().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Warning(nameof(ImageRenderer), "Error loading image: {0}", ex);
            }
            finally
            {
                ((IImageController)Element)?.SetIsLoading(false);
            }
        }

        private void TryFixSourcePath(ImageSource source)
        {
            if (source is FileImageSource)
            {
                if (source is FileImageSource fileSource)
                {
                    var filePath = fileSource.File;
                    if (!filePath.StartsWith(_imagePrefix))
                        filePath = _imagePrefix + filePath;
                    if (!filePath.EndsWith(".png"))
                        filePath += ".png";

                    if (filePath != fileSource.File)
                        fileSource.File = filePath;
                }
            }
        }

        protected async Task UpdateSource2()
        {
            if (_disposed || Element == null || Control == null)
            {
                return;
            }

            Element.SetIsLoading(true);

            ImageSource source = Element.Source;
            TryFixSourcePath(source);

            IImageSourceHandler handler;
            if (source != null && (handler = Registrar.Registered.GetHandler<IImageSourceHandler>(source.GetType())) != null)
            {
                Windows.UI.Xaml.Media.ImageSource imagesource;

                try
                {
                    imagesource = await handler.LoadImageAsync(source);
                }
                catch (OperationCanceledException)
                {
                    imagesource = null;
                }

                // In the time it takes to await the imagesource, some zippy little app
                // might have disposed of this Image already.
                if (Control != null)
                {
                    Control.Source = imagesource;
                }

                RefreshImage();
            }
            else
            {
                Control.Source = null;
                Element.SetIsLoading(false);
            }
        }

        void RefreshImage()
        {
            ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.RendererReady);
        }
    }
}