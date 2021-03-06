﻿using System.Collections.Generic;
using UnityEngine;
using UnityGoogleDrive;

public class TestFilesCreate : AdaptiveWindowGUI
{
    public Texture2D ImageToUpload;

    private GoogleDriveFiles.CreateRequest request;
    private string result;

    protected override void OnWindowGUI (int windowId)
    {
        if (request != null && request.IsRunning)
        {
            GUILayout.Label(string.Format("Loading: {0:P2}", request.Progress));
        }
        else if (GUILayout.Button("Upload To Root")) Upload(false);
        else if (GUILayout.Button("Upload To AddData")) Upload(true);

        if (!string.IsNullOrEmpty(result))
        {
            GUILayout.TextField(result);
        }
    }

    private void Upload (bool toAppData)
    {
        var content = ImageToUpload.EncodeToPNG();
        var file = new UnityGoogleDrive.Data.File() { Name = "TestUnityGoogleDriveFilesUpload.png", Content = content, MimeType = "image/png" };
        if (toAppData) file.Parents = new List<string> { "appDataFolder" };
        request = GoogleDriveFiles.Create(file);
        request.Fields = new List<string> { "id", "name", "size", "createdTime" };
        request.Send().OnDone += PrintResult;
    }

    private void PrintResult (UnityGoogleDrive.Data.File file)
    {
        result = string.Format("Name: {0} Size: {1:0.00}MB Created: {2:dd.MM.yyyy HH:MM:ss}\nID: {3}",
                file.Name,
                file.Size * .000001f,
                file.CreatedTime,
                file.Id);
    }
}
