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
    [Header("Current Tile")]
    [SerializeField] Tile tile;
    [SerializeField] Vector2Int tileCoordinates;

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
    GridManager gridManager;
    Pathfinder pathfinder;

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
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
        //FindEditButtons();
    }

    private void Update()
    {
        if (tile == null)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            HandleEditVFX();
            return;
        }

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

    public void SetTile(Tile currentTile, Vector2Int coordinates)
    {
        tile = currentTile;
        tileCoordinates = coordinates;
        HandleEditVFX();
    }

    private void HandleEditVFX()
    {
        //We are checking is modify button clicked or not.
        if (!CanModify)
            return;

        if (tile == null)
            return;

        bool mouseOnEditButton = EventSystem.current.IsPointerOverGameObject();
        bool changeVFXPos = false;

        //We check hasPlaced besides isPlaceable because of VFX. If we only checked isPlaceable then our cats path also uses our destroyVFX.
        //hasPlaced is helping us to know if a dog is deployed or not. isPlaceable on the other hand is checks for the tile is usable for towers.
        switch (editCase)
        {
            case 0:
                if (gridManager.Grid[tileCoordinates].isWalkable && !tile.HasPlaced && !mouseOnEditButton)
                {
                    canDisplayVFX = true;
                    canDeployVFX = true;
                    canDestroyVFX = false;
                    changeVFXPos = true;
                }
                else if (!gridManager.Grid[tileCoordinates].isWalkable && tile.HasPlaced && !mouseOnEditButton)
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
                if (tile.placedObject != null)
                {
                    if (tile.placedObject.GetComponent<DestroyableObstacle>())
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
            case 2:
                if (tile.placedObject != null)
                {
                    if (tile.placedObject.GetComponent<Tower>() != null)
                    {
                        canDisplayVFX = false;
                        canDeployVFX = true;
                        canDestroyVFX = false;
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

        TowerDisplayerVFX(canDisplayVFX, changeVFXPos, tile.transform);
        SelectorVFX(canDeployVFX, hoverDeployVFX, changeVFXPos, tile.transform);
        SelectorVFX(canDestroyVFX, hoverDestroyVFX, changeVFXPos, tile.transform);
    }

    //We check tiles for dogDisplay purposes. This is also kind of a VFX.
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
        Debug.Log(tile.name);

        switch(editCase)
        {
            case 0:
                if (gridManager.Grid[tileCoordinates].isWalkable && !tile.HasPlaced && !pathfinder.WillBlockPath(tileCoordinates))
                    DogInstantiator();
                else if (!gridManager.Grid[tileCoordinates].isWalkable && tile.HasPlaced && !pathfinder.WillBlockPath(tileCoordinates))
                    DogRemover();
                break;
            case 1:
                if (tile.placedObject != null)
                    ObstacleRemover(tile.placedObject, tile.placedObject.GetComponent<DestroyableObstacle>());
                break;
            case 2:
                if (tile.placedObject != null)
                {
                    if (tile.placedObject.GetComponent<Tower>() != null)
                        DogPowerUp(tile.placedObject.GetComponent<Tower>());
                }
                break;
            default:
                Debug.Log("Olmuyor kanka.");
                break;
        }
    }

    private void DogInstantiator()
    {
        bool placed = tower.CreateTower(tower, tile);
        if (placed)
        {
            gridManager.BlockNode(tileCoordinates);
            pathfinder.NotifyRecievers();
        }
    }

    private void DogRemover()
    {
        GameObject dog = tile.GetComponentInChildren<Tower>().gameObject;
        tower.DestroyTower(dog, tile);
        gridManager.UnblockNode(tileCoordinates);
        pathfinder.NotifyRecievers();
    }

    private void ObstacleRemover(GameObject destroyObject, DestroyableObstacle destroyableObstacle)
    {
        if (destroyableObstacle == null)
            return;

        if (bank.CurrentBalance < destroyableObstacle.RemovePrice)
            return;

        Destroy(destroyObject);
        
        tile.placedObject = null;
        tile.MakeIsPlaceableTrue();
        
        bank.Withdraw(destroyableObstacle.RemovePrice);
    }

    void DogPowerUp(Tower myTower)
    {
        myTower.TowerPowerUp();
    }
}
