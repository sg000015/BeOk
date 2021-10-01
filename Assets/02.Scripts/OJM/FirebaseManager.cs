using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using UnityEngine.Events;

public class Player
{
    public string playerName;
    public int score;

    // 생성자
    public Player(string _playerName, int _score)
    {
        this.playerName = _playerName;
        this.score = _score;
    }

}

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    private readonly string uri = "https://be-ok-6591b-default-rtdb.firebaseio.com/";

    private bool isLoad = false;
    private Query playerNameQuery;
    [HideInInspector]
    public int rankNum = 7;

    [HideInInspector]
    public string[] rankersName;
    [HideInInspector]
    public int[] rankersScore;
    [HideInInspector]
    public int[] rankersMin;
    [HideInInspector]
    public int[] rankersSec;
    [HideInInspector]
    public int lastRankerScore;

    // public defaultev rankingEvent;
    public UnityEvent rankingEvent = new UnityEvent();


    void Awake()
    {
        AppOptions options = new AppOptions();
        options.DatabaseUrl = new System.Uri(uri);
        FirebaseApp app = FirebaseApp.Create(options);
    }

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        rankersName = new string[rankNum + 1];
        rankersScore = new int[rankNum + 1];
        rankersMin = new int[rankNum + 1];
        rankersSec = new int[rankNum + 1];

        InitRankers();
    }

    // 데이터 등록
    public void InsertData(string skill, string nickname, int currentScore)
    {

        Player player = new Player(nickname, currentScore);

        string json = JsonUtility.ToJson(player);
        reference.Child(skill).Child(nickname).SetRawJsonValueAsync(json);
    }

    // 전체 데이터 불러오기
    public void LoadAllData(string skill)
    {
        InitRankers();

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(skill);
        reference.OrderByChild("score").LimitToLast(rankNum).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed load data!!!");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                Debug.Log($"데이터 레코드 갯수 : {snapshot.ChildrenCount}");
                int cnt = (int)snapshot.ChildrenCount;

                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary _data = (IDictionary)data.Value;

                    string _name = _data["playerName"].ToString();
                    lastRankerScore = int.Parse(_data["score"].ToString());

                    int[] datas = processData(lastRankerScore);

                    rankersName[cnt - 1] = _name;
                    rankersScore[cnt - 1] = datas[0];
                    rankersMin[cnt - 1] = datas[1];
                    rankersSec[cnt - 1] = datas[2];

                    cnt--;
                }
                isLoad = true;
            }
        });
        StartCoroutine(UpdateAllData());
    }

    // UI변경
    IEnumerator UpdateAllData()
    {
        yield return new WaitUntil(() => isLoad);
        isLoad = false;

        // rankingEvent.Invoke();

        // for (int i = 0; i < rankNum; i++)
        // {
        //     Debug.Log($"{rankersName[i]} - {rankersScore[i]} / {rankersMin[i]}:{rankersSec[i]}");
        // }
    }

    // 현재 플레이어 검색
    // public void SelectData(string skill)
    // {
    //     string _playerName = nickname;
    //     // 지역 레퍼런스 선언
    //     DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(skill);
    //     playerNameQuery = reference.OrderByChild("playerName").EqualTo(_playerName);

    //     playerNameQuery.ValueChanged += OnDataLoaded;
    // }

    // Query 결과값을 출력하는 함수
    void OnDataLoaded(object sender, ValueChangedEventArgs args)
    {
        DataSnapshot snapshot = args.Snapshot;
        string _name;
        int _score;

        foreach (var data in snapshot.Children)
        {
            IDictionary _data = (IDictionary)data.Value;
            // Debug.Log($"Name : {_data["playerName"]}, Score : {_data["score"]}");

            _name = _data["playerName"].ToString();
            _score = int.Parse(_data["score"].ToString());

            int[] datas = processData(_score);

            rankersName[rankNum] = _name;
            rankersScore[rankNum] = datas[0];
            rankersMin[rankNum] = datas[1];
            rankersSec[rankNum] = datas[2];

        }

        playerNameQuery.ValueChanged -= OnDataLoaded;
    }



    int[] processData(int _score)
    {
        int[] datas = new int[3];

        // Debug.Log($"processData : {_score}");

        datas[0] = _score / 10000;

        _score %= 10000;
        datas[1] = _score / 100;
        datas[1] = 60 - datas[1];

        _score %= 100;
        datas[2] = _score;
        datas[2] = 60 - datas[2];

        // Debug.Log($"processData : {datas[0]}");
        // Debug.Log($"processData : {datas[1]}");
        // Debug.Log($"processData : {datas[2]}");
        // Debug.Log($"--------------------------------------------");

        return datas;
    }

    void InitRankers()
    {
        int i;
        for (i = 0; i <= rankNum; i++)
        {
            rankersName[i] = "None";
            rankersScore[i] = 0;
            rankersMin[i] = 0;
            rankersSec[i] = 0;
        }

    }



}