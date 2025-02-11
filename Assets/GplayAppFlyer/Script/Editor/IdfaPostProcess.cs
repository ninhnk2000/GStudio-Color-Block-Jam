// Copyright (c) 2024 GplayJSC (gplayjsc.com)
// 
// Author: Axolotl (lamanh.w@gmail.com)
// 
// Created: 04/06/2024
// 
// File: IdfaPostProcess.cs
// 
// Note:

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace GplayAppFlyer.Editor
{
    public static class IdfaPostProcess
    {
        [PostProcessBuild(1000)]
        public static void UpdatePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS) return;
            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            var rootDict = plist.root;
            rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://appsflyer-skadnetwork.com/");
            File.WriteAllText(plistPath, plist.WriteToString());
            
            // Framework
            var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            var targetName = project.GetUnityFrameworkTargetGuid(); // note, not "project." ...
            var targetGUID = project.TargetGuidByName(targetName);
            project.AddFrameworkToProject(targetGUID, "AppTrackingTransparency.framework", true);
        }
    }
}