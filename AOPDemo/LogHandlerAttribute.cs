using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;

namespace AOPDemo
{
    /// <summary>
    /// AOP方法处理类,实现了IMessageSink接口
    /// </summary>
    public sealed class MyAopHandler : IMessageSink
    {
        public IMessageSink NextSink { get; set; }
        public MyAopHandler(IMessageSink nextSink)
        {
            this.NextSink = nextSink;
        }

        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            //方法调用消息接口
            return null;

        }

        public IMessage SyncProcessMessage(IMessage msg)
        {

            var t_id = Thread.CurrentContext.ContextID;
            Console.WriteLine($"CurrentContext:{t_id}");
            Console.WriteLine("AOP Call Begin");
            var call = msg as IMethodCallMessage;

            //过滤 只有 CheckBefore 属性的函数
            if (call == null || (Attribute.GetCustomAttribute(call.MethodBase, typeof(CheckBeforeAttribute))) == null) return NextSink.SyncProcessMessage(msg);

            Console.WriteLine($"MethodName:{call.MethodName}");
            IMessage returnMsg = NextSink.SyncProcessMessage(msg);
            Console.WriteLine("AOP Call End");
            return returnMsg;
        }

    }

    /// <summary>
    /// 贴在方法上的标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class CheckBeforeAttribute : Attribute { }

    /// <summary>
    /// 贴在类上的标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class AOPAttribute : ContextAttribute, IContributeObjectSink
    {
        public AOPAttribute() : base("AOP") { }
        /// <summary>
        /// 实现消息接收器接口
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink next)
        {
            return new MyAopHandler(next);
        }
    }
}
