using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    public Sprite[] sprites;

    public Sprite empty;

    public GameObject tilePrefab;
    // Start is called before the first frame update
    GameObject[,] showGrid;

    public static ViewManager instance;
    void Awake(){
        instance = this;
    }

    public void updateAll(){
        //on init la grille avec le tilePrefab
        showGrid = new GameObject[GameManager.instance.width, GameManager.instance.width];
        for (int x = 0; x < GameManager.instance.width; x++){
            for (int y = 0; y < GameManager.instance.width; y++){
                GameObject go = Instantiate(tilePrefab, new Vector3(x+.5f, y+.5f, 0), Quaternion.identity);
                go.transform.SetParent(transform);
                showGrid[x, y] = go;
            }
        }
    
        GameManager.instance.updateAll();
    }

    // Update is called once per frame
    public void UpdateData(int x, int y, int value){
        showGrid[x, y].GetComponent<TileComponent>().UpdateSprite(value);
    }

    public Sprite GetSprite(int value){
        return sprites[value % sprites.Length];
    }

    public void reset(){
        for(int x = 0; x < GameManager.instance.width; x++){
            for(int y = 0; y < GameManager.instance.width; y++){
                Destroy(showGrid[x,y]);
                showGrid[x,y] = null;
            }
        }
    }
}
