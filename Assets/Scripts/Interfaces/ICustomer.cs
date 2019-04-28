public interface ICustomer {
    bool hasAccount { get; }
    void OnAccountCreated(Account account);
    float happiness { get; }
    bool fulfilledNeed { get; }
    bool fulfilledMust { get; }
    void OnDialogueTrigger(string trigger);
}