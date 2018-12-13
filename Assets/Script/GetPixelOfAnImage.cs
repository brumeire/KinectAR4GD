using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPixelOfAnImage : MonoBehaviour {

    public GameObject HitBox;
    public Texture2D MyTexture;
    Texture2D MyNewTexture;
    Texture2D MyCharacter;
    int TextureHeigth;
    int TextureWidth;

    Color[] colors;
    

    // Use this for initialization
    void Start () {
        TextureHeigth = MyTexture.height;
        TextureWidth = MyTexture.width;
        MyNewTexture = new Texture2D(TextureWidth, TextureHeigth, MyTexture.format, false);
        MyCharacter = new Texture2D(TextureWidth, TextureHeigth, MyTexture.format, false);
        MyNewTexture.filterMode = FilterMode.Point; 
        print("Taille Image : " + TextureHeigth + " " + TextureWidth);
        GetPixelColor();
        Color[] BaseAvatarPosition = MyTexture.GetPixels();
        //Instantiate(Image, GameObject.Find("Canvas").gameObject, false);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void LateUpdate()
    {
        //PositionAvatar();
    }

    void GetPixelColor()
    {
        colors = MyTexture.GetPixels();
        print(colors.Length);

        for (int i = 0; i < colors.Length; i++)
        {
            Color MyColor = colors[i];
            if (MyColor.grayscale > 0.9F)
            {
                colors[i] = Color.white;
            }
            else
            {
                colors[i] = Color.black;
                int Colonne = (int) i / MyTexture.width;
                //print(Colonne);
                
                GameObject MyNewObject = Instantiate(HitBox, new Vector3(-31.5f + (i % MyTexture.width)*0.125f, -26.5f + (Colonne) * 0.125f, 0), new Quaternion(0, 0, 0, 0));
                MyNewObject.name = (i % MyTexture.width) + " " + (Colonne);
            }
        }
        
        MyNewTexture.SetPixels(colors);
        MyNewTexture.Apply();
    }

    /*void PositionAvatar()
    {
        bool FindAWayOut = false;
        int NumberOfPath = 1;
        while (FindAWayOut == false)
        {
            print("OutOfArray");
            print(Camera.main.pixelWidth + " " + Camera.main.pixelHeight);
            print(PositionAvatarInPixel - Camera.main.pixelWidth * NumberOfPath);
            if (PositionAvatarInPixel - Camera.main.pixelWidth * NumberOfPath > 0)
            {
                if (colors[PositionAvatarInPixel - Camera.main.pixelWidth * NumberOfPath] != Color.black)
                {

                    SetPositionOfAvatar("Bottom", NumberOfPath);
                    FindAWayOut = true;
                }
            }


            NumberOfPath += 1;
        }
    }*/

    /*void SetPositionOfAvatar(string MyMovingPosition, int NumberOfPath)
    {
        if (MyMovingPosition == "Bottom")
        {
            print("Bottom");
            int PassRemaning = NumberOfPath;
            if (PassRemaning > 0) {
                Color[] BaseAvatarPosition = MyTexture.GetPixels();
                BaseAvatarPosition[BaseAvatarPosition.Length / 2 - Camera.main.pixelHeight *NumberOfPath -PassRemaning +1] = Color.red;
                PassRemaning -= 1;
            }
            else
            {
                return;
            }
        }
    }*/

        IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    void OnGUI()
    {
        //GUI.DrawTexture(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight), MyNewTexture);
        //GUI.DrawTexture(new Rect(Screen.width / 2 - MyNewTexture.width / 2, Screen.height / 2 - MyNewTexture.height / 2, MyNewTexture.width, MyNewTexture.height), MyCharacter);
    }
}
