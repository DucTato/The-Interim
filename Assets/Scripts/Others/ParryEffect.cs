using UnityEngine;

public class ParryEffect : MonoBehaviour
{
    private void OnEffectEnd()
    {
        gameObject.SetActive(false);
    }
}
