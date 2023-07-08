using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossTriggerAssigner : MonoBehaviour
{

    public BoxCollider2D eyeTriggerNormal;
    public BoxCollider2D eyeTriggerAttack;




    public void InvertColliders()
    {
        eyeTriggerNormal.enabled = !eyeTriggerNormal.enabled;
        eyeTriggerAttack.enabled = !eyeTriggerAttack.enabled;

    }


}
