using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArxOne.MrAdvice.Advice;

namespace Common.Attribute
{
    public class MonitorExceptionAttribute : System.Attribute, IMethodAdvice
    {
        private readonly string _description = null;//方法的描述
 
        public MonitorExceptionAttribute(string description)
        {
            this._description = description;
        }

        public void Advise(MethodAdviceContext context)
        {
            var methodInfo = context.TargetType + "-" + context.TargetMethod.Name;
            try
            {
                context.Proceed();
            }
            catch(Exception ex)
            {
                var errMsg =
                    $"执行{methodInfo}方法发生异常，该方法的作用是：{_description}{Environment.NewLine}异常信息如下：{Environment.NewLine}{ex.ToString()}";
                LogHelper.Error(errMsg);
#if DEBUG
                LogHelper.Fatal(errMsg);
#endif
            }
        }
    }
}
