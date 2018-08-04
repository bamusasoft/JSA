using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Jsa.ViewsModel.Helpers
{
    public class PeriodSchedulesCriteria : INotifyPropertyChanged
    {
        string _startDate;
        string _endDate;
        bool _paidFilter;
        bool _unPaidFilter;
        string _propertyNo;

        public string StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                RaisePropertyChanged();
            }
        }
        public string EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RaisePropertyChanged();
            }
        }

        public bool PaidFilter
        {
            get { return _paidFilter; }
            set
            {
                _paidFilter = value;
                RaisePropertyChanged();
            }
        }
        public bool UnPaidFilter
        {
            get { return _unPaidFilter; }
            set
            {
                _unPaidFilter = value;
                RaisePropertyChanged();
            }
        }
        public string PropertyNo
        {
            get { return _propertyNo; }
            set
            {
                _propertyNo = value;
                RaisePropertyChanged();
            }
        }
        

        #region "INotifyPropertyChanged Members"
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        /// <summary>
        /// Build criteria based on current instance's search values.
        /// </summary>
        /// <returns>The Expression built. Null of not any of the proeprties has value.</returns>
        public Expression<Func<PeriodSchedule, bool>> BuildCriteria()
        {
            MethodInfo stringNullOrEmpty = typeof(string).GetMethod("IsNullOrEmpty");
            MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            MethodInfo compareTo = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            MethodInfo startsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            ParameterExpression param = Expression.Parameter(typeof(PeriodSchedule), "periodSchedules");
            Expression expr = null;

            Expression startDateProperty = Expression.PropertyOrField(param, "DateDue");
            Expression endDateProperty = Expression.PropertyOrField(param, "DateDue");
            Expression balancedProperty = Expression.PropertyOrField(param, "Balance");
            Expression unBalancedPropery = Expression.PropertyOrField(param, "Balance");
            Expression propertyNoProperty = Expression.PropertyOrField(param, "PropertyNo");


            Expression fromDateValue = ConstantExpression.Constant(this.StartDate, typeof(string));
            Expression toDateValue = ConstantExpression.Constant(this.EndDate, typeof(string));
            Expression includePaidValue = ConstantExpression.Constant(this.PaidFilter, typeof(bool));
            Expression includeNotPaidValue = ConstantExpression.Constant(this.UnPaidFilter, typeof(bool));
            Expression propertyNoValue = ConstantExpression.Constant(this.PropertyNo, typeof(string));



            bool expressionAssigned = false;
            if (PaidFilter)
            {
                ConstantExpression zeroConstant = Expression.Constant(0);
                Expression temp = Expression.Equal(balancedProperty, zeroConstant);
                expr = temp;
                if (!expressionAssigned) expressionAssigned = true;
            }
            if (UnPaidFilter)
            {
                ConstantExpression zeroConst = Expression.Constant(0);
                if (!expressionAssigned)
                {

                    Expression temp = Expression.GreaterThan(unBalancedPropery, zeroConst);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    Expression temp = Expression.GreaterThan(unBalancedPropery, zeroConst);
                    expr = Expression.AndAlso(expr, temp);

                }
            }


            if (!string.IsNullOrEmpty(StartDate))
            {

                if (!expressionAssigned)
                {
                    ConstantExpression constant = Expression.Constant(0);
                    Expression temp = Expression.GreaterThanOrEqual(
                        Expression.Call(startDateProperty, compareTo, fromDateValue), constant);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    ConstantExpression constant = Expression.Constant(0);
                    Expression temp = Expression.GreaterThanOrEqual(
                        Expression.Call(startDateProperty, compareTo, fromDateValue), constant);
                    expr = Expression.AndAlso(expr, temp);
                }
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                if (!expressionAssigned)
                {
                    ConstantExpression constant = Expression.Constant(0);
                    Expression temp = Expression.LessThanOrEqual(
                        Expression.Call(endDateProperty, compareTo, toDateValue), constant);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    ConstantExpression constant = Expression.Constant(0);
                    Expression temp = Expression.LessThanOrEqual(
                        Expression.Call(endDateProperty, compareTo, toDateValue), constant);
                    expr = Expression.AndAlso(expr, temp);
                }
            }
            if (!string.IsNullOrEmpty(PropertyNo))
            {
                if (!expressionAssigned)
                {
                    Expression temp = Expression.Call(propertyNoProperty, startsWith, propertyNoValue);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    Expression temp = Expression.Call(propertyNoProperty, startsWith, propertyNoValue);
                    expr = Expression.AndAlso(expr, temp);
                }
            }
            Expression<Func<PeriodSchedule, bool>> criteria = null;
            if (expr != null)
            {
                criteria = Expression.Lambda<Func<PeriodSchedule, bool>>(expr, param);
            }
            return criteria;
        }
    }
}
