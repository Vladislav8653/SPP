using Example;
using Xunit;

namespace Core.Tests
{
    public class TracerTests
    {
        private readonly Tracer _tracer;
        private readonly Bar _bar;

        public TracerTests()
        {
            _tracer = new Tracer();
            _bar = new Bar(_tracer);
        }
        

        [Fact]
        public void StopTrace_ShouldUpdateThreadTime()
        {
            _tracer.StartTrace();
            Thread.Sleep(100); 
            _tracer.StopTrace();
            
            var result = _tracer.GetTraceResult();
            var threadInfo = result.Threads.FirstOrDefault();
            
            Assert.NotNull(threadInfo);
            Assert.True(threadInfo.Time >= 100); 
        }

        [Fact]
        public void GetTraceResult_ShouldReturnCorrectData()
        {
            _tracer.StartTrace();
            _bar.InnerMethod(200);
            _tracer.StopTrace();
            
            var result = _tracer.GetTraceResult();
            Assert.Equal(1, result.Threads.Count);
        }

        [Fact]
        public void ConcurrentTracing_ShouldNotInterfereBetweenThreads()
        {
            var threads = new Thread[2];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    _tracer.StartTrace();
                    _bar.InnerMethod(100);
                    _tracer.StopTrace();
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            // Act
            var result = _tracer.GetTraceResult();

            // Assert
            Assert.Equal(2, result.Threads.Count); 
        }
    }
}