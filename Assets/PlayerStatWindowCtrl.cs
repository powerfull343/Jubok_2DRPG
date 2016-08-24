using UnityEngine;
using System.Collections;

public class PlayerStatWindowCtrl : MonoBehaviour {

    private STATPANELID CurrentOpenPanel;
    private STATPANELID ClosingPanel;

    public Animator ScrollAnimator;
    public UIPanel[] SubPanels;
    public UISprite[] ButtonBGSprite;

    void OnEnable()
    {
        CurrentOpenPanel = STATPANELID.ID_INVENTORY;
        ClosingPanel = STATPANELID.ID_NONE;

        int Count = 0;
        foreach(UIPanel panel in SubPanels)
        {
            if(panel == null)
            {
                Debug.Log(Count.ToString() + "th of Panel is null");
                return;
            }
            Count++;
        }

        Count = 0;
        foreach (UISprite Sprite in ButtonBGSprite)
        {
            if (Sprite == null)
            {
                Debug.Log(Count.ToString() + "th of Sprite is null");
                return;
            }
            Sprite.color = Color.cyan;
            Count++;
        }

        if (ScrollAnimator == null)
        {
            Debug.Log("ScrollAnimator is null");
            return;
        }
    }

    IEnumerator HidingSubPanel()
    {
        while (true)
        {
            if (SubPanels[(int)ClosingPanel].alpha > 0)
            {
                SubPanels[(int)ClosingPanel].alpha -= 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                SubPanels[(int)ClosingPanel].gameObject.SetActive(false);
                ScrollAnimator.SetTrigger("Close");
                yield break;
            }
        }
    }

    IEnumerator OpeningSubPanel()
    {
        while (true)
        {
            if (SubPanels[(int)CurrentOpenPanel].alpha < 1)
            {
                SubPanels[(int)CurrentOpenPanel].alpha += 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
            else
                yield break;
        }
    }

    public void OpenSubPanel()
    {
        SubPanels[(int)CurrentOpenPanel].gameObject.SetActive(true);
        SubPanels[(int)CurrentOpenPanel].alpha = 0;
        StartCoroutine("OpeningSubPanel");
    }

    void ChangeButtonColor()
    {
        ButtonBGSprite[(int)CurrentOpenPanel].color = Color.green;
        ButtonBGSprite[(int)ClosingPanel].color = Color.cyan;
    }

    public void Openinventory()
    {
        if (CurrentOpenPanel == STATPANELID.ID_INVENTORY)
            return;
       
        //SubPanel setting
        ClosingPanel = CurrentOpenPanel;
        CurrentOpenPanel = STATPANELID.ID_INVENTORY;

        //Button Setting
        ChangeButtonColor();

        //Start Hiding
        StartCoroutine("HidingSubPanel");

    }

    public void OpenDetailStat()
    {
        if (CurrentOpenPanel == STATPANELID.ID_DETAILSTAT)
            return;

        //SubPanel setting
        ClosingPanel = CurrentOpenPanel;
        CurrentOpenPanel = STATPANELID.ID_DETAILSTAT;
        
        //Button Setting
        ChangeButtonColor();

        //Start Hiding
        StartCoroutine("HidingSubPanel");
    }

    public void OpenSkillStat()
    {
        if (CurrentOpenPanel == STATPANELID.ID_SKILLSTAT)
            return;

        //SubPanel setting
        ClosingPanel = CurrentOpenPanel;
        CurrentOpenPanel = STATPANELID.ID_SKILLSTAT;

        //Button Setting
        ChangeButtonColor();

        //Start Hiding
        StartCoroutine("HidingSubPanel");
    }
}

