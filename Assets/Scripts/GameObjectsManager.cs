using UnityEngine;
using UnityEngine.UI;

public class GameObjectsManager : MonoBehaviour {

    public Text objectNameText;

    [HideInInspector]
    public static GameObject lastSelectedObject = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            lastSelectedObject = GetSelectedObject();
            objectNameText.text = lastSelectedObject.name;
        }
    }

    public void DeleteCurrent()
    {
        if (lastSelectedObject != null && !lastSelectedObject.tag.Equals("DontDelete"))
        {
            Destroy(lastSelectedObject);
            lastSelectedObject = null;
        }
    }

    GameObject GetSelectedObject()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            return interactionInfo.collider.gameObject;
        }
        return lastSelectedObject;
    }

}
