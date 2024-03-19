using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerImage : MonoBehaviour
{
    public int partType;
    public int partCount; 
    public void SelectPicture()
    {
        PlayerPrefs.SetInt("picId" + partType, partCount);
        Debug.Log(PlayerPrefs.GetInt("picId" + partType, partCount)) ;
        startController.instance.SelectPicture();
    }
    public void instantiateit(int count  , int type )
    {
        partType = type;
        partCount = count; 
        switch (type)
        {
            case 0:
                var obj = Instantiate(CharacterDataLoad.instence.hairs[count], transform.GetChild(0));
                transform.GetChild(0).transform.LeanScale(new Vector3(250,250,250) , 0.01f);
                
                break;
            case 1:
                var obj1 = Instantiate(CharacterDataLoad.instence.shoes[count], transform.GetChild(0));
                transform.GetChild(0).transform.LeanScale(new Vector3(600, 600, 600), 0.01f);
                break;
            case 2:
                var obj2= Instantiate(CharacterDataLoad.instence.eyes[count], transform.GetChild(0));
                transform.GetChild(0).transform.LeanScale(new Vector3(400, 400, 400), 0.01f);
                break;
            case 3:
                var obj3 = Instantiate(CharacterDataLoad.instence.tops[count], transform.GetChild(0));
                transform.GetChild(0).transform.LeanScale(new Vector3(400, 400, 400), 0.01f);
                break;
            case 4:
                var obj4  = Instantiate(CharacterDataLoad.instence.bottoms[count], transform.GetChild(0));
                transform.GetChild(0).transform.LeanScale(new Vector3(400, 400, 400), 0.01f);
                break;
        }
    }
}
