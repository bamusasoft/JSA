using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    public class PaymentRepository:RepositoryBase<Payment>
    {

        internal PaymentRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(Payment entity)
        {
            Context.Payments.Add(entity);
        }

        public override void Delete(Payment entity)
        {
            throw new NotImplementedException();
        }

        public override Payment GetById(object id)
        {
            int i = (int)id;
            return Query(x => x.PaymentNo == i).Single();
        }

        public override IQueryable<Payment> Query(System.Linq.Expressions.Expression<Func<Payment, bool>> filter)
        {
            return Context.Payments.Where(filter);
        }

        public override IList<Payment> GetAll()
        {
            return Context.Payments.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Payment> GetContractPayments(int contractNo)
        {
            return Query(x => x.ContractNo == contractNo).OrderBy(t => t.PayDate);
        }
    }
}
