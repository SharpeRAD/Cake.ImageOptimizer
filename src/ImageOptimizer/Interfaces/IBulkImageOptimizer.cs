#region Using Statements
using System;
using System.Collections.Generic;

using Cake.Core.IO;
#endregion



namespace Cake.ImageOptimizer
{
	/// <summary>
	/// An interface for describing an image optimizer.
	/// </summary>
    public interface IBulkImageOptimizer
	{
        #region Properties
        /// <summary>
        /// Gets the list of paths to the optimized images
        /// </summary>
        IList<string> ImagesOptimized { get; }

        /// <summary>
        /// Gets the number of images skipped
        /// </summary>
        int ImagesSkipped { get; }

        /// <summary>
        /// Gets the number of errored images
        /// </summary>
        int ImagesErrored { get; }



        /// <summary>
        /// Gets the size in bytes before optimization
        /// </summary>
        double SizeBefore { get; }

        /// <summary>
        /// Gets the size in bytes after optimization
        /// </summary>
        double SizeAfter { get; }

        /// <summary>
        /// Gets the size in bytes saved during optimization
        /// </summary>
        double SavedSize { get; }

        /// <summary>
        /// Gets the percent in size saved
        /// </summary>
        double SavedPercent { get; }
        #endregion





        #region Events
        /// <summary>
        /// Triggered when the optimizer finishes processing each image
        /// </summary>
        event EventHandler<OptimizedFileEventArgs> Progress;

        /// <summary>
        /// Triggered when the optimizer finishes processing all images
        /// </summary>
        event EventHandler<OptimizedFileEventArgs> Completed;
        #endregion





        #region Methods
        /// <summary>
        /// Optimizes Images from the settings SourceDirectory
        /// </summary>
        /// <param name="sourceDirectory">The directory of the original images.</param>
        /// <param name="outputDirectory">The directory of the optimized images.</param>
        /// <param name="settings">The settings to use when optimizing the images.</param>
        IList<OptimizedFile> Optimize(DirectoryPath sourceDirectory, DirectoryPath outputDirectory,ImageOptimizerSettings settings);

        /// <summary>
        /// Clears all local flags
        /// </summary>
        void Clear();
        #endregion
	}
}