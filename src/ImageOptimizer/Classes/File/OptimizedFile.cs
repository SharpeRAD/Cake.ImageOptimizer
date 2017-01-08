#region Using Statements
    using System;

    using Cake.Core.IO;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// Details of an optimized file
    /// </summary>
    public class OptimizedFile
    {
		#region Constructors (1) 
            /// <summary>
            /// Initializes a new instance of the <see cref="OptimizedFile" /> class.
            /// </summary>
            /// <param name="path">The path to the image.</param>
            /// <param name="service">The name of the service used to optimize the image</param>
            /// <param name="optimizedDate">The date the image was optimized.</param>
            /// <param name="optimizedHash">The hash of the file after optimization.</param>
            /// <param name="sizeBefore">The size of the image before optimzation.</param>
            /// <param name="sizeAfter">The size of the image after optimziation.</param>
            public OptimizedFile(FilePath path, string service, DateTimeOffset optimizedDate, string optimizedHash, double sizeBefore, double sizeAfter)
            {
                this.Path = path;
                this.Service = service;

                this.OptimizedDate = optimizedDate;
                this.OptimizedHash = optimizedHash;

                this.SizeBefore = sizeBefore;
                this.SizeAfter = sizeAfter;
            }
		#endregion





		#region Properties (9) 
            /// <summary>
            /// Gets or sets the path to the image
            /// </summary>
            public FilePath Path { get; set; }

            /// <summary>
            /// Gets or sets the name of the service used to optimize the image
            /// </summary>
            public string Service { get; set; }



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
            /// Returns true if the image was optimized
            /// </summary>
            public bool Optimized 
            {
                get
                {
                    return this.OptimizedDate != DateTime.MinValue;
                }
            }

            /// <summary>
            /// Gets or sets the date the image was optimized
            /// </summary>
            public DateTimeOffset OptimizedDate { get; set; }
        
            /// <summary>
            /// Gets or sets the hash of the file after optimization
            /// </summary>
            public string OptimizedHash { get; set; }
		#endregion
        




        #region Functions (2)
            /// <summary>
            /// Returns true if the services don't match
            /// </summary>
            public bool DifferentService(string service)
            {
                return !String.IsNullOrEmpty(service) && (service != this.Service);
            }

            /// <summary>
            /// Returns true if the hashes don't match
            /// </summary>
            public bool RequiresOptimization(string originalHash)
            {
                return this.OptimizedHash != this.OptimizedHash;
            }
        #endregion
    }
}