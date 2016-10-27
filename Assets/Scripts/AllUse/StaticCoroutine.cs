using UnityEngine;
using System.Collections;

public class StaticCoroutine : MonoBehaviour {

    private static StaticCoroutine _Instance;
    public static StaticCoroutine GetInstance
    {
        get { return _Instance; }
    }
    private IEnumerator _CoroutineTarget;
    public IEnumerator CoroutineTarget
    {
        get { return _CoroutineTarget; }
        set { _CoroutineTarget = value; }
    }

    void Awake()
    {
        _Instance = this;
    }

    public void DoCoroutine()
    {
        _Instance.StartCoroutine(_CoroutineTarget); 
    }
}



