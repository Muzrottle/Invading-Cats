using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Xml.Linq;
using System.Linq.Expressions;
using UnityEngine.EventSystems;

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

    bool canDisplayVFX;
    bool canDeployVFX;
    bool canDestroyVFX;

    Bank bank;

    Vector3 transformUpgrade;
    Vector3 transformConstruction;
    float menuSpeed = 1f;
    int editCase = 0;

    //[Header("Side Menu Buttons")]
    //[SerializeField] List<Button> constructionButtons = new List<Button>();

    [SerializeField] bool canModify;
    public bool CanModify { get { return canModify; } }

    private void Start()
    {
        transformUpgrade = upgradeTab.transform.localPosition;
        transformConstruction = constructionTab.transform.localPosition;
        
        bank = FindObjectOfType<Bank>();
        //FindEditButtons();
    }

    private void Update()
    {
        if (waypoint == null)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (CanModify) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                CaseSelector();
                HandleEditVFX();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                CaseSelector();
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

        bool changeVFXPos = false;

        //We check hasPlaced besides isPlaceable because of VFX. If we only checked isPlaceable then our cats path also uses our destroyVFX.
        //hasPlaced is helping us to know if a dog is deployed or not. isPlaceable on the other hand is checks for the tile is usable for towers.
        switch (editCase)
        {
            case 0:
                if (waypoint.IsPlaceable && !waypoint.HasPlaced)
                {
                    canDisplayVFX = true;
                    canDeployVFX = true;
                    canDestroyVFX = false;
                    changeVFXPos = true;
                }
                else if (waypoint.IsPlaceable && waypoint.HasPlaced)
                {
                    canDisplayVFX = false;
                    canDeployVFX = false;
                    canDestroyVFX = true;
                    changeVFXPos = true;
                }
                else
                {
                    canDisplayVFX = false;
                    canDeployVFX = false;
                    canDestroyVFX = false;
                    changeVFXPos = false;
                }
                break;
            case 1:
                if (waypoint.placedObject != null)
                {
                    if (waypoint.placedObject.GetComponent<DestroyableObstacle>())
                    {
                        canDisplayVFX = false;
                        canDeployVFX = false;
                        canDestroyVFX = true;
                        changeVFXPos = true;
                    }
                    else
                    {
                        Debug.Log("Girdim");
                        canDisplayVFX = false;
                        canDeployVFX = false;
                        canDestroyVFX = false;
                        changeVFXPos = false;
                    }
                }
                else
                {
                    Debug.Log("Girdim");
                    canDisplayVFX = false;
                    canDeployVFX = false;
                    canDestroyVFX = false;
                    changeVFXPos = false;
                }
                break;
            default:
                Debug.Log("Olmuyor kanka.");
                break;
        }

        TowerDisplayerVFX(canDisplayVFX, changeVFXPos, waypoint.transform);
        SelectorVFX(canDeployVFX, hoverDeployVFX, changeVFXPos, waypoint.transform);
        SelectorVFX(canDestroyVFX, hoverDestroyVFX, changeVFXPos, waypoint.transform);
    }

    //We check waypoints(tiles) for dogDisplay purposes. This is also kind of a VFX.
    public void TowerDisplayerVFX(bool isVisible, bool isPosChange, Transform tilePos)
    {
        if (isPosChange)
        {
            dogDisplay.transform.position = tilePos.position;
        }
        
        dogDisplay.SetActive(isVisible);
    }

    private void SelectorVFX(bool isHovering, ParticleSystem selectVFX, bool isPosChange, Transform tilePos)
    {
        if (isHovering)
        {
            selectVFX.Play();
        }
        else
        {
            selectVFX.Stop();
        }

        if (isPosChange)
        {
            selectVFX.gameObject.transform.position = tilePos.position;
        }
    }

    public void SetCase(EditButton editButton)
    {
        editCase = editButton.myEditCase;
        HandleEditVFX();
    }

    private void CaseSelector()
    {
        switch(editCase)
        {
            case 0:
                if (waypoint.IsPlaceable && !waypoint.HasPlaced)
                    DogInstantiator();
                else if (waypoint.IsPlaceable && waypoint.HasPlaced)
                    DogRemover();
                break;
            case 1:
                if (waypoint.placedObject != null)
                    ObstacleRemover(waypoint.placedObject, waypoint.placedObject.GetComponent<DestroyableObstacle>());
                break;
            default:
                Debug.Log("Olmuyor kanka.");
                break;
        }
    }

    private void DogInstantiator()
    {
        bool placed = tower.CreateTower(tower, waypoint.transform.position, waypoint.transform);
        waypoint.SetTilePlacement(placed);
    }

    private void DogRemover()
    {
        GameObject dog = waypoint.GetComponentInChildren<Tower>().gameObject;
        bool placed = tower.DestroyTower(dog);
        waypoint.SetTilePlacement(placed);
    }

    private void ObstacleRemover(GameObject destroyObject, DestroyableObstacle destroyableObstacle)
    {
        if (destroyableObstacle == null)
            return;

        if (bank.CurrentBalance < destroyableObstacle.RemovePrice)
            return;

        Destroy(destroyObject);
        
        waypoint.placedObject = null;
        waypoint.MakeIsPlaceableTrue();
        
        bank.Withdraw(destroyableObstacle.RemovePrice);
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
