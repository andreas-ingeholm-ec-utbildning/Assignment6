using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class ContactsController : Controller
{

    #region Injections

    readonly ContactService contactService;

    public ContactsController(ContactService contactService) =>
        this.contactService = contactService;

    #endregion

    public IActionResult Index() =>
        View(new ContactFormView());

    [HttpPost]
    public async Task<IActionResult> Index(ContactFormView view)
    {

        if (ModelState.IsValid && await contactService.Add(view))
            return RedirectToAction("index");

        ModelState.AddModelError("", "Something went wrong, please try again.");
        return View(view);

    }

}
