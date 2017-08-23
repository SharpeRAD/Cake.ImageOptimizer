#region Using Statements
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
using Cake.Core.Annotations;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// Image optimizer aliases
    /// </summary>
    [CakeAliasCategory("ImageOptimizer")]
    public static class ImageOptimizerAliases
    {
        #region Methods
        private static IBulkImageOptimizer CreateBulkImageOptimizer(this ICakeContext context)
        {
            IImageOptimizerFactory factory = CreateImageOptimizerFactory(context.FileSystem, context.Environment, context.Log);

            return new BulkImageOptimizer(context.FileSystem, context.Environment, context.Log, factory);
        }

        private static IImageOptimizerFactory CreateImageOptimizerFactory(IFileSystem fileSystem, ICakeEnvironment enviroment,  ICakeLog log)
        {
            var factory = new ImageOptimizerFactory(fileSystem);
            factory.Add(new KrakenOptimizer(fileSystem, enviroment, log));

            factory.Configure(enviroment);

            return factory;
        }



        /// <summary>
        /// Optimizes Images from the settings SourceDirectory
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="sourceDirectory">The directory of the original images.</param>
        /// <param name="outputDirectory">The directory of the optimized images.</param>
        /// <param name="settings">The settings to use when optimizing the images.</param>
        [CakeMethodAlias]
        public static IList<OptimizedFile> OptimizeImages(this ICakeContext context, DirectoryPath sourceDirectory, DirectoryPath outputDirectory,ImageOptimizerSettings settings)
        {
            return context.CreateBulkImageOptimizer().Optimize(sourceDirectory, outputDirectory, settings);
        }
        #endregion
    }
}