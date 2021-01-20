using System;

namespace Kamina2.Core.Common
{
    public class TimeService : ITimeService
    {
        public TimeService(DateTime startTime)
        {
            StartTime = startTime;
        }

        public DateTime StartTime { get; }

        public DateTime GetCurrent()
        {
            return DateTime.Now;
        }
    }
}