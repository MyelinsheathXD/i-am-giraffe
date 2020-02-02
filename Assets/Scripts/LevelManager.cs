using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Things to track
    Time
    Num Broken (Different values per item broken)
    Score
    Hats Per Minute
    Main Item
    Secondary Items
*/

public class LevelManager : MonoBehaviour
{
    private static LevelManager s_instance = null;
    public static LevelManager Instance
    {
        get { return s_instance; }
    }

    [SerializeField]
    private Item[] mainItems = null;
    public Item[] MainItems
    {
        get { return mainItems; }
    }

    public string nextLevel = "";

    private Item.Type[] secondaryItems = null;

    private float startTime = 0;
    private float endTime = 0;

    private float hatsPerMinute = 0;

    private void Awake()
    {
        s_instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartLevel()
    {
        startTime = Time.time;
    }

    public void EndLevel()
    {
        endTime = Time.time;
    }

    private void Update()
    {
        // Update hats per minute
        float totalTime = Time.time - startTime;
        float weight = Time.deltaTime / (totalTime + Time.deltaTime);
        hatsPerMinute = ((float)Giraffe.Instance.numHats * weight) + (hatsPerMinute * (1 - weight));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().path);
        }
    }
}
