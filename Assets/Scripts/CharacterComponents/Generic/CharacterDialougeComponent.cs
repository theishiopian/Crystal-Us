using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDialougeComponent : MonoBehaviour, ICharacterComponent
{
    public string[] Lines;

    private TextMeshPro speechBubble;

    public void Start()
    {
        speechBubble = this.gameObject.GetComponentInChildren<TextMeshPro>();
        speechBubble.SetText("");
    }

    public void PrintDialouge()
    {

    }
}
