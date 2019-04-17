using System.Collections.Generic;

public class MultiKeyDictionary<T1, T2, T3> : Dictionary<T1, Dictionary<T2, T3>>
{
    public new Dictionary<T2, T3> this[T1 InKey]
    {
        get
        {
            Dictionary<T2, T3> returnObj;
            if (!TryGetValue(InKey, out returnObj))
            {
                returnObj = new Dictionary<T2, T3>();
                Add(InKey, returnObj);
            }

            return returnObj;
        }
    }

    public bool ContainsKey(T1 InKey1, T2 InKey2)
    {
        Dictionary<T2, T3> returnObj;

        if (TryGetValue(InKey1, out returnObj))
        {
            if (null != returnObj)
            {
                return returnObj.ContainsKey(InKey2);
            }
        }

        return false;
    }
}
