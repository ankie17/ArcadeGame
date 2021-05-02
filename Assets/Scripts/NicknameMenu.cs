using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameMenu : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    public GameObject NickNameMenu;
    private void Start()
    {
        NickNameMenu.SetActive(false);
    }
    public void CheckInputField()
    {
        string nickname = inputField.text;
        GameManager.Instance.LogPlayerStats(nickname);
        gameObject.SetActive(false);
    }
}
