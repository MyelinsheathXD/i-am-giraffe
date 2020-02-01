using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Item.Type[] mainItems = null;
    private Item.Type[] secondaryItems = null;

    private float startTime = 0;
    private float endTime = 0;

    private float hatsPerMinute = 0;

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
    }
}
