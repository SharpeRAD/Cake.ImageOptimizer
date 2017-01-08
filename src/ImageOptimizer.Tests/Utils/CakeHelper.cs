#region Using Statements
    using System;
    using System.IO;

    using Cake;
    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Tooling;
    using Cake.Core.Configuration;
    using Cake.Core.Diagnostics;

    using NSubstitute;
#endregion



namespace Cake.ImageOptimizer.Tests
{
    internal static class CakeHelper
    {
        #region Functions (4)
            public static ICakeEnvironment CreateEnvironment()
            {
                var environment = new CakeEnvironment(new CakePlatform(), new CakeRuntime(), new DebugLog());

                var dir = Directory.GetCurrentDirectory().Replace("\\bin\\Debug", "");
                environment.WorkingDirectory = dir;

                return environment;
            }

            public static ICakeArguments CreateArguments()
            {
                var environment = Substitute.For<ICakeArguments>();

                return environment;
            }
        
            public static ICakeConfiguration CreateConfiguration()
            {
                var environment = Substitute.For<ICakeConfiguration>();

                return environment;
            }



            public static ICakeContext CreateContext()
            {
                ICakeEnvironment enviroment = CakeHelper.CreateEnvironment();

                return new CakeContext(new FileSystem(), enviroment, new Globber(new FileSystem(), enviroment), new DebugLog(), CreateArguments(), new ProcessRunner(enviroment, new DebugLog()), new WindowsRegistry(), new ToolLocator(enviroment, new ToolRepository(enviroment), new ToolResolutionStrategy(new FileSystem(), enviroment, new Globber(new FileSystem(), enviroment), CreateConfiguration())));
            }
        #endregion
    }
}
