﻿namespace Store.Domain.Repositories.Transaction;

public interface ITransactionWriteOnlyRepository
{
    Task Add(Entities.Transaction transaction); 
}