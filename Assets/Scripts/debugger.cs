using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class debugger : MonoBehaviour
{
    public TextMeshProUGUI txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            GameOptions.Setup();
        }
        txt.text = "u: " + PlayerPrefs.GetString("u") + "" +
            "\n > " + TextEncoder.textDecode(PlayerPrefs.GetString("u"))
            + "\n" +
            "inv: " + PlayerPrefs.GetString("i") + "" +
            "\n > " + TextEncoder.textDecode(PlayerPrefs.GetString("i"));
    }
}
