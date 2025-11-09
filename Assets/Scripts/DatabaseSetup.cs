using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Data.Common;
using Firebase.Auth;
public class DatabaseSetup : MonoBehaviour
{
    private void Start()
    {
        var db = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
}
