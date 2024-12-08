using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class StatusPopUpGenerator : MonoBehaviour
{
    public static StatusPopUpGenerator instance;
    public GameObject damagePopUpPrefab;

    // This generator follows the singleton pattern
    private void Awake()
    {
        instance = this;
    }

    public void CreatePopUp(Vector3 pos, string text, Color color)
    {
        var popUp = Instantiate(damagePopUpPrefab, pos, Quaternion.identity);
        var t = popUp.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        t.text = text;
        t.faceColor = color;

        // Destroy the pop up after 1 second
        Destroy(popUp, 1f);
    }
}
