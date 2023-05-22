using WebApp.Contexts;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

public class ContactService
{

    readonly Repo<ContactData, ContactContext> repo;

    public ContactService(Repo<ContactData, ContactContext> repo) =>
        this.repo = repo;

    public async Task<bool> Add(ContactFormView view)
    {
        await repo.AddAsync((ContactData)view);
        return true;
    }

}
