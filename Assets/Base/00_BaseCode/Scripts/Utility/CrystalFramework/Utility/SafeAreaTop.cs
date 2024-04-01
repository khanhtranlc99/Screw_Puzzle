using UnityEngine;

//Chỉ căn tai thỏ ở phía top
public class SafeAreaTop : MonoBehaviour
{
    private RectTransform _rectTransform;
    private RectTransform Panel
    {
        get { return _rectTransform ? _rectTransform : _rectTransform = GetComponent<RectTransform>(); }
    }

    void Awake()
    {
        var heightPanel = Panel.rect.size.y;
        var offsetNotch = ((Screen.height - Screen.safeArea.yMax) / Screen.height) * heightPanel;
        Panel.offsetMax = new Vector2(Panel.offsetMax.x, -offsetNotch);
    }
}