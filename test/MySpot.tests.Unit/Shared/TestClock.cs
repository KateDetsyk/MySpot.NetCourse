using MySpot.Application.Services;

namespace MySpot.tests.Unit.Shared
{
    internal sealed class TestClock : IClock
    {
        public DateTime Current() => new DateTime(2022, 2, 26);
    }
}
