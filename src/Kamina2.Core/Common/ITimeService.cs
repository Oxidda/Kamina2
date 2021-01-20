using System;

namespace Kamina2.Core.Common
{
    public interface ITimeService
    {
        DateTime GetCurrent();
        DateTime StartTime { get; }
    }
}