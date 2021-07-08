using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UILine is a UI component meant to function similiarly to how the default unity LineRenderer functions. Unfortunately making the LineRenderer component work in UI is a bit of a headache which this
// component aims to solve! This component is a plug and play component, which uses UnityEngine.UI to construct a line in UI out of Images with screen positions given in linePositions. Because these lines are 
// made out of gameobjects, they might turn useful in ways where non-gameobject lines would. For example: You can enable isRaycastTarget to get different mouse events! Maybe you want to make the line function  
// like a button?Or just add hover functionalities like displaying a cerain label?

public class UILine : MonoBehaviour
{
    public List<Vector2> LinePositions { get { return linePositions; } set { linePositions = value; } }
    public int LineWidth { get { return lineWidth; } set { lineWidth = value; } }


    [Header("Line Settings")]
    [SerializeField]
    private List<Vector2> linePositions = new List<Vector2>();
    [SerializeField, Min(1)]
    private int lineWidth;
    [Header("Line section image settings")]
    [SerializeField]
    private bool isRaycastTarget;
    [SerializeField]
    private Color lineColor = Color.white;
    [SerializeField]
    private Sprite lineSprite;

    private List<RectTransform> lineObjectRectTransforms = new List<RectTransform>();
    private List<RectTransform> lineImageObjectRectTranforms = new List<RectTransform>();

    private void Update()
    {
        UpdateLineObjectCount();
        if(linePositions.Count > 1)
        UpdateLinePositions();
    }

    private void UpdateLineObjectCount()
    {
        for(int i = lineObjectRectTransforms.Count; i > linePositions.Count - 1; i--)
        {
            if(linePositions.Count > 1)
            {
                GameObject lineObject = lineObjectRectTransforms[lineObjectRectTransforms.Count - 1].gameObject;
                lineObjectRectTransforms.RemoveAt(lineObjectRectTransforms.Count - 1);
                lineImageObjectRectTranforms.RemoveAt(lineImageObjectRectTranforms.Count -1);
                Destroy(lineObject);
            }
        }

        for(int i = lineObjectRectTransforms.Count; i < linePositions.Count - 1; i++)
        {
            // CreateLineObjects
            RectTransform lineObject = new GameObject("LineObject").AddComponent<RectTransform>();
            lineObject.SetParent(transform);
            lineObject.sizeDelta = new Vector2();
            RectTransform lineImageObject = new GameObject("LineImage").AddComponent<RectTransform>();
            lineImageObject.SetParent(lineObject.transform);
            lineImageObject.transform.localPosition = new Vector2();
            lineImageObject.sizeDelta = new Vector2(lineWidth, lineImageObject.sizeDelta.y);
            lineImageObject.anchorMin = new Vector2(0.5f, 0);
            lineImageObject.anchorMax = new Vector2(0.5f, 0);
            lineImageObject.pivot = new Vector2(0.5f, 0);

            Image image = lineImageObject.gameObject.AddComponent<Image>();
            image.raycastTarget = isRaycastTarget;
            image.color = lineColor;
            image.sprite = lineSprite;

            lineObjectRectTransforms.Add(lineObject);
            lineImageObjectRectTranforms.Add(lineImageObject);
        }
    }

    private void UpdateLinePositions()
    {
        Vector2 lineStartPos = linePositions[0];
        for(int i = 1; i < linePositions.Count; i++)
        {
            lineObjectRectTransforms[i - 1].position = lineStartPos;
            lineImageObjectRectTranforms[i - 1].sizeDelta = new Vector2(lineWidth, Vector2.Distance(lineStartPos, linePositions[i]));
            
            Vector3 euler = lineObjectRectTransforms[i - 1].eulerAngles;
            euler.z = Mathf.Atan2(-(linePositions[i].x - lineStartPos.x), (linePositions[i].y - lineStartPos.y))*Mathf.Rad2Deg;
            lineObjectRectTransforms[i - 1].eulerAngles = euler;

            lineStartPos = linePositions[i];
        }
    }



}
