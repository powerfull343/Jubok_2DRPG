using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _Instance;
    protected static bool newObject = false;

    protected Singleton() { }

    protected void CreateInstance()
    {
        if (_Instance == null)
        {
            _Instance = this.gameObject.GetComponent<T>();
            //SetOwnProperty(_Instance);
        }
        else if (_Instance != this && newObject)
            Destroy(gameObject);
    }

    public static T GetInstance()
    {
        if (_Instance == null)
            Debug.Log("You Cannot Create Instance");

        return _Instance;
    }

    protected void SetOwnProperty(T SingletonTarget)
    {
        Type OwnType = this.GetType();
        FieldInfo[] TargetFields = OwnType.GetFields(
            BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);

        Type InstanceType = _Instance.GetType();
        FieldInfo[] CopyFields = InstanceType.GetFields(
            BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);

        //this.member = _Instance.member;
        for (int i = 0; i < TargetFields.Length; ++i)
        {
            Debug.Log("TargetField " + i.ToString() + "th Name : " +
                TargetFields[i].Name);
            Debug.Log("CopyFields " + i.ToString() + "th Name : " +
                CopyFields[i].Name);


            //Debug.Log(TargetFields[i].Name);
            TargetFields[i].SetValue(this,
                CopyFields[i].GetValue(_Instance));
        }

        //Debug.LogError("====End====");
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


