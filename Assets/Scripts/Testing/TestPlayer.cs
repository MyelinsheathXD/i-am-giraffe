using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float vIn = Input.GetAxisRaw("Vertical");
        float hIn = Input.GetAxisRaw("Horizontal");

        this.transform.position += new Vector3(hIn, 0, vIn) * 3f * Time.deltaTime;
    }
}
