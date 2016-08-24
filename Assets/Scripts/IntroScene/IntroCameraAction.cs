using UnityEngine;
using System.Collections;

public class IntroCameraAction : MonoBehaviour {

    //Reference
    private IEnumerator _Coroutine;
    private Camera _cam;
    private GameObject MainCamObject;
    private TweenColor _HideBgTexture;  
    //Camera Move is Done use alpha blending What Target To BG?

    //float
    public float durationtime = 0.5f;
    public float MinusAmount = 0.1f;

    void Awake()
    {
        MainCamObject = GameObject.FindGameObjectWithTag("MainCamera");
        _cam = GetComponent<Camera>();
        _HideBgTexture = GameObject.FindGameObjectWithTag("BackGround").GetComponent<TweenColor>();
    }

    void OnEnable () {
        MainCamObject.SetActive(false);
        _Coroutine = InToGateAction(durationtime);        
        StartCoroutine(_Coroutine);
    }

    private IEnumerator InToGateAction(float _durationTime)
    {
        while (_cam.fieldOfView >= 4)
        {
            _cam.fieldOfView -= MinusAmount;
            yield return new WaitForSeconds(_durationTime);
        }

        _HideBgTexture.enabled = true;
        yield break;
    }
	
	
}
