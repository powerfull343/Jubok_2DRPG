using UnityEngine;
using System.Collections;
using Mecro;

public class Singleton_Parent<T> : MonoBehaviour where T : Object {

    protected static T _Instance;
        
    void Awake()
    {
        _Instance = this.gameObject.GetComponent<T>();

        if (_Instance == null)
        {
            Debug.Log(_Instance.name + "Object is Null");
            return;
        }
    }

    public static T GetInstance()
    {
        return _Instance;
    }
}
