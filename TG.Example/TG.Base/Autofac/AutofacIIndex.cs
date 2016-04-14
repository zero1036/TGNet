using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;
using System.Diagnostics;
using Autofac.Features.Indexed;

namespace TG.Example
{
    public class AutofacIIndex
    {
        public void RegistDym()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SerCla1>().Keyed<IBaseSer>(SerStatus.one);
            builder.RegisterType<SerCla2>().Keyed<IBaseSer>(SerStatus.two);
            builder.RegisterType<MockController>();

            IContainer container = builder.Build();
            var mock = container.Resolve<MockController>();
            mock.Say();
        }
    }

    public enum SerStatus
    {
        one,
        two
    }

    public class MockController
    {
        private readonly IBaseSer _service;
        public MockController(IIndex<SerStatus, IBaseSer> status)
        {
            _service = status[SerStatus.two];
        }

        public void Say()
        {
            _service.Say();
        }
    }
}
