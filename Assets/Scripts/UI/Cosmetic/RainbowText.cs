using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Thanks to Cardboardpi for this text
public class RainbowText : MonoBehaviour
{
    [SerializeField] Gradient gradientText;
    [SerializeField] float gradiantSpeed = .1f;
    private TMP_Text m_TextComponent;
    private float totalTIme;
    void Awake(){
        m_TextComponent = GetComponent<TMP_Text>();
    }

    void Start(){
        StartCoroutine(AnimateVertextColors());
    }

    IEnumerator AnimateVertextColors(){
        m_TextComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        int currentCharacter = 0;

        Color32[] newVertexColors;
        Color32 c0 = m_TextComponent.color;

        while(true){
            int characterCount = textInfo.characterCount;

            if (characterCount == 0){
                yield return new WaitForSeconds(0.25f);
                continue;
            }

            int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            int vertextIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

            if (textInfo.characterInfo[currentCharacter].isVisible){
                float offset = currentCharacter / (float)characterCount;
                c0 = gradientText.Evaluate((totalTIme + offset) % 1f);
                newVertexColors[vertextIndex + 0] = c0;
                newVertexColors[vertextIndex + 1] = c0;
                newVertexColors[vertextIndex + 2] = c0;
                newVertexColors[vertextIndex + 3] = c0;

                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            }
            currentCharacter = (currentCharacter + 1) % characterCount;
            yield return new WaitForSeconds(gradiantSpeed);
        }



    }
}
