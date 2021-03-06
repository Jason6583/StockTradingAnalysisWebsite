﻿using System;
using StockTradingAnalysis.Interfaces.Events;

namespace StockTradingAnalysis.Web.Tests.Objects
{
    public class TestEvent : DomainEvent
    {
        public TestEvent(Guid aggregateId)
            : base(aggregateId, typeof(TestAggregate))
        {
        }

        protected TestEvent()
        {

        }
    }
}