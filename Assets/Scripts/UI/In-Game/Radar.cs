﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class RadarObject
{
    public Image icon {get; set; }
    public GameObject owner {get; set; }
}

public class Radar : NetworkBehaviour
{
    public Transform playerPos;
    float mapScale = 1f;

    public static List<RadarObject> radObjects = new List<RadarObject>();

    public static void RegisterRadarObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObjects.Add(new RadarObject() {owner = o, icon = image});
    }

    public static void RemoveRadarObject(GameObject o)
    {
        List<RadarObject> newList = new List<RadarObject>();
        for (int i = 0; i < radObjects.Count; i++)
        {
            if (radObjects[i].owner == o)
            {
                Destroy(radObjects[i].icon);
                continue;
            }
            else
                newList.Add(radObjects[i]);
        }

        radObjects.RemoveRange(0, radObjects.Count);
        radObjects.AddRange(newList);
    }

    void DrawRadarDots()
    {
        if (playerPos != null)
        {

            foreach (RadarObject ro in radObjects)
            {
                Vector3 radarPos = (ro.owner.transform.position - playerPos.position);
                float distToObject = Vector3.Distance(playerPos.position,ro.owner.transform.position) * mapScale;
                if (distToObject > 70f)
                {
                    distToObject = 70f;
                }
                float deltay = Mathf.Atan2(radarPos.x,radarPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
                radarPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
                radarPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

                ro.icon.transform.SetParent(this.transform);
                ro.icon.transform.position = new Vector3(radarPos.x,radarPos.z,0) + this.transform.position;
            }
        }
    }

    void Update() 
    {
        DrawRadarDots();
    }
}
