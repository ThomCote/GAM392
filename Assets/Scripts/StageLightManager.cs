using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLightManager : MonoBehaviour {

    static StageLightManager instance;

    [SerializeField] SpriteRenderer FarRightLight;
    [SerializeField] SpriteRenderer MiddleRightLight;
    [SerializeField] SpriteRenderer MiddleLeftLight;
    [SerializeField] SpriteRenderer FarLeftLight;

    [SerializeField] Color FarRightColor;
    [SerializeField] Color MiddleRightColor;
    [SerializeField] Color MiddleLeftColor;
    [SerializeField] Color FarLeftColor;

    [SerializeField] float flashTime = 0.1f;

    // Use this for initialization
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance.Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        FarRightLight.color = Color.clear;
        MiddleRightLight.color = Color.clear;
        MiddleLeftLight.color = Color.clear;
        FarLeftLight.color = Color.clear;

        //SetColorAlphas to Opaque
        FarRightColor.a = 1;
        MiddleRightColor.a = 1;
        MiddleLeftColor.a = 1;
        FarLeftColor.a = 1;
    }

    public static void FarRightColorChange()
    {
        instance.FarRightColorChange_Private();
    }

    void FarRightColorChange_Private()
    {
       StartCoroutine(FlashSprite(FarRightLight, FarRightColor, flashTime));
    }

    public static void MiddleRightColorChange()
    {
        instance.MiddleRightColorChange_Private();
    }

    void MiddleRightColorChange_Private()
    {
        StartCoroutine(FlashSprite(MiddleRightLight, MiddleRightColor, flashTime));
    }

    public static void MiddleLeftColorChange()
    {
        instance.MiddleLeftColorChange_Private();
    }

    void MiddleLeftColorChange_Private()
    {
        StartCoroutine(FlashSprite(MiddleLeftLight, MiddleLeftColor, flashTime));
    }

    public static void FarLeftColorChange()
    {
        instance.FarLeftColorChange_Private();
    }

    void FarLeftColorChange_Private()
    {
        StartCoroutine(FlashSprite(FarLeftLight, FarLeftColor, flashTime));
    }

    IEnumerator FlashSprite(SpriteRenderer spr, Color col, float duration)
    {
        spr.color = col;

        yield return new WaitForSeconds(duration);

        spr.color = Color.clear;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
