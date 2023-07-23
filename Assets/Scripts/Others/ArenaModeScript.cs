using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaModeScript : MonoBehaviour
{
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private int mapOption, currentClass, currentSkin;
    [SerializeField] private string[] mapSelections;
    [SerializeField] private Image characterPreview, characterAvatar;
   
    [SerializeField] private Text classText, skinText, statText;
    private string selectedMap;
    [SerializeField] private CharacterClass[] charClass;
    // Start is called before the first frame update
    void Start()
    {
        selectedMap = "Arena_Forest";
        currentClass = 0;
        currentSkin = 0;
        UpdateCurrentCharacterAvatar();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(selectedMap);
    }
    public void BackToPrevious()
    {
        MainMenuScript.instance.ToggleInteractables();
        gameObject.SetActive(false);
    }
    public void MapDropDown(int option)
    {
        selectedMap = mapSelections[option];    
    }
    public void StartButton()
    {
        SceneManager.LoadScene(selectedMap);
    }
    public void CharacterButton()
    {
        characterPanel.SetActive(true);
        UpdateCurrentCharacter();
    }
    public void BackToArenaPanel()
    {
        UpdateCurrentCharacterAvatar();
        characterPanel.SetActive(false);
    }
    public void LeftClassButton()
    {
        currentSkin = 0;
        if (currentClass > 0)
        {
            currentClass--;
        }
        else
        {
            currentClass = charClass.Length - 1;
        }
        UpdateCurrentCharacter();
    }
    public void RightClassButton()
    {
        currentSkin = 0;
        if (currentClass < charClass.Length - 1)
        {
            currentClass++;
        }
        else
        {
            currentClass = 0;
        }
        UpdateCurrentCharacter();
    }
    public void LeftSkinButton()
    {
        if (currentSkin > 0)
        {
            currentSkin--;
        }
        else
        {
            currentSkin = charClass[currentClass].currentCharacterSkin.Length - 1;
        }
        UpdateCurrentCharacter();
    }
    public void RightSkinButton()
    {
        if (currentSkin < charClass[currentClass].currentCharacterSkin.Length - 1)
        {
            currentSkin++;
        }
        else
        {
            currentSkin = 0;
        }
        UpdateCurrentCharacter();
    }
    private void UpdateCurrentCharacter()
    {
        classText.text = "Class\t\t" + "<color=#fff0db>" + charClass[currentClass].classType + "</color>";
        skinText.text = " Skin \t\t\t" + "<color=#fff0db>" + currentSkin + "</color>";
        statText.text = "<color=#ff6663>HP: </color>" + charClass[currentClass].startingHealth + "\t\t<color=#9ec1cf>MP: </color>" + charClass[currentClass].startingMana + "\t\t<color=#9ee09e>SP: </color>" + charClass[currentClass].startingStamina;
        characterPreview.sprite = charClass[currentClass].currentCharacterSkin[currentSkin].characterPreview;
    }
    private void UpdateCurrentCharacterAvatar()
    {
        characterAvatar.sprite = charClass[currentClass].currentCharacterSkin[currentSkin].avatar;
    }
}
[System.Serializable]
public class CharacterClass
{
    public string classType;
    public float startingHealth, startingMana, startingStamina, magResistance, physResistance;
    public CharacterSkin[] currentCharacterSkin;
}
