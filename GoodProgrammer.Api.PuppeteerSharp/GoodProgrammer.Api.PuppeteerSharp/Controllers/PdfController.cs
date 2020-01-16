using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodProgrammer.Api.PuppeteerSharp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using RazorLight;

namespace GoodProgrammer.Api.PuppeteerSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        private readonly string _templateDirectory;
        private CreateFile model;
        private readonly string _fileName;

        public PdfController(IHostingEnvironment env)
        {
            _env = env;
            _templateDirectory = $"{_env.ContentRootPath}/Template";
            _fileName = "test";
        }

        [HttpGet]
        public async Task<ActionResult> Download()
        {
            try
            {
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                });
                using (var page = await browser.NewPageAsync())
                {
                    var body = await CreateBody(model.Body);
                    await page.SetContentAsync(body);
                    var options = GetOptions(await CreateHeader(model.Header), await CreateFooter());
                    return File(await page.PdfDataAsync(options), "application/pdf", $"{_fileName}.pdf");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("saveFile")]
        public async Task<IActionResult> SaveFile(string fileName)
        {
            try
            {
                CreateFileModel();
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                });
                using (var page = await browser.NewPageAsync())
                {
                    var body = await CreateBody(model.Body);
                    await page.SetContentAsync(body);
                    var options = GetOptions(await CreateHeader(model.Header), await CreateFooter());
                    await page.PdfAsync($"{fileName}.pdf", options);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private async Task<string> CreateHeader(Header model)
        {
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(_templateDirectory).UseMemoryCachingProvider().Build();
            return await engine.CompileRenderAsync("Header.cshtml", model);
        }

        private async Task<string> CreateBody(Body model)
        {
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(_templateDirectory).UseMemoryCachingProvider().Build();
            return await engine.CompileRenderAsync("Body.cshtml", model);
        }
        private async Task<string> CreateFooter()
        {
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(_templateDirectory).UseMemoryCachingProvider().Build();
            return await engine.CompileRenderAsync("Footer.cshtml", new object());
        }

        private PdfOptions GetOptions(string header, string footer)
        {
            PdfOptions options = new PdfOptions();
            options.Format = PaperFormat.A4;
            options.DisplayHeaderFooter = true;
            options.HeaderTemplate = header;
            options.FooterTemplate = footer;
            options.MarginOptions = new MarginOptions
            {
                Top = "60px",
                Bottom = "60px",
                Right = "30px",
                Left = "30px",
            };

            return options;
        }

        private void CreateFileModel()
        {
            model = new CreateFile()
            {
                Body = new Body()
                {
                    Row = new List<string>()
                    {
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                    }
                },
                Header = new Header()
                {
                    Date = DateTime.Now,
                    FirmName = "Good Programmer Marcin Buczak"
                }
            };
        }
    }
}