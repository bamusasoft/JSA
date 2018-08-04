using Jsa.DomainModel;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Jsa.ViewsModel.Helpers
{
    public class ContractPaymentsCriteria : INotifyPropertyChanged
    {
        
        string _propertyNo;
        int _customerNo;
        string _startDate;
        string _endDate;
        #region "Criterain"
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
        public string PropertyNo
        {
            get { return _propertyNo; }
            set
            {
                _propertyNo = value;
                RaisePropertyChanged();
            }
        }
        public int CustomerNo
        {
            get { return _customerNo; }
            set
            {
                _customerNo = value;
                RaisePropertyChanged();
            }
        }
     
        #endregion
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
        public Expression<Func<Contract, bool>> BuildCriteria()
        {
            MethodInfo stringNullOrEmpty = typeof(string).GetMethod("IsNullOrEmpty");
            MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            MethodInfo compareTo = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            MethodInfo startsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            ParameterExpression param = Expression.Parameter(typeof(Contract), "contract");
            Expression expr = null;

            Expression startDateProperty = Expression.PropertyOrField(param, "StartDate");
            Expression endDateProperty = Expression.PropertyOrField(param, "EndDate");
            Expression propertyNoProperty = Expression.PropertyOrField(param, "PropertyNo");
            Expression customerNoProperty = Expression.PropertyOrField(param, "CustomerId");


            Expression fromDateValue = ConstantExpression.Constant(this.StartDate, typeof(string));
            Expression toDateValue = ConstantExpression.Constant(this.EndDate, typeof(string));
            Expression propertyNoValue = ConstantExpression.Constant(this.PropertyNo, typeof(string));
            Expression customerNoValue = ConstantExpression.Constant(this.CustomerNo, typeof(int));



            bool expressionAssigned = false;
            if (!string.IsNullOrEmpty(PropertyNo))
            {

                    Expression temp = Expression.Call(propertyNoProperty, startsWith, propertyNoValue);
                    expr = temp;
                    if (!expressionAssigned) expressionAssigned = true;
                
            }
            if (CustomerNo > 0)
            {
                ConstantExpression trueConstant = Expression.Constant(true, typeof(bool));
                if (!expressionAssigned)
                {
                    
                    Expression temp = Expression.Equal(customerNoProperty, customerNoValue);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    Expression temp = Expression.Equal(customerNoProperty, customerNoValue);
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

            Expression<Func<Contract, bool>> criteria = null;
            if (expr != null)
            {
                criteria = Expression.Lambda<Func<Contract, bool>>(expr, param);
            }
            //if (criteria != null)
            //{
            //    return criteria.Compile();
            //}
            //else
            //{
            //    return null;
            //}
            return criteria;
        }
    }
}
