﻿using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public interface ICrud <T>
{
    public int Create(T t);
    public IEnumerable<T> RequestAll();

    public T RequestById(int i);

    public void Update(T t);

    public void Delete(T t);
}