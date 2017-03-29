using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Controllers;
using HoGentLend.Models.Domain;
using HoGentLend.Models.Domain.HoGentApi;
using Moq;
using HoGentLend;
using HoGentLend.ViewModels;
using System.Web.Mvc;
using HoGentLend.Models.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace HoGentLendTests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        /*
            Er is geen 80%+ code coverage bij deze controller omdat de code
            voor het meeste gegenereerde code was door visual studio zelf, dus
            mag men ervanuit gaan dat die geen fouten bevat.
    */

        private AccountController controller;

        private Mock<IGebruikerRepository> mockGebruikerRepository;
        private Mock<IHoGentApiLookupProvider> mockHoGentApiLookupProvider;
        private Mock<ApplicationSignInManager> mockApplicationSignInManager;
        private Mock<ApplicationUserManager> mockApplicationUserManager;

        private const string NEW_VALID_USERNAME = "StudentGebruikersnaam";
        private const string NEW_VALID_PASSWORD = "eenPaswoord123";
        private const string EXISTING_VALID_USERNAME = "AStudent";
        private const string EXISTING_VALID_PASSWORD = "StudentPassword";
        private const string NEW_VALID_USERNAME_LECTOR = "ALector";
        private const string NEW_VALID_PASSWORD_LECTOR = "ALectorPassword";
        private const string NOT_VALID_PASSWORD = "notValidPassword123";
        private HoGentApiLookupResult validHogentLookupResult;
        private HoGentApiLookupResult validHogentLookupResultLector;

        [TestInitialize]
        public void TestInitialize()
        {
            mockGebruikerRepository = new Mock<IGebruikerRepository>();
            mockHoGentApiLookupProvider = new Mock<IHoGentApiLookupProvider>();
            
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            mockApplicationUserManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, authenticationManager.Object);
            
            validHogentLookupResult = new HoGentApiLookupResult()
            {
                FirstName = "Bob",
                LastName = "De Bouwer",
                Email = "Bob_DeBouwer@kunnenwijhetmaken.ja",
                Faculteit = "faculteit",
                Type = "student",
                Base64Foto = null
            };
            validHogentLookupResultLector = new HoGentApiLookupResult()
            {
                FirstName = "Bob",
                LastName = "De Bouwer",
                Email = "Bob_DeBouwer@kunnenwijhetmaken.ja",
                Faculteit = "faculteit",
                Type = "personeel",
                Base64Foto = null
            };

            mockHoGentApiLookupProvider
                .Setup(m => m.Lookup(NEW_VALID_USERNAME, NEW_VALID_PASSWORD))
                .Returns(validHogentLookupResult);
            mockHoGentApiLookupProvider
                .Setup(m => m.Lookup(NEW_VALID_USERNAME_LECTOR, NEW_VALID_PASSWORD_LECTOR))
                .Returns(validHogentLookupResultLector);
            mockHoGentApiLookupProvider
                .Setup(m => m.Lookup(NEW_VALID_USERNAME, NOT_VALID_PASSWORD))
                .Returns(new HoGentApiLookupResult());
            mockHoGentApiLookupProvider
                .Setup(m => m.Lookup(EXISTING_VALID_USERNAME, EXISTING_VALID_PASSWORD))
                .Returns(validHogentLookupResult);
            mockApplicationSignInManager
                .Setup(m => m.PasswordSignInAsync(NEW_VALID_USERNAME, It.IsAny<string>(), false, false))
                .Returns(Task.FromResult(SignInStatus.Failure));
            mockApplicationSignInManager
                .Setup(m => m.PasswordSignInAsync(NEW_VALID_USERNAME_LECTOR, It.IsAny<string>(), false, false))
                .Returns(Task.FromResult(SignInStatus.Failure));
            mockApplicationSignInManager
                .Setup(m => m.PasswordSignInAsync(EXISTING_VALID_USERNAME, EXISTING_VALID_PASSWORD, false, false))
                .Returns(Task.FromResult(SignInStatus.Success));
            mockApplicationUserManager
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            controller = new AccountController(
                mockApplicationSignInManager.Object,
                mockApplicationUserManager.Object,
                mockGebruikerRepository.Object,
                mockHoGentApiLookupProvider.Object
            );
        }

        [TestMethod]
        public void POST_Login_NewValidStudent_PersistsGebruikerAndSignsIn()
        {
            controller.Login(CreateViewModel(NEW_VALID_USERNAME, NEW_VALID_PASSWORD), "");
            mockGebruikerRepository.Verify(m => m.Add(It.IsAny<Student>()), Times.Once);
            mockGebruikerRepository.Verify(m => m.SaveChanges(), Times.Once);
            mockApplicationSignInManager.Verify(m => m.PasswordSignInAsync(NEW_VALID_USERNAME, It.IsAny<string>(), false, false), Times.Exactly(2));
        }
        [TestMethod]
        public void POST_Login_NewValidLector_PersistsGebruikerAndSignsIn()
        {
            controller.Login(CreateViewModel(NEW_VALID_USERNAME_LECTOR, NEW_VALID_PASSWORD_LECTOR), "");
            mockGebruikerRepository.Verify(m => m.Add(It.IsAny<Lector>()), Times.Once);
            mockGebruikerRepository.Verify(m => m.SaveChanges(), Times.Once);
            mockApplicationSignInManager.Verify(m => m.PasswordSignInAsync(NEW_VALID_USERNAME_LECTOR, It.IsAny<string>(), false, false), Times.Exactly(2));
        }
        [TestMethod]
        public void POST_Login_NewValid_RedirectsToCatalogus()
        {
            ActionResult actionResult = controller.Login(CreateViewModel(NEW_VALID_USERNAME, NEW_VALID_PASSWORD), "");
            actionResult.AssertRedirectActionTo("Index", "Catalogus");
        }

        [TestMethod]
        public void POST_Login_Invalid_GivesModelError()
        {
            ActionResult actionResult = controller.Login(CreateViewModel(NEW_VALID_USERNAME, NOT_VALID_PASSWORD), "");
            actionResult.AssertViewWasReturned("Login", "Login");
            Assert.AreEqual(1, controller.ViewData.ModelState.Count);
        }

        [TestMethod]
        public void POST_Login_Invalid_DoesNotPersistGebruikerAndDoesNotSignIn()
        {
            controller.Login(CreateViewModel(NEW_VALID_USERNAME, NOT_VALID_PASSWORD), "");
            mockGebruikerRepository.Verify(m => m.Add(It.IsAny<Gebruiker>()), Times.Never);
            mockGebruikerRepository.Verify(m => m.SaveChanges(), Times.Never);
            mockApplicationSignInManager.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, false), Times.Never);
        }


        [TestMethod]
        public void POST_Login_ExistingValid_RedirectsToCatalogusAndSignsIn()
        {
            ActionResult actionResult = controller.Login(CreateViewModel(EXISTING_VALID_USERNAME, EXISTING_VALID_PASSWORD), "");
            actionResult.AssertRedirectActionTo("Index", "Catalogus");
            mockApplicationSignInManager.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, false), Times.Never);
        }
        [TestMethod]
        public void POST_Login_ExistingValid_DoesNotPersistGebruiker()
        {
            ActionResult actionResult = controller.Login(CreateViewModel(EXISTING_VALID_USERNAME, EXISTING_VALID_PASSWORD), "");
            mockGebruikerRepository.Verify(m => m.Add(It.IsAny<Gebruiker>()), Times.Never);
            mockGebruikerRepository.Verify(m => m.SaveChanges(), Times.Never);
        }

        private LoginViewModel CreateViewModel(string userName, string password)
        {
            return new LoginViewModel()
            {
                UserId = userName,
                Password = password,
                RememberMe = false
            };
        }
    }
}
