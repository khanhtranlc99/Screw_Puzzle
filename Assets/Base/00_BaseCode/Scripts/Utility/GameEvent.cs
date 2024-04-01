using UnityEngine;
using System.Collections;

public class GameEvent
{
    /// <summary>
    /// Class này là để cập nhật lại UI khi có bất kỳ sự thay đổi (tăng, giảm) số lượng item (Project cũ đã dùng)
    /// </summary>
    public class OnIAPSucsses : Singleton<OnIAPSucsses>
    {
        //   public float coin;
        public string googleItemID;

        public OnIAPSucsses()
        {
            googleItemID = string.Empty;
        }
    }
}
