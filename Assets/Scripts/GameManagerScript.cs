using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum Mode
{
    SCAN,
    EXTRACT
}
public class GameManagerScript : MonoBehaviour
{
    public Mode state;
    public AudioClip TileScan;
    public AudioClip TileExtract;
    public AudioClip Gameover;
    public AudioClip ButtonClick;
    AudioSource audioSource;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scanText;
    public TextMeshProUGUI extractText;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI GameOverScoreText;
    public Button modeButton;
    public Canvas GameOverCanvas;
    public int scanCount;
    public int extractCount;
    public int CountX = 32;
    public int CountY = 32;
    public float BlockSize = 20.0f;
    public float Space = 1.0f;
    public GameObject BlockPrefab;
    public Vector2 StartPos = new Vector2(0.0f, 0.0f);
    public int Score;
    private GameObject[] BlockArray;
    public int MaxResourceCount = 20;
 
    [SerializeField]
    public TMP_Text gameStat;

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
        scanCount = 6;
        extractCount = 3;
        Score = 0;
        state = Mode.SCAN;
        GameOverCanvas.enabled = false;
        scoreText.text = Score.ToString();
        scanText.text = scanCount.ToString();
        extractText.text = extractCount.ToString();       
        BlockArray = new GameObject[CountX * CountY];

        for (int i = 0; i < CountY; ++i)
        {
            for (int j = 0; j < CountX; ++j)
            {
                Vector2 Pos = new Vector2();
                Pos.x = StartPos.x + BlockSize * j + Space * j;
                Pos.y = StartPos.y - BlockSize * i - Space * i;

                GameObject Block = Instantiate(BlockPrefab, Vector3.zero,
                    Quaternion.identity, GameObject.Find("Canvas").transform);

                Block.GetComponent<Resource>().IndexX = j;
                Block.GetComponent<Resource>().IndexY = i;

                Block.transform.GetComponent<Image>().rectTransform.position = Pos;

                BlockArray[i * CountX + j] = Block;
            }
        }

        for (int i = 0; i < MaxResourceCount; ++i)
        {
            int Index = Random.Range(0, CountX * CountY);

            if (BlockArray[Index].GetComponent<Resource>().m_BlockType == Block_Type.Minimal)
            {
                BlockArray[Index].GetComponent<Resource>().m_BlockType = Block_Type.Max;
            }
            int IndexY = Index / CountX;
            int IndexX = Index % CountX;
            SetBlock(IndexX, IndexY);
        }

    }

    void PlaySound(string action)
    {
        switch(action)
        {
            case "TILE":
                audioSource.clip = TileScan;
                break;
            case "EXTRACT":
                audioSource.clip = TileExtract;
                break;
            case "GAMEOVER":
                audioSource.clip = Gameover;
                break;
            case "BUTTON":
                audioSource.clip = ButtonClick;
                break;
        }
        audioSource.Play();
    }
    void Update()
    {
        GameOver();
    }
    public void ExitButton()
    {
        PlaySound("BUTTON");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ChangeMode()
    {
        switch (state)
        {
            case Mode.SCAN:
                PlaySound("BUTTON");
                state = Mode.EXTRACT;
                modeText.text = "Extract Mode";
                break;
            case Mode.EXTRACT:
                PlaySound("BUTTON");
                state = Mode.SCAN;
                modeText.text = "Scan Mode";
                break;
        }
    }

    private void GameOver()
    {
        if (extractCount == 0)
        {
            PlaySound("GAMEOVER");
            GameOverCanvas.enabled = true;
            GameOverScoreText.text = Score.ToString();
        }
    }

    private void SetBlock(int IndexX, int IndexY)
    {
        int[] Index = new int[8];

        for(int i = 0;i < 8; ++i)
        {
            Index[i] = -1;
        }

        int[] QIndex = new int[16];

        for (int i = 0; i < 16; ++i)
        {
            QIndex[i] = -1;
        }

        if (IndexY - 1 >= 0)
            Index[0] = (IndexY - 1) * CountX + IndexX;

        if (IndexY - 1 >= 0 && IndexX + 1 < CountX)
            Index[1] = (IndexY - 1) * CountX + (IndexX + 1);

        if (IndexX + 1 < CountX)
            Index[2] = IndexY * CountX + (IndexX + 1);

        if (IndexY + 1 < CountY && IndexX + 1 < CountX)
            Index[3] = (IndexY + 1) * CountX + (IndexX + 1);

        if (IndexY + 1 < CountY)
            Index[4] = (IndexY + 1) * CountX + IndexX;

        if (IndexY + 1 < CountY && IndexX - 1 >= 0)
            Index[5] = (IndexY + 1) * CountX + (IndexX - 1);

        if (IndexX - 1 >= 0)
            Index[6] = IndexY * CountX + (IndexX - 1);

        if (IndexY - 1 >= 0 && IndexX - 1 >= 0)
            Index[7] = (IndexY - 1) * CountX + (IndexX - 1);

        for (int i = 0; i < 8; ++i)
        {
            if (Index[i] == -1)
                continue;

            if (BlockArray[Index[i]].GetComponent<Resource>().m_BlockType == Block_Type.Minimal)
            {
                BlockArray[Index[i]].GetComponent<Resource>().m_BlockType = Block_Type.Half;
            }
        }

        if (IndexY - 2 >= 0)
            QIndex[0] = (IndexY - 2) * CountX + IndexX;

        if (IndexY - 2 >= 0 && IndexX + 1 < CountX)
            QIndex[1] = (IndexY - 2) * CountX + (IndexX + 1);

        if (IndexY - 2 >= 0 && IndexX + 2 < CountX)
            QIndex[2] = (IndexY - 2) * CountX + (IndexX + 2);

        if (IndexY - 1 >= 0 && IndexX + 2 < CountX)
            QIndex[3] = (IndexY - 1) * CountX + (IndexX + 2);

        if (IndexX + 2 < CountX)
            QIndex[4] = IndexY * CountX + (IndexX + 2);

        if (IndexY + 1 < CountY && IndexX + 2 < CountX)
            QIndex[5] = (IndexY + 1) * CountX + (IndexX + 2);

        if (IndexY + 2 < CountY && IndexX + 2 < CountX)
            QIndex[6] = (IndexY + 2) * CountX + (IndexX + 2);

        if (IndexY + 2 < CountY && IndexX + 1 < CountX)
            QIndex[7] = (IndexY + 2) * CountX + (IndexX + 1);

        if (IndexY + 2 < CountY)
            QIndex[8] = (IndexY + 2) * CountX + IndexX;

        if (IndexY + 2 < CountY && IndexX - 1 >= 0)
            QIndex[9] = (IndexY + 2) * CountX + (IndexX - 1);

        if (IndexY + 2 < CountY && IndexX - 2 >= 0)
            QIndex[10] = (IndexY + 2) * CountX + (IndexX - 2);

        if (IndexY + 1 < CountY && IndexX - 2 >= 0)
            QIndex[11] = (IndexY + 1) * CountX + (IndexX - 2);

        if (IndexX - 2 >= 0)
            QIndex[12] = IndexY * CountX + (IndexX - 2);

        if (IndexY - 1 >= 0 && IndexX - 2 >= 0)
            QIndex[13] = (IndexY - 1) * CountX + (IndexX - 2);

        if (IndexY - 2 >= 0 && IndexX - 2 >= 0)
            QIndex[14] = (IndexY - 2) * CountX + (IndexX - 2);

        if (IndexY - 2 >= 0 && IndexX - 1 >= 0)
            QIndex[15] = (IndexY - 2) * CountX + (IndexX - 1);

        for (int i = 0; i < 16; ++i)
        {
            if (QIndex[i] == -1)
                continue;

            if (BlockArray[QIndex[i]].GetComponent<Resource>().m_BlockType == Block_Type.Minimal)
            {
                BlockArray[QIndex[i]].GetComponent<Resource>().m_BlockType = Block_Type.Quater;
            }
        }
    }
   
    public void OpenBlock(int IndexX, int IndexY)
    {
        --scanCount;
        scanText.text = scanCount.ToString();
        PlaySound("TILE");

        if (IndexY - 1 >= 0)
        {
            BlockArray[(IndexY - 1) * CountX + IndexX].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
           

        if (IndexY - 1 >= 0 && IndexX + 1 < CountX)
        {
            BlockArray[(IndexY - 1) * CountX + (IndexX + 1)].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
           

        if (IndexX + 1 < CountX)
        {
            BlockArray[IndexY * CountX + (IndexX + 1)].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
           

        if (IndexY + 1 < CountY && IndexX + 1 < CountX)
        {

            BlockArray[(IndexY + 1) * CountX + (IndexX + 1)].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
          

        if (IndexY + 1 < CountY)
        {
            BlockArray[(IndexY + 1) * CountX + IndexX].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
           

        if (IndexY + 1 < CountY && IndexX - 1 >= 0)
        {
            BlockArray[(IndexY + 1) * CountX + (IndexX - 1)].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
           

        if (IndexX - 1 >= 0)
        {
            BlockArray[IndexY * CountX + (IndexX - 1)].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
          

        if (IndexY - 1 >= 0 && IndexX - 1 >= 0)
        {
            BlockArray[(IndexY - 1) * CountX + (IndexX - 1)].GetComponent<Resource>().m_Open = true;
            gameStat.text = string.Format("You scanned at x: {0}, y: {1}", IndexX, IndexY);
        }
           
    }

    public void ExtractBlock(int IndexX, int IndexY)
    {
        --extractCount;
        extractText.text = extractCount.ToString();
        PlaySound("EXTRACT");

        Resource block = BlockArray[IndexY * CountX + IndexX].GetComponent<Resource>();

        switch (block.m_BlockType)
        {
            case Block_Type.Minimal:
                break;
            case Block_Type.Quater:
                block.m_BlockType = Block_Type.Minimal;
                Score += 500;
                break;
            case Block_Type.Half:
                block.m_BlockType = Block_Type.Quater;
                Score += 1000;
                break;
            case Block_Type.Max:
                block.m_BlockType = Block_Type.Half;
                Score += 1500;
                break;
        }
        scoreText.text = Score.ToString();
    }
}
