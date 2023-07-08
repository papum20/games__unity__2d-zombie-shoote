using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsSave
{

    public int GrenadeThrowDistance;
    //RESOLUTION
    public int resWidth;
    public int resHeight;
    public int sensibility;
    public float touchControls;



    public OptionsSave(int grenade = 0, int rw = 0, int rh = 0, int sens = -1, float _touch = -1f)
    {
        if (grenade > 0) GrenadeThrowDistance = grenade;
        if (rw != 0) resWidth = rw;
        if (rh != 0) resHeight = rh;
        if (sens >= 0) sensibility = sens;
        if (_touch >= 0) touchControls = _touch;
    }

}
