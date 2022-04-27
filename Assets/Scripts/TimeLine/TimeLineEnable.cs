using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineEnable : MonoBehaviour
{
    public GameObject timelineController;

    private void Start()
    {
        timelineController.GetComponent<EndTimeline>().TimelineEnds();
    }
}
