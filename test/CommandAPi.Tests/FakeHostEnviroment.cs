using System;
using Microsoft.AspNetCore.Hosting;     //Remove
using Microsoft.Extensions.FileProviders;

namespace CommandAPi.Tests
{
    public class FakeHostEnvironment : IHostingEnvironment
    {
        public string EnvironmentName {get; set;}
        public string ApplicationName {get; set;}
        public string WebRootPath {get; set;}
        public IFileProvider WebRootFileProvider {get; set;}
        public string ContentRootPath {get; set;}
        public IFileProvider ContentRootFileProvider {get; set;}
    }
}