using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Web.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.SaleService
{
    public class SaleService : DeleteableEntityService, ISaleService
    {
        public SaleService(ApplicationDbContext context)
            :base(context)
        {

        }

        public List<ProductSale> CreateProductSales(List<BuyProductInputModel> inputModels)
        {
            throw new NotImplementedException();
        }
    }
}
