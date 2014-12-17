using Logary;
using Logary.Configuration;
using Logary.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogaryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var logary = LogaryFactory.New("Logary Example",
                with => with.Target<Debugger.Builder>("debugger")
                    .Target<Logstash.Builder>(
                    "ls", conf => conf.Target
                       .Hostname("127.0.0.1")
                       .Port(1939)
                       .EventVersion(Logstash.EventVersion.One)
                       .Done()
                    )
            );

            var logger = logary.GetLogger("Sample.Config");

            logger.Log("Hello world", LogLevel.Debug, new
            {
                important = "yes",
                start = DateTime.Now.AddDays(-2),
                end = DateTime.Now,
                age = new Random().Next(80)
            });

            logger.Log(LogLevel.Fatal, "Fatal error occurred");

            logger.Verbose("Dispose the logger to flush all the logs!!", "tag1", "tag2");

            logary.Dispose();            
        }
    }
}
