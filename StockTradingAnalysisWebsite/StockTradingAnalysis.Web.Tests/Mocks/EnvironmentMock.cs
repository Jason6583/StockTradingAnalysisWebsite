﻿using StockTradingAnalysis.EventSourcing.DomainContext;
using StockTradingAnalysis.EventSourcing.Events;
using StockTradingAnalysis.EventSourcing.Messaging;
using StockTradingAnalysis.EventSourcing.Storage;
using StockTradingAnalysis.Interfaces.DomainContext;
using StockTradingAnalysis.Interfaces.Events;
using StockTradingAnalysis.Web.Tests.Objects;
using System.Collections.Generic;

namespace StockTradingAnalysis.Web.Tests.Mocks
{
	public static class EnvironmentMock
	{
		public static AggregateRepository<TAggregate> CreateEnvironment<TAggregate>(
			out EventBus eventBus,
			out EventStore eventStore,
			out SnapshotStore snapshotStore,
			out SnapshotProcessor snapshotProcessor,
			out InMemorySnapshotStore memorySnapshotStore,
			int snapshotThreashold = 10)
			where TAggregate : IAggregateRoot, new()
		{
			TestDatabase.Items.Clear();

			var savedHandler = new TestAggregateSavedEventHandler();
			DependencyServiceMock.SetMock(new List<DependencyDescriptor>()
			{
				new DependencyDescriptor(typeof (IEventHandler<TestAggregateCreatedEvent>),
					new TestAggregateCreatedEventHandler()),
				new DependencyDescriptor(typeof (IEventHandler<TestAggregateChangedEvent>),
					new TestAggregateChangedEventHandler()),
				new DependencyDescriptor(typeof (IEventHandler<AggregateSavedEvent>), savedHandler)
			});

			eventBus = new EventBus();
			eventStore = new EventStore(eventBus, new InMemoryEventStore(), PerformanceCounterMock.GetMock());
			memorySnapshotStore = new InMemorySnapshotStore();
			snapshotStore = new SnapshotStore(memorySnapshotStore);
			var repository = new AggregateRepository<TAggregate>(eventStore, snapshotStore);
			snapshotProcessor = new SnapshotProcessor(snapshotStore, new List<ISnapshotableRepository>() { repository },
				snapshotThreashold, PerformanceCounterMock.GetMock());
			savedHandler.Processor = snapshotProcessor;

			return repository;
		}

		public static AggregateRepository<TAggregate> CreateEnvironment<TAggregate>(
			out EventBus eventBus,
			out EventStore eventStore,
			int snapshotThreashold = 10)
			where TAggregate : IAggregateRoot, new()
		{
			TestDatabase.Items.Clear();

			var savedHandler = new TestAggregateSavedEventHandler();
			DependencyServiceMock.SetMock(new List<DependencyDescriptor>()
			{
				new DependencyDescriptor(typeof (IEventHandler<TestAggregateCreatedEvent>),
					new TestAggregateCreatedEventHandler()),
				new DependencyDescriptor(typeof (IEventHandler<TestAggregateChangedEvent>),
					new TestAggregateChangedEventHandler()),
				new DependencyDescriptor(typeof (IEventHandler<AggregateSavedEvent>), savedHandler)
			});

			eventBus = new EventBus();
			eventStore = new EventStore(eventBus, new InMemoryEventStore(), PerformanceCounterMock.GetMock());
			var snapshotStore = new SnapshotStore(new InMemorySnapshotStore());
			var repository = new AggregateRepository<TAggregate>(eventStore, snapshotStore);
			var snapshotProcessor = new SnapshotProcessor(snapshotStore,
				new List<ISnapshotableRepository>() { repository },
				snapshotThreashold, PerformanceCounterMock.GetMock());
			savedHandler.Processor = snapshotProcessor;

			return repository;
		}
	}
}