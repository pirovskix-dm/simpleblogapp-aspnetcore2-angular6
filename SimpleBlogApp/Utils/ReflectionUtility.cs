using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SimpleBlogApp.Utils
{
	public static class ReflectionUtility
	{
		private static MemberInfo GetMemberInfo(Expression expression)
		{
			if (expression is MemberExpression)
				return (expression as MemberExpression).Member;

			if (expression is MethodCallExpression)
				return (expression as MethodCallExpression).Method;

			if (expression is UnaryExpression)
				return GetMemberInfo((expression as UnaryExpression).Operand);

			throw new ArgumentException("Invalid expression");
		}

		public static MemberInfo GetMemberInfo<T>(Expression<Func<T, object>> expression)
		{
			if (expression == null)
				throw new ArgumentException("The expression cannot be null.");

			return GetMemberInfo(expression.Body);
		}

		public static MemberInfo GetMemberInfo<T>(Expression<Action<T>> expression)
		{
			if (expression == null)
				throw new ArgumentException("The expression cannot be null.");

			return GetMemberInfo(expression.Body);
		}

		public static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			var attrs = memberInfo.GetCustomAttributes(true);
			if (attrs != null && attrs.Length != 0)
			{
				foreach (Attribute a in attrs)
				{
					if (a is T)
						return (T)a;
				}
			}
			return null;
		}
	}
}
