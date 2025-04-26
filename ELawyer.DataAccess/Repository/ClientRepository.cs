using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Client obj)
        {
            Client client = _context.Clients.FirstOrDefault(u => u.ID == obj.ID);
            if (client != null)
            {


                client.FirstName = obj.FirstName;
                client.LastName = obj.LastName;
                client.Address = obj.Address;
                client.Address = obj.Address;
                client.FrontCardImage = obj.FrontCardImage;
                client.BackCardImage = obj.BackCardImage;
                client.ImageUrl = obj.ImageUrl;
                client.LastLogin = obj.LastLogin;
                client.ClientType = obj.ClientType;
                client.UserStatus = obj.UserStatus;
                client.ClientRatingID = obj.ClientRatingID;
                client.NoOfLawyers = obj.NoOfLawyers;


            }
        }

    }
}
