using System.Collections.Generic;

namespace Szn.Framework.UtilPackage.Event
{
    public enum EventKey
    {
        None,
        One,
        Two,
        Three,
        Four,
        Max
    }

    public class EventKeyComparer : IEqualityComparer<EventKey>
    {
        public bool Equals(EventKey InX, EventKey InY)
        {
            return (int) InX == (int) InY;
        }

        public int GetHashCode(EventKey InObj)
        {
            return (int) InObj;
        }
    }
}