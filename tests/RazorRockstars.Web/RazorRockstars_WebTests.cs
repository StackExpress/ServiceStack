using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using NUnit.Framework;
using ServiceStack.Text;

namespace RazorRockstars.Web
{
	[TestFixture]
	public class RazorRockstars_WebTests
	{
        Stopwatch startedAt;

	    [TestFixtureSetUp]
	    public void TestFixtureSetUp()
	    {
	        startedAt = Stopwatch.StartNew();
	    }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            "Time Taken {0}ms".Fmt(startedAt.ElapsedMilliseconds).Print();
        }

        [Ignore("Debug Run")][Test]
	    public void RunFor10Mins()
	    {
	        Thread.Sleep(TimeSpan.FromMinutes(10));
	    }

		public static string AcceptContentType = "*/*";
		public void Assert200(string url, params string[] containsItems)
		{
            url.Print();
			var text = url.GetStringFromUrl(AcceptContentType, r => {
				if (r.StatusCode != HttpStatusCode.OK)
					Assert.Fail(url + " did not return 200 OK");
			});
			foreach (var item in containsItems)
			{
				if (!text.Contains(item))
				{
					Assert.Fail(item + " was not found in " + url);
				}
			}
		}

		public void Assert200UrlContentType(string url, string contentType)
		{
            url.Print();
            url.GetStringFromUrl(AcceptContentType, r => {
				if (r.StatusCode != HttpStatusCode.OK)
					Assert.Fail(url + " did not return 200 OK: " + r.StatusCode);
				if (!r.ContentType.StartsWith(contentType))
					Assert.Fail(url + " did not return contentType " + contentType);
			});
		}

		public static string Host = "http://localhost:1338";

		static string ViewRockstars = "<!--view:Rockstars.cshtml-->";
        static string ViewRockstars2 = "<!--view:Rockstars2.cshtml-->";
        static string ViewRockstars3 = "<!--view:Rockstars3.cshtml-->";
		static string ViewRockstarsMark = "<!--view:RockstarsMark.md-->";
		static string ViewNoModelNoController = "<!--view:NoModelNoController.cshtml-->";
		static string ViewTypedModelNoController = "<!--view:TypedModelNoController.cshtml-->";
        static string ViewPage1 = "<!--view:Page1.cshtml-->";
        static string ViewPage2 = "<!--view:Page2.cshtml-->";
        static string ViewPage3 = "<!--view:Page3.cshtml-->";
        static string ViewPage4 = "<!--view:Page4.cshtml-->";
        static string ViewMarkdownRootPage = "<!--view:MRootPage.md-->";
        static string ViewMPage1 = "<!--view:MPage1.md-->";
        static string ViewMPage2 = "<!--view:MPage2.md-->";
        static string ViewMPage3 = "<!--view:MPage3.md-->";
        static string ViewMPage4 = "<!--view:MPage4.md-->";
        static string ViewRazorPartial = "<!--view:RazorPartial.cshtml-->";
        static string ViewMarkdownPartial = "<!--view:MarkdownPartial.md-->";
        static string ViewRazorPartialModel = "<!--view:RazorPartialModel.cshtml-->";

        static string View_Default = "<!--view:default.cshtml-->";
        static string View_Pages_Default = "<!--view:Pages/default.cshtml-->";
        static string View_Pages_Dir_Default = "<!--view:Pages/Dir/default.cshtml-->";
        static string ViewM_Pages_Dir2_Default = "<!--view:Pages/Dir2/default.md-->";

		static string Template_Layout = "<!--template:_Layout.cshtml-->";
        static string Template_Pages_Layout = "<!--template:Pages/_Layout.cshtml-->";
        static string Template_Pages_Dir_Layout = "<!--template:Pages/Dir/_Layout.cshtml-->";
        static string Template_SimpleLayout = "<!--template:SimpleLayout.cshtml-->";
        static string Template_SimpleLayout2 = "<!--template:SimpleLayout2.cshtml-->";
        static string Template_HtmlReport = "<!--template:HtmlReport.cshtml-->";

        static string TemplateM_Layout = "<!--template:_Layout.shtml-->";
        static string TemplateM_Pages_Layout = "<!--template:Pages/_Layout.shtml-->";
        static string TemplateM_Pages_Dir_Layout = "<!--template:Pages/Dir/_Layout.shtml-->";
        static string TemplateM_HtmlReport = "<!--template:HtmlReport.shtml-->";

		[Test]
		public void Can_get_page_with_default_view_and_template()
		{
            Assert200(Host + "/rockstars", ViewRockstars, Template_HtmlReport);
		}

		[Test]
		public void Can_get_page_with_alt_view_and_default_template()
		{
            Assert200(Host + "/rockstars?View=Rockstars2", ViewRockstars2, Template_Layout);
		}
		
		[Test]
		public void Can_get_page_with_alt_viewengine_view_and_default_template()
		{
            Assert200(Host + "/rockstars?View=RockstarsMark", ViewRockstarsMark, TemplateM_HtmlReport);
		}

        [Test]
        public void Can_get_page_with_default_view_and_alt_template()
        {
            Assert200(Host + "/rockstars?Template=SimpleLayout", ViewRockstars, Template_SimpleLayout);
        }

        [Test]
        public void Can_get_page_with_alt_viewengine_view_and_alt_razor_template()
        {
            Assert200(Host + "/rockstars?View=Rockstars2&Template=SimpleLayout2", ViewRockstars2, Template_SimpleLayout2);
        }

        [Test]
        public void Can_get_razor_content_pages()
        {
            Assert200(Host + "/TypedModelNoController",
                ViewTypedModelNoController, Template_SimpleLayout, ViewRazorPartial, ViewMarkdownPartial, ViewRazorPartialModel);
            Assert200(Host + "/nomodelnocontroller",
                ViewNoModelNoController, Template_SimpleLayout, ViewRazorPartial, ViewMarkdownPartial);
            Assert200(Host + "/pages/page1",
                ViewPage1, Template_Pages_Layout, ViewRazorPartialModel, ViewMarkdownPartial);
            Assert200(Host + "/pages/dir/Page2",
                ViewPage2, Template_Pages_Dir_Layout, ViewRazorPartial, ViewMarkdownPartial);
            Assert200(Host + "/pages/dir2/Page3",
                ViewPage3, Template_Pages_Layout, ViewRazorPartial, ViewMarkdownPartial);
            Assert200(Host + "/pages/dir2/Page4",
                ViewPage4, Template_HtmlReport, ViewRazorPartial, ViewMarkdownPartial);
        }

        [Test]
        public void Can_get_markdown_content_pages()
        {
            Assert200(Host + "/MRootPage",
                ViewMarkdownRootPage, TemplateM_Layout);
            Assert200(Host + "/pages/mpage1",
                ViewMPage1, TemplateM_Pages_Layout);
            Assert200(Host + "/pages/dir/mPage2",
                ViewMPage2, TemplateM_Pages_Dir_Layout);
        }

        [Test]
        public void Can_get_default_razor_pages()
        {
            Assert200(Host + "/",
                View_Default, Template_SimpleLayout, ViewRazorPartial, ViewMarkdownPartial, ViewRazorPartialModel);
            Assert200(Host + "/Pages/",
                View_Pages_Default, Template_Pages_Layout, ViewRazorPartial, ViewMarkdownPartial, ViewRazorPartialModel);
            Assert200(Host + "/Pages/Dir/",
                View_Pages_Dir_Default, Template_SimpleLayout, ViewRazorPartial, ViewMarkdownPartial, ViewRazorPartialModel);
        }

        [Test]
        public void Can_get_default_markdown_pages()
        {
            Assert200(Host + "/Pages/Dir2/",
                ViewM_Pages_Dir2_Default, TemplateM_Pages_Layout);
        }
        
        [Test] //Good for testing adhoc compilation
        public void Can_get_last_view_template_compiled()
        {
            Assert200(Host + "/rockstars?View=Rockstars3", ViewRockstars3, Template_SimpleLayout2);
        }
    }
}

