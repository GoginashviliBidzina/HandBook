﻿using System;
using HandBook.Shared;
using System.Reflection;
using System.Collections.Generic;
using HandBook.Infrastructure.DataBase;

namespace HandBook.Infrastructure.EventDispatching
{
    public class InternalDomainEventDispatcher : IInternalEventDispatcher<DomainEvent>
    {
        private readonly IDictionary<Type, List<Type>> eventhandlerMaps =
            new Dictionary<Type, List<Type>>();
        private readonly IServiceProvider serviceProvider;

        public InternalDomainEventDispatcher(IServiceProvider serviceProvider,
                                             Assembly domainEventsAssembly,
                                             params Assembly[] eventHandlersAssemblies)
        {
            eventhandlerMaps = EventHandlerMapping.DomainEventHandlerMapping<IHandleEvent<DomainEvent>>(domainEventsAssembly, eventHandlersAssemblies);
            this.serviceProvider = serviceProvider;
        }

        private void Dispatch(dynamic @event, DatabaseContext dbContext)
        {
            var type = @event.GetType();
            if (!eventhandlerMaps.ContainsKey(type))
                return;
            var @eventHandlers = eventhandlerMaps[type];

            foreach (var handler in @eventHandlers)
            {
                var domainEventHandler = serviceProvider.GetService(handler);
                if (domainEventHandler == null)
                {
                    domainEventHandler = Activator.CreateInstance(handler);
                }
                var handlerInstance = domainEventHandler as dynamic;
                handlerInstance.Handle(@event, dbContext);
            }
        }

        public void Dispatch(IReadOnlyList<DomainEvent> events, DatabaseContext dbContext)
        {
            try
            {
                foreach (var @event in events)
                {
                    Dispatch(@event, dbContext);
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public T GetService<T>() => (T)serviceProvider.GetService(typeof(T));
    }
}
