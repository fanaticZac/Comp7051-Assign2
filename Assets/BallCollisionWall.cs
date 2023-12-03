using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BallCollisionWall : MonoBehaviour
{
    int player1Points = 0;
    int player2Points = 0;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject EdgeLeft = GameObject.Find("EdgeLeft");
        GameObject EdgeRight = GameObject.Find("EdgeRight");
        GameObject Player1Score = GameObject.Find("Player1Score");
        GameObject Player2Score = GameObject.Find("Player2Score");
        GameObject Ball = GameObject.Find("Ball");
        GameObject Ready = GameObject.Find("ReadyPrompt");

        
        
        if (collision.gameObject.name == "Ball")
        {
            //if I am left wall, increase p1 score, else...
            if (gameObject.name == EdgeLeft.name){
                Ball.GetComponent<BallBehavior>().StopBall();
                Debug.Log("Left Edge hit");
                Debug.Log(Player1Score.GetComponent<TextMeshProUGUI>().text);
                player1Points++;
                Player1Score.GetComponent<TextMeshProUGUI>().text = player1Points.ToString();
                if (player1Points > 9){
                    // player 1 wins
                    Ready.GetComponent<TextMeshProUGUI>().text = "P1 Wins!";
                    player1Points = 0;
                    Player1Score.GetComponent<TextMeshProUGUI>().text = player1Points.ToString();
                    player2Points = 0;
                    Player2Score.GetComponent<TextMeshProUGUI>().text = player2Points.ToString();

                    // Reset game
                } else {
                    Ready.GetComponent<TextMeshProUGUI>().text = "Ready?";
                }
            }
            if (gameObject.name == EdgeRight.name){
                Ball.GetComponent<BallBehavior>().StopBall();
                Debug.Log("Right Edge hit");
                Debug.Log(Player2Score.GetComponent<TextMeshProUGUI>().text);
                player2Points++;
                Player2Score.GetComponent<TextMeshProUGUI>().text = player2Points.ToString();

                if (player2Points > 9){
                    // player 1 wins
                    Ready.GetComponent<TextMeshProUGUI>().text = "P2 Wins!";
                    player1Points = 0;
                    Player1Score.GetComponent<TextMeshProUGUI>().text = player1Points.ToString();
                    player2Points = 0;
                    Player2Score.GetComponent<TextMeshProUGUI>().text = player2Points.ToString();

                    // Reset game
                } else {
                    Ready.GetComponent<TextMeshProUGUI>().text = "Ready?";
                }
            }
        }
    }
}
