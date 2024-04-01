using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RankSceneController : SceneBase
{
    public LableRank lableRank;
    public List<LableRank> lsLableRank;
    public List<FakeUser> fakeUser;
    public Transform parent;
    public List<string> lsName;

    public LableRank lableUser;

    public override void Init()
    {

        var tempName = new List<string>();
        tempName = lsName;

        for (int i = 0; i < fakeUser.Count; i ++)
        {
            var randomScore = Random.Range(100,10000);
            fakeUser[i].score = randomScore;
            fakeUser[i].name = getRandomName();
        }

        fakeUser = fakeUser.OrderByDescending(user => user.score).ToList();

        for (int i = 0; i < fakeUser.Count; i++)
        {

            var temp = Instantiate(lableRank, parent);
            temp.Init((i+1).ToString(), fakeUser[i].name, fakeUser[i].score , fakeUser[i].avatar);
            lsLableRank.Add(temp);
        }


        SetDataUser();




        string getRandomName()
        {
            int random = Random.RandomRange(0, tempName.Count);
            string name = tempName[random];
            tempName.RemoveAt(random);
            return name;
        }






    }

    private void SetDataUser()
    {
      

    }


}
[System.Serializable]
public class FakeUser
{
   
    public string name;
    public int score;
    public Sprite avatar;
}