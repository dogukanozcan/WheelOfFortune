using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Page
{
    MainMenu,
    WheelPage
}
public class PageController : MonoSingleton<PageController>
{
    [SerializeField] private List<GameObject> m_pages;
    [SerializeField] private GameObject m_defaultPage;

    public event Action OnPageChanged;

    private void Awake()
    {
        ChangePage(m_defaultPage);
    }

    public void ClosePage(GameObject targetPage)
    {
        //Deactivate target page
        targetPage.SetActive(false);
    }
    public void OpenPage(GameObject targetPage)
    {
        //Activate target page
        targetPage.SetActive(true);
    }
    public void ChangePage(GameObject targetPage)
    {
        //Deactive all page
        m_pages.ForEach(page => ClosePage(page));

        //Activate target page
        OpenPage(targetPage);

        OnPageChanged?.Invoke();
    }
    public void ChangePage(string pageName)
    {
        //Deactive all page
        m_pages.ForEach(page => ClosePage(page));

        //Activate target page
        OpenPage(GetPageByName(pageName));

        OnPageChanged?.Invoke();
    }

    public GameObject GetPageByName(string name)
    {
        if (string.IsNullOrEmpty(name)) 
        {
            Debug.LogError("GetPageByName Error | Parameter is null or empty");
            return null;
        }
   

        var targetPage = m_pages.Find(page => page.name == name);
        if (targetPage == null)
        {
            Debug.LogError("GetPageByName Error | Cannot find page named: " + name);
        }

        return targetPage;
    }

}
