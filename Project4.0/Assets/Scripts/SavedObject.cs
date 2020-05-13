using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SavedObject
{
    public string saved_thing;

    public float rotation_x, rotation_y, rotation_z;

    public float position_x, position_y, position_z;

    public List<int> ints;

    public List<float> floats;

    public List<string> strings;

    public List<SavedObject> objects;
}
