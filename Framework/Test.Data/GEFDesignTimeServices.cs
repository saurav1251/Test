using Test.Data.DesignTemplate;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data
{
    public class GEFDesignTimeServices : IDesignTimeServices
    {
        //Used for scaffolding database to code
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICSharpEntityTypeGenerator, GEntityTypeGenerator>();
        }
    }
}
