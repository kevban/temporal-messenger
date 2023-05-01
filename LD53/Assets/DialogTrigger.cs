using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog[] dialogs;
    public bool transition;

    public void TriggerDialog() { 
        FindObjectOfType<DialogManager>().StartDialog(dialogs);
    }
}
