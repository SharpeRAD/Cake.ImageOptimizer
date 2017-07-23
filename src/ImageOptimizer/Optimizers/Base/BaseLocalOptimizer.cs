#region Using Statements
using System;
using IO = System.IO;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    ///  The base class for local, tool based optimizers 
    /// </summary>
    public abstract class BaseLocalOptimizer : BaseOptimizer
    {
        #region Fields (2)
        /// <summary>
        ///  Represents a process runner.
        /// </summary>
        protected readonly IProcessRunner _Runner;

        /// <summary>
        /// Responsible for finding the tool.
        /// </summary>
        protected readonly IToolLocator _Tools;
        #endregion





        #region Constructors (1)
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLocalOptimizer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="runner">The process runner.</param>
        /// <param name="tools">The locator.</param>
        public BaseLocalOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, IProcessRunner runner, IToolLocator tools)
            : base(fileSystem, environment, log)
        {
            if (runner == null)
            {
                throw new ArgumentNullException("runner");
            }
            if (tools == null)
            {
                throw new ArgumentNullException("tools");
            }

            _Runner = runner;
            _Tools = tools;
        }
        #endregion





        #region Methods (2)
        /// <summary>
        /// Optimizes the specified file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public ImageOptimizerResult Optimize(FilePath path)
        {
            path = path.MakeAbsolute(_Environment);
            FilePath resultFile = new FilePath(IO.Path.ChangeExtension(IO.Path.GetTempFileName(), path.GetExtension()));

            try
            {
                this.RunTool(path, resultFile);

                return new ImageOptimizerResult(this.Name, path, "")
                {
                    SizeBefore = _FileSystem.GetFile(path).Length,
                    SizeAfter = _FileSystem.GetFile(resultFile).Length,

                    DownloadUrl = resultFile.FullPath
                };
            }
            catch (Exception ex)
            {
                return new ImageOptimizerResult(this.Name, path, ex.Message);
            }
        }

        /// <summary>
        /// Runs the optimizer tool
        /// </summary>
        /// <param name="sourceFile">The source path of the image to optimize.</param>
        /// <param name="outputFile">The output path of the optimized image.</param>
        protected virtual void RunTool(FilePath sourceFile, FilePath outputFile)
        {

        }
        #endregion
    }
}