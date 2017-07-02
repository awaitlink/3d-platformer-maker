using UnityEngine;

public class GameObjectsManager : MonoBehaviour {

    public static GameObject lastSelectedObject = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            lastSelectedObject = GetSelectedObject();
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
