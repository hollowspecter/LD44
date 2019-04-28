using UnityEngine;

public interface ICustomer {
    void OnArrivedAtDesk(Dispensary customerDesk);
    void OnLeaveDesk();
    Account account { get; }
    void OnAccountCreated(Account account);
    float happiness { get; }
    bool fulfilledNeed { get; }
    Vector3 position { get; set; }

    void OnDialogueTrigger(string trigger);
}