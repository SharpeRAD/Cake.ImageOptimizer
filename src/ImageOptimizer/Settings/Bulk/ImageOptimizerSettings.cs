#region Using Statements
using Cake.Core.IO;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The settings to use with when optimzing images
    /// </summary>
    public class ImageOptimizerSettings
    {
        #region Constructors (1)
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageOptimizerSettings"/> class.
        /// </summary>
        public ImageOptimizerSettings()
        {
            this.SearchFilter = "*";
            this.SearchScope = SearchScope.Recursive;
        }
        #endregion





        #region Properties (4)
        /// <summary>
        /// Gets or sets the location of the config file to use
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets the list of services to use when opimzing
        /// </summary>
        public string Services { get; set; }



        /// <summary>
        /// The filter to use when searching for files
        /// </summary>
        public string SearchFilter { get; set; }

        /// <summary>
        /// The scope to use when searching for files
        /// </summary>
        public SearchScope SearchScope { get; set; }
        #endregion
    }
}