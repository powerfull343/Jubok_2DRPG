using UnityEngine;
using System.Collections;

namespace LobbyButtonFunc
{
    public enum IDSUBPANEL
    {
        PANELID_NONE = -1,
        PANELID_BLACKSMITH,
        PANELID_TOWNHALL,
        PANELID_GROCERYSTORE,
        PANELID_BATTLEFIELD,
        PANELID_PLAYERSTAT,
        PANELID_OPTIONMENU,

    }

    public abstract class LobbyButton : MonoBehaviour
    {
        protected abstract void PlayAnimation();
        protected abstract void StopAnimation();
        public abstract void ClickFunction();
    }

}