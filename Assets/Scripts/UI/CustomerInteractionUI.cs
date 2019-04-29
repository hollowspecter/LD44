using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Handles the moremoney and goaway interaction with the current customer.
/// the customer has to subscribe to the events themselves
/// </summary>
public class CustomerInteractionUI : MonoBehaviour
{
    // Singleton
    public static CustomerInteractionUI Instance { get; private set; }

    public Button moreMoneyButton;
    public Button goAwayButton;

    private void Awake()
    {
        // Singleton
        if ( Instance != null && Instance != this )
        {
            Destroy ( gameObject );
        }
        Instance = this;

        moreMoneyButton.gameObject.SetActive ( false );
        goAwayButton.gameObject.SetActive ( false );
    }

    #region public_methods

    /// <summary>
    /// Call to subscribe the current customer to the button callbacks
    /// </summary>
    public void SubscribeCustomer(UnityAction onMoreMoney, UnityAction onGoAway)
    {
        if ( onMoreMoney == null ) throw new System.ArgumentNullException ( nameof ( onMoreMoney ) );
        if ( onGoAway == null ) throw new System.ArgumentNullException ( nameof ( onGoAway ) );

        moreMoneyButton.onClick.RemoveAllListeners ();
        goAwayButton.onClick.RemoveAllListeners ();

        moreMoneyButton.gameObject.SetActive ( true );
        goAwayButton.gameObject.SetActive ( true );

        moreMoneyButton.onClick.AddListener ( onMoreMoney );
        goAwayButton.onClick.AddListener ( onGoAway );
        goAwayButton.onClick.AddListener ( Unsubscribe );
    }

    #endregion

    #region private_methods

    /// <summary>
    /// Call to clear all subscription and hide the buttons
    /// </summary>
    private void Unsubscribe()
    {
        moreMoneyButton.onClick.RemoveAllListeners ();
        goAwayButton.onClick.RemoveAllListeners ();

        moreMoneyButton.gameObject.SetActive ( false );
        goAwayButton.gameObject.SetActive ( false );
    }

    #endregion
}
