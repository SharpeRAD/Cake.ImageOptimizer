#region Using Statements
using Cake.Core;
using Cake.Core.IO;
#endregion



namespace Cake.ImageOptimizer
{
	/// <summary>
	/// An interface for describing an image optimizer factory
	/// </summary>
    public interface IImageOptimizerFactory
	{
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
        /// Adds an optimizer to the factory
        /// </summary>
        /// <param name="optimizer">The optimizer to add.</param>
        void Add(IImageOptimizer optimizer);

        /// <summary>
        /// Removes an optimizer from the factory
        /// </summary>
        /// <param name="optimizer">The optimizer to remove.</param>
        void Remove(IImageOptimizer optimizer);

        /// <summary>
        /// Removes all optimizers from the factory
        /// </summary>
        void Clear();



        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="name">The name of the optimizer.</param>
        IImageOptimizer GetByName(string name);

        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="path">The path of the file that the optimzer needs to support.</param>
        IImageOptimizer GetByPath(FilePath path);

        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="extension">The extension of the file that the optimzer needs to support.</param>
        IImageOptimizer GetByExtension(string extension);



        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="optimizer">The name of the optimizer.</param>
        /// <param name="path">The path of the file to optimize.</param>
        ImageOptimizerResult Optimize(string optimizer, FilePath path);

        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="optimizer">The name of the optimizer.</param>
        /// <param name="sourcePath">The path of the file to optimize.</param>
        /// <param name="outputPath">The output path for the optimized image.</param>
        ImageOptimizerResult Optimize(string optimizer, FilePath sourcePath, FilePath outputPath);
        #endregion
	}
}