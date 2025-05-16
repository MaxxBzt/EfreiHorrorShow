using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private float _DELAY = 0.05f;
    [SerializeField] private float _POPSCALE = 1.2f;
    [SerializeField] private float popUpDuration = 0.07f;
    [SerializeField] private float popDownDuration = 0.15f;

    public AnimationCurve popUpCurve = AnimationCurve.EaseInOut(0, 1f, 1, 1.2f);
    public AnimationCurve popDownCurve = AnimationCurve.EaseInOut(0, 1.2f, 1, 1f);

    private string fullText;

    private void Start()
    {
        fullText = tmpText.text + " ";
        tmpText.text = "";
        tmpText.ForceMeshUpdate();
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        tmpText.ForceMeshUpdate();

        for (int i = 0; i < fullText.Length; i++)
        {
            tmpText.text += fullText[i];
            tmpText.ForceMeshUpdate();

            if (fullText[i] != ' ')
                yield return StartCoroutine(PopCharacter(i));
            else
                yield return new WaitForSeconds(_DELAY);
        }
    }

    IEnumerator PopCharacter(int charIndex)
    {
        yield return new WaitForEndOfFrame();

        tmpText.ForceMeshUpdate();
        TMP_TextInfo textInfo = tmpText.textInfo;

        if (charIndex >= textInfo.characterCount || !textInfo.characterInfo[charIndex].isVisible)
            yield break;

        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

        Vector3[] originalVertices = new Vector3[4];
        for (int i = 0; i < 4; i++)
            originalVertices[i] = vertices[vertexIndex + i];

        Vector3 center = (originalVertices[0] + originalVertices[2]) / 2f;


        float elapsed = 0f;
        while (elapsed < popUpDuration)
        {
            float t = elapsed / popUpDuration;
            float scale = popUpCurve.Evaluate(t);

            vertices = textInfo.meshInfo[materialIndex].vertices;
            for (int i = 0; i < 4; i++)
                vertices[vertexIndex + i] = center + (originalVertices[i] - center) * scale;

            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < popDownDuration)
        {
            float t = elapsed / popDownDuration;
            float scale = popDownCurve.Evaluate(t);

            vertices = textInfo.meshInfo[materialIndex].vertices;
            for (int i = 0; i < 4; i++)
                vertices[vertexIndex + i] = center + (originalVertices[i] - center) * scale;

            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Restore original vertices to be sure
        vertices = textInfo.meshInfo[materialIndex].vertices;
        for (int i = 0; i < 4; i++)
            vertices[vertexIndex + i] = originalVertices[i];

        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
