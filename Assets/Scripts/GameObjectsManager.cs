using System;
using System.Collections.Generic;
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

    public void SaveLevel()
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }

        string data = "";
        foreach (GameObject go in children)
        {
            bool needToSaveChild = false;
            switch (go.name)
            {
                case "Player": data += "player:"; needToSaveChild = true; break;
                case "Finish": data += "finish:"; needToSaveChild = true; break;
                case "Do not fall here!": data += "fall:"; break;
            }

            data += SaveGameObjectData(go);

            if (needToSaveChild)
            {
                GameObject child = go.transform.GetChild(0).gameObject;
                switch (go.name)
                {
                    case "Player(Top)": data += "playerTop:"; break;
                    case "Finish(Top)": data += "finishTop:"; break;
                }
                data += SaveGameObjectData(child);
            }
        }

        string path = Application.persistentDataPath + "/level_" + GenerateUniqueID() + ".txt";
        System.IO.File.WriteAllText(path, data);
        ShowExplorer(path);
    }

    private string SaveGameObjectData(GameObject go)
    {
        return go.transform.position.ToString() +
               go.transform.rotation.ToString() +
               go.transform.localScale.ToString() +
               go.GetComponent<Renderer>().material.color.ToString() +
               ";";
    }

    private string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString("N");
    }

    private void ShowExplorer(string itemPath)
    {
        itemPath = itemPath.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
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
