#region Using Statements
using System;

using Cake.Core.IO;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The result of the image optimzation
    /// </summary>
	public class ImageOptimizerResult
	{
		#region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageOptimizerResult" /> class.
        /// </summary>
        /// <param name="service">The name of the service used to optimize the image.</param>
        /// <param name="path">The path to the image.</param>
        /// <param name="error">The error message that occured during optimization.</param>
        public ImageOptimizerResult(string service, FilePath path, string error)
		{
            this.Service = service;
			this.Path = path;

            this.ErrorMessage = error;
		}
		#endregion





		#region Properties 
        /// <summary>
        /// Gets or sets the name of the service used to optimize the image
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets the date the image was optimized
        /// </summary>
        public DateTimeOffset ModifiedDate { get; set; }



        /// <summary>
        /// Gets or sets the path to the image
        /// </summary>
		public FilePath Path { get; set; }

        /// <summary>
        /// Gets or sets the url to download the image from the optimizer
        /// </summary>
        public string DownloadUrl { get; set; }



        /// <summary>
        /// Gets or sets the size of the image before optimzation
        /// </summary>
        public double SizeBefore { get; set; }

        /// <summary>
        /// Gets or sets the size of the image after optimziation
        /// </summary>
		public double SizeAfter { get; set; }

        /// <summary>
        /// Gets or sets the size difference of the image after optimziation
        /// </summary>
        public double SavedSize
        { 
            get
            {
                if (this.SizeAfter < this.SizeBefore)
                {
                    return this.SizeBefore - this.SizeAfter;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the percentage of the image saved after optimziation
        /// </summary>
        public double SavedPercent
        { 
            get
            {
                if (this.SizeAfter < this.SizeBefore)
                {
                    return 100 - Math.Round((100 / this.SizeBefore) * this.SizeAfter, 1);
                }
                else
                {
                    return 0;
                }
            }
        }



        /// <summary>
        /// Gets or sets the error message that occured during optimization
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets if an error occured during optimziation
        /// </summary>
        public bool HasError
        {
            get 
            { 
                return !string.IsNullOrEmpty(this.ErrorMessage); 
            }
        }
		#endregion
	}
}