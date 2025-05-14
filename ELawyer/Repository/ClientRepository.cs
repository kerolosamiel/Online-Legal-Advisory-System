using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using Microsoft.EntityFrameworkCore;

namespace ELawyer.DataAccess.Repository;

public class ClientRepository : Repository<Client>, IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Client obj)
    {
        var client = _context.Clients.Include(client => client.ApplicationUser).FirstOrDefault(u => u.Id == obj.Id);
        if (client != null)
        {
            client.ApplicationUser.FirstName = obj.ApplicationUser.FirstName;
            client.ApplicationUser.LastName = obj.ApplicationUser.LastName;
            client.Address = obj.Address;
            client.Address = obj.Address;
            client.ImageUrl = obj.ImageUrl;
            client.ApplicationUser.LastLogin = obj.ApplicationUser.LastLogin;
            client.ClientType = obj.ClientType;
            client.UserStatus = obj.UserStatus;
            client.ClientRatingId = obj.ClientRatingId;
            client.NoOfLawyers = obj.NoOfLawyers;
        }
    }
}