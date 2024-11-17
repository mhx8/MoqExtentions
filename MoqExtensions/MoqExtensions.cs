using System.Linq.Expressions;
using System.Reflection;
using Moq;
using Moq.Language.Flow;

namespace MoqExtensions;

public static class MoqExtensions
{
    public static ISetup<T, TResult> SetupSimple<T, TResult>(
        this Mock<T> mock,
        string methodName)
        where T : class
        => SetupSimple<T, TResult>(
            mock,
            methodName,
            true);

    public static ISetup<T, Task<TResult>> SetupSimpleAsync<T, TResult>(
        this Mock<T> mock,
        string methodName)
        where T : class
        => SetupSimple<T, Task<TResult>>(
            mock,
            methodName,
            false);

    public static void VerifySimple<T>(
        this Mock<T> mock,
        string methodName,
        Func<Times> times)
        where T : class
        => VerifySimple(
            mock,
            methodName,
            true,
            times);

    public static void VerifySimpleAsync<T>(
        this Mock<T> mock,
        string methodName,
        Func<Times> times)
        where T : class
        => VerifySimple(
            mock,
            methodName,
            false,
            times);

    private static void VerifySimple<T>(
        Mock<T> mock,
        string methodName,
        bool withoutCancellationToken,
        Func<Times> times)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(mock);

        Expression<Action<T>> exp = BuildVerifyExpression<T>(
            methodName,
            withoutCancellationToken);
        mock.Verify(
            exp,
            times);
    }

    private static ISetup<T, TResult> SetupSimple<T, TResult>(
        Mock<T> mock,
        string methodName,
        bool withoutCancellationToken)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(mock);

        Expression<Func<T, TResult>> exp = BuildSetupExpression<T, TResult>(
            methodName,
            withoutCancellationToken);
        return mock.Setup(exp);
    }

    private static Expression<Func<T, TResult>> BuildSetupExpression<T, TResult>(
        string methodName,
        bool withoutCancellationToken)
        where T : class
    {
        (ParameterExpression instance, MethodCallExpression callExp) =
            BuildExpression<T>(
                methodName,
                withoutCancellationToken);

        return Expression.Lambda<Func<T, TResult>>(
            callExp,
            instance);
    }

    private static Expression<Action<T>> BuildVerifyExpression<T>(
        string methodName,
        bool withoutCancellationToken)
        where T : class
    {
        (ParameterExpression instance, MethodCallExpression callExp) =
            BuildExpression<T>(
                methodName,
                withoutCancellationToken);

        return Expression.Lambda<Action<T>>(
            callExp,
            instance);
    }
    
    private static (ParameterExpression Instance, MethodCallExpression CallExp) BuildExpression<T>(
        string methodName,
        bool withoutCancellationToken)
        where T : class
    {
        List<MethodInfo> methods = typeof(T).GetMethods().Where(
            method => method.Name == methodName).ToList();

        MethodInfo? method = (withoutCancellationToken
                                 ? methods.FirstOrDefault(
                                     method
                                         => method.GetParameters().All(
                                             param
                                                 => param.ParameterType != typeof(CancellationToken)))
                                 : methods.FirstOrDefault(
                                     method
                                         => method.GetParameters().Any(
                                             param
                                                 => param.ParameterType == typeof(CancellationToken)))) ??
                             methods.FirstOrDefault();

        if (method == null)
        {
            throw new ArgumentException($"No method named '{methodName}' exists on type '{typeof(T).Name}'");
        }

        ParameterInfo[] parameterInfos = method.GetParameters();
        ParameterExpression instance = Expression.Parameter(
            typeof(T),
            "m");
        MethodCallExpression callExp =
            Expression.Call(
                instance,
                method,
                parameterInfos.Select(p => GenerateItIsAny(p.ParameterType)));

        return (instance, callExp);
    }

    private static MethodCallExpression GenerateItIsAny(
        Type type)
    {
        MethodInfo itIsAnyT =
            typeof(It)
                .GetMethod("IsAny")!
                .MakeGenericMethod(type);
        return Expression.Call(itIsAnyT);
    }
}