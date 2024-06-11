using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITransition
{
    IState To {  get; }
    IPredicate Condition { get; }
}
