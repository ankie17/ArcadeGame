using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameScipr : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    
    public void CheckInputField()
    {
        string nickname = inputField.text;
        Debug.Log($"Player name is {nickname}");
        GameManager.Instance.LogPlayerStats(nickname);

        gameObject.SetActive(false);
    }
}
