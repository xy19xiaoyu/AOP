using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOPDemo
{
    [AOP]
    public abstract class Calc : ContextBoundObject
    {
        [CheckBefore]
        public int add(int a, int b)
        {
            return a + b;
        }

        public int diff(int a, int b)
        {
            return a - b;
        }
    }

    public class Calc1 : Calc
    {

    }
}
