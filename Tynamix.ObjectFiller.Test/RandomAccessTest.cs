﻿namespace ObjectFiller.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;
    using Tynamix.ObjectFiller;


    public class RandomAccessTest
    {
        [Fact]
        public void GetRandomIntOnDifferentThreadsGetsDifferentResults()
        {
            var numberToGenerate = 1000;
            var intRange = new IntRange(1, 10);
            var task1 = Task.Factory.StartNew(() =>
                {
                    var taskResults = new List<int>();
                    for (int i = 0; i < numberToGenerate; ++i)
                    {
                        taskResults.Add(Randomizer<int>.Create(intRange));
                    }
                    return taskResults;
                },
                TaskCreationOptions.LongRunning
            );
            var task2 = Task.Factory.StartNew(() =>
                {
                    var taskResults = new List<int>();
                    for (int i = 0; i < numberToGenerate; ++i)
                    {
                        taskResults.Add(Randomizer<int>.Create(intRange));
                    }
                    return taskResults;
                },
                TaskCreationOptions.LongRunning
            );
            var results = Task.WhenAll(task1, task2).Result;
            var firstResults = results[0].Sum();
            var secondResults = results[1].Sum();
            Assert.NotEqual(firstResults, secondResults);
        }
    }
}