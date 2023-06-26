using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserNameInputScript : MonoBehaviour
{
    public TMP_InputField NameInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColorWhite(){
        NameInput.image.color = Color.white;
        TextMeshProUGUI placeholder = (TextMeshProUGUI)NameInput.placeholder;
        placeholder.text = "Choose Your Name...";
    }

}
