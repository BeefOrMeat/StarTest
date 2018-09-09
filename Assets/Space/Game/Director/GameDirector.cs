using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private Transform mDatasPrefab;
    private ConstellationDataList mDatas;

    [SerializeField]
    private Transform mDrawerGameManagerPrefab;
  
    [SerializeField]
    private Transform mGuideGameManagerPrefab;

    private BaseGameManager mGameManager;

    [SerializeField]
    private Transform mCanvasPrefab;
    private Canvas mCanvas;

    [SerializeField]
    private Transform mStarGeneratorPrefab;
    private StarGenerator mStarGenerator;

    private Text mText;
    
    [SerializeField]
    private float mMaxTime = 30;
    private float mTime;

    [SerializeField]
    private RenderTexture mRenderTexture;

    void Start ()
    {
        mDatas = Instantiate(mDatasPrefab).GetComponent<ConstellationDataList>();

        mStarGenerator = Instantiate(mStarGeneratorPrefab).gameObject.GetComponent<StarGenerator>();

        int index = Random.Range(0, mDatas.Datas.Count - 1);
        ConstellationData data = Instantiate(mDatas.Datas[index]).GetComponent<ConstellationData>();

        Vector3 lookAtPos = Vector3.zero;
        float rad = Random.value + Mathf.PI * 0.5f;
        lookAtPos.x = Mathf.Cos(rad);
        lookAtPos.z = Mathf.Sin(rad);
        lookAtPos *= 100;

        Camera camera = data.gameObject.AddComponent<Camera>();
        camera.fov = 25.0f;
        camera.targetTexture = mRenderTexture;

        data.transform.LookAt(lookAtPos);
        data.transform.Rotate(new Vector3(Random.Range(-Mathf.PI, 0.0f) * Mathf.Rad2Deg, 0.0f, 0.0f));

        List<Line> lines = data.GetChildrenLines();
        foreach (Line line in lines)
        {
            Vector3 scale = line.transform.localScale;
            line.gameObject.layer = LayerMask.NameToLayer("Target");
            line.transform.localScale = new Vector3(0.25f, 0.25f, scale.z);
            Destroy(line);
        }

        mStarGenerator.AddConstellation(data);

        mDatas.Datas.Clear();

        mCanvas = Instantiate(mCanvasPrefab).GetComponent<Canvas>();
        Image image = mCanvas.transform.Find("Image").GetComponent<Image>();
        image.material.mainTexture = mRenderTexture;
        mText = mCanvas.transform.Find("Text").GetComponent<Text>();

        if (GlobalData.role == Role.Drawer)
        {
            mGameManager = Instantiate(mDrawerGameManagerPrefab).gameObject.GetComponent<DrawerGameManager>();
        }
        else if (GlobalData.role == Role.Guide)
        {
            mGameManager = Instantiate(mGuideGameManagerPrefab).gameObject.GetComponent<GuideGameManager>();
        }
    }

    void Update ()
    {
        float deltaTime = Time.deltaTime;
        mTime -= deltaTime;
        mText.text = "Time:" + mTime;
        if (mTime <= 0.0f)
        {
            mTime = 0.0f;
        }
    }

    public void SetRandomSeed(int seed)
    {
        mStarGenerator.SetSeed(seed);
    }
}
