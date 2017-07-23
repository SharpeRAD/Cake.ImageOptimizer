#region Using Statements
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// PngOut image optimizer.
    /// </summary>
    public class PngOutOptimizer : BaseLocalOptimizer, IImageOptimizer
	{
        #region Constructors (1)
        /// <summary>
        /// Initializes a new instance of the <see cref="PngOutOptimizer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="runner">The process runner.</param>
        /// <param name="tools">The locator.</param>
        public PngOutOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, IProcessRunner runner, IToolLocator tools)
            : base(fileSystem, environment, log, runner, tools)
        {

        }
        #endregion




        #region Properties (2)
        /// <summary>
        /// Gets the name of the optimizer.
        /// </summary>
        /// <value>The optimizer name.</value>
        public override string Name
        {
            get
            {
                return "PngOut";
            }
        }

        /// <summary>
        /// A list of extensions supported by the Optimizer
        /// </summary>
        /// <value>The file extensions.</value>
        public override IList<string> Extensions 
        { 
            get
            {
                return new List<string>() { ".png" };
            }
        }
		#endregion





		#region Methods (3) 
        /// <summary>
        /// Configure the optimizer
        /// </summary>
        /// <param name="environment">The environment.</param>
        public void Configure(ICakeEnvironment environment)
        {

        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return new PngOutOptimizer(_FileSystem, _Environment, _Log, _Runner, _Tools);
        }

        /// <summary>
        /// Runs the optimizer tool
        /// </summary>
        /// <param name="sourceFile">The source path of the image to optimize.</param>
        /// <param name="outputFile">The output path of the optimized image.</param>
        protected override void RunTool(FilePath sourceFile, FilePath outputFile)
        {
            new PngOutRunner(_FileSystem, _Environment, _Runner, _Tools).Run(new ImageOptimizerToolSettings()
            {
                SourcePath = sourceFile,
                OutputPath = outputFile
            });
        }
		#endregion
	}
}