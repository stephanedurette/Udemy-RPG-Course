using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayerTriggers : MonoBehaviour
{
    [SerializeField] private Player player;
    
    //called by the animator
    public void SetPlayerEndTrigger()
    {
        player.SetCurrentStateEndTrigger();
    }
}
