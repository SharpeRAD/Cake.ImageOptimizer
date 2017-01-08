#region Using Statements
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using System.IO;
    using System.Threading.Tasks;
    using System.Security.Cryptography;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Diagnostics;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// Optimizes images in bulk
    /// </summary>
    public class BulkImageOptimizer : IBulkImageOptimizer
    {
        #region Fields (8)
            private readonly IFileSystem _FileSystem;
            private readonly ICakeEnvironment _Environment;
            private readonly ICakeLog _Log;

            private readonly IImageOptimizerFactory _OptimizerFactory;

            private IList<string> _ImagesOptimized;
            private int _ImagesSkipped;
            private int _ImagesErrored;

            private double _SizeBefore;
            private double _SizeAfter;
        #endregion





        #region Constructor (1)
            /// <summary>
            /// Initializes a new instance of the <see cref="BaseOptimizer" /> class.
            /// </summary>
            /// <param name="fileSystem">The file system.</param>
            /// <param name="environment">The environment.</param>
            /// <param name="log">The log.</param>
            /// <param name="optimizerFactory">The factory to use when selecting the optimizer.</param>
            public BulkImageOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, IImageOptimizerFactory optimizerFactory)
            {
                if (fileSystem == null)
                {
                    throw new ArgumentNullException("fileSystem");
                }
                if (environment == null)
                {
                    throw new ArgumentNullException("environment");
                }
                if (log == null)
                {
                    throw new ArgumentNullException("log");
                }
                if (optimizerFactory == null)
                {
                    throw new ArgumentNullException("optimizerFactory");
                }

                _FileSystem = fileSystem;
                _Environment = environment;
                _Log = log;
                _OptimizerFactory = optimizerFactory;

                this.Clear();
            }
        #endregion





        #region Properties (8)
            /// <summary>
            /// Gets the list of paths to the optimized images
            /// </summary>
            public IList<string> ImagesOptimized
            {
                get
                {
                    return _ImagesOptimized;
                }
            }

            /// <summary>
            /// Gets the number of images skipped
            /// </summary>
            public int ImagesSkipped
            {
                get
                {
                    return _ImagesSkipped;
                }
            }

            /// <summary>
            /// Gets the number of errored images
            /// </summary>
            public int ImagesErrored
            {
                get
                {
                    return _ImagesErrored;
                }
            }



            /// <summary>
            /// Gets the size in bytes before optimization
            /// </summary>
            public double SizeBefore
            {
                get
                {
                    return _SizeBefore;
                }
            }

            /// <summary>
            /// Gets the size in bytes after optimization
            /// </summary>
            public double SizeAfter
            {
                get
                {
                    return _SizeAfter;
                }
            }

            /// <summary>
            /// Gets the size in bytes saved during optimization
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
            /// Gets the percent in size saved
            /// </summary>
            public double SavedPercent
            {
                get
                {
                    if (this.SizeAfter < this.SizeBefore)
                    {
                        return Math.Round((100 / this.SizeBefore) * this.SizeAfter, 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }



            //Hash
            private string GetHash(IFile file)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = file.OpenRead())
                    {
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","").ToLower();
                    }
                }
            }
        #endregion





        #region Functions (4)
            /// <summary>
            /// Optimizes Images from the settings SourceDirectory
            /// </summary>
            /// <param name="sourceDirectory">The directory of the original images.</param>
            /// <param name="outputDirectory">The directory of the optimized images.</param>
            /// <param name="settings">The settings to use when optimizing the images.</param>
            public IList<OptimizedFile> Optimize(DirectoryPath sourceDirectory, DirectoryPath outputDirectory, ImageOptimizerSettings settings)
            {
                DirectoryPath source = sourceDirectory.MakeAbsolute(_Environment);
                DirectoryPath output = outputDirectory.MakeAbsolute(_Environment);

                IDirectory dir = _FileSystem.GetDirectory(source);

                if (dir.Exists)
                {
                    FileConfig config = new FileConfig(_FileSystem, _Environment, _Log);
                    config.Load(settings.ConfigFile);

                    IList<IFile> files = dir.GetFiles(settings.SearchFilter, settings.SearchScope).ToList();

                    IList<OptimizedFile> results = this.Optimize("", source, output, files, config.Files);

                    config.AddResults(results);
                    config.Save(settings.ConfigFile);

                    return results;
                }
                else
                {
                    throw new Exception("The source directory does not exist '" + source.FullPath + "'");
                }
            }

            /// <summary>
            /// Optimizes a list of files using a given optimizer
            /// </summary>
            /// <param name="optimizer">The optimizer to use.</param>
            /// <param name="sourceDirectory">The directory of the original images.</param>
            /// <param name="outputDirectory">The directory of the optimized images.</param>
            /// <param name="files">The files to optimze.</param>
            /// <param name="configFiles">The config files to compare against.</param>
            public IList<OptimizedFile> Optimize(string optimizer, DirectoryPath sourceDirectory, DirectoryPath outputDirectory, IList<IFile> files, IList<OptimizedFile> configFiles)
            {
                //Clear
                IList<OptimizedFile> results = new List<OptimizedFile>();

                this.Clear();



                //Optimize
                if ((files.Count > 0) && (configFiles != null))
                {
                    Parallel.ForEach(files, file =>
                    {
                        FilePath source = sourceDirectory.GetRelativePath(file.Path);
                        FilePath output = outputDirectory.CombineWithFilePath(source);

                        OptimizedFile result = this.Optimize(optimizer, file, output, configFiles);

                        if (result != null)
                        {
                            results.Add(result);
                        }
                    });
                }
                


                //Complete
                this.OnCompleted(results);

                return results;
            }

            /// <summary>
            /// Optimizes a list of files using a given optimizer
            /// </summary>
            /// <param name="optimizer">The optimizer to use.</param>
            /// <param name="file">The file to optimze.</param>
            /// <param name="outputPath">The path of the optimized file.</param>
            /// <param name="configFiles">The config files to compare against.</param>
            public OptimizedFile Optimize(string optimizer, IFile file, FilePath outputPath, IList<OptimizedFile> configFiles)
            {
                //Check Config
                OptimizedFile config = configFiles.FirstOrDefault(o => o.Path.FullPath.ToLower() == file.Path.FullPath.ToLower());

                OptimizedFile optimizedFile = null;
                string hash = this.GetHash(file);



                if ((config == null) || config.RequiresOptimization(hash) || config.DifferentService(optimizer))
                {
                    //Optimize
                    ImageOptimizerResult result = _OptimizerFactory.Optimize(optimizer, file.Path, outputPath);

                    if ((result != null) && !result.HasError)
                    {
                        //Optimzed
                        optimizedFile = new OptimizedFile(file.Path, result.Service, result.ModifiedDate, hash, result.SizeBefore, result.SizeAfter);

                        _ImagesOptimized.Add(file.Path.FullPath);
                        _SizeBefore += result.SizeBefore;
                        _SizeAfter += result.SizeAfter;

                        _Log.Information("Optimized:  " + file.Path + " - Saved: " + result.SavedSize + " bytes (" + result.SavedPercent.ToString("N0") + "%)");
                    }
                    else if ((result.ErrorMessage == "Unsupported File") || (result.ErrorMessage == "Invalid FileSize"))
                    {
                        //Skipped
                        _ImagesSkipped++;

                        _SizeBefore += file.Length;
                        _SizeAfter += file.Length;

                        _Log.Information("Skipped:  " + file.Path + " - " + result.ErrorMessage);
                    }
                    else
                    {
                        //Errored
                        _ImagesErrored++;

                        _Log.Information("Errored:  " + file.Path + " - " + result.ErrorMessage);
                    }
                }
                else
                {
                    //Skipped
                    _ImagesSkipped++;

                    if (config != null)
                    {
                        optimizedFile = new OptimizedFile(file.Path, config.Service, config.OptimizedDate, hash, config.SizeBefore, config.SizeAfter);

                        _SizeBefore += config.SizeBefore;
                        _SizeAfter += config.SizeAfter;
                    }
                    else
                    {
                        _SizeBefore += file.Length;
                        _SizeAfter += file.Length;
                    }

                    _Log.Information("Skipped:  " + file.Path + " - Saved: " + config.SavedSize + " bytes (" + config.SavedPercent.ToString("N0") + "%)");
                }

                this.OnProgress(optimizedFile);

                return optimizedFile;
            }



            /// <summary>
            /// Clears all local flags
            /// </summary>
            public void Clear()
            {
                _ImagesOptimized = new List<string>();
                _ImagesSkipped = 0;
                _ImagesErrored = 0;

                _SizeBefore = 0;
                _SizeAfter = 0;
            }
        #endregion





        #region Events (4)
            /// <summary>
            /// Triggered when the optimizer finishes processing each image
            /// </summary>
            public event EventHandler<OptimizedFileEventArgs> Progress;

            /// <summary>
            /// Triggered when the optimizer finishes processing all images
            /// </summary>
            public event EventHandler<OptimizedFileEventArgs> Completed;



            private void OnProgress(OptimizedFile file)
            {
                if (this.Progress != null)
                {
                    this.Progress(this, new OptimizedFileEventArgs(file));
                }
            }

            private void OnCompleted(IList<OptimizedFile> files)
            {
                if (this.Completed != null)
                {
                    this.Completed(this, new OptimizedFileEventArgs(files));
                }
            }
        #endregion
    }
}
