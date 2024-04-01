#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.iOS.Xcode;
public class CustomPostprocessScript : ScriptableObject
{
    public DefaultAsset m_entitlementsFile;
/// <summary>
    /// Description for IDFA request notification 
    /// [sets NSUserTrackingUsageDescription]
    /// </summary>
    const string TrackingDescription =
        "This identifier will be used to deliver personalized ads to you. ";

    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            var rootDict = plist.root;

            // add key
            rootDict.SetString("NSLocationAlwaysUsageDescription", "Location is required to find out where you are");
            rootDict.SetString("NSLocationWhenInUseUsageDescription", "Location is required to find out where you are");
            rootDict.SetString("NSCalendarsUsageDescription", "Share score your friends");
            rootDict.SetString("NSUserTrackingUsageDescription", TrackingDescription);
rootDict.SetString("SKAdNetworkIdentifier", "su67r6k2v3.skadnetwork");
rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "ttps://postbacks-is.com");

            var appID = "ca-app-pub-3926209354441959~5125635339";
            rootDict.SetString("GADApplicationIdentifier", appID);
            var buildKey = "UIBackgroundModes";
            rootDict.CreateArray(buildKey).AddString("remote-notification");
            if (rootDict.values.ContainsKey("UIApplicationExitsOnSuspend"))
            {
                rootDict.values.Remove("UIApplicationExitsOnSuspend");
            }


            //write http protoco
            PlistElementDict allowsDict = plist.root.CreateDict("NSAppTransportSecurity");

            allowsDict.SetBoolean("NSAllowsArbitraryLoads", true);
            allowsDict.SetBoolean("NSAllowsArbitraryLoadsInWebContent", true);

            var exceptionsDict = allowsDict.CreateDict("NSExceptionDomains");

            var domainDict = exceptionsDict.CreateDict("ip-api.com");
            var domainDictRocket = exceptionsDict.CreateDict("rocketstudio.com.vn");
            var domainDictHerokuapp = exceptionsDict.CreateDict("space-shooter-server-colyseus.herokuapp.com");
            var domainDictLoveMoney = exceptionsDict.CreateDict("lovemoney.vn");
            var domainDictIP = exceptionsDict.CreateDict("127.0.0.1");
            var domainDictCountryFlag = exceptionsDict.CreateDict("countryflags.io");

            domainDict.SetBoolean("NSExceptionAllowsInsecureHTTPLoads", true);
            domainDict.SetBoolean("NSIncludesSubdomains", true);

            domainDictRocket.SetBoolean("NSExceptionAllowsInsecureHTTPLoads", true);
            domainDictRocket.SetBoolean("NSIncludesSubdomains", true);

            domainDictLoveMoney.SetBoolean("NSExceptionAllowsInsecureHTTPLoads", true);
            domainDictLoveMoney.SetBoolean("NSIncludesSubdomains", true);

            domainDictIP.SetBoolean("NSExceptionAllowsInsecureHTTPLoads", true);
            domainDictIP.SetBoolean("NSIncludesSubdomains", true);

            domainDictCountryFlag.SetBoolean("NSExceptionAllowsInsecureHTTPLoads", true);
            domainDictCountryFlag.SetBoolean("NSIncludesSubdomains", true);

            domainDictHerokuapp.SetBoolean("NSExceptionAllowsInsecureHTTPLoads", true);
            domainDictHerokuapp.SetBoolean("NSIncludesSubdomains", true);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }

    [PostProcessBuild]
    public static void OnPostProcessAssociedDomains(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS) return;

        var dummy = CreateInstance<CustomPostprocessScript>();
        var file = dummy.m_entitlementsFile;

        DestroyImmediate(dummy);
        if (file == null) return;

        var proj_path = PBXProject.GetPBXProjectPath(buildPath);
        var proj = new PBXProject();
        proj.ReadFromFile(proj_path);

        // target_name = "Unity-iPhone"
        var target_name = proj.GetUnityFrameworkTargetGuid();
        var target_guid = proj.GetUnityMainTargetGuid();
        var src = AssetDatabase.GetAssetPath(file);
        var file_name = Path.GetFileName(src);
        var dst = buildPath + "/" + target_name + "/" + file_name;
        FileUtil.CopyFileOrDirectory(src, dst);
        proj.AddFile(target_name + "/" + file_name, file_name);
        proj.AddBuildProperty(target_guid, "CODE_SIGN_ENTITLEMENTS", target_name + "/" + file_name);
        proj.WriteToFile(proj_path);
    }

    [PostProcessBuild(1)]
    public static void OnPostProcessBuildFramework(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            // Read.
            var projectPath = PBXProject.GetPBXProjectPath(path);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            var targetName = project.GetUnityFrameworkTargetGuid(); // note, not "project." ...
            var targetGUID = project.GetUnityMainTargetGuid();
            project.AddCapability(targetGUID, PBXCapabilityType.InAppPurchase);
            project.AddCapability(targetGUID, PBXCapabilityType.PushNotifications);
            AddFrameworks(project, targetGUID);
            // Write.
            File.WriteAllText(projectPath, project.WriteToString());
        }
    }

    [PostProcessBuild(999)]
    public static void OnPostProcessBuildDisableBitCode(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
            var target = pbxProject.GetUnityMainTargetGuid();
            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            pbxProject.SetBuildProperty(target, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
            pbxProject.WriteToFile(projectPath);
        }
    }

    private static void AddFrameworks(PBXProject project, string targetGUID)
    {
        project.AddFrameworkToProject(targetGUID, "UserNotificationsUI.framework", true);
        project.AddFrameworkToProject(targetGUID, "UserNotifications.framework", true);
        project.AddFrameworkToProject(targetGUID, "iAd.framework", true);
        project.AddFrameworkToProject(targetGUID, "StoreKit.framework", true);
    }
}
#endif