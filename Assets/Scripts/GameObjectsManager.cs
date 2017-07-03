using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameObjectsManager : MonoBehaviour {

    [Header("UI Elements")]
    public Text objectNameText;
    public Button deleteButton;
    public GameObject spawnToggle;
    public GameObject saveUI;
    public InputField saveUIInputField;
    public GameObject loadUI;
    public GameObject loadUIContent;

    [Header("Base Objects")]
    public GameObject player;
    public GameObject playerTop;
    public GameObject finish;
    public GameObject finishTop;
    public GameObject fall;

    [HideInInspector]
    public static GameObject lastSelectedObject = null;
    [HideInInspector]
    public static bool isSaveShown = false;

    private GameObject elementPrefab;

    void Start()
    {
        saveUI.SetActive(false);
        loadUI.SetActive(false);
        elementPrefab = Resources.Load<GameObject>("Element");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            lastSelectedObject = GetSelectedObject();
            objectNameText.text = lastSelectedObject.name;
        }

        if (IsDeletionAvailable())
        {
            deleteButton.gameObject.SetActive(true);
            if (lastSelectedObject.name != "Quad" && lastSelectedObject.name != "Plane")
            {
                spawnToggle.gameObject.SetActive(true);
            }
            else
            {
                spawnToggle.gameObject.SetActive(false);
            }
        }
        else
        {
            deleteButton.gameObject.SetActive(false);
            spawnToggle.gameObject.SetActive(false);
        }
    }

    public void CloseSaveUI()
    {
        saveUI.SetActive(false);
        isSaveShown = false;
    }

    public void PrepareForSaving()
    {
        saveUI.SetActive(true);
        isSaveShown = true;
    }

    public void SaveLevel()
    {
        string fileName = saveUIInputField.text;
        string path = Application.persistentDataPath + "/" + fileName + ".3pm";

        bool isValid = !string.IsNullOrEmpty(fileName) &&
              fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
              !File.Exists(path);

        if (isValid)
        {
            saveUI.SetActive(false);
            isSaveShown = false;

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

            File.WriteAllText(path, data);
            ShowExplorer(path);
        }
        else
        {
            NotificationSystem.instance.ShowNotification("Filename is not valid, or file already exists. Please choose another one.");
        }
    }

    private string SaveGameObjectData(GameObject go)
    {
        Spawner s = go.GetComponent<Spawner>();

        string data = "";

        data += go.transform.position.ToString() +
               go.transform.rotation.eulerAngles.ToString() +
               go.transform.localScale.ToString() +
               go.GetComponent<Renderer>().material.color.ToString();

        if (s != null)
        {
            data += go.GetComponent<Spawner>().isSpawning.ToString();
        }

        data += "\r\n";

        return data;
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

    public void CloseLoadUI()
    {
        loadUI.SetActive(false);
    }

    public void PrepareForLoading()
    {
        for (int i = 0; i < loadUIContent.transform.childCount; i++)
        {
            GameObject element = loadUIContent.transform.GetChild(i).gameObject;
            Destroy(element);
        }

        string[] levels = Directory.GetFiles(Application.persistentDataPath + "/", "*.3pm");
        foreach (string fileName in levels)
        {
            GameObject newElement = Instantiate(elementPrefab, loadUIContent.transform);
            newElement.GetComponentInChildren<Text>().text = fileName.Replace(".3pm", "").Replace(Application.persistentDataPath + "/", "");

            Button newElementButton = newElement.GetComponent<Button>();
            newElementButton.onClick.AddListener(delegate { LoadLevel(newElementButton); });
        }

        loadUI.SetActive(true);
    }

    public void LoadLevel(Button b)
    {
        loadUI.SetActive(false);

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject element = transform.GetChild(i).gameObject;
            if (!element.tag.Equals("DontDelete"))
            {
                Destroy(element);
            }
        }

        string path = Application.persistentDataPath + "/" + b.GetComponentInChildren<Text>().text + ".3pm";
        string data = File.ReadAllText(path);
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
                NotificationSystem.instance.ShowNotification("ERROR: Level data corrupted.\nSome parts may be missing.");
            }
        }
        lastSelectedObject = player;
        objectNameText.text = player.name;
    }

    private void ParseObject(string[] data)
    {
        Vector3 position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
        Vector3 rotation = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
        Vector3 scale    = new Vector3(float.Parse(data[7]), float.Parse(data[8]), float.Parse(data[9]));
        Color color = new Color(float.Parse(data[11]), float.Parse(data[12]), float.Parse(data[13]));

        PrimitiveType parsed_enum = (PrimitiveType) Enum.Parse(typeof(PrimitiveType), data[0]);
        GameObject newObject = GameObject.CreatePrimitive(parsed_enum);

        newObject.transform.parent = gameObject.transform;
        newObject.transform.position = position;
        newObject.transform.rotation = Quaternion.Euler(rotation);
        newObject.transform.localScale = scale;
        newObject.GetComponent<Renderer>().material.color = color;

        Spawner ns = newObject.AddComponent<Spawner>();
        ns.isSpawning = bool.Parse(data[15]);
        ns.type = parsed_enum;
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
        if (IsDeletionAvailable())
        {
            Destroy(lastSelectedObject);
            lastSelectedObject = null;
        }
    }

    private bool IsDeletionAvailable()
    {
        return lastSelectedObject != null && !lastSelectedObject.tag.Equals("DontDelete");
    }

    private GameObject GetSelectedObject()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            return interactionInfo.collider.gameObject;
        }
        return lastSelectedObject;
    }

    public void SwitchBetweenObjectAndTop()
    {
        GameObject[][] pairs = new GameObject[][]
        {
            new GameObject[] { player, playerTop },
            new GameObject[] { finish, finishTop }
        };

        foreach (GameObject[] pair in pairs)
        {
            if (Equals(pair[0], lastSelectedObject))
                lastSelectedObject = pair[1];

            else if (Equals(pair[1], lastSelectedObject))
                lastSelectedObject = pair[0];
        }

        objectNameText.text = lastSelectedObject.name;
    }

}
