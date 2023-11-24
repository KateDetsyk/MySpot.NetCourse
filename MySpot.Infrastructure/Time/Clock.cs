﻿using System.Runtime.CompilerServices;
using MySpot.Application.Services;

namespace MySpot.Infrastructure.Services
{
    internal sealed class Clock : IClock
    {
        public DateTime Current() => DateTime.Now;
    }
}
