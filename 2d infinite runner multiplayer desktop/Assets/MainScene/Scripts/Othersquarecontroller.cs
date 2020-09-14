using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Othersquarecontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isMoving = false;
    private float lastPosx;
    private float lastPosy;

    bool loopOnce;

    private void Start()
    {
        loopOnce = true;
    }

    private void Update()
    {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;

        if (loopOnce)
        {
            loopOnce = false;
            lastPosx = gameObject.transform.position.x;
            lastPosy = gameObject.transform.position.y;
            StartCoroutine(moveBox(this.gameObject, x, y));
        }
    }

    IEnumerator moveBox(GameObject box, float x, float y)
    {
        yield return new WaitForSeconds(5f);

        if (lastPosy != 0 && lastPosx != 0)
        {
            if (lastPosx == x && lastPosy == y)
            {
                Destroy(box);
            }
            else
            {
                lastPosy = x;
                lastPosy = y;
            }
        }
    }

}
