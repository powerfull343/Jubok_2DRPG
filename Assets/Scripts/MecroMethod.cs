using UnityEngine;
using System.Collections;

namespace Mecro
{
    public sealed class MecroMethod : MonoBehaviour
    {
        /// <summary>
        /// Find Component in this Gameobject and checking exist Component
        /// </summary>

        public static T CheckGetComponent<T>(GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError(component.GetType() + " component is null");
            }

            return component;
        }

        /// <summary>
        /// Find Component in this Transform and checking exist Component
        /// </summary>

        public static T CheckGetComponent<T>(Transform trans) where T : Component
        {
            T component = trans.gameObject.GetComponent<T>();

            if (component == null)
            {
                Debug.Log(component.GetType() + 
                    " component is null");
            }

            return component;
        }

        public static void CheckExistObject<T>(T obj) where T : Object
        {
            if (obj == null)
            {
                Debug.LogError(obj.name + " object is null");
                return;
            }
        }

        public static void CheckExistComponent<T>(T comp) where T : Component
        {
            if (comp == null)
            {
                Debug.LogError(comp.name + " Component is null");
                return;
            }
        }

        public static GameObject Instantinate_Extension(
            GameObject Original, Transform Parent)
        {
            if (Original == null)
                return null;

            GameObject ResultObject = Instantiate(Original) as GameObject;

            ResultObject.SetActive(false);
            ResultObject.transform.parent = Parent.transform;
            ResultObject.transform.localScale = Vector3.one;
            ResultObject.transform.localPosition = Vector3.zero;
            ResultObject.SetActive(true);

            return ResultObject;
        }

        public static int AngleDir(Vector3 vTemp, Vector3 vTargetDir, Vector3 vUp)
        {
            Vector3 vCrossedVector = Vector3.Cross(vTemp, vTargetDir);
            float dir = Vector3.Dot(vCrossedVector, vUp);

            if (dir > 0.0)
                return 1;
            else if (dir < 0.0)
                return -1;
            else
                return 0;
        }

        public static float GetAngle(Vector3 vTemp, Vector3 vTargetDir)
        {
            float angle = Vector3.Angle(vTemp, vTargetDir);

            if (AngleDir(vTemp, vTargetDir, Vector3.up) == -1)
            {
                angle = 360.0f - angle;
                if (angle > 359.9999f)
                    angle -= 360.0f;
                return angle;
            }
            else
                return angle;
        }

        public static Vector3 NGUIToNoramlWorldPos(Vector3 NGUIWorldPos)
        {
            Vector3 vResult;

            vResult = BattleScene_NGUI_Panel.GetInstance(
                ).NGUICamera.WorldToScreenPoint(NGUIWorldPos);
            vResult = Camera.main.ScreenToWorldPoint(vResult);

            return vResult;
        }

        public static Vector3 NormalToNGUIWorldPos(Vector3 NormalWorldPos)
        {
            Vector3 vResult;

            vResult = Camera.main.WorldToScreenPoint(NormalWorldPos);
            vResult = BattleScene_NGUI_Panel.GetInstance(
                ).NGUICamera.ScreenToWorldPoint(vResult);

            return vResult;
        }

        public static void SetPartent(Transform ChildTarget, Transform ParentTarget)
        {
            ChildTarget.parent = ParentTarget;
            ChildTarget.localPosition = Vector3.zero;
            ChildTarget.localScale = Vector3.one;

            UIWidget ChildWidget = ChildTarget.GetComponent<UIWidget>();
            if (ChildWidget != null)
                ChildWidget.ParentHasChanged();
        }

        //Second Option use Vector3.zero
        public static Vector3 GetWorldPos(Transform Target,
            Vector3 vCalcWorldPos)
        {
            Vector3 vResult = Vector3.zero;

            if (vCalcWorldPos != Vector3.zero)
                vResult = vCalcWorldPos;

            vResult += Target.localPosition;
            if (Target.parent != null && !Target.parent.CompareTag("HighestPanel"))
            {
                vResult = GetWorldPos(Target.parent, vResult);
            }

            return vResult;
        }

        public static string TraceHierarchyObject(Transform Target)
        {
            string strResult = Target.name;

            if (Target.parent != null)
                strResult = TraceHierarchyObject(Target.parent) + "\\" + strResult;

            return strResult;
        }

        public static void ShowLogConsole(object T)
        {
            Debug.Log(T.ToString() + T);
        }

        public static void ShowSceneLogConsole(object Message, bool ShowConsole)
        {
            if (DebugingPanel.GetInstance().gameObject != null)
                DebugingPanel.GetInstance().AddDebugingLog(Message);

            if (ShowConsole)
                Debug.Log(Message);
        }
    }
}