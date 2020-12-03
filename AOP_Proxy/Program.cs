using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace AOP_Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new Proxydemo(typeof(Demo), new Demo());
            Demo demo = (Demo)proxy.GetTransparentProxy();
            demo.Add(1, 2);
            Console.Read();

        }
    }
    public class Demo : MarshalByRefObject
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
    public class Proxydemo : RealProxy
    {
        public object instance = null;

        public Proxydemo(Type T, object demo) : base(T)
        {
            this.instance = demo;
        }
        public override IMessage Invoke(IMessage msg)
        {
            Console.WriteLine("代理方法");
            var methodCall = (IMethodCallMessage)msg;
            Console.WriteLine($"函数:{methodCall.MethodName} args:{string.Join(",", methodCall.Args)} 调用前");
            var result = methodCall.MethodBase.Invoke(instance, methodCall.Args);
            Console.WriteLine($"函数:{methodCall.MethodName} 调用后");
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);

        }
    }

}
