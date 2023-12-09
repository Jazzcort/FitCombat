using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameResultText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 75, 0);
    public float timeToFade = 1f;
    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;


    private float timeElapsed = 0f;
    private Color startColor;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        if (timeElapsed < timeToFade)
        {
            float fadeAlpha = startColor.a * (timeElapsed / timeToFade);
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
            textTransform.position += moveSpeed * Time.deltaTime;
            timeElapsed += Time.deltaTime;
        }
        

    }
}
