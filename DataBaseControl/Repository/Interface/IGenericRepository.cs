﻿using BlackJackDataBaseAccess.Entities;

namespace BlackJackDataBaseAccess.Repository.Interface
{
    public interface IGenericRepository<T> where T : BasicEntities
    {
        long Create(T item);
        T FindById(long id);
        void Remove(T item);
        void Update(T item);
    }
}
