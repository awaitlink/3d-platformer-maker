using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public static int MAX_SPAWNED_OBJECTS = 10;

    public PrimitiveType type;
    public bool isSpawning;

    private float timePassed;

    void Update()
    {
        if (State.isPlaying)
        {
            timePassed += Time.deltaTime;
            if (isSpawning && timePassed >= 1f)
            {
                timePassed--;
                GameObject newObj = GameObject.CreatePrimitive(type);
                newObj.transform.parent = transform;
                newObj.transform.position = transform.position;
                newObj.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
                newObj.AddComponent<Rigidbody>();

                newObj.transform.localScale = Vector3.zero;
                StartCoroutine(ScaleUp(newObj.transform));
            }

            if (transform.childCount > MAX_SPAWNED_OBJECTS)
            {
                Transform c = transform.GetChild(0);
                c.localScale -= Vector3.one * Time.deltaTime;
                if (c.localScale.x <= 0f)
                {
                    Destroy(c.gameObject);
                }
            }
        }
        else
        {
            timePassed = 0f;
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        if (isSpawning && (type == PrimitiveType.Quad || type == PrimitiveType.Plane))
        {
            isSpawning = false;
        }
    }

    private IEnumerator ScaleUp(Transform t)
    {
        for (int i = 0; i <= 50; i++)
        {
            if (t != null)
            {
                t.localScale += Vector3.one / 50f;
                yield return new WaitForSeconds(.01f);
            }
        }
    }

}
