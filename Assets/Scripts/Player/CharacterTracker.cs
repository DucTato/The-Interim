using UnityEngine;


public class CharacterTracker : MonoBehaviour 
{
    public string currentCharacterName, currentMap;
    public GameObject currentCharacter;
    public static CharacterTracker instance;
    public Sprite avatarInGame;
    public float Health, Mana, Stamina, MagResist, PhysResist;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetCurrentCharacter (string name, float HP, float MP, float SP, float magicRes, float physicRes, GameObject characterSkin, Sprite avatar)
    {
        currentCharacter = characterSkin;
        currentCharacterName = name;
        Health = HP;
        Mana = MP;
        Stamina = SP;
        MagResist = magicRes;
        PhysResist = physicRes;
        avatarInGame = avatar;
    }
    public void DestroyCurrentCharacter()
    {
        // Only needs to be used when loading from the current game to the Main menu
        Destroy(gameObject);    
    }
}
