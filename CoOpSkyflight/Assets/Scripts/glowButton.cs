using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowButton : PushButton
{
    protected Material buttonMaterial;

    protected override void Awake()
    {
        base.Awake();
        buttonMaterial = GetComponent<Renderer>().material;

        if (pressed)
            buttonMaterial.EnableKeyword("_EMISSION");
        else
            buttonMaterial.DisableKeyword("_EMISSION");
        DynamicGI.UpdateEnvironment();
    }

    protected override void ChangePressedState()
    {
        base.ChangePressedState();

        if (pressed)
            buttonMaterial.EnableKeyword("_EMISSION");
        else
            buttonMaterial.DisableKeyword("_EMISSION");

        DynamicGI.UpdateEnvironment();
    }
}
