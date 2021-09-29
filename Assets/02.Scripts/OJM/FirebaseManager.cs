using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

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
    private int rankNum = 10;

    private string[] rankersName;
    private int[] rankersScore;
    private int[] rankersMin;
    private int[] rankersSec;


    void Awake()
    {
        AppOptions options = new AppOptions();
        options.DatabaseUrl = new System.Uri(uri);
        FirebaseApp app = FirebaseApp.Create(options);
    }

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        rankersName = new string[rankNum];
        rankersScore = new int[rankNum];
        rankersMin = new int[rankNum];
        rankersSec = new int[rankNum];
    }

    // 데이터 등록
    public void InsertData(string skill, string nickname, int currentScore)
    {
        // UI 입력값
        string _playerName = nickname;
        int _score = currentScore;

        Player player = new Player(_playerName, _score);

        string json = JsonUtility.ToJson(player);
        reference.Child(skill).Child(_playerName).SetRawJsonValueAsync(json);
    }

    // 전체 데이터 불러오기
    public void LoadAllData(string skill)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(skill);
        //.OrderByChild("min").OrderByChild("score")
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
                    int _score = int.Parse(_data["score"].ToString());

                    int score = _score / 10000;

                    _score %= 10000;
                    int min = _score / 100;

                    _score %= 100;
                    int sec = _score;

                    rankersName[cnt - 1] = _name;
                    rankersScore[cnt - 1] = score;
                    rankersMin[cnt - 1] = 60 - min;
                    rankersSec[cnt - 1] = 60 - sec;

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

        for (int i = 0; i < rankNum; i++)
        {
            Debug.Log($"{rankersName[i]} - {rankersScore[i]} / {rankersMin[i]}:{rankersSec[i]}");
        }
    }

    // 정맥주사
    // 현재 플레이어 검색
    public void SelectData(string skill, string nickname)
    {
        string _playerName = nickname;
        // 지역 레퍼런스 선언
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(skill);
        playerNameQuery = reference.OrderByChild("playerName").EqualTo(_playerName);

        playerNameQuery.ValueChanged += OnDataLoaded;
    }

    // Query 결과값을 출력하는 함수
    void OnDataLoaded(object sender, ValueChangedEventArgs args)
    {
        DataSnapshot snapshot = args.Snapshot;

        foreach (var data in snapshot.Children)
        {
            IDictionary _data = (IDictionary)data.Value;
            Debug.Log($"Name : {_data["playerName"]}, Score : {_data["score"]}");
        }

        playerNameQuery.ValueChanged -= OnDataLoaded;
    }
}