using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

public abstract class DisposableObject : IDisposable
{
    protected FieldInfo[] m_TargetFields;
    public bool isRemoved { get { return this == null; } }

    //문제점. 클래스 내에 Collection 객체가 존재시 위험할수도 있다.
    public virtual void Dispose()
    {
        Debug.Log("Dispose");
    }
    

    protected void SetOwnProperty()
    {
        Type OwnType = this.GetType();
        m_TargetFields = OwnType.GetFields(
            BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);

    }

}
