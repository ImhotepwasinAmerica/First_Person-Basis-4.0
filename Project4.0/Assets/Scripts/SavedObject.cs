using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SavedObject
{
    public string saved_thing;

    public string saved_thing_virtual;

    public string ai_stage;

    public float rotation_x, rotation_y, rotation_z;

    public float position_x, position_y, position_z;

    public int health, max_health, stack_count;

    public float weight;

    public bool invincible, switch_on, weapon_ammo_count;

    public ArrayList inventory;

    public SavedObject[] toolbelt;
}
