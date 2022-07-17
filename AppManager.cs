using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using System.Linq;
using SimpleJSON;
using UnityEngine.UI;
public class AppManager : MonoBehaviour
{

    public GameObject nameDisplay;
    public GameObject ArtistName;
    public GameObject song_url;
    public string url;
    public string ACCESS_TOKEN;
    public JSONNode jsonResult;

    void Start ()
    {
           StartCoroutine(GetRequest(url));
           
    }
private int frames =0;
    void Update(){
        frames++;
        if (frames % 1000 == 0) {
         StartCoroutine(GetRequest(url));
      
        }
    }
     
      IEnumerator GetRequest(string uri)
    {

       UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        webRequest.SetRequestHeader("Authorization","Bearer "+ACCESS_TOKEN);
            yield return webRequest.SendWebRequest();
        
            string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
            jsonResult = JSON.Parse(rawJson);
            nameDisplay.GetComponent<Text>().text = jsonResult["item"]["name"];
            ArtistName.GetComponent<Text>().text = jsonResult["item"]["artists"][0]["name"];
           

           
            StartCoroutine(DownloadImage(jsonResult["item"]["album"]["images"][0]["url"]));

            IEnumerator DownloadImage(string MediaUrl)
    {
       UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
         yield return request.SendWebRequest();
      try {
        Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
			Sprite webSprite = SpriteFromTexture2D(webTexture);
			song_url.GetComponent<Image>().sprite = webSprite;
    }
    catch (Exception e) {
        
    }  
         
    }

    Sprite SpriteFromTexture2D(Texture2D texture) {
		return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
	}
    
}

 

}