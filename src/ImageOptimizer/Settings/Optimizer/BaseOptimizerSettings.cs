


namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The settings to use with when optimzing images
    /// </summary>
    public class BaseOptimizerSettings
    {
        #region Properties (5)
            /// <summary>
            /// Gets or sets the maximum file size to optimize.
            /// </summary>
            public int FileSize { get; set; }
  
            /// <summary>
            /// Gets or sets the maximum timespan for the request.
            /// </summary>
            public int Timeout { get; set; }
        #endregion
    }
}