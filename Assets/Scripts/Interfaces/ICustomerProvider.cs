public interface ICustomerProvider {
    ICustomer GetNextCustomer();
    Account[] existingAccounts { get; }
}