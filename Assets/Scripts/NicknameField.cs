using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameField : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    
    public void CheckInputField()
    {
        string nickname = inputField.text;
        GameManager.Instance.LogPlayerStats(nickname);
        gameObject.SetActive(false);
    }
}
