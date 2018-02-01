using System.Linq.Expressions;

namespace Monolit.DataLayer.Expressions
{
	public interface IFilterExpressionBuilder
	{
		FilterExpressionType ExpressionType { get; }
		object Value { get; }
		Expression BuildExpression(MemberExpression memberExpression, Expression constantExpression);
	}
}