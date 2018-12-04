﻿using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Kin.Stellar.Sdk
{
    public interface IEventSource
    {
        NameValueCollection Headers { get; }
        string LastEventId { get; }
        string[] MessageTypes { get; }
        EventSource.EventSourceState ReadyState { get; }
        int Timeout { get; set; }
        Uri Url { get; set; }

        event EventHandler<EventSource.ServerSentErrorEventArgs> Error;
        event EventHandler<EventSource.ServerSentEventArgs> Message;
        event EventHandler<EventSource.StateChangeEventArgs> StateChange;

        Task Connect();
        void Dispose();
        void Shutdown();
    }
}