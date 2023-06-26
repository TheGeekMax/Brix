using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour{
    bool finished = false;

    public static WinManager instance;

    void Awake(){
        instance = this;
    }

    void Update(){
        if(finished || !GameManager.instance.defined){return;}
        //on test a chaque execution si le plateau est vide
        bool isEmpty = true;
        int width = GameManager.instance.width;
        for(int i = 0; i < width; i ++){
            for(int j = 0; j < width; j ++){
                if(GameManager.instance.getCell(i,j) > 0){
                    isEmpty = false;
                    //stop la boucle
                    i = width;
                    j = width;
                }
            }    
        }

        if(isEmpty){
            //youpi on a gagné
            Debug.Log("Win !");
            finished = true;

            if(DataLoaderScript.instance.lvl == "endless"){
                //on augmente le level
                GameManager.instance.level ++;
                
                //on sauvegarde le level
            
                PlayerPrefs.SetInt("LEVEL_INF",GameManager.instance.level);
                //on regenere le plateau
                Invoke("Regenerate",0.5f);
            }else{
                DataLoaderScript.instance.time = TimeManager.instance.getTime();
                //on lance la fonction win décalé de 2 secondes
                Invoke("win",1);
            }
        }
    }

    void win(){
        //pour l'instant on reload la scene
        GameManager.instance.home();
    }

    void Regenerate(){
        ViewManager.instance.reset();
        GameManager.instance.generateEndless();
        finished = false;
    }

    public bool winned(){
        return finished;
    }
}
