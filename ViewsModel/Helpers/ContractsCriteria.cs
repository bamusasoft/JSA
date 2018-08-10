using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Jsa.DomainModel;

namespace Jsa.ViewsModel.Helpers
{
    public class ContractsCriteria : INotifyPropertyChanged
    {
        string _startDate;
        string _endDate;
        bool _scheduled;
        bool _nonScheduled;
        bool _closed;
        bool _opene;
        bool _hasBalance;
        private string _propertyNo;
        private int _customerId;
        private bool _signed;
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
        public bool Scheduled
        {
            get { return _scheduled; }
            set
            {
                _scheduled = value;
                RaisePropertyChanged();
            }
        }
        public bool NonScheduled
        {
            get { return _nonScheduled; }
            set
            {
                _nonScheduled = value;
                RaisePropertyChanged();
            }
        }
        public bool Closed
        {
            get { return _closed; }
            set
            {
                _closed = value;
                RaisePropertyChanged();
            }
        }
        public bool Open
        {
            get { return _opene; }
            set
            {
                _opene = value;
                RaisePropertyChanged();

            }
        }
        public bool HasBalance
        {
            get
            {
                return _hasBalance;

            }
            set
            {
                _hasBalance = value;
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

        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                RaisePropertyChanged();
            }

        }

        public bool Signed
        {
            get { return _signed; }
            set
            {
                _signed = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// Build criteria based on current instance's search values.
        /// </summary>
        /// <returns>The Expression built. Null of not any of the proeprties has value.</returns>
        public Expression< Func<Contract, bool>> BuildCriteria()
        {
            MethodInfo stringNullOrEmpty = typeof(string).GetMethod("IsNullOrEmpty");
            MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            MethodInfo compareTo = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            MethodInfo startsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            ParameterExpression param = Expression.Parameter(typeof(Contract), "contract");
            Expression expr = null;

            Expression startDateProperty = Expression.PropertyOrField(param, "StartDate");
            Expression endDateProperty = Expression.PropertyOrField(param, "EndDate");
            Expression closedProperty = Expression.PropertyOrField(param, "Closed");
            Expression scheduledPropery = Expression.PropertyOrField(param, "Scheduled");
            Expression balancePropery = Expression.PropertyOrField(param, "RentBalance");
            Expression propertyNoProperty = Expression.PropertyOrField(param, "PropertyNo");
            Expression customerNoProperty = Expression.PropertyOrField(param, "CustomerId");
            Expression photoPathProprety = Expression.PropertyOrField(param, "PhotoPath");

            Expression fromDateValue = Expression.Constant(StartDate, typeof(string));
            Expression toDateValue = Expression.Constant(EndDate, typeof(string));
            Expression closedValue = Expression.Constant(Closed, typeof(bool));
            Expression openValue = Expression.Constant(Open, typeof(bool));
            Expression scheduleValue = Expression.Constant(Scheduled, typeof(bool?));
            Expression hasbalance = Expression.Constant(HasBalance, typeof(bool));
            Expression propertyNoValue = Expression.Constant(PropertyNo, typeof (string));
            Expression customerNoValue = Expression.Constant(CustomerId, typeof (int));

            bool expressionAssigned = false;
            if (Closed)
            {
                ConstantExpression trueConstant = Expression.Constant(true, typeof(bool));
                Expression temp = Expression.Equal(closedProperty, trueConstant);
                expr = temp;
                expressionAssigned = true;
                
            }
            if(Open)
            {
                ConstantExpression falseConstant = Expression.Constant(false, typeof(bool));
                Expression temp = Expression.Equal(closedProperty, falseConstant);
                expr = temp;
                if (!expressionAssigned) expressionAssigned = true;
            }
            if (Scheduled)
            {
                ConstantExpression trueConstant = Expression.Constant(true, typeof(bool));
                if (!expressionAssigned)
                {

                    Expression temp = Expression.Equal(scheduledPropery, trueConstant);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    Expression temp = Expression.Equal(scheduledPropery, trueConstant);
                    expr = Expression.AndAlso(expr, temp);

                }
            }
            if(NonScheduled)
            {
                ConstantExpression falseConstant = Expression.Constant(false, typeof(bool));
                if (!expressionAssigned)
                {

                    Expression temp = Expression.Equal(scheduledPropery, falseConstant);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    Expression temp = Expression.Equal(scheduledPropery, falseConstant);
                    expr = Expression.AndAlso(expr, temp);

                }
            }
            if (HasBalance)
            {
                ConstantExpression zeroConstant = Expression.Constant(0);
                if (!expressionAssigned)
                {
                    Expression temp = Expression.GreaterThan(balancePropery, zeroConstant);
                    expr = temp;
                    expressionAssigned = true;
                }
                else
                {
                    Expression temp = Expression.GreaterThan(balancePropery, zeroConstant);
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
            if (CustomerId > 0)
            {
                ConstantExpression trueConstant = Expression.Constant(true, typeof(bool));
                if (!expressionAssigned)
                {

                    Expression temp = Expression.Equal(customerNoProperty, customerNoValue);
                    expr = temp;
                }
                else
                {
                    Expression temp = Expression.Equal(customerNoProperty, customerNoValue);
                    expr = Expression.AndAlso(expr, temp);

                }
            }
            if (Signed)
            {
                ConstantExpression nullConstant = Expression.Constant(null, typeof(string));
                if (!expressionAssigned)
                {
                    Expression temp = Expression.NotEqual(photoPathProprety, nullConstant);
                    expr = temp;

                }
                else
                {
                    Expression temp = Expression.NotEqual(photoPathProprety, nullConstant);
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
    }
}
