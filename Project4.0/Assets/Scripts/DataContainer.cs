using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataContainer : MonoBehaviour
{
    public GameObject this_thing;

    //  When a game is loaded, these shall comprise the saved file location.
    //  "savedgames/saved_game_slot/saved_game_scene/..."
    public string saved_game_slot, saved_game_scene;

    public Game game;
    public SavedObject character;
    public Scene scene;

    private GameObject[] alla_deeze;

    // Start is called before the first frame update
    void Start()
    {
        this_thing = GameObject.FindGameObjectWithTag("DataContainer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
