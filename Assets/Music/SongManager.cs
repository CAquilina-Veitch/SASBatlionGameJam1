using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public void StartSong()
    {

    }
    public List<Song> SongList;




}
[Serializable]
public struct Song
{
    public float BPM;

    public List<NotePattern> notes;


}
[Serializable]
public struct NotePattern
{
    public bool[] b;
}


