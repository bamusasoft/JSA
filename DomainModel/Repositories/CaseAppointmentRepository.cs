using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Jsa.DomainModel.Repositories
{
    public class CaseAppointmentRepository : RepositoryBase<CaseAppointment>
    {
        public CaseAppointmentRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(CaseAppointment entity)
        {
            Context.CaseAppointments.Add(entity);
        }

        public override void Delete(CaseAppointment entity)
        {
            Context.CaseAppointments.Remove(entity);
        }

        public override CaseAppointment GetById(object id)
        {
            int i = (int)id;
            return Query(x => x.Id == i).Single();
        }

        public override IQueryable<CaseAppointment> Query(Expression<Func<CaseAppointment, bool>> filter)
        {
            return Context.CaseAppointments.Where(filter)
                .Include(x => x.LegalCase);
        }

        public override IList<CaseAppointment> GetAll()
        {
            return Context.CaseAppointments.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public IList<CaseAppointment> GetCaseAppointments(int caseNo)
        {
            return Query(x => x.CaseNo == caseNo).ToList();
        }
        public bool TryCheckExist(int id, out CaseAppointment caseAppointment)
        {
            bool exist = false;
            try
            {
                var s = Context.CaseAppointments.Single(x => x.Id == id);
                caseAppointment = s;
                exist = true;
            }
            catch
            {

                caseAppointment = new CaseAppointment();
            }
            return exist;
        }
        public IList<CaseAppointment> DueCases(int days)
        {
            //Use DbFunctikons.AddDays to get rid of limitation in linq to entities that it does not support 
            //DataTime.AddDays, instead of loading all the store data then filter using linq to objects.
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            DateTime dueDate = DateTime.Now.AddDays(days);
            var s = Query(x =>
                DbFunctions.CreateDateTime(x.AppointmentGregDate.Year, x.AppointmentGregDate.Month, x.AppointmentGregDate.Day,0,0,0) >= DateTime.Now
                &&
                DbFunctions.CreateDateTime(x.AppointmentGregDate.Year, x.AppointmentGregDate.Month, x.AppointmentGregDate.Day, 0, 0, 0) <= dueDate
                );
            return s.ToList();

        }
        

        private DateTime ToGreg(DateTime d)
        {
           CultureInfo usInfo = new CultureInfo("en-US");
            var  s =Convert.ToDateTime(d, usInfo.DateTimeFormat);
            return s;
        }
    }
}