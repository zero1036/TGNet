using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace EG.WeChat.Utility.Tools
{
    public static class WcfClient<TChannel>
    {
        public static ChannelFactory<TChannel> ChannelFactory = new ChannelFactory<TChannel>("*");

        public static TReturn Invoke<TReturn>(Func<TChannel, TReturn> FunctionCall)
        {
            var proxy = (IClientChannel)ChannelFactory.CreateChannel();
            TReturn result;

            using (OperationContextScope scope = new OperationContextScope(proxy))
            {
                result = FunctionCall((TChannel)proxy);
                proxy.Close();
            }

            proxy.Abort();

            return result;
        }

    }
}
