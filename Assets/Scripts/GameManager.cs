using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private Vector3 OFFSET = new Vector3(0.5f, 0.5f, 0);
    public int width = 5;

    //pour le mode endless 
    public int level = 0;
    

    int[,] grid;

    public bool defined = false;

    //2 piles pour les undo et redo
    Stack<Vector2Int> undoStack = new Stack<Vector2Int>();
    Stack<Vector2Int> redoStack = new Stack<Vector2Int>();

    public static GameManager instance;
    
    void Awake(){
        instance = this;

    }

    void Start(){
        //on desactive le fait de pouvoir avoir plusieurs doigts
        Input.multiTouchEnabled = false;
        //on test si on est en mode endless
        if(DataLoaderScript.instance.lvl == "endless"){
            //on verif si la valeur LEVEL_INF est définie
            level = PlayerPrefs.GetInt("LEVEL_INF",0);
            generateEndless();
        }
    }

    void Update(){
        if(!defined){return;}
        //on gere le tactile (mobile)
        if (Input.touchCount > 0){
            //on test si le type de touch est un touch de début de doigt
            if (Input.GetTouch(0).phase != TouchPhase.Began) return;
            //on recupere la position du doigt
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            pos.z = 0;

            //on converti la position en case
            int x = Mathf.FloorToInt(pos.x);
            int y = Mathf.FloorToInt(pos.y);

            //on verifie si on est pas oob
            if (x < 0 || x >= width || y < 0 || y >= width) return;

            //on détruit la case
            bool done = dropBomb(x, y);
            //on ajoute la case à la pile undo
            if(done) undoStack.Push(new Vector2Int(x, y));

        }        
    }

    public void generate(int newWidth,int stepCount){
        width = newWidth;
        
        grid = new int[width, width];

        //on le remplit de valeurs aléatoires
        for (int x = 0; x < width; x++){
            for (int y = 0; y < width; y++){
                grid[x, y] = 0;
            }
        }

        //on joue 5 coup au pif
        for (int i = 0; i < stepCount; i++){
            int x = Random.Range(0, width);
            int y = Random.Range(0, width);

            placeBricks(x, y);
        }
        defined = true;
        ViewManager.instance.updateAll();
        //todo la CAMERA BORDEL !

        //on la bouge de width/2+0.5f
        Camera.main.transform.position = new Vector3(width/2f, width/2f, -10);

        //la taille orhograpic en longueur
        Camera.main.orthographicSize = width + 2;
    }

    public void updateAll(){
        //on update toutes les cases
        for (int x = 0; x < width; x++){
            for (int y = 0; y < width; y++){
                ViewManager.instance.UpdateData(x, y, grid[x, y]);
            }
        }
    }

    public void generateEndless(){
        //on empty les piles
        EmptyStacks();
        //on genere un niveau de 5x5 (après on fera un niveau de plus en plus dur)
        if(level < 3){
            generate(5, 5);
        }else if(level < 10){
            generate(5, 10);
        }else if(level < 15){
            generate(5, 20);
        }else if(level < 20){
            generate(5, 30);
        }else{
            generate(6, 40);
        }
    }

    //fonctions utilitaire

    public void home(){
        //on relooad la scene
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    bool canBeDestroyed(int x, int y){
        //on vérifie que la case existe
        if (x < 0 || x >= width || y < 0 || y >= width) return true;

        //on vérifie que la case est >= 1
        if (grid[x, y] < 1) return false;

        return true;
    }

    void destroy(int x, int y){
        //on vérifie que la case existe
        if (x < 0 || x >= width || y < 0 || y >= width) return;

        //on vérifie que la case est >= 1
        if (grid[x, y] < 1) return;

        grid[x, y] --;

        //on update la case
        ViewManager.instance.UpdateData(x, y, grid[x, y]);
    }

    void place(int x, int y){
        //on vérifie que la case existe
        if (x < 0 || x >= width || y < 0 || y >= width) return;

        grid[x, y] ++;

        //on update la case
        ViewManager.instance.UpdateData(x, y, grid[x, y]);
    }

    void placeWithoutUpdate(int x, int y){
        //on vérifie que la case existe
        if (x < 0 || x >= width || y < 0 || y >= width) return;

        grid[x, y] ++;
    }

    bool dropBomb(int x,int y){
        //on test les case autour (haut, bas, gauche, droite) et le centre
        if(canBeDestroyed(x, y) && canBeDestroyed(x, y+1) && canBeDestroyed(x, y-1) && canBeDestroyed(x+1, y) && canBeDestroyed(x-1, y)){
            //si toutes les cases sont bonnes, on les détruit
            destroy(x, y);
            destroy(x, y+1);
            destroy(x, y-1);
            destroy(x+1, y);
            destroy(x-1, y);

            return true;
        }
        return false;
    }

    void Undo(int x,int y){
        //on les places sans reflechir
        place(x, y);
        place(x, y+1);
        place(x, y-1);
        place(x+1, y);
        place(x-1, y);
    }

    void placeBricks(int x,int y){
        //on les places sans reflechir
        placeWithoutUpdate(x, y);
        placeWithoutUpdate(x, y+1);
        placeWithoutUpdate(x, y-1);
        placeWithoutUpdate(x+1, y);
        placeWithoutUpdate(x-1, y);
    }

    public void EmptyStacks(){
        undoStack.Clear();
        redoStack.Clear();
    }

    //pour les piles
    public void UndoCoup(){
        if(undoStack.Count > 0){
            Vector2Int pos = undoStack.Pop();
            redoStack.Push(pos);
            Undo(pos.x, pos.y);
        }
    }

    public void RedoCoup(){
        if(redoStack.Count > 0){
            Vector2Int pos = redoStack.Pop();
            undoStack.Push(pos);
            dropBomb(pos.x, pos.y);
        }
    }

    //fonctions de recuperation et de modification de la grille
    public int getCell(int x, int y){
        return grid[x,y];
    }
}
