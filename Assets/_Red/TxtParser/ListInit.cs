using UnityEngine;
using System.IO;

public class ListInit : MonoBehaviour
{
    public string whitelistFilePath; // Path to the text document
    public string blacklistFilePath; // Path to the text document
    public string adminListFilePath; // Path to the text document
    public string staffListFilePath; // Path to the text document
    public string VIPListFilePath; // Path to the text document

    [HideInInspector] public string[] whiteList; // Array to store the usernames
    [HideInInspector] public string[] blackList; // Array to store the usernames
    [HideInInspector] public string[] adminList; // Array to store the usernames
    [HideInInspector] public string[] staffList; // Array to store the usernames
    [HideInInspector] public string[] VIPList; // Array to store the usernames

    private void Start()
    {
        Debug.Log(" ===== whiteList Usernames: ===== ");
        whiteList = ReadFilelist(whitelistFilePath);
        
        Debug.Log(" ===== blackList Usernames: ===== ");
        blackList = ReadFilelist(blacklistFilePath);
        
        Debug.Log(" ===== adminList Usernames: ===== ");
        adminList = ReadFilelist(adminListFilePath);
        
        Debug.Log(" ===== staffList Usernames: ===== ");
        staffList = ReadFilelist(staffListFilePath);
        
        Debug.Log(" ===== VIPList Usernames: ===== ");
        VIPList   = ReadFilelist(VIPListFilePath);

    }

    private string[] ReadFilelist(string _FilePath)
    {
        // Check if the text file exists
        if (File.Exists(_FilePath))
        {
            // Read all lines from the text file
            string[] lines = File.ReadAllLines(_FilePath);

            // Output the usernames to the console
            foreach (string username in lines)
            {
                Debug.Log("Username: " + username);
            }

            return lines;
        }
        else
        {
            Debug.LogError("Text file not found: " + _FilePath);
            return new string[0];
        }
    }
}