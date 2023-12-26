using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Xml.Linq;

public class EditTowers : MonoBehaviour
{
    [Header("Current Waypoint")]
    [SerializeField]Waypoint waypoint;

    [Header("EditVFX")]
    [SerializeField] ParticleSystem hoverDeployVFX;
    [SerializeField] ParticleSystem hoverDestroyVFX;

    [Header("Tower Type")]
    [SerializeField] Tower tower;

    [Header("Tower Model")]
    [SerializeField] GameObject dogDisplay;

    [Header("Side Menus")]
    [SerializeField] GameObject upgradeTab;
    [SerializeField] GameObject constructionTab;
    
    Vector3 transformUpgrade;
    Vector3 transformConstruction;
    float menuSpeed = 1f;

    //[Header("Side Menu Buttons")]
    //[SerializeField] List<Button> constructionButtons = new List<Button>();

    [SerializeField] bool canModify;
    bool gecici;
    public bool CanModify { get { return canModify; } }

    private void Start()
    {
        transformUpgrade = upgradeTab.transform.localPosition;
        transformConstruction = constructionTab.transform.localPosition;
        
        //FindEditButtons();
    }

    private void Update()
    {
        if (waypoint == null)
            return;

        if (gecici) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                DogInstantiator();
                HandleEditVFX();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                DogRemover();
                HandleEditVFX();
            }
        }
    }

    //private void FindEditButtons()
    //{
    //    foreach (Button button in constructionTab.GetComponentsInChildren<Button>())
    //    {
    //        if (button != null)
    //        {
    //            constructionButtons.Add(button);
    //        }
    //    }
    //}

    public void ModifySwitch()
    {
        canModify = !canModify;

        if (canModify)
        {
            MoveSideTabs(Vector3.zero, Vector3.zero);
        }
        else
        {
            MoveSideTabs(transformUpgrade, transformConstruction);
        }
    }

    void MoveSideTabs(Vector3 nextUpgradePos, Vector3 nextConstructionPos)
    {
        upgradeTab.transform.DOLocalMove(nextUpgradePos, menuSpeed).SetEase(Ease.OutBounce);
        constructionTab.transform.DOLocalMove(nextConstructionPos, menuSpeed).SetEase(Ease.OutBounce);
    }

    public void SetWaypoint(Waypoint currentWaypoint)
    {
        waypoint = currentWaypoint;
        HandleEditVFX();
    }

    private void HandleEditVFX()
    {
        //We are checking is modify button clicked or not.
        if (!CanModify)
            return;

        if (waypoint == null)
            return;

        //We check hasPlaced besides isPlaceable because of VFX. If we only checked isPlaceable then our cats path also uses our destroyVFX.
        //hasPlaced is helping us to know if a dog is deployed or not. isPlaceable on the other hand is checks for the tile is usable for towers.
        if (waypoint.IsPlaceable && !waypoint.HasPlaced)
        {
            TowerDisplayerVFX(true, waypoint.transform);
            SelectorVFX(true, hoverDeployVFX, waypoint.transform);
            SelectorVFX(false, hoverDestroyVFX, waypoint.transform);

            //if (Input.GetMouseButtonDown(0))
            //{
            //    DogInstantiator();
            //}

            gecici = true;
        }
        else if (waypoint.IsPlaceable && waypoint.HasPlaced)
        {
            TowerDisplayerVFX(false, waypoint.transform);
            SelectorVFX(false, hoverDeployVFX, waypoint.transform);
            SelectorVFX(true, hoverDestroyVFX, waypoint.transform);

            //if (Input.GetMouseButtonDown(0))
            //{
            //    DogRemover();
            //}

            gecici = false;
        }
    }

    //We check waypoints(tiles) for dogDisplay purposes. This is also kind of a VFX.
    public void TowerDisplayerVFX(bool isVisible, Transform tilePos)
    {
        dogDisplay.transform.position = tilePos.position;
        dogDisplay.SetActive(isVisible);
    }

    private void SelectorVFX(bool isHovering, ParticleSystem selectVFX, Transform tilePos)
    {
        if (isHovering)
        {
            selectVFX.gameObject.transform.position = tilePos.position;
            selectVFX.Play();
        }
        else
        {
            selectVFX.gameObject.transform.position = tilePos.position;
            selectVFX.Stop();
        }
    }

    private void DogInstantiator()
    {
        bool placed = tower.CreateTower(tower, waypoint.transform.position, waypoint.transform);
        waypoint.TowerPlaced(placed);

        if (waypoint.HasPlaced)
        {
            SelectorVFX(false, hoverDeployVFX, waypoint.transform);
        }
    }

    private void DogRemover()
    {
        GameObject dog = waypoint.GetComponentInChildren<Tower>().gameObject;
        bool placed = tower.DestroyTower(dog);
        waypoint.TowerPlaced(placed);

        SelectorVFX(false, hoverDestroyVFX, waypoint.transform);
    }

    //public void DisplayName(Button pressedButton)
    //{
    //    int index = 0;

    //    if (constructionButtons.Contains(pressedButton))
    //    {
    //        index = constructionButtons.IndexOf(pressedButton);
    //        Debug.Log(constructionButtons[index].name);

    //    }
    //}
}
