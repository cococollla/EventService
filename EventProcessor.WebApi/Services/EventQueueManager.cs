using EventProcessor.WebApi.Data.Models;
using System.Collections.Concurrent;

namespace EventProcessor.WebApi.Services
{
    public static class EventQueueManager
    {
        private static readonly ConcurrentQueue<Event> _eventQueue = new ConcurrentQueue<Event>();

        public static void EnqueueEvent(Event newEvent)
        {
            _eventQueue.Enqueue(newEvent);
        }

        public static bool TryPeekEvent(out Event dequeuedEvent)
        {
            return _eventQueue.TryPeek(out dequeuedEvent);
        }

        public static bool TryDequeueEvent()
        {
            return _eventQueue.TryDequeue(out _);
        }

        public static bool TryDequeueEvent(out Event dequeuedEvent)
        {
            return _eventQueue.TryDequeue(out dequeuedEvent);
        }

        public static void ClearQueue()
        {
            while (_eventQueue.TryDequeue(out _)) { }
        }

        public static int GetQueueCount()
        {
            return _eventQueue.Count;
        }
    }
}
