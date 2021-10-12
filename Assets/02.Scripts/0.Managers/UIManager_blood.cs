using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_blood : MonoBehaviour
{
    [Range(0, 11)]
    public int NUM;

    public SoundManager soundManager;
    public FirebaseManager fb;
    public RankingCtrl rankingCtrl;

    [Header("UI")]
    public Image progress;
    public TMP_Text infoText;
    public TMP_Text infoTextPhone;
    public Image tubesImg;

    public Timer timer;

    [Header("Ranking UI")]
    public TMP_Text[] names;
    public TMP_Text[] scores;
    public TMP_Text[] times;


    private int[] scoreList;
    public int myScore;

    public GoogleSheetManager sheet;

    void Start()
    {
        // UpdateUI(0);
        sheet = GameObject.Find("NetworkManager").GetComponent<GoogleSheetManager>();
    }

    void Update()
    {
        // UpdateUI(NUM);

    }


    public void UpdateUI(int num)
    {
        if (num == 11)
        {
            scoreList = DrawingMgr.drawing.scoreList;

            // 점수 UI on
            StartCoroutine(nameof(ShowScore));
        }
        else
        {
            infoText.text = sheet.Sentence_Blood[num];
            infoTextPhone.text = sheet.Sentence_Blood[num];
        }

        if (num == 8)
        {
            tubesImg.gameObject.SetActive(true);
        }
        else
        {
            tubesImg.gameObject.SetActive(false);
        }
        progress.fillAmount = num / 11.0f;
        /*
        switch (num)
        {
            case 0:
                infoText.text = sheet.Sentence_Blood[0];
                infoTextPhone.text = sheet.Sentence_Blood[0];
                // infoText.text = "손 위생과 물품 준비가 끝난 상황입니다.\n토니켓을 묶어 혈관을 확인해주세요.\n<color=#ff0000>주의: 1분 이상 묶어둘 경우 혈액이 농축되어\n검사에 영향을 줄 수 있습니다.</color>";
                // infoTextPhone.text = "손 위생과 물품 준비가 끝난 상황입니다.\n토니켓을 묶어 혈관을 확인해주세요.\n<color=#ff0000>주의: 1분 이상 묶어둘 경우 혈액이 농축되어\n검사에 영향을 줄 수 있습니다.</color>";
                break;
            case 1:
                // infoText.text = sheet.Sentence_Blood[1];
                infoText.text = "소독솜으로 천자부위를 안에서 밖으로 둥글게 닦아주세요.";
                infoTextPhone.text = "소독솜으로 천자부위를 안에서 밖으로 둥글게 닦아주세요.";
                break;
            case 2:
                infoText.text = "주사기 내에 공기가 남아있지 않도록 플런저를 움직여주세요.\n(주사기 잡은 채로 뒷부분을 앞뒤로 3회 움직여주세요.)";
                infoTextPhone.text = "주사기 내에 공기가 남아있지 않도록 플런저를 움직여주세요.\n(주사기 잡은 채로 뒷부분을 앞뒤로 3회 움직여주세요.)";
                break;
            case 3:
                infoText.text = "주사기 바늘의 커버를 분리해주세요.";
                infoTextPhone.text = "주사기 바늘의 커버를 분리해주세요.";
                break;
            case 4:
                infoText.text = "채혈 바늘을 혈관에 15~20도로 삽입해주세요.";
                infoTextPhone.text = "채혈 바늘을 혈관에 15~20도로 삽입해주세요.";
                break;
            case 5:
                infoText.text = "플런저(주사기 뒷부분)를 당겨 채혈해주세요.\n<color=#ff0000>주의: 너무 급하게 당기면 혈액이 용혈될 수 있습니다.</color>";
                infoTextPhone.text = "플런저(주사기 뒷부분)를 당겨 채혈해주세요.\n<color=#ff0000>주의: 너무 급하게 당기면 혈액이 용혈될 수 있습니다.</color>";
                break;
            case 6:
                infoText.text = "토니켓을 풀어주세요.";
                infoTextPhone.text = "토니켓을 풀어주세요.";
                break;
            case 7:
                infoText.text = "소독솜을 천자부위에 가볍게 댄 상태에서 주사기을 빼세요.";
                infoTextPhone.text = "소독솜을 천자부위에 가볍게 댄 상태에서 주사기을 빼세요.";
                break;
            case 8:
                infoText.text = "주사기 내의 혈액을 진공튜브에 옮겨주세요.\n<color=#ff0000>주의: 캡 색상을 확인하시고 순서에 맞게 주입해주세요.</color>";
                infoTextPhone.text = "주사기 내의 혈액을 진공튜브에 옮겨주세요.\n<color=#ff0000>주의: 캡 색상을 확인하시고 순서에 맞게 주입해주세요.</color>";
                tubesImg.gameObject.SetActive(true);
                break;
            case 9:
                infoText.text = "혈액을 주입한 튜브를 8회 이상 Up-down mix해주세요.\n(손목을 회전시켜주세요.)";
                infoTextPhone.text = "혈액을 주입한 튜브를 8회 이상 Up-down mix해주세요.\n(손목을 회전시켜주세요.)";
                tubesImg.gameObject.SetActive(false);
                break;
            case 10:
                infoText.text = "튜브를 꽂아주세요.";
                infoTextPhone.text = "튜브를 꽂아주세요.";
                break;
            case 11:
                scoreList = DrawingMgr.drawing.scoreList;

                // 점수 UI on
                StartCoroutine(nameof(ShowScore));
                break;
        }
        */
    }

    IEnumerator ShowScore()
    {
        soundManager.SoundStop();
        // Delay Time
        float time = 1.0f;
        // Total Score
        int score = 0;

        // 점수 총합
        foreach (int i in scoreList)
        {
            // score += i;
            score += i;
        }

        // 주사 각도, 지혈 시간, 플런저 당기는 속도, 혈액 순서, 시간
        infoText.text = $"주사 각도 : {scoreList[0]}";
        infoTextPhone.text = $"주사 각도 : {scoreList[0]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoText.text += $"\n지혈 시간 : {scoreList[1]}";
        infoTextPhone.text += $"\n지혈 시간 : {scoreList[1]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoText.text += $"\n플런저 당기는 속도 : {scoreList[2]}";
        infoTextPhone.text += $"\n플런저 당기는 속도 : {scoreList[2]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoText.text += $"\n혈액 주입 순서 : {scoreList[3]}";
        infoTextPhone.text += $"\n혈액 주입 순서 : {scoreList[3]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoText.text += $"\n시간 : {scoreList[4]}";
        infoTextPhone.text += $"\n시간 : {scoreList[4]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoText.text += $"\n\n<b>총합 : {score}</b>";
        infoTextPhone.text += $"\n\n<b>총합 : {score}</b>";

        // Background Sound
        // 50점 초과
        if (score > 50)
        {
            // 4초
            soundManager.Sound(6);
            soundManager.Sound(6);
            soundManager.Sound(6);
            soundManager.Sound(6);

            // Patient Sound
            if (score >= 70)
            {
                // 최고
                StartCoroutine(nameof(PlayPatientSound), 3);
            }
            else
            {
                // 나쁘지않네요
                StartCoroutine(nameof(PlayPatientSound), 2);
            }
        }
        else
        {
            // 2초
            soundManager.Sound(5);
            soundManager.Sound(5);
            soundManager.Sound(5);
            soundManager.Sound(5);

            // 최악
            StartCoroutine(nameof(PlayPatientSound), 1);
        }

        yield return new WaitForSeconds(4);



        // 랭킹 UI
        Ranking(score);
    }

    IEnumerator PlayPatientSound(int num)
    {
        if (num == 1)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(4f);
        }
        soundManager.PlayPatientSound(num);
    }


    void Ranking(int score)
    {
        // 랭킹 불러오기
        fb.LoadAllData("Blood");

        rankingCtrl.ChangeCanvas();

        StartCoroutine(nameof(CheckRanking), score);
    }

    IEnumerator CheckRanking(int score)
    {
        yield return new WaitUntil(() => fb.isLoad);
        fb.isLoad = false;

        // 랭킹 안에 들었는지 확인
        //!1
        // scoreMgr.myScore = ConvertScore(score);
        myScore = ConvertScore(score);
        Debug.Log($"lastRankerScore : {fb.lastRankerScore}");
        Debug.Log($"myScore : {myScore}");
        if (fb.lastRankerScore <= myScore)
        {
            Debug.Log("7-1. Ranking in");
            // 랭킹 안에 들었으면 키보드 UI on
            rankingCtrl.ActiveNickname();
        }
        else
        {
            Debug.Log("7-2. Ranking out");
            // 랭킹 안에 들지 못했으면
            StartCoroutine(nameof(ActiveRanking));
            // ActiveRanking();
        }

    }

    IEnumerator ActiveRanking()
    {
        // 랭킹 불러오기
        fb.LoadAllData("Blood");

        yield return new WaitUntil(() => fb.isLoad);
        fb.isLoad = false;

        // UI 변경
        int _rankNum = fb.rankNum;

        for (int i = 0; i < _rankNum; i++)
        {
            names[i].text = fb.rankersName[i];
            scores[i].text = $"{fb.rankersScore[i]}";
            times[i].text = $"{fb.rankersMin[i]}:{fb.rankersSec[i]}";

            // Debug.Log($"____{names[i].text} / {scores[i].text} / {times[i].text}");
        }
        // Debug.Log("텍스트 변경 완료");

        // UI 활성화
        rankingCtrl.ActiveRankingUI();

    }

    int ConvertScore(int score)
    {
        int[] times = timer.GetTime();
        int _score = score * 10000;
        int min = 60 - times[0];
        int sec = 60 - times[1];

        _score += min * 100;
        _score += sec;

        return _score;
    }

    public void InsertData(string nickname)
    {
        fb.InsertData("Blood", nickname, myScore);
        // ActiveRanking();
        StartCoroutine(nameof(ActiveRanking));
    }



}
