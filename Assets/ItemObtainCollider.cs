using UnityEngine;
using System.Collections;

public class ItemObtainCollider : MonoBehaviour {

    [SerializeField]
    private Transform mBagUI;
    private float mStartTime;
    private float mEndTime = 0.05f;

    void Start()
    {
        Mecro.MecroMethod.CheckGetComponent<Transform>(mBagUI);
        transform.localScale = Vector3.one * 0.1f;
        transform.position = Mecro.MecroMethod.NGUIToNoramlWorldPos(mBagUI.position);
    }
  

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            mStartTime = Time.time;
            //mBagUI.localScale = Vector3.one;
            StopCoroutine("HighlightedScale");

            StartCoroutine("HighlightedScale");
        }
    }

    IEnumerator HighlightedScale()
    {
        while(true)
        {
            float Freq = (Time.time - mStartTime) / mEndTime;
            TweenScale.Begin(mBagUI.gameObject, mEndTime, new Vector3(1.2f, 1.2f, 1f));

            if (Freq >= 1f)
                break;

            yield return new WaitForFixedUpdate();
        }

        mStartTime = Time.time;

        while (true)
        {
            float Freq = (Time.time - mStartTime) / mEndTime;
            TweenScale.Begin(mBagUI.gameObject, mEndTime, Vector3.one);

            if (Freq >= 1f)
                break;

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    
}
