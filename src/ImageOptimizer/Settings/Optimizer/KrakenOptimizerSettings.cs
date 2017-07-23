


namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The settings to use with when calling Kraken
    /// </summary>
    public class KrakenOptimizerSettings : BaseOptimizerSettings
    {
        #region Properties (4)
        /// <summary>
        /// Gets or sets the API Key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the API Secret Key
        /// </summary>
        public string SecretKey { get; set; }



        /// <summary>
        /// Gets or sets weather to use lossless image compression
        /// </summary>
        public bool Lossy { get; set; }
        
        /// <summary>
        /// Gets or sets the quality of JPEG compression
        /// </summary>
        public int Quantity { get; set; }
        #endregion
    }
}