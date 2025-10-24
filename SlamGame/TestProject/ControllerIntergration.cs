using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlamGame;

namespace TestProject
{
    internal class ControllerIntergration : IClassFixture<WebApplicationFactory<MainProgram>>
    {
    }
}
