using System.Linq.Expressions;
using System.Reflection;
using WebAssembly.Runtime;

namespace Demo
{
	public static class DelegateExtensions
	{
		public static FunctionImport AsFunctionImport<T>(this Action<T> action)
		{
			return new FunctionImport(action);
		}

		public static FunctionImport AsFunctionImport<T1, TRet>(this Func<T1, TRet> @func)
		{
			return new FunctionImport(func);
		}
		
		public static FunctionImport AsFunctionImport<T1, T2, TRet>(this Func<T1, T2, TRet> @func)
		{
			return new FunctionImport(func);
		}
		public static FunctionImport AsFunctionImport<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> @func)
		{
			return new FunctionImport(func);
		}
		public static FunctionImport AsFunctionImport<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> @func)
		{
			return new FunctionImport(func);
		}
		public static FunctionImport AsFunctionImport<T1, T2, T3, T4, T5, TRet>(this Func<T1, T2, T3, T4, T5, TRet> @func)
		{
			return new FunctionImport(func);
		}
		
		public static FunctionImport AsFunctionImport<T1, T2, T3, T4, T5, T6, T7, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, TRet> @func)
		{
			return new FunctionImport(func);
		}
		
		public static FunctionImport AsFunctionImport<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TRet> @func)
		{
			return new FunctionImport(func);
		}
		public static FunctionImport AsFunctionImport<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> @func)
		{
			return new FunctionImport(func);
		}
		
		public static Delegate CreateDelegate(this MethodInfo methodInfo, object target) {
			Func<Type[], Type> getType;
			var isAction = methodInfo.ReturnType.Equals((typeof(void)));
			var types = methodInfo.GetParameters().Select(p => p.ParameterType);

			if (isAction) {
				getType = Expression.GetActionType;
			}
			else {
				getType = Expression.GetFuncType;
				types = types.Concat(new[] { methodInfo.ReturnType });
			}

			if (methodInfo.IsStatic) {
				return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
			}

			return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
		}
	}
}