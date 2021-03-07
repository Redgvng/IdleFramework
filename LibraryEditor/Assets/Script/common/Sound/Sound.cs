using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static Main;

public class Sound  {

    public AudioSource BGMSource;

    public AudioClip positiveClip,negativeClip, craftClip1, craftClip2, saleClip, undoClip
        , click1Clip, click3Clip,upClip,endClip,levelUpClip,jemDropClip,rareEnemyClip;

    public void PlaySound(AudioClip Clip)
    {
        //playClip(Clip);
        main.SoundEffectSource.PlayOneShot(Clip);
    }

    /// <summary>
    /// Vol : 0.0f ~ 1.0f
    /// </summary>
    public void ChangeSEVolume(float Vol)
    {
        main.SoundEffectSource.volume = Vol;
    }

    /// <summary>
    /// Vol : 0.0f = 1.0f
    /// </summary>
    public void ChangeBGMVolume(float Vol)
    {
        BGMSource.volume = Vol;
    }

}
