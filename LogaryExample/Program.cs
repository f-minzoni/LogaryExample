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
                       .Hostname("localhost")
                       .Port(1939)
                       .EventVersion(Logstash.EventVersion.One)
                       .Done()
                    )
            );

            var logger = logary.GetLogger("Sample.Config");

            logger.Log("Hello world", LogLevel.Debug, new
            {
                important = "yes"
            });

            logger.Log(LogLevel.Fatal, "Fatal application error on finaliser thread");

            logger.Verbose("immegawd immegawd immegawd!!", "tag1", "tag2");

            var val = logger.TimePath("sample.config.compute_answer_to_everything", () =>
            {
                for (int i = 0; i < 100; i++)
                    System.Threading.Thread.Sleep(1);

                return 32;
            });

            logger.LogFormat(LogLevel.Warn, "{0} is the answer to the universe and everything", val);

            logger.Time(() => logger.Debug("I wonder how long this takes", "introspection", "navel-gazing"));

            try
            {
                throw new ApplicationException("thing went haywire");
            }
            catch (Exception e)
            {
                logger.DebugException("expecting haywire, so we're telling with debug", e, "haywire", "external");
            }
        }
    }
}
