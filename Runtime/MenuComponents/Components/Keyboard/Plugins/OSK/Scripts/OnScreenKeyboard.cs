using MenuComponents.Components.Keyboard;
using MenuComponents.DynamicSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class OnScreenKeyboard : MonoBehaviour
{

    public TMP_InputField focus;
    public bool showNumeric = false;
    public Color32 textColor;
    public Color32 mainColor;
    public Color32 backgroundColor;
    public Sprite mainSprite;
    public Sprite specialSprite;
    public GameObject[] panels;
    public GameObject[] keys;
    public GameObject[] specialKeys;

    [FormerlySerializedAs("specialColorPallet")] public ColourDynamicData specialColorDynamicData;
    [FormerlySerializedAs("specialTextColorPallet")] public ColourDynamicData specialTextColorDynamicData;
    [FormerlySerializedAs("textColorPallet")] public ColourDynamicData textColorDynamicData;

    [HideInInspector]
    public bool isActive = false;
    [HideInInspector]
    public bool capsEnabled = false;

    public Vector3 parentLocation;

    private int currentBuildIdex
    {
        get
        {
            return 
                SceneManager
                .GetActiveScene()
                .buildIndex;
        }
    }

    private void Awake()
    {
        StaticManager.ReferenceKeyboard(this);
    }

    private void OnDisable()
    {
        StaticManager.ClearKeyboard();
    }

    void Start()
    {
        ShowNumeric(showNumeric);
        SetTextColor(textColorDynamicData.colour, specialTextColorDynamicData.colour);
        SetMainColor(mainColor);
        SetSpecialColor(specialColorDynamicData.colour);
        SetBackgroundColor(backgroundColor);
        SetMainSprite(mainSprite);
        SetSpecialSprite(specialSprite);
    }

    public void ShowNumeric(bool b)
    {
        panels[3].SetActive(b);
        showNumeric = b;
    }

    public void SetTextColor(Color c, Color sc)
    {
        foreach (GameObject go in keys)
        {
            go.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = c;
        }

        foreach (GameObject go in specialKeys)
        {
            go.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = sc;
        }
    }

    public void SetMainColor(Color32 c)
    {
        foreach (GameObject go in keys)
        {
            go.GetComponent<Image>().color = c;
        }
    }

    public void SetSpecialColor(Color32 c)
    {
        foreach (GameObject go in specialKeys)
        {
            go.GetComponent<Image>().color = c;
        }
    }

    public void SetBackgroundColor(Color32 c)
    {
        gameObject.GetComponent<Image>().color = c;
    }

    public void SetMainSprite(Sprite s)
    {
        foreach (GameObject go in keys)
        {
            go.GetComponent<Image>().sprite = s;
        }
    }

    public void SetSpecialSprite(Sprite s)
    {
        foreach (GameObject go in specialKeys)
        {
            go.GetComponent<Image>().sprite = s;
        }
    }

    public void SetFocus(TMP_InputField i)
    {
        focus = i;
    }

    public void SetActiveFocus(TMP_InputField i)
    {
        SetKeyboardType(0);
        focus = i;
        SetActive(true);
        focus.MoveTextEnd(true);
    }
    public void SetActiveFocusNumeric(TMP_InputField i)
    {
        SetKeyboardType(2);
        focus = i;
        SetActive(true);
        focus.MoveTextEnd(true);
    }

    public void SetActiveFocusNumericPin(TMP_InputField i)
    {
        SetKeyboardType(3);
        focus = i;
        SetActive(true);
        focus.MoveTextEnd(true);
    }

    public void WriteKey(TextMeshProUGUI t)
    {

        if (!focus) { return; }
        focus.text += t.text;
    }

    public void WriteSpecialKey(int n)
    {
        switch (n)
        {
            case 0:
                if (!focus) { return; }
                if (focus.text.Length > 0)
                {
                    focus.text = focus.text.Substring(0, focus.text.Length - 1);
                }
                break;
            case 1:
                focus.text = "";
                break;
            case 2:
                SwitchCaps();
                break;
            case 3:
                SetActive(false);
                break;
            case 4:
                SetKeyboardType(1);
                break;
            case 5:
                SetKeyboardType(2);
                break;
            case 6:
                FocusPrevious();
                break;
            case 7:
                FocusNext();
                break;
            case 8:
                SetKeyboardType(0);
                break;
        }
    }

    public void SetActive(bool b)
    {
        var animator = gameObject.GetComponent<Animator>();
        if (b)
        {
            if (!isActive)
            {
                animator.Rebind();
                animator.enabled = true;
            }
        }
        else
        {
            if (isActive)
            {
                //  transform.parent.position = parentLocation;
                animator.SetBool("Hide", true);
            }
        }

        isActive = b;
    }

    public void SetCaps(bool b)
    {
        if (b)
        {
            foreach (GameObject go in keys)
            {
                TextMeshProUGUI t = go.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                t.text = t.text.ToUpper();
            }
        }
        else
        {
            foreach (GameObject go in keys)
            {
                TextMeshProUGUI t = go.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                t.text = t.text.ToLower();
            }
        }
        capsEnabled = b;
    }

    public void SwitchCaps()
    {
        SetCaps(!capsEnabled);
    }

    public void FocusPrevious()
    {


        EventSystem system;
        system = EventSystem.current;

        if (!focus) { return; }

        Selectable current = focus.GetComponent<Selectable>();
        Selectable next = current.FindSelectableOnLeft();

        if (!next)
        {
            next = current.FindSelectableOnUp();
        }
        if (!next)
        {
            return;
        }
        if (next.transform.parent.name == "Email_Input")
        {
            SetKeyboardType(0);
        }
        TMP_InputField inputfield = next.GetComponent<TMP_InputField>();
        if (inputfield != null)
        {
            inputfield.OnPointerClick(new PointerEventData(system));
            focus = inputfield;
        }
        system.SetSelectedGameObject(next.gameObject);
    }

    public void FocusNext()
    {
        EventSystem system;
        system = EventSystem.current;

        if (!focus) { return; }

        Selectable current = focus.GetComponent<Selectable>();
        Selectable next = current.FindSelectableOnRight();
        if (!next)
        {
            next = current.FindSelectableOnDown();
        }
        if (!next)
        {
            return;
        }
        if (next.transform.parent.name == "Email_Input")
        {
            SetKeyboardType(0);
        }
        TMP_InputField inputfield = next.GetComponent<TMP_InputField>();
        if (inputfield != null)
        {
            inputfield.OnPointerClick(new PointerEventData(system));
            focus = inputfield;
        }
        system.SetSelectedGameObject(next.gameObject);
    }

    public void SetKeyboardType(int n)
    {
        switch (n)
        {
            case 0:
                panels[0].SetActive(true);
                panels[1].SetActive(false);
                panels[2].SetActive(false);
                panels[3].SetActive(false);

                break;
            case 1:
                panels[0].SetActive(false);
                panels[2].SetActive(false);
                panels[1].SetActive(true);

                break;
            case 2:
                panels[0].SetActive(false);
                panels[1].SetActive(false);
                panels[2].SetActive(false);
                panels[3].SetActive(true);

                break;
            case 3:
                panels[0].SetActive(false);
                panels[1].SetActive(false);
                panels[2].SetActive(true);
                panels[3].SetActive(false);

                break;
        }
    }

}
