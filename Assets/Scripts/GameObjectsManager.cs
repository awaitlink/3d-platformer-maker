using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectsManager : MonoBehaviour {

    public Text objectNameText;

    [Header("Base Objects")]
    public GameObject player;
    public GameObject playerTop;
    public GameObject finish;
    public GameObject finishTop;
    public GameObject fall;

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
                case "Player": needToSaveChild = true; break;
                case "Finish": needToSaveChild = true; break;
            }

            data += go.name;
            data += SaveGameObjectData(go);

            if (needToSaveChild)
            {
                GameObject child = go.transform.GetChild(0).gameObject;
                data += child.name;
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
               go.transform.rotation.eulerAngles.ToString() +
               go.transform.localScale.ToString() +
               go.GetComponent<Renderer>().material.color.ToString() +
               "\r\n";
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

    public void LoadLevel()
    {
        string path = Application.persistentDataPath + "/level.txt";

        if (System.IO.File.Exists(path))
        {
            string data = System.IO.File.ReadAllText(path);
            string[] objects = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            char[] delims = "(),".ToCharArray();
            foreach (string obj in objects)
            {
                string[] objInfoParts = obj.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    switch (objInfoParts[0])
                    {
                        case "Player":
                            ParseObject(objInfoParts, player);
                            break;
                        case "Player[Top]":
                            ParseObject(objInfoParts, playerTop);
                            break;
                        case "Finish":
                            ParseObject(objInfoParts, finish);
                            break;
                        case "Finish[Top]":
                            ParseObject(objInfoParts, finishTop);
                            break;
                        case "Do not fall here!":
                            ParseObject(objInfoParts, fall);
                            break;
                        default:
                            ParseObject(objInfoParts);
                            break;
                    }
                }
                catch (Exception e)
                {
                    objectNameText.text = "ERROR: Level data corrupted.\nSome parts may be missing.";
                }
            }
        }
        else
        {
            string readmePath = Application.persistentDataPath + "/README.txt";
            System.IO.File.WriteAllText(readmePath, "Please put your level in this folder with name:\r\n\r\nlevel.txt\r\n\r\nand click Load again.");
            ShowExplorer(readmePath);
        }
    }

    private void ParseObject(string[] data)
    {
        Vector3 position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
        Vector3 rotation = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
        Vector3 scale    = new Vector3(float.Parse(data[7]), float.Parse(data[8]), float.Parse(data[9]));
        Color color = new Color(float.Parse(data[11]), float.Parse(data[12]), float.Parse(data[13]));

        PrimitiveType parsed_enum = (PrimitiveType) Enum.Parse(typeof(PrimitiveType), data[0]);
        GameObject newObject = GameObject.CreatePrimitive(parsed_enum);

        newObject.transform.position = position;
        newObject.transform.rotation = Quaternion.Euler(rotation);
        newObject.transform.localScale = scale;
        newObject.GetComponent<Renderer>().material.color = color;
    }

    private void ParseObject(string[] data, GameObject dest)
    {
        Vector3 position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
        Vector3 rotation = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
        Vector3 scale = new Vector3(float.Parse(data[7]), float.Parse(data[8]), float.Parse(data[9]));
        Color color = new Color(float.Parse(data[11]), float.Parse(data[12]), float.Parse(data[13]));

        dest.transform.position = position;
        dest.transform.rotation = Quaternion.Euler(rotation);
        dest.transform.localScale = scale;
        dest.GetComponent<Renderer>().material.color = color;
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
