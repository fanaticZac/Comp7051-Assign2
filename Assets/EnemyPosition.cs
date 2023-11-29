using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Player Position: (" + GameController.gCtrl.GetEnemyPosition()[0] + ", " + GameController.gCtrl.GetEnemyPosition()[1] + ", " + GameController.gCtrl.GetEnemyPosition()[2] + ")";
    }
}
