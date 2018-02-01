using System;
using System.Linq.Expressions;

namespace Monolit.DataLayer.Expressions
{
	public class FilterExpressionBuilder : IFilterExpressionBuilder
	{
		public FilterExpressionBuilder(FilterExpressionType expressionType, object value)
		{
			ExpressionType = expressionType;
			Value = value;
		}

		public FilterExpressionType ExpressionType { get; private set; }
		public object Value { get; private set; }

		public Expression BuildExpression(MemberExpression memberExpression, Expression constantExpression)
		{
			switch (ExpressionType)
			{
				case FilterExpressionType.Equals:
					return Expression.Equal(memberExpression, constantExpression);
				case FilterExpressionType.GreaterThan:
					return Expression.GreaterThan(memberExpression, constantExpression);
				default:
					throw new NotSupportedException();
			}
		}
	}

}