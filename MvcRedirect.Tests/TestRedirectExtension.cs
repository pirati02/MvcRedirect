using System;
using MvcRedirect.Extension;
using NUnit.Framework;
using Sample.Controllers;

namespace MvcRedirect.Tests
{
    public class TestRedirectExtension
    {
        [Test]
        public void TEST_NULL_REDIRECT_ACTION()
        {
            var controller = new HomeController();
            Assert.Throws<InvalidOperationException>(() => { controller.RedirectTo<HomeController>(null); });
        }

        [Test]
        public void TEST_REDIRECT_ACTION_WITH_NULL_PARAMETERS()
        {
            var controller = new HomeController();
            Assert.DoesNotThrow(() => { controller.RedirectTo<HomeController>(a => a.Privacy(null, null)); });
        }

        [Test]
        public void TEST_REDIRECT_ACTION_WITH_Valid_PARAMETERS()
        {
            var title = "some title";
            var controller = new HomeController();
            Assert.DoesNotThrow(() => { controller.RedirectTo<HomeController>(a => a.Privacy(title, "title 2")); });
        }


        [Test]
        public void TEST_REDIRECT_ACTION_WITH_EMPTY_PARAMETERS()
        {
            var title = "some title";
            var controller = new HomeController();
            Assert.DoesNotThrow(() => { controller.RedirectTo<HomeController>(a => a.Index()); });
        }
    }
}