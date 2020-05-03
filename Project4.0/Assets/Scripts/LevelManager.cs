using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject data_container, character, camera;

    private Vector3 character_position, character_rotation;
    private SavedObject guy;

    public void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player");

        if (Serialization.SaveExists(Application.persistentDataPath + "/saves/savedgames/" +
            PlayerPrefs.GetString("saved_game_slot") + "/game.dat"))
        {
            data_container.GetComponent<DataContainer>().game =
                Serialization.Load<Game>(Application.persistentDataPath + "/saves/savedgames/" +
                PlayerPrefs.GetString("saved_game_slot") + "/game.dat");
        }
        else
        {
            data_container.GetComponent<DataContainer>().game = new Game();
        }

        if (Serialization.SaveExists(Application.persistentDataPath + "/saves/savedgames/" +
            PlayerPrefs.GetString("saved_game_slot") + "/" + SceneManager.GetActiveScene().name + "/scene.dat"))
        {
            data_container.GetComponent<DataContainer>().scene =
                Serialization.Load<Scene>(Application.persistentDataPath + "/saves/savedgames/" +
                PlayerPrefs.GetString("saved_game_slot") + "/" + SceneManager.GetActiveScene().name + "/scene.dat");
        }
        else
        {
            data_container.GetComponent<DataContainer>().character = new SavedObject();
        }
        
        if (Serialization.SaveExists(Application.persistentDataPath + "/saves/savedgames/" +
            PlayerPrefs.GetString("saved_game_slot") + "/character.dat"))
        {
            data_container.GetComponent<DataContainer>().character =
                Serialization.Load<SavedObject>(Application.persistentDataPath + "/saves/savedgames/" +
                PlayerPrefs.GetString("saved_game_slot") + "/character.dat");
        }
        else
        {
            data_container.GetComponent<DataContainer>().scene = new Scene();
        }
        

        data_container.GetComponent<DataContainer>().saved_game_scene = SceneManager.GetActiveScene().name;
        data_container.GetComponent<DataContainer>().saved_game_slot = PlayerPrefs.GetString("saved_game_slot");
        data_container.GetComponent<DataContainer>().game.current_scene_name = SceneManager.GetActiveScene().name;
        data_container.GetComponent<DataContainer>().game.saveslot_label = PlayerPrefs.GetString("saved_game_slot");
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        guy = data_container.GetComponent<DataContainer>().character;

        if (data_container.GetComponent<DataContainer>().character.rotation_x != null
            && Serialization.SaveExists(Application.persistentDataPath + "/saves/savedgames/"
            + PlayerPrefs.GetString("saved_game_slot") + "/character.dat")) // It will be necessary to alter the position and rotation of the character when entering a new scene
        {
            character.GetComponent<CharacterController>().enabled = false;

            character.transform.SetPositionAndRotation(
                new Vector3(guy.position_x,guy.position_y,guy.position_z), 
                Quaternion.Euler(guy.rotation_x,guy.rotation_y,guy.rotation_z));

            

            character.GetComponent<CharacterController>().enabled = true;

            camera.GetComponent<PlayerLooking>().enabled = false;

            camera.transform.rotation = Quaternion.Euler(guy.rotation_x, guy.rotation_y, guy.rotation_z);

            camera.GetComponent<PlayerLooking>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        character_position = character.transform.position;
        character_rotation = camera.transform.eulerAngles;

        guy.position_x = character_position.x;
        guy.position_y = character_position.y;
        guy.position_z = character_position.z;

        guy.rotation_x = character_rotation.x;
        guy.rotation_y = character_rotation.y;
        guy.rotation_z = character_rotation.z;
    }
}