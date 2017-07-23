#region Using Statements
using Cake.Core.IO;
using Cake.Core.Tooling;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// Contains settings used by <see cref="ImageOptimizerToolSettings" />.
    /// </summary>
    public sealed class ImageOptimizerToolSettings : ToolSettings
    {
        #region Properties (2)
        /// <summary>
        /// Gets or sets the source path of the image to optimize
        /// </summary>
        /// <value>
        /// The source path of the image to optimize.
        /// </value>
        public FilePath SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the output path of the optimized image
        /// </summary>
        /// <value>
        /// The output path of the optimized image.
        /// </value>
        public FilePath OutputPath { get; set; }
        #endregion
    }
}