using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UniRx;
//using Script.Common.PlayFab;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using CodeStage.AntiCheat.Storage;

public struct DataField
{
    public string Key;
    public object Val;
}

public struct EventStreamData
{
    public string Event_Name;
    public Dictionary<string, object> Data;
}

public class UserDataServices : SingletonClass<UserDataServices>, IService
{
    public static string ReasonUpdateData { get; set; }
    private static SortedDictionary<string, DataField> privateDataQueue_new;

    private static bool private_change = true;
    private const string PRIVATE_DATAQUEUE_NEW = "PRIVATE_DATAQUEUE_V1";
    private static bool allowUpload;

    private Subject<Unit> OnDataChange = new Subject<Unit>();

    #region Event Stream
    private const string EVENT_STREAM_CACHED = "EVENT_STREAM_CACHED";

    private static SortedDictionary<string, EventStreamData> event_queue;

    private SortedDictionary<string, EventStreamData> Event_queue
    {
        get
        {
            if (event_queue == null)
            {
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(EVENT_STREAM_CACHED)))
                {
                    event_queue = JsonConvert.DeserializeObject<SortedDictionary<string, EventStreamData>>(PlayerPrefs.GetString(EVENT_STREAM_CACHED));
                }
                if (event_queue == null)
                    event_queue = new SortedDictionary<string, EventStreamData>();
            }

            return event_queue;
        }
        set
        {
            event_queue = value;
        }
    }
    private Subject<Unit> OnStreamEventChange = new Subject<Unit>();

    public static int DataEventCount
    {
        get
        {
            return PlayerPrefs.GetInt("DATA_EVENT_COUNT");
        }
        set
        {
            PlayerPrefs.SetInt("DATA_EVENT_COUNT", value);
            PlayerPrefs.Save();
        }
    }

    #endregion



    private SortedDictionary<string, DataField> PrivateDataQueueNew
    {
        get
        {
            if (privateDataQueue_new == null)
            {
                if (!string.IsNullOrEmpty(ObscuredPrefs.GetString(PRIVATE_DATAQUEUE_NEW)))
                    privateDataQueue_new = JsonConvert.DeserializeObject<SortedDictionary<string, DataField>>(ObscuredPrefs.GetString(PRIVATE_DATAQUEUE_NEW));
                if (privateDataQueue_new == null)
                    privateDataQueue_new = new SortedDictionary<string, DataField>();
            }

            return privateDataQueue_new;
        }
        set
        {
            privateDataQueue_new = value;
        }
    }


    // Use this for initialization
    public void Init()
    {
        OnDataChange.ThrottleFrame(1, FrameCountType.Update)
        .Subscribe(_ =>
        {
            UploadPrivateData();
        });

        OnStreamEventChange.ThrottleFrame(1, FrameCountType.Update)
        .Subscribe(_ =>
        {
          
        });
    }

  
    public static int DataChangeCount
    {
        get
        {
            return PlayerPrefs.GetInt("DATA_CHANGE_COUNT");
        }
        set
        {
            PlayerPrefs.SetInt("DATA_CHANGE_COUNT", value);
            PlayerPrefs.Save();
        }
    }

  

    private void SaveCachedData()
    {
        try
        {
            if (private_change)
            {
                string privateValueNew = JsonConvert.SerializeObject(PrivateDataQueueNew);
                ObscuredPrefs.SetString(PRIVATE_DATAQUEUE_NEW, privateValueNew);
                ObscuredPrefs.Save();
                private_change = false;
            }

        }
        catch (System.Exception ex)
        {
            Debug.LogError("Save Error" + ex.Message);
        }
    }

    private void UploadPrivateData()
    {
       
    }

    public void ClearPrivateQueue(string location)
    {
       
    }

    public bool NonternetConnection()
    {
        return Application.internetReachability == NetworkReachability.NotReachable;
    }


    public void HandlePrivateData(Dictionary<string, object> Data, List<string> ListKey, int DataVersion, string localtion)
    {
#if TEST_DATA
            return;
#endif
    }

    public void HandleDataChanged(string msg, Dictionary<string, object> Data, List<string> ListKey, int DataVersion, string localtion)
    {
     
    }




    public void AllowUpload(bool allow, string location)
    {
      
    }


    private static HashSet<Type> IntegerType = new HashSet<Type>
        {
            typeof(int),
            typeof(Int64),
            typeof(decimal),
        };

    private static HashSet<Type> FloatTypes = new HashSet<Type>
        {
            typeof(float),
            typeof(double),
        };

    private static HashSet<Type> StringType = new HashSet<Type>
        {
            typeof(string),
        };


    public static bool IsInteger(Type type)
    {
        return IntegerType.Contains(type) ||
               IntegerType.Contains(Nullable.GetUnderlyingType(type));
    }

    public static bool IsNumericFloat(Type type)
    {
        return FloatTypes.Contains(type) ||
               FloatTypes.Contains(Nullable.GetUnderlyingType(type));
    }

 
}


