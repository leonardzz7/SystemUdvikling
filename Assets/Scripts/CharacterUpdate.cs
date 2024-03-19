using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUpdate : MonoBehaviour
{
    public int hair=0;
    public int eye=0;
    public int shoe=0;
    public int top=0;
    public int botom=0;

    public RuntimeAnimatorController controller;

    public Material mat;

    GameObject tempeye = null;
    GameObject temphair = null;
    GameObject tempshoe = null;
    GameObject temptop = null;
    GameObject tempbotom = null;

    private void Start()
    {
        //Updateit(0,0,0,0,0);  
    }

    public void Updateit(int hair , int shoe, int eye, int top, int botom)
    {
        this.hair = hair;
        this.eye = eye;
        this.shoe = shoe;
        this.top = top;
        this.botom = botom;
        instantiateParts();
    }

    public void  instantiateParts()
    {
        if(tempeye!= null) { Destroy(tempeye); }
        tempeye = Instantiate(CharacterDataLoad.instence.eyes[eye], transform);
        addAnimator(tempeye);
        if (temphair != null) { Destroy(temphair); }
        temphair = Instantiate(CharacterDataLoad.instence.hairs[hair], transform);
        addAnimator(temphair);
        if (tempshoe != null) { Destroy(tempshoe); }
        tempshoe = Instantiate(CharacterDataLoad.instence.shoes[shoe], transform);
        addAnimator(tempshoe);
        if (temptop != null) { Destroy(temptop); }
        temptop = Instantiate(CharacterDataLoad.instence.tops[top], transform);
        addAnimator(temptop);
        if (tempbotom != null) { Destroy(tempbotom); }
        tempbotom = Instantiate(CharacterDataLoad.instence.bottoms[botom], transform);
        addAnimator(tempbotom);
    }
    public void addAnimator(GameObject obj)
    {
        var anim = obj.AddComponent<Animator>();
        anim.runtimeAnimatorController = controller;

    }
}
