#region Using Statements
using System;
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The Gifsicle opimizer runner.
    /// </summary>
    public sealed class GifsicleRunner : Tool<ImageOptimizerToolSettings>
    {
        #region Fields
        private readonly ICakeEnvironment _Environment;
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GifsicleRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The locator.</param>
        public GifsicleRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _Environment = environment;
        }
        #endregion





        #region Methods
        /// <summary>
        /// Runs the tests in the specified assemblies, using the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Run(ImageOptimizerToolSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(ImageOptimizerToolSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            //string.Format(CultureInfo.CurrentCulture, "/c gifsicle --crop-transparency --no-comments --no-extensions --no-names --optimize=3 --batch \"{0}\" --output=\"{1}\"", sourceFile, resultFile);

            builder.Append("--crop-transparency");
            builder.Append("--no-comments");
            builder.Append("--no-extensions");
            builder.Append("--no-names");
            builder.Append("--optimize=3");

            builder.Append("--batch");
            builder.AppendQuoted(settings.SourcePath.MakeAbsolute(_Environment).FullPath);

            builder.Append("--output");
            builder.AppendQuoted(settings.OutputPath.MakeAbsolute(_Environment).FullPath);
            
            return builder;
        }



        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Gifsicle";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "gifsicle.exe" };
        }
        #endregion
    }
}