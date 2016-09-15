using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TG.Example;
using System.IO;

namespace TG.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //QuartzClass qu = new QuartzClass();
            //qu.Main();


            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log4net.config"));
            //UserService service = new UserService();
            //User user = service.GetUser(3);
            //System.Console.WriteLine("SysUserId:" + user.SysUserId);

            //UserService service = new UserService();
            //int result = service.Update();

            RedisTrans rt = new RedisTrans();
            rt.MoneyBug_MultiBulk();
        }
    }
}
