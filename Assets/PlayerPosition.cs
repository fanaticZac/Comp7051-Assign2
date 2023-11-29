using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Player Position: (" + GameController.gCtrl.GetPlayerPosition()[0] + ", " + GameController.gCtrl.GetPlayerPosition()[1] + ", " + GameController.gCtrl.GetPlayerPosition()[2] + ")";
    }
}
