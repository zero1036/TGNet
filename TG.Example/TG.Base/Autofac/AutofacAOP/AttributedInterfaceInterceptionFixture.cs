﻿﻿using System;
using Autofac;
using Autofac.Core;
﻿using AutofacContrib.DynamicProxy2;
﻿using Castle.DynamicProxy;

namespace Autofac.Extras.Tests.DynamicProxy2
{
    public class AttributedInterfaceInterceptionFixture
    {
        [Intercept(typeof(AddOneInterceptor))]
        public interface IHasI
        {
            int GetI();
        }

        public class C : IHasI
        {
            public int I { get; private set; }

            public C()
            {
                I = 10;
            }

            public int GetI()
            {
                return I;
            }
        }

        class AddOneInterceptor : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
                invocation.Proceed();
                if (invocation.Method.Name == "GetI")
                    invocation.ReturnValue = 1 + (int)invocation.ReturnValue;
            }
        }

        public void DetectsNonInterfaceServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<C>().EnableInterfaceInterceptors();
            builder.RegisterType<AddOneInterceptor>();
            var c = builder.Build();
            //var dx = Assert.Throws<DependencyResolutionException>(() => c.Resolve<C>());
            //Assert.IsInstanceOf<InvalidOperationException>(dx.InnerException);
        }

        public void FindsInterceptionAttributeOnReflectionComponent()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<C>().As<IHasI>().EnableInterfaceInterceptors();
            builder.RegisterType<AddOneInterceptor>();
            var cpt = builder.Build().Resolve<IHasI>();

            //Assert.AreEqual(11, cpt.GetI()); // proxied
        }

        public void FindsInterceptionAttributeOnExpressionComponent()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new C()).As<IHasI>().EnableInterfaceInterceptors();
            builder.RegisterType<AddOneInterceptor>();
            var cpt = builder.Build().Resolve<IHasI>();

            //Assert.AreEqual(11, cpt.GetI()); // proxied
        }
    }
}