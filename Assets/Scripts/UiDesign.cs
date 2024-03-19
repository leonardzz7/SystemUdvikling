using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDesign : MonoBehaviour
{
    public Sprite[] Backgrounds;
    public Image BackGroundImage;

    public Sprite[] AvatarBackGround;
    public Image[] AvatarBackGroundImage;


    private void Start()
    {
        BackGroundImage.sprite = Backgrounds[persistantmanager.instence.backGround];
        AvatarBackGroundImage[0].sprite = AvatarBackGround[0];
        AvatarBackGroundImage[1].sprite = AvatarBackGround[1];
        AvatarBackGroundImage[2].sprite = AvatarBackGround[2];
        AvatarBackGroundImage[3].sprite = AvatarBackGround[3];
        
    }
    public void ChangeBackGround( int x)
    {
        BackGroundImage.sprite = Backgrounds[x];
    }
    public void ChangeAvatarBackGound(int x )
    {
        AvatarBackGroundImage[0].sprite = AvatarBackGround[x];
        AvatarBackGroundImage[1].sprite = AvatarBackGround[x];
        AvatarBackGroundImage[2].sprite = AvatarBackGround[x];
        AvatarBackGroundImage[3].sprite = AvatarBackGround[x];
    }
}
