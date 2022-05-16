using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Just attach this script to a UI button in a worldspace canvas.
//And it will make it compatiable with my VR laser pointer straight away.
public class ButtonInput : LaserInput
{

    private Image buttonImg;
    private Color originalColor;

    private Button button;

    private void Awake()
    {
        //Get button and image components so the three functions can interact with it.
        button = GetComponent<Button>();

        buttonImg = GetComponent<Image>();
        originalColor = buttonImg.color;

        //Automatically generate us a box collider for the button, and fit it to the correct size.
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = GetComponent<RectTransform>().rect.size;
        collider.size += new Vector3(0.0f, 0.0f, 1.0f);

    }

    public override void onLaserSelected(CharacterScript character)
    {
        buttonImg.color = originalColor * (button.colors.highlightedColor * button.colors.colorMultiplier);
    }

    public override void onLaserClick(CharacterScript character, bool clickState)
    {
        if (clickState)
        {
            buttonImg.color = originalColor * (button.colors.pressedColor * button.colors.colorMultiplier);
        }
        else
        {
            onLaserSelected(character);
        }
    }

    public override void onLaserDeselected(CharacterScript character)
    {
        buttonImg.color = originalColor;
    }

}
