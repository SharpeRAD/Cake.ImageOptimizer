﻿#region Using Statements
using System;
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The JpegTran opimizer runner.
    /// </summary>
    public sealed class JpegTranRunner : Tool<ImageOptimizerToolSettings>
    {
        #region Fields
        private readonly ICakeEnvironment _Environment;
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JpegTranRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The locator.</param>
        public JpegTranRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
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

            //string.Format(CultureInfo.CurrentCulture, "/c jpegtran -copy none -optimize -progressive \"{0}\" \"{1}\"", sourceFile, resultFile);

            builder.Append("-copy none");
            builder.Append("-optimize");
            builder.Append("-progressive");

            builder.AppendQuoted(settings.SourcePath.MakeAbsolute(_Environment).FullPath);

            builder.AppendQuoted(settings.OutputPath.MakeAbsolute(_Environment).FullPath);
            
            return builder;
        }



        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "JpegTran";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "jpegtran.exe" };
        }
        #endregion
    }
}