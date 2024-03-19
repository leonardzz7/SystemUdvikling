using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyPolicy : MonoBehaviour
{
    public string PrivacyPolicyAddress;

    public void LaunchPrivacyPolicy()
    {
        Application.OpenURL(PrivacyPolicyAddress);
    }
}
