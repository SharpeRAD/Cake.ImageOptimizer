


namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The settings to use with when resizing an image with Kraken
    /// </summary>
    public class KrakenResizeSettings 
    {
        #region Properties (3)
            /// <summary>
            /// Gets or sets the width of the new image
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets the height of the new image
            /// </summary>
            public int Height { get; set; }



            /// <summary>
            /// Gets or sets weather to use lossless image compression
            /// </summary>
            public string Strategy { get; set; }
        #endregion
    }
}