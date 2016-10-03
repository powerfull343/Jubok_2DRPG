using UnityEngine;
using System.Collections;


public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _Instance;
    protected static bool newObject = false;

    protected Singleton() { }

    protected void CreateInstance()
    {
        if (_Instance == null)
            _Instance = this.gameObject.GetComponent<T>();
        else if (_Instance != this && newObject)
            Destroy(gameObject);

    }

    public static T GetInstance()
    {
        if (_Instance == null)
            Debug.Log("You Cannot Create Instance");

        return _Instance;
    }

    //    //Class Propertys
    //    protected static readonly T _instance =
    //        new T();

    //    protected static T _Existinstance;

    //    //Constructor

    //    private Singleton()   {}

    //    /// <summary>
    //    /// if you exist member use to singleton pattern 
    //    /// use this Constructor 
    //    /// </summary>

    //    //Inner Methods
    //    public static T GetInstance()
    //    {
    //        return _instance;
    //    }

    //    public static T GetExistInstance(T obj)
    //    {
    //        if(_Existinstance == null)
    //            _Existinstance = obj;

    //        return _Existinstance;
    //    }
}


