#region Using Statements
using System.IO;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Core.Configuration;
using Cake.Testing;

using NSubstitute;
#endregion



namespace Cake.ImageOptimizer.Tests
{
    internal static class CakeHelper
    {
        #region Methods
        public static ICakeEnvironment CreateEnvironment()
        {
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            environment.WorkingDirectory = Directory.GetCurrentDirectory();
            environment.WorkingDirectory = environment.WorkingDirectory.Combine("../../../");

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

            return new CakeContext(new FileSystem(), enviroment, new Globber(new FileSystem(), enviroment), new DebugLog(), CreateArguments(), new ProcessRunner(enviroment, new DebugLog()), new WindowsRegistry(), new ToolLocator(enviroment, new ToolRepository(enviroment), new ToolResolutionStrategy(new FileSystem(), enviroment, new Globber(new FileSystem(), enviroment), CreateConfiguration())), new CakeDataService());
        }
        #endregion
    }
}