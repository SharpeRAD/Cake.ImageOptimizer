#region Using Statements
using System;
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
#endregion



namespace Cake.ImageOptimizer
{
	/// <summary>
	/// An interface for describing an image optimizer.
	/// </summary>
    public interface IImageOptimizer : ICloneable
	{
        #region Properties
        /// <summary>
        /// Gets the name of the service used to optimize the image.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A list of extensions supported by the Optimizer
        /// </summary>
        IList<string> Extensions { get; }


        /// <summary>
        /// Gets the maximum file size to optimize.
        /// </summary>
        int FileSize { get; }
        #endregion





        #region Methods
        /// <summary>
        /// Does the optimzer support the image
        /// </summary>
        /// <param name="extension">The extension of the file to optimize.</param>
        bool Supports(string extension);

        /// <summary>
        /// Configure the optimizer
        /// </summary>
        /// <param name="environment">The environment.</param>
        void Configure(ICakeEnvironment environment);



        /// <summary>
        /// Optimizes the specified file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        ImageOptimizerResult Optimize(FilePath path);
        #endregion
	}
}