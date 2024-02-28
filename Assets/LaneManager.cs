using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour 
{
    public static float lanePos(int i)
    {
        return i * 3 - 6;
    }

    public float numLanes = 5;

    [SerializeField] GameObject laneObj;
    public List<NoteSpawner> Lanes = new List<NoteSpawner>();

    private void Start()
    {
        for(int i = 0; i < numLanes; i++)
        {
            GameObject temp = Instantiate(laneObj, new Vector3(lanePos(i),0,0),Quaternion.identity, transform);
            Lanes.Add(temp.GetComponent<NoteSpawner>());
        }
        sM.StartSong();
    }


    public SongManager sM;



}
