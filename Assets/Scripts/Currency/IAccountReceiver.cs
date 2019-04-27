public interface IAccountReceiver {
    bool hasAccount { get; }
    void OnAccountCreated(Account account);
}