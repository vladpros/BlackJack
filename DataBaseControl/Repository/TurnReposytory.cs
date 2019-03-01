using System.Collections.Generic;
using DataBaseControl.Entities;
using DataBaseControl.Repository.Interface;

namespace DataBaseControl.Repository
{
    public class TurnReposytory : DefaultGenericRepository<Turn>, ITurnRepository
    {
        private BlackJackContext _context;

        public TurnReposytory(BlackJackContext context) : base(context)
        {
            _context = context;
        }

    }
}
