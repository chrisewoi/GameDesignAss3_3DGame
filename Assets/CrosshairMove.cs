using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairMove : MonoBehaviour
{
    public Camera mc;
    public Canvas canvas;
    public Vector2 canvasSize, screenSize;
    public Vector2 scale;
    public Vector2 localPoint;

    public Vector3 offset;

    private Image image;
    private Color baseColor;
    public Color powerColor;
    public ClickToShoot powerupStats;
    private Sprite baseSprite;
    public Sprite powerSprite;

    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasSize = canvas.GetComponent<RectTransform>().rect.size;
        screenSize = canvas.renderingDisplaySize;
        scale = new(screenSize.x / canvasSize.x, screenSize.y / canvasSize.y);
        offset = new Vector3(scale.x, scale.y, 0);
        image = GetComponent<Image>();
        baseColor = image.color;
        powerupStats = FindObjectOfType<ClickToShoot>();
        baseSprite = image.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (powerupStats.powerupActive > 0)
        {
            image.color = powerColor;
            image.sprite = powerSprite;
        }
        else
        {
            image.color = baseColor;
            image.sprite = baseSprite;
        }
        Vector2 screenPos = Input.mousePosition;
        (transform as RectTransform).anchoredPosition = new Vector2(screenPos.x / scale.x, screenPos.y / scale.y);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition,
        //    canvas.worldCamera, out localPoint);
        //transform.position = canvas.transform.TransformPoint(localPoint);
        //transform.localPosition = localPoint;
        //transform.localPosition = mc.ScreenToViewportPoint(Input.mousePosition);
    }
}
