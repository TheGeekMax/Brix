using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileComponent : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    //TMpro text
    public TextMeshProUGUI textMesh;


    public void UpdateSprite(int value){
        if(value == 0){
            //on met le sprite empty de viewManager
            spriteRenderer.sprite = ViewManager.instance.empty;
            textMesh.enabled = false;
        }else{
            textMesh.enabled = true;
            spriteRenderer.sprite = ViewManager.instance.GetSprite(value);
            textMesh.text = value.ToString();
        } 
    }
}
