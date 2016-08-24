using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MultiDictionary<TKey, TValue>
{
    private Dictionary<TKey, List<TValue>> mContainer = null;

    public MultiDictionary()
    {
        mContainer = new Dictionary<TKey, List<TValue>>();
    }



}
