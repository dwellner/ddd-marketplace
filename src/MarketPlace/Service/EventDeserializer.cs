using System;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace MarketPlace.Service
{
    static class EventDeserializer
    {
        public static object Deserialize(this ResolvedEvent resolvedEvent) => JsonConvert.DeserializeObject(
            GetEventJsonData(resolvedEvent),
            Type.GetType(GetMetaData(resolvedEvent).ClrType));

        private static EventMetaData GetMetaData(ResolvedEvent e) => JsonConvert.DeserializeObject<EventMetaData>(Encoding.UTF8.GetString(e.Event.Metadata));
        private static string GetEventJsonData(ResolvedEvent e) => Encoding.UTF8.GetString(e.Event.Data);
    }
}
