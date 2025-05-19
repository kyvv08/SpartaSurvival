using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.GetComponent<Interaction>().promptText = this.GetComponent<TextMeshProUGUI>();
    }
}
