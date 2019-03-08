using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TempTextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI player1;

    [SerializeField]
    private TextMeshProUGUI player2;

    [SerializeField]
    private TextMeshProUGUI player3;

    [SerializeField]
    private TextMeshProUGUI player4;

    private void Start()
    {
        var playerOne = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == 1);
        var playerTwo = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == 2);
        var playerThree = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == 3);
        var playerFour = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == 4);

        if (playerOne != null)
        {
            player1.text = "Controller 1: " + playerOne.Material.name.Split('(')[0];
        }
        if (playerTwo != null)
        {
            player2.text = "Controller 2: " + playerTwo.Material.name.Split('(')[0];
        }
        if (playerThree != null)
        {
            player3.text = "Controller 3: " + playerThree.Material.name.Split('(')[0];
        }
        if (playerFour != null)
        {
            player4.text = "Controller 4: " + playerFour.Material.name.Split('(')[0];
        }
    }
}
