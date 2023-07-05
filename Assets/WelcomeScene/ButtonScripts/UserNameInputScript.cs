using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserNameInputScript : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TMP_InputField CodeInput;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NameColorWhite(){
        NameInput.image.color = Color.white;
        TextMeshProUGUI placeholder = (TextMeshProUGUI)NameInput.placeholder;
        placeholder.text = "Choose Your Name...";
    }

    public void CodeColorWhite(){
        CodeInput.image.color = Color.white;
    }

    public bool CheckName() {
        if(NameInput.text == "" || NameInput.text.Contains(" ")) {
            NameInput.image.color = new Color(1f, 0f, 0f, 0.4f);
            TextMeshProUGUI placeholder = (TextMeshProUGUI)NameInput.placeholder;
            placeholder.text = "Please Choose Name...";
            return false;
        }
        return true;
    }

    public bool CheckCode() {
        if(CodeInput.text == "" || CodeInput.text.Length != 6) {
            CodeInput.image.color = new Color(1f, 0f, 0f, 0.4f);
            return false;
        }
        return true;
    }

}
