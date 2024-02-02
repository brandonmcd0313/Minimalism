using System.Collections;
using UnityEngine;

public class ExampleCivilian : Civilian, IInteractable
{
    public void Interact()
    {
        FadeColor();
    }

    public void ShowInteractionPrompt()
    {
        Debug.Log("Show interaction prompt");
    }

    public void HideInteractionPrompt()
    {
        Debug.Log("Hide interaction prompt");
    }


    protected override void FadeColor()
    {
        //can add stuff like an inital attack here...
        base.FadeColor(); // Call the base interaction logic, which changes the color to gray
        base.StealColor();
    }
}
