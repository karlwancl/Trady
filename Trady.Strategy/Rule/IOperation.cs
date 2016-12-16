using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Strategy.Rule
{
    public interface IOperation<T>
    {
        IRule<T> Operate(T obj);
    }
}
