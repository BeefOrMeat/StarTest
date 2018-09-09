using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

    //効果音
    private AudioSource audioSource;        // AudioSorceを格納する変数の宣言.
    public List<AudioClip> audioClip = new List<AudioClip>();
    

    void Start()
    {
        //audioSource = gameObject.AddComponent<AudioSource>();

    }

    public void OnStartButtonClicked()
    {
        Debug.Log("ccc");
        //ボタンを押したら音を鳴らして下の関数へ
        //audioSource.PlayOneShot(audioClip[0]);
        //Application.LoadLevel("select");
        //StartCoroutine("GoToSelect");
        Application.LoadLevel( "Game" );
    }

}
