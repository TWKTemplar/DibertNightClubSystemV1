using UnityEngine;
using System.IO;
namespace Dilbert
{
    public class ListInit : MonoBehaviour
    {
        public bool DoDebug = true;
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
            if(DoDebug) Debug.Log(" ===== whiteList Usernames: ===== ");
            whiteList = ReadFilelist(whitelistFilePath);

            if (DoDebug) Debug.Log(" ===== blackList Usernames: ===== ");
            blackList = ReadFilelist(blacklistFilePath);

            if (DoDebug) Debug.Log(" ===== adminList Usernames: ===== ");
            adminList = ReadFilelist(adminListFilePath);

            if (DoDebug) Debug.Log(" ===== staffList Usernames: ===== ");
            staffList = ReadFilelist(staffListFilePath);

            if (DoDebug) Debug.Log(" ===== VIPList Usernames: ===== ");
            VIPList = ReadFilelist(VIPListFilePath);

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
                    if (DoDebug) Debug.Log("Username: " + username);
                }

                return lines;
            }
            else
            {
                if (DoDebug) Debug.LogError("Text file not found: " + _FilePath);
                return new string[0];
            }
        }
    }
}