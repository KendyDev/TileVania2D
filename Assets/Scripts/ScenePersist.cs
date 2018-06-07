using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour {

    private int SceneBuildIndex;

    private void Awake()
    {
        int numOfScenePersist = FindObjectsOfType<ScenePersist>().Length;

        if (numOfScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        SceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
	}
	
	// Update is called once per frame
	void Update () {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex != SceneBuildIndex)
        {
            Destroy(gameObject);
        }
	}
}
