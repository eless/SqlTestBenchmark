using BenchmarkDotNet.Running;
namespace SqlTestBenchmark31
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }

    }
}
