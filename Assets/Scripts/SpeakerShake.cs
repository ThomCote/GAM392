using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerShake : MonoBehaviour {

    public float speed = 1.0f; //how fast it shakes
    public  float amount = 1.0f; //how much it shakes
    private bool isShaking = true;
    float xPos;

    void Start()
    {

      
    }

    void Update()
    {
        if (isShaking)
        {
            xPos = Mathf.Sin(Time.deltaTime * speed) * amount;
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }

    bool GetIsShaking()
    {
        return isShaking;
    }

    void SetIsShaking(bool result)
    {
        isShaking = result;
    }
}
