using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArxOne.MrAdvice.Advice;

namespace Common.Attribute
{
    public class MonitorParametersAttribute : System.Attribute, IMethodAdvice
    {
        public void Advise(MethodAdviceContext context)
        {
            var sp = new Stopwatch();
            sp.Start();
            var methodinfo = context.TargetType + "-" + context.TargetMethod.Name;
            var parameters = context.TargetMethod.GetParameters();

            var sb = new StringBuilder();
            for (var i = 0; i < parameters.Length; i++)
            {
                sb.Append($"参数名{parameters[i].Name}-值{JsonHelper.Serialize(context.Arguments[i])} ");
            }

            LogHelper.Info($"执行{methodinfo}方法，入参信息：{sb.ToString()}");

            context.Proceed();

            sp.Stop();

            LogHelper.Info(context.HasReturnValue
                ? $"执行{methodinfo}完毕，耗时{sp.ElapsedMilliseconds}毫秒，返回值{JsonHelper.Serialize(context.ReturnValue)}。"
                : $"执行{methodinfo}完毕，耗时{sp.ElapsedMilliseconds}毫秒，本方法无返回值。");
        }
    }
}
