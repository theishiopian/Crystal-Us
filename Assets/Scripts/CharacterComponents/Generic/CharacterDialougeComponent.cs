using TMPro;
using UnityEngine;

public class CharacterDialougeComponent : MonoBehaviour, ICharacterComponent
{
    public string[] lines;

    private TextMeshPro speechBubble;
    private int index = 0;
    private bool isTalking = false;
    private GameObject player;

    private void Start()
    {
        Object cache;

        GlobalVariables.globalObjects.TryGetValue("player", out cache);

        if(cache != null)
        {
            player = (GameObject)cache;
        }
        speechBubble = this.gameObject.GetComponentInChildren<TextMeshPro>();
        speechBubble.SetText("");
    }

    private void Update()
    {
        if (isTalking && Vector3.Distance(this.transform.position, player.transform.position) > 2)
        {
            isTalking = false;
            speechBubble.SetText("");
        }
    }

    public void PrintDialouge()
    {
        isTalking = true;

        speechBubble.SetText(lines[index]);
        index++;

        if(index > lines.Length-1)
        {
            index = 0;
        }
    }
}
