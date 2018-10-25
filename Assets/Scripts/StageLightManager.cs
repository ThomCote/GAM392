﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLightManager : MonoBehaviour {

    static StageLightManager instance;

    [SerializeField] float flashTime = 0.1f;


    private int numLights = 4;
    private int LightIndex = 0;
    [SerializeField] SpriteRenderer[] StageLightsArray;
    [SerializeField] SpriteRenderer FarRightLight;
    [SerializeField] SpriteRenderer MiddleRightLight;
    [SerializeField] SpriteRenderer MiddleLeftLight;
    [SerializeField] SpriteRenderer FarLeftLight;

    private Color currColor;
    private int totalNumColors;
    private int colorIndex;
    [SerializeField] Color[] LightColorsArray;


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
        //Populate Light array in order
        PopulateLightArray();

        //Make Light Colors Clear at start
        InitializeLightsAsClear();

        //Set Color Alphas to Opaque
        SetColorArrayAlphas(1);

        //Set totalNumberOfColors
        totalNumColors = LightColorsArray.Length;

        //Set currentColor to first Color in array
        colorIndex = 0;
        currColor = LightColorsArray[colorIndex];
    }

    void SetColorArrayAlphas(int alphaNum)
    {
        for(int i = 0; i < LightColorsArray.Length; i++)
        {
            LightColorsArray[i].a = alphaNum;
        }
    }

    void InitializeLightsAsClear()
    {
        for (int i = 0; i < numLights; i++)
        {
            StageLightsArray[i].color = Color.clear;
        }
    }

    public static void CycleStageLights()
    {
        instance.CycleStageLights_Private();
    }

    void CycleStageLights_Private()
    {
        if(LightIndex <= 3)
        {
            StartCoroutine(FlashSprite(StageLightsArray[LightIndex], currColor, flashTime));
            LightIndex++;
        }
        else
        {
            for (int i = 0; i < numLights; i++)
            {
                StartCoroutine(FlashSprite(StageLightsArray[i], currColor, flashTime));
            }
            //StartCoroutine(FlashSprite(StageLightsArray[0], currColor, flashTime));
            //StartCoroutine(FlashSprite(StageLightsArray[1], currColor, flashTime));
            //StartCoroutine(FlashSprite(StageLightsArray[2], currColor, flashTime));
            //StartCoroutine(FlashSprite(StageLightsArray[3], currColor, flashTime));

            LightIndex = 0;
            ChooseNextColor();
        }
       
    }

    void ChooseNextColor()
    {
        if (colorIndex < LightColorsArray.Length - 1)
        {
            colorIndex++;
            SetCurrentColor(LightColorsArray[colorIndex]);
        }
        else
        {
            colorIndex = 0;
            SetCurrentColor(LightColorsArray[colorIndex]);
        }
    }

    void PopulateLightArray()
    {
        StageLightsArray = new SpriteRenderer[numLights];
        StageLightsArray[0] = FarRightLight;
        StageLightsArray[1] = MiddleRightLight;
        StageLightsArray[2] = MiddleLeftLight;
        StageLightsArray[3] = FarLeftLight;
    }

    void SetCurrentColor(Color col)
    {
        currColor = col;
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
