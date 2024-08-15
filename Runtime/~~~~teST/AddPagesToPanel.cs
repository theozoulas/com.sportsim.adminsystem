using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AddPagesToPanel : MonoBehaviour
{
    public void AddPages(Sprite[] pages, GameObject pagePrefab)
    {
        var childrenImages = transform.GetComponentsInChildren<Image>();
        var childrenSprites = new List<Sprite>(childrenImages.Select(i => i.sprite));

        foreach (var page in pages)
        {
            if(childrenSprites.Contains(page)) continue;
            
            var documentPage = Instantiate(pagePrefab, transform);

            documentPage.GetComponent<Image>().sprite = page;
        }

        var verticalLayout = transform.GetComponent<VerticalLayoutGroup>();
        var rectTransform = transform.GetComponent<RectTransform>();
        var pageRectTransform = pagePrefab.GetComponent<RectTransform>();
        
        var pageHeight = pageRectTransform.sizeDelta.y + verticalLayout.spacing;
        var padding = (verticalLayout.padding.top * 2) +  + verticalLayout.spacing;
        
        var newLayoutSize = new Vector2(rectTransform.sizeDelta.x, (pages.Length * pageHeight) + padding);
        
        rectTransform.sizeDelta = newLayoutSize;
    }
}
