using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPing : MonoBehaviour {

    [DebugGUIGraph(min: 1, max: 300, r: 1, g: 1, b: 0, autoScale: true)]
    float latency;

    //private List<int> _pingTime = new List<int>();

    void Start()
    {
        StartCoroutine(PingServer1Update());
    }

    IEnumerator PingServer1Update()
    {
        RestartLoop:
        Ping ping = new Ping("31.13.92.36");

        yield return new WaitForSeconds(1f);
        while (!ping.isDone) yield return null;

        print(ping.time);

        latency = ping.time;

        //_pingTime.Add(ping.time);

        goto RestartLoop;
    }

}
