using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{

    [SerializeField] private Image crosshairImage;
    [SerializeField] private TextMeshProUGUI hint;
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.4f);
    [SerializeField] private Color activeColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Vector3 defaultScale = Vector3.one;
    [SerializeField] private Vector3 activeScale = new Vector3(1.5f, 1.5f, 1f);

    public void Awake()
    {
        ClearUI();
    }

    public void Update()
    {
        
    }
}
