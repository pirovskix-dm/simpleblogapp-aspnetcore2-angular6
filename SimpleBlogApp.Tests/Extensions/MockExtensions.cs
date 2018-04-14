using AutoMapper;
using Moq;
using System;

namespace SimpleBlogApp.Tests.Extensions
{
	public static class MockExtensions
	{
		public static void SetupMap<TIn, TOut>(this Mock<IMapper> mapper, TIn gets, TOut returns)
		{
			mapper
				.Setup(m => m.Map<TIn, TOut>(gets))
				.Returns(returns);
		}

		public static void SetupMap<TIn, TOut>(this Mock<IMapper> mapper, TIn gets, Func<TIn, TOut> valueFunction)
		{
			mapper
				.Setup(m => m.Map<TIn, TOut>(gets))
				.Returns(valueFunction);
		}

		public static void SetupMap<TIn, TOut>(this Mock<IMapper> mapper, Func<TIn, TOut> valueFunction)
		{
			mapper
				.Setup(m => m.Map<TIn, TOut>(It.IsAny<TIn>()))
				.Returns(valueFunction);
		}
	}
}
