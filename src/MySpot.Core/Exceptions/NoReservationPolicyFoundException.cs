﻿using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions
{
    public sealed  class NoReservationPolicyFoundException : CustomExcption
    {
        public JobTitle JobTitle { get; }

        public NoReservationPolicyFoundException(JobTitle jobTitle) 
            : base($"No reservation policy for {jobTitle} found.")
        {
            JobTitle = jobTitle;
        }
    }
}
