using System.Collections.Generic;

public interface ICustomerProvider {
    Account[] existingAccounts { get; }
    ICustomer[] customers { get; }
    ICustomer currentCustomer { get; }
    bool allCustomersLeft { get; }

    ICustomer ProceedCustomer();
    void CreateQueue(int length);
}