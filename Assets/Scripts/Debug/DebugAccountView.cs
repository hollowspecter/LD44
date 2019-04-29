using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class DebugAccountView : MonoBehaviour
{
    public TellerMachine machine;
    public Text debugOutputText;
    public Button refreshButton;

    private void Start()
    {
        refreshButton.onClick.AsObservable ()
            .Subscribe ( _ => RefreshText())
            .AddTo ( this );
    }

    private void RefreshText()
    {
        Debug.Log ( "Refreshing Text in DebugAccountView" );
        string result = "Number of Accounts: " + machine.accounts.Count + "\n--------";
        foreach (var NameAccountPair in machine.accounts)
        {
            result += "\nKey = " + NameAccountPair.Key;
            result += "\nName = " + NameAccountPair.Value.firstName + NameAccountPair.Value.lastName;
            result += "\nAccount Number = " + NameAccountPair.Value.accountNumber;
            result += "\nBalance = " + NameAccountPair.Value.balance;
            result += "\n---------";
        }
        debugOutputText.text = result;
    }
}
